Koturn.Zopfli
=============

[![.NET](https://github.com/koturn/Koturn.Zopfli/actions/workflows/dotnet.yml/badge.svg?branch=main)](https://github.com/koturn/Koturn.Zopfli/actions/workflows/dotnet.yml)

[![Test status](https://ci.appveyor.com/api/projects/status/19lmdvo61hc385l0/branch/main?svg=true)](https://ci.appveyor.com/project/koturn/koturn-zopfli "AppVeyor | koturn/Koturn.Zopfli")

A P/Invoke library for [google/zopfli](https://github.com/google/zopfli "google/zopfli") (zopfli.dll and zopflipng.dll).


## Supported Frameworks

- .NET Standard 2.0
- .NET 6.0
- .NET 8.0
- .NET 9.0


## Dependent Libraries

### Submodules

- [google/zopfli](https://github.com/google/zopfli "google/zopfli")

### NuGet Packages

- [System.Memory 4.6.3](https://www.nuget.org/packages/system.memory/ "NuGet Gallery | System.Memory 4.6.3")
    - .NET Standard 2.0 only


## Build

```shell
> nmake restore
> nmake
```


## Deploy

```shell
> nmake deploy
```


## Sample Code

### Compress as GZIP format

In addition to the gzip format, the zlib and deflate formats can also be specified.

```cs

using System;
using System.Diagnostics;
using System.IO;
using Koturn.Zopfli;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli.Sample
{
    /// <summary>
    /// An entry point of this program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// An sample code of GZIP compression.
        /// </summary>
        public static void Main()
        {
            const string filePath = "Sample.txt";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var compressedData = Zopfli.Compress(data, ZopfliFormat.GZip);

            Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

            File.WriteAllBytes(filePath + ".gz", compressedData);
        }
    }
}
```

### Compress as GZIP format using stream.

```cs
using System;
using System.Diagnostics;
using System.IO;
using Koturn.Zopfli;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli.Sample
{
    /// <summary>
    /// An entry point of this program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// An sample code of GZIP streaming compression.
        /// </summary>
        public static void Main()
        {
            const string filePath = "Sample.txt";
            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            using (var ms = new MemoryStream())
            {
                using (var zs = new ZopfliStream(ms, ZopfliOptions.GetDefault(), ZopfliFormat.GZip))
                using (var fs = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read))
                {
                    fs.CopyTo(zs);
                }
                var recompressedData = ms.ToArray();

                Console.WriteLine($"Compress {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");
            }
        }
    }
}
```

### Optimize PNG image

```cs
using System;
using System.Diagnostics;
using System.IO;
using Koturn.Zopfli;
using Koturn.Zopfli.Enums;


namespace Koturn.Zopfli.Sample
{
    /// <summary>
    /// An entry point of this program.
    /// </summary>
    static class Program
    {
        /// <summary>
        /// An sample code of PNG recompression.
        /// </summary>
        public static void Main()
        {
            const string filePath = "Sample.png";

            var data = File.ReadAllBytes(filePath);

            var sw = Stopwatch.StartNew();
            var compressedData = Zopfli.OptimizePng(data);

            Console.WriteLine($"Optimize {filePath}: Original=[{data.Length}]Bytes Compressed=[{compressedData.Length}]Bytes; Elapsed=[{sw.ElapsedMilliseconds}]ms");

            if (compressedData.Length < data.Length)
            {
                File.WriteAllBytes(filePath, compressedData);
            }
        }
    }
}
```


## Sample Applications

- [koturn/RecompressZip](https://github.com/koturn/RecompressZip "koturn/RecompressZip")
- [koturn/RecompressPng](https://github.com/koturn/RecompressPng "koturn/RecompressPng")


## LICENSE

This software is released under the MIT License, see [LICENSE](LICENSE "LICENSE").
