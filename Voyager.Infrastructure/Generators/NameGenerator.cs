using System;

namespace Voyager.Infrastructure.Generators;

public static class NameGenerator
{
    public static string GenerateName()
    {
        return $"{Environment.MachineName}-{Environment.OSVersion.Platform}-{Environment.UserName}";
    }
}
