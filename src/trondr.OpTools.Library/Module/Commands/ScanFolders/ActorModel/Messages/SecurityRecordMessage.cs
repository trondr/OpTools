namespace trondr.OpTools.Library.Module.Commands.ScanFolders.ActorModel.Messages
{
    public class SecurityRecordMessage
    {
        public SecurityRecordMessage(string hostname, string path, string accesscontroltype, string identity, string accessmask, string isInherited, string inheritanceflags, string propagationflags)
        {
            Hostname = hostname;
            Path = path;
            Accesscontroltype = accesscontroltype;
            Identity = identity;
            Accessmask = accessmask;
            IsInherited = isInherited;
            Inheritanceflags = inheritanceflags;
            Propagationflags = propagationflags;
        }

        public string Hostname { get;  }
        public string Path { get;  }
        public string Accesscontroltype { get;  }
        public string Identity { get;  }
        public string Accessmask { get;  }
        public string IsInherited { get;  }
        public string Inheritanceflags { get;  }
        public string Propagationflags { get;  }
    }
}