namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class ActorFailedMessage
    {
        public string Message { get; }
        public int ExitCode { get; }

        public ActorFailedMessage(string message, int exitCode)
        {
            Message = message;
            ExitCode = exitCode;
        }
    }
}