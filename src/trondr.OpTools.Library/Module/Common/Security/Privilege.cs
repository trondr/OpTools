#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion
using System;
using Common.Logging;

namespace trondr.OpTools.Library.Module.Common.Security
{
    [CLSCompliant(false)]
    public class Privilege
    {
        /// <summary>  Default constructor. Disabled. </summary>
        // ReSharper disable UnusedMember.Local
        private Privilege() { }
        // ReSharper restore UnusedMember.Local
        /// <summary>  Constructor.</summary>
        ///
        /// <param name="systemName">    Name of the system. </param>
        /// <param name="privilegeId">   Identifier for the privilege. </param>
        /// <param name="attributes">    The attributes. </param>
        public Privilege(string systemName, PrivilegeValue privilegeId, UInt32 attributes)
        {
            _systemName = systemName;
            _privilegeId = privilegeId;
            Attributes = (PrivilegeAttributes)attributes;
        }
        private readonly PrivilegeValue _privilegeId;
        private readonly string _systemName;
        
        /// <summary>
        /// Get privilege attributes
        /// </summary>
        public PrivilegeAttributes Attributes { get; }

        /// <summary>  Gets the name of the privilege. </summary>
        ///
        /// <value> The name of the privilege. </value>
        public PrivilegeName PrivilegeName
        {
            get
            {
                if (!_privilegeName.HasValue)
                {
                    _privilegeName = PrivilegeName.UnKnownPrivilege;
                    var privilegeName = PrivilegeProvider.LookupPrivilegeName(_systemName, _privilegeId);
                    privilegeName.IfSucc(name =>
                    {
                        _privilegeName = (PrivilegeName)Enum.Parse(typeof(PrivilegeName), name);
                    });
                    privilegeName.IfFail(exception =>
                    {                        
                        LogManager.GetLogger<Privilege>().Error($"Failed to get privilege name. {exception.Message}");
                        _privilegeName = PrivilegeName.UnKnownPrivilege;
                    });
                }
                // ReSharper disable once PossibleInvalidOperationException
                return _privilegeName.Value;
            }
        }
        private PrivilegeName? _privilegeName;

        public override string ToString()
        {
            return PrivilegeName.ToString();
        }

    }
}