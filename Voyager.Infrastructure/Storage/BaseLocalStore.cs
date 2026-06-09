using System;
using System.IO;

namespace Voyager.Infrastructure.Storage;

public abstract class BaseLocalStore
{
    protected readonly string _commonApplicationPath;

    protected BaseLocalStore()
    {
        if (OperatingSystem.IsWindows())
        {
            string appFolderName = "Voyager";
            string commonAppPath = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
            _commonApplicationPath = Path.Combine(commonAppPath, appFolderName);
        }
        else if (OperatingSystem.IsLinux())
        {
            _commonApplicationPath = "/var/lib/voyager";
        }
        else
        {
            throw new PlatformNotSupportedException();
        }
    }
}
