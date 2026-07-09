using System.IO;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Voyager.Application.Abstractions;
using Voyager.Domain.Helpers;
using Voyager.Domain.Models;

namespace Voyager.Infrastructure.Storage;

public class EnrollmentCredentialsStore : BaseLocalStore, IEnrollmentCredentialsStore
{
    private readonly string _enrollmentCredentialsFilePath;

    public EnrollmentCredentialsStore()
    {
        _enrollmentCredentialsFilePath = Path.Combine(
            _commonApplicationPath,
            AgentFileNames.EnrollmentCredentialsFileName
        );
    }

    public async Task<EnrollmentCredentials?> ReadEnrollmentCredentialsAsync(CancellationToken ct)
    {
        try
        {
            if (!File.Exists(_enrollmentCredentialsFilePath))
                return null;

            string json = await File.ReadAllTextAsync(_enrollmentCredentialsFilePath, ct);
            EnrollmentCredentials? creds = JsonSerializer.Deserialize<EnrollmentCredentials>(json);

            if (creds is null || !creds.IsValid())
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
        try
        {
            if (!File.Exists(_enrollmentCredentialsFilePath))
            {
                File.Delete(_enrollmentCredentialsFilePath);
            }
        }
        catch { }
    }
}
