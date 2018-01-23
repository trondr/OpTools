using System;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class HostName : Record<HostName>
    {
        public string Value { get; }

        private HostName(string hostName)
        {
            Value = hostName;
        }

        public static Result<HostName> Create(string hostName)
        {
            if (F.IsValidHostName(hostName))
                return new Result<HostName>(new HostName(hostName));
            return new Result<HostName>(new InvalidOperationException($"'{hostName}' is not a valid host name."));
        }        
    }
}