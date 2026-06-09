using System;
using System.Collections.Generic;

namespace VoyageManager.Domain.Models;

/// <summary>
/// Created on tenant creation.
/// Current plan is to have it created via RabbitMQ on 
/// tenant creation event in tenant_management or user_management.
/// </summary>
public class Tenant
{
    public Guid Id { get; set; }

    public required string Name { get; set; }

    public int PurchasedLicenses { get; set; }

    public int UsedLicenses { get; set; }

    public int AvailableLicenses => PurchasedLicenses - UsedLicenses;

    public required string EnrollmentSecret { get; set; }

    public ICollection<VoyagerAgent> VoyagerAgents { get; set; } = [];

    public ICollection<VoyagerGroup> VoyagerGroups { get; set; } = [];
}
