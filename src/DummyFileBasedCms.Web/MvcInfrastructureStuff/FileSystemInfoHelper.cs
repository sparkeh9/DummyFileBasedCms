namespace DummyFileBasedCms.Web.MvcInfrastructureStuff {
    using System;
    using System.IO;
    using Microsoft.Extensions.FileProviders.Physical;

    internal static class FileSystemInfoHelper
    {
        public static bool IsExcluded( FileSystemInfo fileSystemInfo, ExclusionFilters filters )
        {
            return filters != ExclusionFilters.None && ( fileSystemInfo.Name.StartsWith( ".", StringComparison.Ordinal ) && ( filters & ExclusionFilters.DotPrefixed ) != ExclusionFilters.None || fileSystemInfo.Exists &&
                                                         ( ( fileSystemInfo.Attributes & FileAttributes.Hidden ) != (FileAttributes) 0 && ( filters & ExclusionFilters.Hidden ) != ExclusionFilters.None ||
                                                           ( fileSystemInfo.Attributes & FileAttributes.System ) != (FileAttributes) 0 && ( filters & ExclusionFilters.System ) != ExclusionFilters.None ) );
        }
    }
}