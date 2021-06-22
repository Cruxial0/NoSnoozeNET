using System;
using System.IO;

namespace NoSnoozeNET.Extensions.IO
{
    public static class DirectoryExt
    {
        public static void CreateIfNotExist(string filePath)
        {
            var directoryName = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryName))
                Directory.CreateDirectory(directoryName ?? throw new InvalidOperationException());
        }
    }
}
