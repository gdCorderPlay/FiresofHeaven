using System.Diagnostics;
public class CommandRunner
{
    private string ExecutablePath;
    private string WorkingPath;
    public CommandRunner(string exe, string workingPath)
    {
        ExecutablePath = exe;
        WorkingPath = workingPath;
    }
    public string Run(string args)
    {
        ProcessStartInfo info = new ProcessStartInfo(ExecutablePath, args)
        {
            CreateNoWindow = true,
            RedirectStandardOutput = true,
            UseShellExecute=false,
            WorkingDirectory=WorkingPath
        };
        Process process = new Process()
        {
            StartInfo=info
        };
        process.Start();
        process.WaitForExit();
        string output = process.StandardOutput.ReadToEnd();
        process.Close();
        return output;
    }
}
