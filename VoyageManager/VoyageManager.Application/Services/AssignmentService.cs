using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using ErrorOr;
using VoyageManager.Application.Abstractions;
using VoyageManager.Application.DTOs;
using VoyageManager.Application.Helpers;
using VoyageManager.Application.Interfaces;
using VoyageManager.Domain.Engines;
using VoyageManager.Domain.Enums;
using VoyageManager.Domain.Models;

namespace VoyageManager.Application.Services;

public class AssignmentService : IAssignmentService
{
    private readonly IAssignmentRepository _assignmentRepository;
    private readonly IMapper _mapper;
    private readonly IUnitOfWork _unitOfWork;

    public AssignmentService(
        IAssignmentRepository commandManagementRepository,
        IMapper mapper,
        IUnitOfWork unitOfWork
    )
    {
        _assignmentRepository = commandManagementRepository;
        _mapper = mapper;
        _unitOfWork = unitOfWork;
    }

    private async Task<ErrorOr<List<CommandAssignmentDTO>>> GetAssignmentsByWorkerIdAsync(
        Guid workerId,
        CancellationToken ct
    )
    {
        bool exists = await _assignmentRepository.WorkerExistsAsync(workerId, ct);
        if (!exists)
            return Error.NotFound(description: "Worker not found.");

        List<CommandAssignment> assignments =
            await _assignmentRepository.GetAssignmentByWorkerIdAsync(workerId, ct);
        return _mapper.Map<List<CommandAssignmentDTO>>(assignments);
    }

    private async Task<ErrorOr<List<CommandAssignmentDTO>>> GetAssignmentsByGroupIdAsync(
        Guid groupId,
        CancellationToken ct
    )
    {
        bool exists = await _assignmentRepository.GroupExistsAsync(groupId, ct);
        if (!exists)
            return Error.NotFound(description: "Group not found.");

        List<CommandAssignment> assignments =
            await _assignmentRepository.GetAssignmentByGroupIdAsync(groupId, ct);
        return _mapper.Map<List<CommandAssignmentDTO>>(assignments);
    }

    public async Task<ErrorOr<List<CommandAssignmentDTO>>> GetAssignmentsAsync(
        Guid? workerId,
        Guid? groupId,
        CancellationToken ct
    )
    {
        if (workerId is not null)
        {
            return await GetAssignmentsByWorkerIdAsync(workerId.Value, ct);
        }
        else if (groupId is not null)
        {
            return await GetAssignmentsByGroupIdAsync(groupId.Value, ct);
        }
        return Error.Validation(description: "Either workerId or groupId must be provided.");
    }

    public async Task<ErrorOr<Success>> CreateAssignmentAsync(
        CreateAssignmentRequest request,
        string? username,
        CancellationToken ct
    )
    {
        bool canCreateAssignment = await _assignmentRepository.CanCreateAssignmentAsync(ct);

        if (!canCreateAssignment)
        {
            return Error.Forbidden(description: "Tenant cannot create assignment.");
        }
        List<CommandAssignment> assignments = [];
        if (request.TargetType == TargetType.Group)
        {
            IEnumerable<Worker> workers = await _assignmentRepository.GetWorkersByGroupIdAsync(
                request.TargetId,
                ct
            );
            foreach (Worker worker in workers)
            {
                assignments.Add(
                    new()
                    {
                        WorkerId = worker.Id,
                        CommandType = request.CommandType,
                        CreatedBy = username,
                    }
                );
            }
        }
        else if (request.TargetType == TargetType.Worker)
        {
            if (!await _assignmentRepository.WorkerExistsAsync(request.TargetId, ct))
            {
                return Error.NotFound(description: "Worker not found.");
            }
            assignments.Add(
                new()
                {
                    WorkerId = request.TargetId,
                    CommandType = request.CommandType,
                    CreatedBy = username,
                }
            );
        }
        else
        {
            return Error.Validation(description: "TargetType not supported.");
        }

        await _assignmentRepository.CreateAssignmentsAsync(assignments, ct);
        return Result.Success;
    }

    public async Task<ErrorOr<Success>> CancelAssignmentAsync(Guid id, CancellationToken ct)
    {
        CommandAssignment? assignment = await _assignmentRepository.GetAssignmentAsync(id, ct);
        if (assignment is null)
        {
            return Error.NotFound(description: "Assignment not found.");
        }

        AssignmentState currentStatus = assignment.State;
        AssignmentState newStatus = AssignmentState.CancelRequested;

        if (!CommandAssignmentEngine.CanTransition(currentStatus, newStatus))
        {
            return Error.Conflict(
                description: AssignmentStatusHelper.TransitionErrorDescription(
                    currentStatus,
                    newStatus
                )
            );
        }

        assignment.State = newStatus;

        await _unitOfWork.SaveChangesAsync(ct);
        return Result.Success;
    }
}
