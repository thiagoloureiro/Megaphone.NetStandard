# MegaPhone NetStandard
A framework to self-register services on Consul.IO, now supporting .NET Core 6+ and .Net Standard

*MegaPhone*

MegaPhone is a lightweight framework to run self-hosting REST services using AspnetCore Web API on top of a Consul. Each service will start by allocating a free port to run on, once the service is started, it will register itself in the local cluster provider.

**MegaPhone.DotnetCore**

[![NuGet](https://buildstats.info/nuget/Megaphone.DotnetCore)](http://www.nuget.org/packages/Megaphone.DotnetCore)

[![Build Status](https://img.shields.io/appveyor/ci/thiagoloureiro/megaphone-netstandard/master.svg)](https://ci.appveyor.com/project/thiagoloureiro/megaphone-netstandard) 

[![Build history](https://buildstats.info/appveyor/chart/thiagoloureiro/megaphone-netstandard)](https://ci.appveyor.com/project/thiagoloureiro/megaphone-netstandard/history)

**Install the Package**

```Install-Package Megaphone.DotnetCore```

```Install-Package Megaphone.DotnetCore.WebApi```

**AppSettings Sample Configuration**
```
  "ConsulConfig": {
    "Host": "localhost",
    "Port": "8500",
    "StatusEndPoint": "status",
    "StatusEndPointFrequencyCheck": "5s"
  }
  ```
