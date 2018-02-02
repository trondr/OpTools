#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion
using System;
using System.ComponentModel;

namespace trondr.OpTools.Library.Module.Common.Security
{
    public class WinError
    {
        #region Constants
        public const int InsufficientBuffer = 0x7a;
        #endregion


        #region Methods
        /// <summary>
        /// Get Win32 error text
        /// </summary>
        /// <param name="errorCode"></param>
        /// <returns></returns>
        public string GetWin32ErrorText(int errorCode)
        {
            try
            {
                throw new Win32Exception(errorCode);
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}