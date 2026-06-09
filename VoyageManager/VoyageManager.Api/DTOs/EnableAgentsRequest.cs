using System;

namespace VoyageManager.Api.DTOs;

public class EnableAgentsRequest
{
    public Guid? AgentId { get; set; }

    public Guid? GroupId { get; set; }
}
