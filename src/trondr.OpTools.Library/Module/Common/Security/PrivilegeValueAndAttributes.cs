#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion
using System;
using System.Runtime.InteropServices;

namespace trondr.OpTools.Library.Module.Common.Security
{
    ///<summary>
    /// The PrivilegeValueAndAttributes structure represents a locally unique privilge value and its attributes.
    /// </summary>
    [CLSCompliant(false)]
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PrivilegeValueAndAttributes
    {
        /// <summary>
        /// Specifies an locally unique privilege value.
        /// </summary>
        public PrivilegeValue PrivilegeValue;
        /// <summary>
        /// Specifies attributes of the locally unique privilege value. This value contains up to 32 one-bit flags. Its meaning is dependent on the definition and use of the locally unique privilege value.
        /// </summary>
        public UInt32 Attributes;
    }
}