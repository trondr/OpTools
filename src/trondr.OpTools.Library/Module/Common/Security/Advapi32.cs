#region License
//License: New BSD License (BSD) http://www.opensource.org/licenses/BSD-3-Clause
//Project Home: http://baseservices.kilnhg.com/
//Credits: MSDN documentation and pinvoke.net
//Copyright (c) <trondr@outlook.com> 2012
//All rights reserved.
#endregion

using System;
using System.Runtime.InteropServices;
using System.Text;

namespace trondr.OpTools.Library.Module.Common.Security
{
    public class Advapi32
    {
        /// <summary>
        /// The LookupPrivilegeValue function retrieves the locally unique identifier (LUID) used on a specified system to locally represent the specified privilege name.
        /// </summary>
        /// <param name="lpSystemName">Pointer to a null-terminated string specifying the name of the system on which the privilege name is looked up. If a null string is specified, the function attempts to find the privilege name on the local system.</param>
        /// <param name="lpName">Pointer to a null-terminated string that specifies the name of the privilege, as defined in the Winnt.h header file. For example, this parameter could specify the constant SE_SECURITY_NAME, or its corresponding string, "SeSecurityPrivilege".</param>
        /// <param name="lpPrivilegeValue">Pointer to a variable that receives the locally unique identifier by which the privilege is known on the system, specified by the lpSystemName parameter.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error.</br></returns>
        [DllImport("advapi32.dll", EntryPoint = "LookupPrivilegeValueA", CharSet = CharSet.Ansi)]
        public static extern int LookupPrivilegeValue(string lpSystemName, string lpName, ref PrivilegeValue lpPrivilegeValue);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeName(string lpSystemName, IntPtr lpLuid, StringBuilder lpName, ref int cchName);

        [DllImport("advapi32.dll", SetLastError = true, CharSet = CharSet.Auto)]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool LookupPrivilegeDisplayName(string lpSystemName, string lpName, StringBuilder lpDisplayName, ref int cchDisplayName, ref int lpLanguageId);

        /// <summary>
        /// The OpenProcessToken function opens the access token associated with a process.
        /// </summary>
        /// <param name="processHandle">Handle to the process whose access token is opened.</param>
        /// <param name="desiredAccess">Specifies an access mask that specifies the requested types of access to the access token. These requested access types are compared with the token's discretionary access-control list (DACL) to determine which accesses are granted or denied.</param>
        /// <param name="tokenHandle">Pointer to a handle identifying the newly-opened access token when the function returns.</param>
        /// <returns>If the function succeeds, the return value is nonzero.<br></br><br>If the function fails, the return value is zero. To get extended error information, call Marshal.GetLastWin32Error.</br></returns>
        [DllImport("advapi32.dll", EntryPoint = "OpenProcessToken", CharSet = CharSet.Ansi)]
        public static extern int OpenProcessToken(IntPtr processHandle, int desiredAccess, ref IntPtr tokenHandle);

        /// <summary>
        /// The AdjustTokenPrivileges function enables or disables privileges in the specified access token. Enabling or disabling privileges in an access token requires TOKEN_ADJUST_PRIVILEGES access.
        /// </summary>
        /// <param name="tokenHandle">Handle to the access token that contains the privileges to be modified. The handle must have TOKEN_ADJUST_PRIVILEGES access to the token. If the PreviousState parameter is not NULL, the handle must also have TOKEN_QUERY access.</param>
        /// <param name="disableAllPrivileges">Specifies whether the function disables all of the token's privileges. If this value is TRUE, the function disables all privileges and ignores the NewState parameter. If it is FALSE, the function modifies privileges based on the information pointed to by the NewState parameter.</param>
        /// <param name="newState">Pointer to a TOKEN_PRIVILEGES structure that specifies an array of privileges and their attributes. If the DisableAllPrivileges parameter is FALSE, AdjustTokenPrivileges enables or disables these privileges for the token. If you set the SE_PRIVILEGE_ENABLED attribute for a privilege, the function enables that privilege; otherwise, it disables the privilege. If DisableAllPrivileges is TRUE, the function ignores this parameter.</param>      
        /// <param name="bufferLength">Specifies the size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be zero if the PreviousState parameter is NULL.</param>
        /// <param name="previousState">Pointer to a buffer that the function fills with a TOKEN_PRIVILEGES structure that contains the previous state of any privileges that the function modifies. This parameter can be NULL.</param>      
        /// <param name="returnLength">Pointer to a variable that receives the required size, in bytes, of the buffer pointed to by the PreviousState parameter. This parameter can be NULL if PreviousState is NULL.</param>
        /// <returns>If the function succeeds, the return value is nonzero. To determine whether the function adjusted all of the specified privileges, call Marshal.GetLastWin32Error.</returns>
        /*    
           BOOL WINAPI AdjustTokenPrivileges(
           __in       HANDLE TokenHandle,
           __in       BOOL DisableAllPrivileges,
           __in_opt   PTOKEN_PRIVILEGES NewState,
           __in       DWORD BufferLength,
           __out_opt  PTOKEN_PRIVILEGES PreviousState,
           __out_opt  PDWORD ReturnLength
           );
         */
        [DllImport("advapi32.dll", EntryPoint = "AdjustTokenPrivileges", CharSet = CharSet.Ansi, SetLastError = true)]
        public static extern int AdjustTokenPrivileges([In] IntPtr tokenHandle, [In] [MarshalAs(UnmanagedType.Bool)]bool disableAllPrivileges, [In] ref Privileges newState, [In] int bufferLength, [Out, In] ref Privileges previousState, [Out] out int returnLength);

        /// <summary>
        /// The GetTokenInformation function retrieves a specified type of information about an access token. The calling process must have appropriate access rights to obtain the information. For more information see http://msdn.microsoft.com/en-us/library/aa446671(v=VS.85).aspx
        /// </summary>
        /// <param name="tokenHandle"></param>
        /// <param name="tokenInformationClass"></param>
        /// <param name="tokenInformation"></param>
        /// <param name="tokenInformationLength"></param>
        /// <param name="returnLength"></param>
        /// <returns></returns>
        /*
         BOOL WINAPI GetTokenInformation(
          __in       HANDLE TokenHandle,
          __in       TOKEN_INFORMATION_CLASS TokenInformationClass,
          __out_opt  LPVOID TokenInformation,
          __in       DWORD TokenInformationLength,
          __out      PDWORD ReturnLength
         );
        */
        [DllImport("advapi32.dll", SetLastError = true)]
        public static extern bool GetTokenInformation([In] IntPtr tokenHandle, [In]TokenInformationClass tokenInformationClass, [Out] IntPtr tokenInformation, [In] uint tokenInformationLength, [Out] out uint returnLength);
    }
}