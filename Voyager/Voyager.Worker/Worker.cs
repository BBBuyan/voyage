using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Voyager.Application.Interfaces;
using Voyager.Domain.Models;

namespace Voyager.Worker;

public class Worker : BackgroundService
{
    private readonly ILogger<Worker> _logger;
    private readonly IEnrollmentService _enrollmentService;

    public Worker(ILogger<Worker> logger, IEnrollmentService enrollmentService)
    {
        _logger = logger;
        _enrollmentService = enrollmentService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        VoyagerAgentCredentials VoyagerAgentCredentials = await _enrollmentService.Enroll(stoppingToken);
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }
            await Task.Delay(10000, stoppingToken);
        }
    }
}
