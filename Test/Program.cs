using System;
using Microsoft.Win32;

public class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");
        string? test = Registry.GetValue(@"HKEY_LOCAL_MACHINE\SOFTWARE\Microsoft\Cryptography", "MachineGuid", null)?.ToString();
        //string test = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData);
        //string linPath = "/var/lib/voyager";
        //string appName = "test.json";
        Console.WriteLine(test);
    }
}
