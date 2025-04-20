using System;


namespace Koturn.Zopfli.Tests.Internals
{
    /// <summary>
    /// Flag values for <see cref="UnitTestAssemblyFixture.SafeNativeMethods.SetDefaultDllDirectories(LoadLibrarySearchFlags)"/>.
    /// </summary>
    [Flags]
    internal enum LoadLibrarySearchFlags
    {
        /// <summary>
        /// If this value is used, the application's installation directory is searched.
        /// </summary>
        ApplicationDir = 0x00000200,
        /// <summary>
        /// <para>If this value is used, any path explicitly added using the AddDllDirectory or SetDllDirectory function is searched.</para>
        /// <para>If more than one directory has been added, the order in which those directories are searched is unspecified.</para>
        /// </summary>
        UserDirs = 0x00000400,
        /// <summary>
        /// If this value is used, %windows%\system32 is searched.
        /// </summary>
        System32 = 0x00000800,
        /// <summary>
        /// <para>This value is a combination of <see cref="ApplicationDir"/>, <see cref="System32"/>, and <see cref="UserDirs"/>.</para>
        /// <para>This value represents the recommended maximum number of directories an application should include in its DLL search path.</para>
        /// </summary>
        DefaultDirs = 0x00001000
    }
}
