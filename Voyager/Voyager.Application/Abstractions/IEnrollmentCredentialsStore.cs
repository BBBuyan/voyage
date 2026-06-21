using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Abstractions;

public interface IEnrollmentCredentialsStore
{
    Task<EnrollmentCredentials?> ReadEnrollmentCredentialsAsync(CancellationToken ct);

    void DeleteEnrollmentToken();
}
