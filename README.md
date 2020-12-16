ZopfliSharp
===========

[![Test status](https://ci.appveyor.com/api/projects/status/wkt98fd86l4v9xlt?svg=true)](https://ci.appveyor.com/project/koturn/zopflisharp "AppVeyor | koturn/ZopfliSharp")

A P/Invoke library for [google/zopfli](https://github.com/google/zopfli "google/zopfli") (zopfli.dll and zopflipng.dll).


## Build

```shell
> msbuild /nologo /m /t:restore /p:Configuration=Release;Platform="Any CPU" ZopfliSharp.sln
> msbuild /nologo /m /p:Configuration=Release;Platform="Any CPU" ZopfliSharp.sln
```


## Dependent Libraries

- [google/zopfli](https://github.com/google/zopfli "google/zopfli")
    - But this repository doesn't contain zopfli.dll and zopflipng.dll.


## LICENSE

This software is released under the MIT License, see [LICENSE](LICENSE "LICENSE").
