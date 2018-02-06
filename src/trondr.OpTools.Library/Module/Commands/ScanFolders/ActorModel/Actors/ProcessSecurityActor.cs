using System;
using System.Collections.Immutable;
using Akka.Actor;
using Akka.Event;
using LanguageExt;
using trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages;

namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Actors
{
    public class ProcessSecurityActor : ReceiveActor
    {
        private ILoggingAdapter _logger;
        private ProcessActorSecurityOpenMessage _message;
        private ILoggingAdapter Logger => _logger ?? (_logger = Context.GetLogger());

        private Func<string, ImmutableList<SecurityAccessRuleRecordMessage>> _getAccessRuleRecords;
        private Func<string, ImmutableList<SecurityAccessRuleRecordMessage>> _getCachedAccessRuleRecords;

        public ProcessSecurityActor()
        {
            Become(Initializing);
        }

        private void Initializing()
        {
            Receive<ProcessActorSecurityOpenMessage>(message => OnOpen(message));
            Receive<UsageRecordMessage>(message => Logger.Error($"Unabled process usage records. {GetType().Name} has not been initialized."));            
        }

        private void Opened()
        {
            Receive<ProcessActorSecurityOpenMessage>(message => Logger.Error($"{GetType().Name} is allready initialized and ready to process."));
            Receive<UsageRecordMessage>(message => OnProcessSecurity(message));
        }

        private void OnOpen(ProcessActorSecurityOpenMessage message)
        {
            _message = message;
            
            //Define function definition for getting access rule records from sddl string
            _getAccessRuleRecords = sddl => SecurityFileUtil.GetSecurityAccessRuleRecords(sddl).ToImmutableList();

            //Add caching to the function definition. The Memo function definition will cache calls and return result from cache
            //if function has previously been called with the same input. In this case if the sddl string has been previously been
            //looked up, the result will be found in the cache, avoiding potentially costly network calls.
            _getCachedAccessRuleRecords = _getAccessRuleRecords.Memo(); 
            Become(Opened);
        }

        private void OnProcessSecurity(UsageRecordMessage usageRecord)
        {
            var accessRuleRecordMessages = _getCachedAccessRuleRecords(usageRecord.Sddl);
            foreach (var securityAccessRuleRecordMessage in accessRuleRecordMessages)
            {
                if (securityAccessRuleRecordMessage.IsInherited == "False")
                {
                    var securityRecordMessage = new SecurityRecordMessage(usageRecord.Hostname, usageRecord.Path,
                        securityAccessRuleRecordMessage.Accesscontroltype, securityAccessRuleRecordMessage.Identity,
                        securityAccessRuleRecordMessage.Accessmask, securityAccessRuleRecordMessage.IsInherited,
                        securityAccessRuleRecordMessage.Inheritanceflags,
                        securityAccessRuleRecordMessage.Propagationflags);
                    _message.Coordinator.Tell(securityRecordMessage);
                }
            }
            _message.Coordinator.Tell(new UsageRecordProcessedMessage());
        }        
    }
}
