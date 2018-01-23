using System;
using System.Diagnostics;
using System.IO;
using Akka.Actor;
using Akka.Event;
using trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.RunScript.ActorModel.Actors
{
    public class RunScriptOnHostActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        public RunScriptOnHostActor()
        {
            Receive<RunScriptOnHostMessage>(message => HandleRunScriptOnHostMessage(message));
        }

        private void HandleRunScriptOnHostMessage(RunScriptOnHostMessage message)
        {
            var hostName = message.ResolveToIpv4Address ? message.Host.IpAddress.Value: message.Host.HostName.Value;
            Logger.Info($"Running script on {hostName} : '{message.ScriptPath}' -HostName=\"{hostName}\" -ResultFolderPath=\"{message.ResultFolderPath}\"");
            var process = new Process
            {
                StartInfo =
                {
                    FileName = GetPowerShellExe(),
                    Arguments = $"-ExecutionPolicy ByPass -File \"{message.ScriptPath}\" -HostName {hostName} -ResultFolderPath {message.ResultFolderPath}"
                },
                EnableRaisingEvents = true
            };
            process.OutputDataReceived += (sender, args) => { Logger.Info(args.Data); };
            process.ErrorDataReceived += (sender, args) => { Logger.Error(args.Data); };
            process.Exited += (sender, args) => { Logger.Info($"Script on {hostName} returned {process.ExitCode.ToString()}"); };
            process.Start();
            process.WaitForExit();            
            Sender.Tell(new RunScriptOnHostIsDoneMessage(message.Host));
        }

        private string GetPowerShellExe()
        {
            var powerShellExe = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "WindowsPowershell", "v1.0", "PowerShell.exe");
            if (!File.Exists(powerShellExe))
            {
                throw new FileNotFoundException("Could not find Powershell.exe.", powerShellExe);
            }
            return powerShellExe;
        }
    }
}