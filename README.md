# ![AnnotationServiceBuilder Icon](https://github.com/genryianchev/AnnotationServiceBuilder/raw/main/AnnotationServiceBuilder/icon.png) AnnotationServiceBuilder

Annotation Service Builder is an ASP.NET library that simplifies dependency injection by using custom annotations to automatically register services in the DI container.

## Table of Contents

1. [Prerequisites](#prerequisites)
   - [Web Application](#web-application)
   - [Important Note](#important-note)
2. [Setting up Annotations](#setting-up-annotations)
3. [Installing AnnotationServiceBuilder](#installing-annotationservicebuilder)
   - [Step 1: Install the AnnotationServiceBuilder NuGet Package](#step-1-install-the-annotationservicebuilder-nuget-package)
     - [Using .NET Core CLI](#using-net-core-cli)
4. [Usage](#usage)
   - [1. Using Scoped Services](#1-using-scoped-services)
   - [2. Using Singleton Services](#2-using-singleton-services)
   - [3. Using Transient Services](#3-using-transient-services)
   - [4. Example of a Refit Client](#4-example-of-a-refit-client)
   - [5. Example with Different `baseUrl` for Multiple Refit Clients](#5-example-with-different-baseurl-for-multiple-refit-clients)
5. [Trimming Safety Considerations](#trimming-safety-considerations)
6. [ASB: Observing the Logic of Pattern Standards](#asb-observing-the-logic-of-pattern-standards)
7. [Benefits of Using AnnotationServiceBuilder](#benefits-of-using-annotationservicebuilder)

---

## Prerequisites

### Web Application

1. **.NET 6.0 or later**
2. **Visual Studio 2022** (or any other IDE with .NET support)

### Important Note

Ensure that your .NET SDK is up-to-date. This project requires .NET 6.0 or later.

## Setting up Annotations

The `Annotations` folder in the project contains the key attributes used for automatically registering services in the DI container. Below are the files and their purposes, along with examples of how they are used.

---

## Installing AnnotationServiceBuilder

### Step 1: Install the AnnotationServiceBuilder NuGet Package

You can install the AnnotationServiceBuilder package via NuGet.

#### Using .NET Core CLI

```bash
dotnet add package AnnotationServiceBuilder
```

---

## Usage

### Registering Services with `AnnotationServiceRegistrar`

To register your services, follow these steps:

1. Initialize the `AnnotationServiceRegistrar`:

```csharp
AnnotationServiceRegistrar.Initialize(Assembly.GetExecutingAssembly());
```

2. Add your services to the DI container:

```csharp
AnnotationServiceRegistrar.AddSingletonServices(services);
AnnotationServiceRegistrar.AddScopedServices(services);
AnnotationServiceRegistrar.AddTransientServices(services);
AnnotationServiceRegistrar.AddRefitClients(services, "https://api.yourservice.com"); // Replace with your API base URL
```

If you need to use a custom `DelegatingHandler`, use the following:

```csharp
var customHandler = new MyCustomHandler();
AnnotationServiceRegistrar.AddRefitClients(services, "https://api.yourservice.com", customHandler);
```

---

## Other Examples

### 1. Using Scoped Services

```csharp
using AnnotationServiceBuilder.Annotations.Scoped;

[ScopedService(typeof(IMyScopedService))]
public class MyScopedService : IMyScopedService
{
    // Implementation...
}
```

### 2. Using Singleton Services

```csharp
using AnnotationServiceBuilder.Annotations.Singleton;

[SingletonService]
public class MySingletonService
{
    // Implementation...
}
```

### 3. Using Transient Services

```csharp
using AnnotationServiceBuilder.Annotations.Transient;

[TransientService]
public class MyTransientService
{
    // Implementation...
}
```

### 4. Example of a Refit Client

```csharp
using AnnotationServiceBuilder.Annotations.Refit;
using AnnotationServiceBuilder.Data.Models;
using Refit;

namespace AnnotationServiceBuilder.Network.Repositories
{
    [RefitClient]
    public interface IPostsApi
    {
        [Get("/posts")]
        Task<List<Post>> GetPostsAsync();

        [Get("/posts/{id}")]
        Task<Post> GetPostByIdAsync(int id);
    }
}
```

### 5. Example with Different `baseUrl` for Multiple Refit Clients

```csharp
using AnnotationServiceBuilder.Annotations.Refit;
using Refit;

namespace AnnotationServiceBuilder.Network.Repositories
{
    [RefitClient(BaseUrl = "https://api.service1.com")]
    public interface IService1Api
    {
        [Get("/endpoint1")]
        Task<List<ResponseModel1>> GetService1DataAsync();
    }

    [RefitClient(BaseUrl = "https://api.service2.com")]
    public interface IService2Api
    {
        [Get("/endpoint2")]
        Task<List<ResponseModel2>> GetService2DataAsync();
    }

    [RefitClient(BaseUrl = "https://api.service3.com")]
    public interface IService3Api
    {
        [Get("/endpoint3")]
        Task<List<ResponseModel3>> GetService3DataAsync();
    }
}
```

---

## Trimming Safety Considerations

To ensure that your services are not trimmed during the optimization process, use the trimming-safe registration methods provided by **AnnotationServiceBuilder**. These methods will guarantee that necessary types are preserved and registered correctly.

```csharp
AnnotationServiceRegistrar.Initialize(Assembly.GetExecutingAssembly());

AnnotationServiceRegistrar.AddSingletonServicesWithTrimmingSafety(services);
AnnotationServiceRegistrar.AddScopedServicesWithTrimmingSafety(services);
AnnotationServiceRegistrar.AddTransientServicesWithTrimmingSafety(services);
AnnotationServiceRegistrar.AddRefitClientsWithTrimmingSafety(services, "https://api.yourservice.com"); // Replace with your API base URL
```

### Manual Trimming Considerations

If this approach doesn't help, you may try to manually apply trimming safety considerations.

#### Example of Using `DynamicDependency`

```csharp
using System.Diagnostics.CodeAnalysis;

[SingletonService]
public class StockPartsService
{
    [DynamicDependency(DynamicallyAccessedMemberTypes.All, typeof(MyDependentService))]
    public StockPartsService()
    {
        // Method implementation ensuring MyDependentService is retained
    }
}
```

#### Example of Using `Preserve`

```csharp
using System.Runtime.CompilerServices;

[Preserve]
[SingletonService]
public class MyDependentService
{
    public void PerformOperation()
    {
        // Implementation that must be retained during trimming
    }
}
```

---

## ASB: Observing the Logic of Pattern Standards

> **Note**: Factory patterns have been moved to a new library, **AnnotationServiceBuilder.Patterns**. Please update your code accordingly to use the new library. For more details, see: [AnnotationServiceBuilder.Patterns](https://github.com/genryianchev/AnnotationServiceBuilder.Patterns).

---

## Benefits of Using AnnotationServiceBuilder

### 1. Automation of Service Registration

Automatically register your services in the DI container without needing to manually add each service in `Startup.cs` or `Program.cs`. This reduces boilerplate code and makes your setup process much more streamlined.

### 2. Clear and Organized Codebase

Annotations define the lifetime of services (Singleton, Scoped, Transient), making your code more organized and easier to maintain.

### 3. Time Efficiency

Automating service registration saves time, especially in large projects. Developers can focus on building features instead of managing service registrations manually.

### 4. Ease of Use

The library provides a simple, intuitive API for registering services and Refit clients, making the process user-friendly.

### 5. Caching for Performance

Registered classes and interfaces are cached to improve performance and reduce the overhead of repeated reflection operations.

---

## Video Guides

For video guides on how to use AnnotationServiceBuilder, you can watch these YouTube videos:
- [AnnotationServiceBuilder Guide 1](https://www.youtube.com/watch?v=kofPf606OBE)
- [AnnotationServiceBuilder Guide 2](https://www.youtube.com/watch?v=tspUekM_UHg&t=3s)

---

## Additional Resources

- [AnnotationServiceBuilder Examples](https://github.com/genryianchev/AnnotationServiceBuilderExamples)

---

## Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss your ideas or report bugs.

---

## License

This project is licensed under the MIT License.

```text
MIT License

Copyright (c) 2024 Gennadii Ianchev

Permission is hereby granted, free of charge, to any person obtaining a copy
of this software and associated documentation files (the "Software"), to deal
in the Software without restriction, including without limitation the rights
to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
copies of the Software, and to permit persons to whom the Software is
furnished to do so, subject to the following conditions:

The above copyright notice and this permission notice shall be included in all
copies or substantial portions of the Software.

THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
SOFTWARE.
```

