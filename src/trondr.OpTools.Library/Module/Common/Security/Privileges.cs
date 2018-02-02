#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using System.Runtime.InteropServices;
using trondr.OpTools.Library.Infrastructure;

namespace trondr.OpTools.Library.Module.Common.Security
{
    /// <summary>
    /// The Privileges structure contains information about a set of privileges for an access token.
    /// </summary>
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct Privileges
    {
        static Privileges()
        {
            if (ToDo.IsToDoEnabled) ToDo.Implement(ToDoPriority.Low, "trond", "Implement marshalling of variable size array for the 'PrivilegeValueAndAttributesArray' array. Not sure if this is even possible without some kind of dynamic compilation. For now a max count is used to create a hopefully big enough array on the cost of using more than strictly necessary memory.");
        }
        /// <summary>
        /// Specifies the number of entries in the Privileges array.
        /// </summary>
        public int PrivilegeCount;
        /// <summary>
        /// Specifies an array of PrivilegeValueAndAttributes structures. Each structure contains the Luid and attributes of a privilege.
        /// </summary>            
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = PrivilegeConstants.MaxPrivilegeCount)]
        public PrivilegeValueAndAttributes[] PrivilegeValueAndAttributesArray;

    }
}