#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion
using System;

namespace trondr.OpTools.Library.Module.Common.Security
{
    [CLSCompliant(false)]
    [Flags]
    public enum PrivilegeAttributes : uint
    {
        PrivilegeDisabled = 0x00000,
        PrivilegeEnabledByDefault = 0x00001,
        PrivilegeEnabled = 0x00002,
        PrivilegeUsedForAccess = 0x80000000
    }
}