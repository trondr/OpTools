using System;
using Common.Logging;

namespace trondr.OpTools.Infrastructure.ContainerExtensions
{
    public interface ILogFactory
    {
        ILog GetLogger(Type type);
    }
}