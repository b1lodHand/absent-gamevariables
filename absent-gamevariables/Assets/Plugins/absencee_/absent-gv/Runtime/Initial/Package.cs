#define ABSENT_GAMEVARIABLES
#define ABSENT_GAMEVARIABLES_1_1_0

using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Assembly-CSharp-Editor-firstpass")]

namespace com.absence.gamevariables.internals
{
    /// <summary>
    /// The static class responsible for holding the package info.
    /// </summary>
    public static class Package
    {
        /// <summary>
        /// A reference type responsible for holding version information of a package.
        /// </summary>
        public class PackageVersion
        {
            public int Major;
            public int Minor;
            public int Patch;

            public string Text => $"{Major}.{Minor}.{Patch}";
        }

        /// <summary>
        /// Version info of this package.
        /// </summary>
        public static readonly PackageVersion Version = new PackageVersion()
        {
            Major = 1,
            Minor = 1,
            Patch = 0,
        };
    }
}