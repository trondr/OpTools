using System;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Commands.RunScript
{
    public class IpAddress: Record<IpAddress>
    {
        private IpAddress(string ipAddress)
        {
            Value = ipAddress;
        }

        public string Value { get; }

        public static Result<IpAddress> Create(string ipAdress)
        {
            if (F.IsValidIpAddress(ipAdress))
                return new Result<IpAddress>(new IpAddress(ipAdress));
            return new Result<IpAddress>(new InvalidOperationException($"'{ipAdress}' is not a valid ip address."));
        }
    }
}