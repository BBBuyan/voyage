using System;

namespace Voyager.Domain.Models;

public class EnrollmentCredentials
{
    public Guid TenantId { get; set; }

    public required string EnrollmentSecret { get; set; }
}
