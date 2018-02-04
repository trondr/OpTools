namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel
{
    public class SecurityAccessRuleRecordMessage
    {
        public SecurityAccessRuleRecordMessage(string accesscontroltype, string identity, string accessmask, string isInherited, string inheritanceflags, string propagationflags)
        {
            Accesscontroltype = accesscontroltype;
            Identity = identity;
            Accessmask = accessmask;
            IsInherited = isInherited;
            Inheritanceflags = inheritanceflags;
            Propagationflags = propagationflags;
        }
        public string Accesscontroltype { get; }
        public string Identity { get; }
        public string Accessmask { get; }
        public string IsInherited { get; }
        public string Inheritanceflags { get; }
        public string Propagationflags { get; }
    }
}