# MegaPhone NetStandard
A framework to self register services on Consul.IO, now supporting .NET Core 2.x and .Net Standard

*MegaPhone*

MegaPhone is a lightweight framework to run self hosting REST services using AspnetCore Web Api ontop of a Consul. Each service will start out by allocating a free port to run on, once the service is started, it will register itself in the local cluster provider.

**MegaPhone.DotnetCore**
[![NuGet](https://buildstats.info/nuget/Megaphone.DotnetCore)](http://www.nuget.org/packages/Megaphone.DotnetCore)

[![Build Status](https://img.shields.io/appveyor/ci/thiagoloureiro/megaphone-netstandard/master.svg)](https://ci.appveyor.com/project/thiagoloureiro/megaphone-netstandard) 

```
AppSettings Configuration:

  "ConsulConfig": {
    "Host": "localhost",
    "Port": "8500",
    "StatusEndPoint": "management/health",
    "StatusEndPointFrequencyCheck": "5s"
  }```
