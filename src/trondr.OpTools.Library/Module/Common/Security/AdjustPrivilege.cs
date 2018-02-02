#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using System.Diagnostics;

namespace trondr.OpTools.Library.Module.Common.Security
{
    /// <inheritdoc />
    ///  <summary>  Adjust privilege. Privilege is enabled on object construction and reset on object finalization</summary>
    ///  <seealso cref="T:System.IDisposable" />
    public class AdjustPrivilege : IDisposable
    {
        /// <summary>  Constructor. </summary>
        ///
        /// <param name="privilegeName"> Name of the privilege. </param>
        public AdjustPrivilege(PrivilegeName privilegeName)
        {
            _privilegeName = privilegeName;
            _process = Process.GetCurrentProcess();
            var result = PrivilegeProvider.EnablePrivilege(_process, privilegeName);
            result.IfSucc(privileges => { _previousState = privileges; });            
            _isInitialized = true;
        }

        /// <summary>  Finaliser. </summary>
        ~AdjustPrivilege()
        {
            Cleanup();
        }

        private readonly PrivilegeName _privilegeName;
        private readonly Process _process;
        private Privileges _previousState;
        private bool _isInitialized;
        


        /// <summary>  Cleanups this object. </summary>
        protected virtual void Cleanup()
        {
            //Restore previous state
            if (_isInitialized)
            {
                PrivilegeProvider.DisablePrivilege(_process, _privilegeName);
                PrivilegeProvider.AdjustTokenPrivileges(_process, false, _previousState);
            }
        }
        /// <summary>  Gets a value indicating whether the object has been disposed. </summary>
        ///
        /// <value> true if disposed, false if not. </value>
        protected bool Disposed
        {
            get
            {
                return _disposed;
            }
        }
        private bool _disposed;



        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged
        /// resources.
        /// </summary>
        ///
        /// <seealso cref="System.IDisposable.Dispose()"/>
        public void Dispose()
        {
            if (_disposed == false)
            {
                Cleanup();
                _disposed = true;
                GC.SuppressFinalize(this);
            }
        }        
    }
}