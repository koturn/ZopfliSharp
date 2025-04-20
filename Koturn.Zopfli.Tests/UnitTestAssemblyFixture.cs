#if NET7_0_OR_GREATER
#    define SUPPORT_LIBRARY_IMPORT
#endif  // NET7_0_OR_GREATER

using Koturn.Zopfli.Tests.Internals;
using System;
using System.IO;
using System.Runtime.InteropServices;
using System.Security;
using Xunit.Extensions.AssemblyFixture;

[assembly: TestFramework(AssemblyFixtureFramework.TypeName, AssemblyFixtureFramework.AssemblyName)]


namespace Koturn.Zopfli.Tests
{
    /// <summary>
    /// Common initialization.
    /// </summary>
#if SUPPORT_LIBRARY_IMPORT
    public partial class UnitTestAssemblyFixture
#else
    public class UnitTestAssemblyFixture
#endif  // SUPPORT_LIBRARY_IMPORT
    {
        /// <summary>
        /// Setup native dll directory.
        /// </summary>
        public UnitTestAssemblyFixture()
        {
            File.WriteAllText("test.txt", AppContext.BaseDirectory);
            var dllDir = Path.Combine(
                AppContext.BaseDirectory,
                Environment.Is64BitProcess ? "x64" : "x86");
            SafeNativeMethods.AddDllDirectory(dllDir);
            SafeNativeMethods.SetDefaultDllDirectories(LoadLibrarySearchFlags.DefaultDirs);
        }

        /// <summary>
        /// Native methods.
        /// </summary>
        [SuppressUnmanagedCodeSecurity]
#if SUPPORT_LIBRARY_IMPORT
        internal static partial class SafeNativeMethods
#else
        internal static class SafeNativeMethods
#endif  // SUPPORT_LIBRARY_IMPORT
        {
            /// <summary>
            /// Adds a directory to the process DLL search path.
            /// </summary>
            /// <param name="path">Path to DLL directory.</param>
            /// <returns>
            /// <para>If the function succeeds, the return value is an opaque pointer that can be passed
            /// to <see href="https://learn.microsoft.com/en-us/windows/desktop/api/libloaderapi/nf-libloaderapi-removedlldirectory">RemoveDllDirectory</see>
            /// to remove the DLL from the process DLL search path.</para>
            /// <para>If the function fails, the return value is zero.
            /// To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</para>
            /// </returns>
            /// <remarks>
            /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-adddlldirectory"/>
            /// </remarks>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(AddDllDirectory), StringMarshalling = StringMarshalling.Utf16, SetLastError = true)]
            public static partial IntPtr AddDllDirectory(string path);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(AddDllDirectory), ExactSpelling = true, CharSet = CharSet.Unicode, SetLastError = true)]
            public static extern IntPtr AddDllDirectory(string path);
#endif  // SUPPORT_LIBRARY_IMPORT
            /// <summary>
            /// Specifies a default set of directories to search when the calling process loads a DLL.
            /// This search path is used when <see href="https://learn.microsoft.com/en-us/windows/desktop/api/libloaderapi/nf-libloaderapi-loadlibraryexa">LoadLibraryEx</see> is called
            /// with no <see cref="LoadLibrarySearchFlags"/> flags.
            /// </summary>
            /// <param name="directoryFlags">The directories to search. This parameter can be any combination of the <see cref="LoadLibrarySearchFlags"/> values.</param>
            /// <returns>
            /// <para>If the function succeeds, the return value is true.</para>
            /// <para>If the function fails, the return value is false. To get extended error information, call <see cref="Marshal.GetLastWin32Error"/>.</para>
            /// </returns>
            /// <remarks>
            /// <see href="https://learn.microsoft.com/en-us/windows/win32/api/libloaderapi/nf-libloaderapi-setdefaultdlldirectories"/>
            /// </remarks>
#if SUPPORT_LIBRARY_IMPORT
            [LibraryImport("kernel32.dll", EntryPoint = nameof(SetDefaultDllDirectories), SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static partial bool SetDefaultDllDirectories(LoadLibrarySearchFlags directoryFlags);
#else
            [DllImport("kernel32.dll", EntryPoint = nameof(SetDefaultDllDirectories), ExactSpelling = true, SetLastError = true)]
            [return: MarshalAs(UnmanagedType.Bool)]
            public static extern bool SetDefaultDllDirectories(LoadLibrarySearchFlags directoryFlags);
#endif  // SUPPORT_LIBRARY_IMPORT
        }
    }
}
