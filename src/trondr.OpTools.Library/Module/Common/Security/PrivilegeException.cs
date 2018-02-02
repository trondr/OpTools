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
    /// <summary>
    /// The exception that is thrown when an error occures when requesting a specific privilege.
    /// </summary>
    public class PrivilegeException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the PrivilegeException class.
        /// </summary>
        public PrivilegeException() : base() { }
        /// <summary>
        /// Initializes a new instance of the PrivilegeException class with a specified error message.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public PrivilegeException(string message) : base(message) { }
    }
}