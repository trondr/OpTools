#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using System.Security.Principal;

namespace trondr.OpTools.Library.Module.Common.Security
{
    class PrincipalInformation : IPrincipalInformation
    {
        #region Implementation of IPrincipalInformation

        /// <summary>
        /// Check if current principal is administrator
        /// </summary>
        public bool IsAdministrator
        {
            get
            {
                if (!_isAdministrator.HasValue)
                {
                    try
                    {
                        WindowsIdentity currentIdentity = WindowsIdentity.GetCurrent();
                        if (currentIdentity != null)
                        {
                            WindowsPrincipal currentPrincipal = new WindowsPrincipal(currentIdentity);
                            _isAdministrator = currentPrincipal.IsInRole(WindowsBuiltInRole.Administrator);
                        }
                        else
                        {
                            _isAdministrator = false;
                        }
                    }
                    catch (Exception)
                    {
                        _isAdministrator = false;
                    }
                }
                return _isAdministrator.Value;
            }
        }
        private bool? _isAdministrator;
        #endregion
    }
}