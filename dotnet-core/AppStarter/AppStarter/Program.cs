using System;
using System.Diagnostics;
using System.IO;
using System.Threading;

namespace AppStarter
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Start app....");

            Thread.Sleep(5000);

            var workingDirectory = @"..\PublishOutput";
            var dotnetCmd = "dotnet.exe";
            var dllName = "ConsoleCpuRunningApp.dll";

            var processStartInfo = new ProcessStartInfo()
            {
                FileName = dotnetCmd,
                WorkingDirectory = workingDirectory,
                Arguments = dllName,                
                UseShellExecute = false,
                RedirectStandardError = true,
                RedirectStandardOutput = true
            };

            var process = new Process()
            {
                StartInfo = processStartInfo,
                EnableRaisingEvents = true
            };

            process.OutputDataReceived += (sender, e) =>
            {
                return;
            };

            process.ErrorDataReceived += (sender, e) =>
            {
                return;
            };

            process.Start();

            process.BeginErrorReadLine();
            process.BeginOutputReadLine();

            process.WaitForExit();
        }
    }
}
