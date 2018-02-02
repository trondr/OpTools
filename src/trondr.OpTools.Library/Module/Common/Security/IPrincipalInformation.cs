#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion
namespace trondr.OpTools.Library.Module.Common.Security
{
    public interface IPrincipalInformation
    {
        /// <summary>
        /// Check if current principal is administrator
        /// </summary>
        bool IsAdministrator { get; }
    }
}