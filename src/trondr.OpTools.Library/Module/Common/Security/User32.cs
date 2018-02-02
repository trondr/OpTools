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
    public static class User32
    {
        /// <summary>
        /// The FormatMessage function formats a message string. The function requires a message definition as input. The message definition can come from a buffer passed into the function. It can come from a message table resource in an already-loaded module. Or the caller can ask the function to search the system's message table resource(s) for the message definition. The function finds the message definition in a message table resource based on a message identifier and a language identifier. The function copies the formatted message text to an output buffer, processing any embedded insert sequences if requested.
        /// </summary>
        /// <param name="dwFlags">Specifies aspects of the formatting process and how to interpret the lpSource parameter. The low-order
        /// byte of dwFlags specifies how the function handles line breaks in the output buffer. The low-order byte can
        /// also specify the maximum width of a formatted output line.</param>
        /// <param name="lpSource">Specifies the location of the message definition. The type of this parameter depends
        /// upon the settings in the dwFlags parameter.</param>
        /// <param name="dwMessageId">Specifies the message identifier for the requested message. This parameter is ignored
        /// if dwFlags includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="dwLanguageId">Specifies the language identifier for the requested message. This parameter is ignored
        /// if dwFlags includes FORMAT_MESSAGE_FROM_STRING.</param>
        /// <param name="lpBuffer">Pointer to a buffer for the formatted (and null-terminated) message. If dwFlags includes
        /// FORMAT_MESSAGE_ALLOCATE_BUFFER, the function allocates a buffer using the LocalAlloc function, and places the
        /// pointer to the buffer at the address specified in lpBuffer.</param>
        /// <param name="nSize">If the FORMAT_MESSAGE_ALLOCATE_BUFFER flag is not set, this parameter specifies the maximum
        /// number of TCHARs that can be stored in the output buffer. If FORMAT_MESSAGE_ALLOCATE_BUFFER is set, this parameter specifies
        /// the minimum number of TCHARs to allocate for an output buffer. For ANSI text, this is the number of bytes; for Unicode
        /// text, this is the number of characters.</param>
        /// <param name="arguments">Pointer to an array of values that are used as insert values in the formatted message. A %1 in
        /// the format string indicates the first value in the Arguments array; a %2 indicates the second argument; and so on.</param>
        /// <returns>If the function succeeds, the return value is the number of TCHARs stored in the output buffer,
        /// excluding the terminating null character.<br></br><br>If the function fails, the return value is zero. To get extended
        /// error information, call Marshal.GetLastWin32Error.</br></returns>
        [DllImport("user32.dll", EntryPoint = "FormatMessageA", CharSet = CharSet.Ansi)]
        public static extern int FormatMessage(int dwFlags, IntPtr lpSource, int dwMessageId, int dwLanguageId, StringBuilder lpBuffer, int nSize, int arguments);

        /// <summary>
        /// Formats an error number into an error message.
        /// </summary>
        /// <param name="number">The error number to convert.</param>
        /// <returns>A string representation of the specified error number.</returns>
        public static string FormatError(int number)
        {
            try
            {
                StringBuilder buffer = new StringBuilder(255);
                FormatMessage(FormatMessageFromSystem, IntPtr.Zero, number, 0, buffer, buffer.Capacity, 0);
                return buffer.ToString();
            }
            catch (Exception)
            {
                return "Unspecified error [" + number + "]";
            }
        }
        private const int FormatMessageFromSystem = 0x1000;

    }
}
