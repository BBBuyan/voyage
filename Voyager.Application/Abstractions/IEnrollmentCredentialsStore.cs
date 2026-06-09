using System.Threading;
using System.Threading.Tasks;
using Voyager.Domain.Models;

namespace Voyager.Application.Abstractions;

public interface IEnrollmentCredentialsStore
{
    Task<EnrollmentCredentials?> ReadEnrollmentCredentials(CancellationToken ct);

    void DeleteEnrollmentToken();
}
