using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Domain.Models;

namespace Voyager.Infrastructure.Storage;

public class EnrollmentCredentialsStore : BaseLocalStore, IEnrollmentCredentialsStore
{
    private readonly string _enrollmentCredentialsFilePath;
    private readonly string _enrollmentCredentialsFileName = "voyager-enrollment-credentials.json";

    public EnrollmentCredentialsStore()
    {
        _enrollmentCredentialsFilePath = Path.Combine(_commonApplicationPath, _enrollmentCredentialsFileName);
    }

    public async Task<EnrollmentCredentials?> ReadEnrollmentCredentials(CancellationToken ct)
    {
        try
        {
            if (!File.Exists(_enrollmentCredentialsFilePath))
                return null;

            string json = await File.ReadAllTextAsync(_enrollmentCredentialsFilePath, ct);
            if (string.IsNullOrWhiteSpace(json))
                return null;

            EnrollmentCredentials? creds = JsonSerializer.Deserialize<EnrollmentCredentials>(json);
            if (creds is null)
                return null;

            if (creds.TenantId == Guid.Empty)
                return null;

            if (string.IsNullOrWhiteSpace(creds.EnrollmentSecret))
                return null;

            return creds;
        }
        catch
        {
            return null;
        }
    }

    public void DeleteEnrollmentToken()
    {
        if (!File.Exists(_enrollmentCredentialsFilePath))
        {
            File.Delete(_enrollmentCredentialsFilePath);
        }
    }
}
