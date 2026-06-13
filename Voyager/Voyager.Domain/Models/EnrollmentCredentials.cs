using System;

namespace Voyager.Domain.Models;

public class EnrollmentCredentials
{
    public Guid TenantId { get; set; }

    public required string EnrollmentSecret { get; set; }

    public bool IsValid()
    {
        if (TenantId == Guid.Empty)
            return false;

        if (string.IsNullOrEmpty(EnrollmentSecret))
            return false;

        return true;
    }
}
