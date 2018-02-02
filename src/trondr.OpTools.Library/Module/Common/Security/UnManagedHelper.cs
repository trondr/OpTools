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
    public static class UnManagedHelper
    {
        /// <summary>  Free unmanaged memory. </summary>
        ///
        /// <param name="intPtrs"> A variable-length parameters list containing int ptrs. </param>
        public static void FreeUnmanagedMemory(params IntPtr[] intPtrs)
        {
            foreach (IntPtr intPtr in intPtrs)
            {
                if (intPtr != IntPtr.Zero)
                {
                    Marshal.FreeHGlobal(intPtr);
                }
            }
        }
        /// <summary>  Closes a handle. </summary>
        ///
        /// <param name="handleIntPtr">  The handle int pointer. </param>
        public static void CloseHandle(IntPtr handleIntPtr)
        {
            if (handleIntPtr != IntPtr.Zero)
            {
                Kernel32.CloseHandle(handleIntPtr);
            }
        }
    }
}