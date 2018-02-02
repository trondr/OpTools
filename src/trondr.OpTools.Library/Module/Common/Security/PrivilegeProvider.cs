#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Text;
using Common.Logging;
using LanguageExt;

namespace trondr.OpTools.Library.Module.Common.Security
{
    [CLSCompliant(false)]
    public class PrivilegeProvider
    {
        /// <summary>
        /// Lookup privilege value
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="privilegeName"></param>
        /// <returns></returns>
        public static Result<PrivilegeValue> LookupPrivilegeValue(string systemName, PrivilegeName privilegeName)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || !CheckEntryPoint("advapi32.dll", "LookupPrivilegeValueA"))
            {
                return new Result<PrivilegeValue>(new PrivilegeException("Failed to lookup privilege value. LookupPrivilegeValue() is not supported."));
            }

            PrivilegeValue privilegePrivilegeValue = new PrivilegeValue();
            if (Advapi32.LookupPrivilegeValue(systemName, privilegeName.ToString(), ref privilegePrivilegeValue) == 0)
            {
                return new Result<PrivilegeValue>(new PrivilegeException($"Failed to lookup privilege value for privilege '{privilegeName}'. Win32 error: {User32.FormatError(Marshal.GetLastWin32Error())}"));
            }
            return privilegePrivilegeValue;
        }

        /// <summary>
        /// Look up privilege name
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="privilegeValue"></param>
        /// <returns></returns>
        public static Result<string> LookupPrivilegeName(string systemName, PrivilegeValue privilegeValue)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || !CheckEntryPoint("advapi32.dll", "LookupPrivilegeNameA"))
            {
                return new Result<string>(new PrivilegeException("Failed to lookup privilege name. LookupPrivilegeName() is not supported."));
            }

            var name = new StringBuilder();
            var nameLength = 0;
            var ptrLuid = IntPtr.Zero;
            try
            {
                ptrLuid = Marshal.AllocHGlobal(Marshal.SizeOf(privilegeValue));
                Marshal.StructureToPtr(privilegeValue, ptrLuid, true);
                Advapi32.LookupPrivilegeName(systemName, ptrLuid, null, ref nameLength); // Call once to get the name length
                if (nameLength == 0)
                {
                    return new Result<string>(new PrivilegeException("Failed to lookup privilege name. The specified LocalyUniquePrivilegeValue resulted in a privilege name lenght of size 0. LocalyUniquePrivilegeValue is most probably invalid. Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                }
                name.EnsureCapacity(nameLength + 1);
                if (!Advapi32.LookupPrivilegeName(null, ptrLuid, name, ref nameLength)) // call again to get the actual privilege name
                {
                    return new Result<string>(new PrivilegeException("Failed to lookup privilege name (last call). Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                }
                return name.ToString();
            }
            finally
            {
                Marshal.FreeHGlobal(ptrLuid);
            }
        }

        /// <summary>
        /// Look up privilege name
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="name"></param>
        /// <param name="languageId"></param>
        /// <returns></returns>
        public static Result<string> LookupPrivilegeDisplayName(string systemName, string name, ref int languageId)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || !CheckEntryPoint("advapi32.dll", "LookupPrivilegeDisplayNameA"))
            {
                throw new PrivilegeException("Failed to lookup privilege displayName. LookupPrivilegeDisplayName() is not supported.");
            }

            StringBuilder displayName = new StringBuilder();
            int displayNameLength = 0;
            languageId = 0;
            Advapi32.LookupPrivilegeDisplayName(systemName, name, displayName, ref displayNameLength, ref languageId); // Call once to get the display name length
            if (displayNameLength == 0)
            {
                return new Result<string>(new PrivilegeException(string.Format("Failed to lookup privilege display name. The specified privilege name '{0}' resulted in a privilege display name lenght of size 0. Privilege name '{0}' is most probably invalid. Win32 error: {1}", name, User32.FormatError(Marshal.GetLastWin32Error()))));
            }
            displayName.EnsureCapacity(displayNameLength + 1);
            if (!Advapi32.LookupPrivilegeDisplayName(null, name, displayName, ref displayNameLength, ref languageId)) // call again to get the actual privilege display name
            {
                return new Result<string>(new PrivilegeException($"Failed to lookup privilege display name for privilege name '{name}'. Win32 error: {User32.FormatError(Marshal.GetLastWin32Error())}"));
            }
            return displayName.ToString();
        }

        /// <summary>
        /// Adjust token privileges for specified process
        /// </summary>
        /// <param name="process"></param>
        /// <param name="disableAllPrivileges"></param>
        /// <param name="newState"></param>
        /// <returns>Previous state</returns>
        public static Result<Privileges> AdjustTokenPrivileges(Process process, bool disableAllPrivileges, Privileges newState)
        {
            if (Environment.OSVersion.Platform != PlatformID.Win32NT || !CheckEntryPoint("advapi32.dll", "AdjustTokenPrivileges"))
            {
                return new Result<Privileges>(new PrivilegeException("Failed to adjust token privilege. AdjustTokenPrivileges() is not supported."));
            }
            var tokenHandle = IntPtr.Zero;
            try
            {
                if (Advapi32.OpenProcessToken(process.Handle, (int)(TokenAccessRights.TokenAdjustPrivileges | TokenAccessRights.TokenQuery), ref tokenHandle) == 0)
                {
                    return new Result<Privileges>(new PrivilegeException("Failed to open process token. Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                }
                var previousState = new Privileges();
                
                var newStateSize = sizeof(int) + (sizeof(int) * 2 + sizeof(UInt32)) * newState.PrivilegeCount;
                // ReSharper disable once UnusedVariable
                if (Advapi32.AdjustTokenPrivileges(tokenHandle, disableAllPrivileges, ref newState, newStateSize, ref previousState, out int previousStateSize) == 0)
                {
                    return new Result<Privileges>(new PrivilegeException("Failed to enable token privilege. Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                }
                return previousState;
            }
            finally
            {
                UnManagedHelper.CloseHandle(tokenHandle);
            }
        }

        /// <summary>
        /// Get privileges of the specified process
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="process"></param>
        /// <returns></returns>
        public static Result<List<Privilege>> GetPrivileges(string systemName, Process process)
        {
            IntPtr tokenHandle = IntPtr.Zero;
            try
            {
                List<Privilege> privilegeList = new List<Privilege>();
                //Get process token            
                if (Advapi32.OpenProcessToken(process.Handle, (int)(TokenAccessRights.TokenAdjustPrivileges | TokenAccessRights.TokenQuery), ref tokenHandle) == 0)
                {
                    return new Result<List< Privilege>>(new PrivilegeException("Failed to open process token. Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                }
                // Get length required for Privileges by specifying buffer length of 0
                Advapi32.GetTokenInformation(tokenHandle, TokenInformationClass.TokenPrivileges, IntPtr.Zero, 0, out var privilegesSize);
                var lastError = Marshal.GetLastWin32Error();
                if (lastError == WinError.InsufficientBuffer)
                {
                    var privilegesBuffer = IntPtr.Zero;
                    try
                    {
                        privilegesBuffer = Marshal.AllocHGlobal((int)privilegesSize);
                        if (!Advapi32.GetTokenInformation(tokenHandle, TokenInformationClass.TokenPrivileges, privilegesBuffer, privilegesSize, out privilegesSize))
                        {
                            return new Result<List<Privilege>>(new PrivilegeException("Failed to get token information. Win32 error: " + User32.FormatError(Marshal.GetLastWin32Error())));
                        }
                        var privileges = (Privileges)Marshal.PtrToStructure(privilegesBuffer, typeof(Privileges));
                        if (privileges.PrivilegeCount > PrivilegeConstants.MaxPrivilegeCount)
                        {
                            return new Result<List<Privilege>>(new PrivilegeException(
                                $"Number of privileges exceeds hardcoded maximum of {PrivilegeConstants.MaxPrivilegeCount}. Max size must be set to greater or equal to {privileges.PrivilegeCount} and code be recompiled by a developer to fix this problem."));
                        }
                        for (int i = 0; i < privileges.PrivilegeCount; i++)
                        {
                            var privilege = new Privilege(systemName, privileges.PrivilegeValueAndAttributesArray[i].PrivilegeValue, privileges.PrivilegeValueAndAttributesArray[i].Attributes);
                            privilegeList.Add(privilege);
                        }
                    }
                    finally
                    {
                        Marshal.FreeHGlobal(privilegesBuffer);
                    }
                }
                return privilegeList;
            }
            finally
            {
                UnManagedHelper.CloseHandle(tokenHandle);
            }
        }

        /// <summary>
        /// Check if specifed process has a privilege
        /// </summary>
        /// <param name="systemName"></param>
        /// <param name="process"></param>
        /// <param name="privilegeName"></param>
        public static bool HasPrivilege(string systemName, Process process, PrivilegeName privilegeName)
        {
            var hasPrivilege = false;
            var privileges = GetPrivileges(systemName, process);
            privileges.IfSucc(list =>
            {
                foreach (var privilege in list)
                {
                    if (privilege.PrivilegeName == privilegeName)
                    {
                        if ((privilege.Attributes & PrivilegeAttributes.PrivilegeEnabled) > 0)
                        {
                            hasPrivilege = true; break;
                        }
                        hasPrivilege = false; break;
                    }
                }
            });
            privileges.IfFail(exception => { LogManager.GetLogger(typeof(PrivilegeProvider)).Error($"Failed to process '{process.ProcessName}' has privilege '{privilegeName}'. {exception.Message}"); });
            return hasPrivilege;
        }
        /// <summary>  Query if a given privilege exists in the access token of the specified process. </summary>
        ///
        /// <param name="systemName">    . </param>
        /// <param name="process">       . </param>
        /// <param name="privilegeName"> . </param>
        ///
        /// <returns>  true if it succeeds, false if it fails. </returns>
        public static bool PrivilegeExists(string systemName, Process process, PrivilegeName privilegeName)
        {
            var privileges = GetPrivileges(systemName, process);
            var privilegeExists = false;
            privileges.IfSucc(list =>
            {
                foreach (var privilege in list)
                {
                    if (privilege.PrivilegeName == privilegeName)
                    {
                        privilegeExists = true; break;
                    }
                }
            });
            privileges.IfFail(exception => { LogManager.GetLogger(typeof(PrivilegeProvider)).Error($"Failed to check if privilege '{privilegeName}' exists. {exception.Message}"); });
            return privilegeExists;
        }

        /// <summary>
        /// Enable privilege
        /// </summary>      
        /// <param name="process"></param>
        /// <param name="privilegeName"></param>
        /// <returns>Previous state for the privilege</returns>
        public static Result<Privileges> EnablePrivilege(Process process, PrivilegeName privilegeName)
        {
            if (!PrivilegeExists(null, process, privilegeName))
            {
                return new Result<Privileges>( new PrivilegeException($"Failed to enable privilege '{privilegeName.ToString()}' because the privilege does not exist in the access token of the current process. Admin privileges for the process might be required."));
            }

            Privileges newState = new Privileges
            {
                PrivilegeCount = 1,
                PrivilegeValueAndAttributesArray = new PrivilegeValueAndAttributes[PrivilegeConstants.MaxPrivilegeCount]
            };
            newState.PrivilegeValueAndAttributesArray[0].Attributes = (uint)PrivilegeAttributes.PrivilegeEnabled;
            var lookupPrivilegeValue = LookupPrivilegeValue(null, privilegeName);
            var result = new Result<Privileges>();
            lookupPrivilegeValue.IfSucc(value =>
            {
                newState.PrivilegeValueAndAttributesArray[0].PrivilegeValue = value;
                result = AdjustTokenPrivileges(process, false, newState);
            });
            lookupPrivilegeValue.IfFail(exception =>
            {
                result = new Result<Privileges>(exception);
            });
            return result;
        }

        /// <summary>
        /// Disable privilege
        /// </summary>
        /// <param name="process"></param>
        /// <param name="privilegeName"></param>
        /// <returns>Previous state for the privilege</returns>
        public static Result<Privileges> DisablePrivilege(Process process, PrivilegeName privilegeName)
        {
            if (!PrivilegeExists(null, process, privilegeName))
            {
                return new Result<Privileges>(new PrivilegeException($"Failed to disable privilege '{privilegeName.ToString()}' because the privilege does not exist in the access token of the current process."));
            }

            var newState = new Privileges
            {
                PrivilegeCount = 1,
                PrivilegeValueAndAttributesArray = new PrivilegeValueAndAttributes[PrivilegeConstants.MaxPrivilegeCount]
            };

            var lookupPrivilegeValue = LookupPrivilegeValue(null, privilegeName);
            newState.PrivilegeValueAndAttributesArray[0].Attributes = (uint)PrivilegeAttributes.PrivilegeDisabled;
            Result<Privileges> result = new Result<Privileges>();
            lookupPrivilegeValue.IfSucc(value =>
            {
                newState.PrivilegeValueAndAttributesArray[0].PrivilegeValue = value;
                result = AdjustTokenPrivileges(process, false, newState);
            });
            lookupPrivilegeValue.IfFail(exception =>
            {
                result = new Result<Privileges>(exception);
            });
            
            return result;
        }

        /// <summary>
        /// Checks whether a specified method exists on the local computer.
        /// </summary>
        /// <param name="library">The library that holds the method.</param>
        /// <param name="method">The entry point of the requested method.</param>
        /// <returns>True if the specified method is present, false otherwise.</returns>
        internal static bool CheckEntryPoint(string library, string method)
        {
            IntPtr libPtr = Kernel32.LoadLibrary(library);
            try
            {
                if (!libPtr.Equals(IntPtr.Zero))
                {
                    if (!Kernel32.GetProcAddress(libPtr, method).Equals(IntPtr.Zero))
                    {
                        return true;
                    }
                }
                return false;
            }
            finally
            {
                if (libPtr != IntPtr.Zero)
                    Kernel32.FreeLibrary(libPtr);
            }
        }


    }
}