using System;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class Hostname : Record<Hostname>
    {
        public string Value { get; }

        private Hostname(string hostName)
        {
            Value = hostName;
        }

        public static Result<Hostname> Create(string hostName)
        {
            if (F.IsValidHostName(hostName))
                return new Result<Hostname>(new Hostname(hostName));
            return new Result<Hostname>(new InvalidOperationException($"'{hostName}' is not a valid host name."));
        }        
    }
}