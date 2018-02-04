using System.Security.AccessControl;
using System.Security.Principal;

namespace trondr.OpTools.Library.Module.Common
{
    public static class DirectorySecurityExensions
    {
        public static string GetAccessControlSddlForm(this DirectorySecurity directorySecurity, AccessControlSections accessControlSections, bool explicitOnly)
        {
            if (explicitOnly)
            {
                var explicitAccessRules = directorySecurity.GetAccessRules(true, false, typeof(SecurityIdentifier));
                if (explicitAccessRules.Count > 0)
                    return directorySecurity.GetSecurityDescriptorSddlForm(accessControlSections);
            }
            else
            {
                return directorySecurity.GetSecurityDescriptorSddlForm(accessControlSections);
            }
            return string.Empty;
        }
    }
}