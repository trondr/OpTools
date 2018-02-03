using System.Collections.Generic;
using System.Linq;
using Akka.Actor;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ActorStatuses
    {
        private readonly Dictionary<IActorRef, ActorStatus> _actors;

        public ActorStatuses()
        {
            _actors = new Dictionary<IActorRef, ActorStatus>();
        }

        public void AddOrUpdate(IActorRef actor, ActorStatus status)
        {
            if (_actors.ContainsKey(actor))
                _actors[actor] = status;
            else
                _actors.Add(actor, status);
        }

        public bool AllHasTerminated()
        {
            return _actors.Values.All(status => status == ActorStatus.Terminated);
        }        
    }
}