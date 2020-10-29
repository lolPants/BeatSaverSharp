# 🎵 BeatSaverSharp &nbsp;[![Actions Status](https://github.com/lolPants/BeatSaverSharp/workflows/.NET%20Build/badge.svg)](https://github.com/lolPants/BeatSaverSharp/actions)
> Official C# Library to interact with the BeatSaver API

## 💾 Download
This package is available on [NuGet](https://www.nuget.org/packages/BeatSaverSharp/). It is built targeting .NET Standard 2.0, and should run in any supporting runtime.

Experimental DLLs are also built using [GitHub Actions](https://github.com/lolPants/BeatSaverSharp/actions) for each push to this repository. **These are not guaranteed to be stable, so use them at your own risk.**  
You can find the download as an artifact on any passing builds.

## 🔧 Usage
XML Documentation has been provided for this library. Most tasks will be done through the `BeatSaver` class.

```csharp
using System;
using BeatSaverSharp;

// Setup the client's HTTP User Agent 
HttpOptions options = new HttpOptions(name: "Test Client", version: new Version(1, 0, 0));

// Use this to interact with the API
BeatSaver beatsaver = new BeatSaver(options);
```

## 🚀 Async Enumerable Extensions
This project also targets .NET Standard 2.1. In addition to some compiler optimisations, this contains extra methods that enable the use of `IAsyncEnumerable<Beatmap>` for all methods that return a `Task<Page>`. If you are using a supported runtime, you can grab this DLL from GitHub Actions CI or directly using the NuGet Package.
