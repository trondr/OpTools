#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System.Runtime.InteropServices;

namespace trondr.OpTools.Library.Module.Common.Security
{
    /// <summary>
    /// An privilege value is a 64-bit value guaranteed to be unique only on the system on which it was generated and uniqueness is guaranteed only until the system is restarted.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct PrivilegeValue
    {
        /// <summary>
        /// The low order part of the 64 bit value.
        /// </summary>
        public int LowPart;

        ///<summary>
        /// The high order part of the 64 bit value.
        /// </summary>
        public int HighPart;
    }
}