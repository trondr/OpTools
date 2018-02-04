using System.Security.AccessControl;
using System.Security.Principal;
using Pri.LongPath;

namespace trondr.OpTools.Library.Module.Common
{
    public static class DirectoryInfoExensions
    {
        public static string GetAccessControlSddlForm(this DirectoryInfo directory, bool explicitOnly)
        {
            var securityDescriptor = directory.GetAccessControl(AccessControlSections.Access);
            if (explicitOnly)
            {
                var explicitAccessRules = securityDescriptor.GetAccessRules(true, false, typeof(SecurityIdentifier));
                if (explicitAccessRules.Count > 0)
                    return securityDescriptor.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
            }
            else
            {
                return securityDescriptor.GetSecurityDescriptorSddlForm(AccessControlSections.Access);
            }
            return string.Empty;
        }
    }
}