
# AnnotationServiceBuilder
Annotation Service Builder is an ASP.NET library that simplifies dependency injection by using custom annotations to automatically register services in the DI container.

## Table of Contents

1. [Prerequisites](#prerequisites)
   - [Web Application](#web-application)
   - [Important Note](#important-note)
2. [Setting up Annotations](#setting-up-annotations)
3. [Installing AnnotationServiceBuilder](#installing-annotationservicebuilder)
   - [Step 1: Install the AnnotationServiceBuilder NuGet Package](#step-1-install-the-annotationservicebuilder-nuget-package)
     - [Using Package Manager Console](#using-package-manager-console)
     - [Using .NET Core CLI](#using-net-core-cli)
   - [Step 2: Set Up Annotations](#step-2-set-up-annotations)
     - [If You're Using before Version 1.0.9](#if-youre-using-before-version-109)
     - [If You're Using Version 1.0.9](#if-youre-using-version-109)
     - [If You're Using Version 1.1.1 or Later](#if-youre-using-version-111-or-later)
4. [Usage](#usage)
   - [1. Using Scoped Services](#1-using-scoped-services)
   - [2. Using Singleton Services](#2-using-singleton-services)
   - [3. Using Transient Services](#3-using-transient-services)
   - [4. Example of a Refit Client](#4-example-of-a-refit-client)
   - [5. Example with Different `baseUrl` for Multiple Refit Clients](#5-example-with-different-baseurl-for-multiple-refit-clients)
5. [Trimming Safety Considerations](#trimming-safety-considerations)
6. [ASB: Observing the Logic of Pattern Standards](#asb-observing-the-logic-of-pattern-standards)
   - [Registering Factory Pattern Services](#registering-factory-pattern-services)
   - [Example Using Factory Pattern](#example-using-factory-pattern)
7. [Benefits of Using AnnotationServiceBuilder](#benefits-of-using-annotationservicebuilder)

---

## Prerequisites

### Web Application

1. **.NET 6.0 or later**
2. **Visual Studio 2022** (or any other IDE with .NET support)

### Important Note

Ensure that your .NET SDK is up-to-date. This project requires .NET 6.0 or later.

## Setting up Annotations

The `Annotations` folder in project contains the key attributes used for automatically registering services in the DI container. Below are the files and their purposes, along with examples of how they are used.

---

## Installing AnnotationServiceBuilder

### Step 1: Install the AnnotationServiceBuilder NuGet Package

You can install the AnnotationServiceBuilder package via NuGet.

#### Using Package Manager Console

```powershell
Install-Package AnnotationServiceBuilder
```

#### Using .NET Core CLI

```bash
dotnet add package AnnotationServiceBuilder
```

### Step 2: Set Up Annotations

Follow these steps to configure and use AnnotationServiceBuilder in your project.

### If You're Using before Version 1.0.9

Add the following to your `Startup.cs` or `Program.cs`:

```csharp
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddAnnotatedSingletonServices(assembly);
builder.Services.AddAnnotatedScopedServices(assembly);
builder.Services.AddAnnotatedTransientServices(assembly);
builder.Services.AddRefitClientsFromAttributes(assembly, "https://api.yourservice.com"); // Replace with your API base URL
```

### If You're Using Version 1.0.9

First, create an instance of `AnnotationServiceRegistrar`:

```csharp
var registrar = new AnnotationServiceRegistrar(Assembly.GetExecutingAssembly());
```

Then, register your services:

```csharp
registrar.AddSingletonServices(services);
registrar.AddScopedServices(services);
registrar.AddTransientServices(services);
registrar.AddRefitClients(services, "https://api.yourservice.com"); // Replace with your API base URL
```

If you need to use a custom `DelegatingHandler`, you can do so with the following:

```csharp
var customHandler = new MyCustomHandler();
registrar.AddRefitClients(services, "https://api.yourservice.com", customHandler);
```

### If You're Using Version 1.1.1 or Later

Instead of creating an instance of `AnnotationServiceRegistrar`, use the static method `Initialize` to set up the types and then call the registration methods:

```csharp
AnnotationServiceRegistrar.Initialize(Assembly.GetExecutingAssembly());

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

## Usage

Here are examples of how to use each annotation in your project:

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
using AnnotationServiceBuilder.Annotations.Transient_Services;

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

When using advanced features like trimming or Ahead-of-Time (AOT) compilation, assembly scanning can cause issues by trimming out concrete implementations not directly referenced in the code. **AnnotationServiceBuilder** addresses these challenges by ensuring compatibility with AOT and trimming, while providing warnings via trimming analyzers to help prevent potential issues. You can also use attributes like `DynamicDependency` or `Preserve` to retain necessary types during the trimming process.

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

### Example of Using `Preserve`

```csharp
using System.Runtime.CompilerServices;

[Preserve]
[SingletonService]
public class My

DependentService
{
    public void PerformOperation()
    {
        // Implementation that must be retained during trimming
    }
}
```

---

## ASB: Observing the Logic of Pattern Standards

**AnnotationServiceBuilder** supports design patterns like the Factory Pattern, to improve code maintainability and structure. Below is an example of how patterns are implemented using ASB’s annotations.

### Registering Factory Pattern Services

Before registering, initialize the pattern registrar:

```csharp
AnnotationPatternRegistrar.Initialize(Assembly.GetExecutingAssembly());
AnnotationPatternRegistrar.AddFactoryPatternServices(builder.Services);
```

> **Note**: If you use `AnnotationServiceRegistrar.Initialize(Assembly.GetExecutingAssembly())`, **do not** use `AnnotationPatternRegistrar.Initialize(Assembly.GetExecutingAssembly())`.

### Example Using Factory Pattern

**PostFactory.cs:**

```csharp
using AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory;
using AnnotationServiceBuilderExamples.Data.Models;

namespace AnnotationServiceBuilderExamples.Data
{
    [FactoryPattern(typeof(IFactory<Post>))]
    public class PostFactory : IFactory<Post>
    {
        public Post Create()
        {
            return new Post();
        }
    }
}
```

**PostFactoryService.cs:**

```csharp
using AnnotationServiceBuilder.Annotations.Patterns.CreationalDesignPatterns.Factory;
using AnnotationServiceBuilder.Annotations.Transient;
using AnnotationServiceBuilderExamples.Data.Models;

namespace AnnotationServiceBuilderExamples.Data
{
    [TransientService]
    public class PostFactoryService
    {
        private readonly IFactory<Post> _postFactory;

        public PostFactoryService(IFactory<Post> postFactory)
        {
            _postFactory = postFactory;
        }

        public Post CreateNewPost(int id, string title, string body)
        {
            var post = _postFactory.Create();
            post.Id = id;
            post.Title = title;
            post.Body = body;
            return post.
        }
    }
}
```

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

