# AnnotationServiceBuilder

**AnnotationServiceBuilder** is an ASP.NET Core library that simplifies dependency injection by using custom annotations to automatically register services in the DI container.

## Prerequisites

### Web Application

1. **.NET 6.0 or later**
2. **Visual Studio 2022** (or any other IDE with .NET support)

### Important Note

Ensure that your .NET SDK is up-to-date. This project requires .NET 6.0 or later.

## Setting up Annotations

The `Annotations` folder in your project contains the key attributes used for automatically registering services in the DI container. Below are the files and their purposes, along with examples of how they are used:

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

Add the following to your `Startup.cs` or `Program.cs`:

```csharp
var assembly = Assembly.GetExecutingAssembly();
builder.Services.AddAnnotatedSingletonServices(assembly);
builder.Services.AddAnnotatedScopedServices(assembly);
builder.Services.AddAnnotatedTransientServices(assembly);
builder.Services.AddRefitClientsFromAttributes(assembly, "https://api.yourservice.com"); // Replace with your API base URL
```

## Usage

Here are examples of how to use each annotation in your project:

### **1. Using Scoped Services**

```csharp
using AnnotationServiceBuilder.Annotations.Scoped;

[ScopedService(typeof(IMyScopedService))]
public class MyScopedService : IMyScopedService
{
    // Implementation...
}
```

### **2. Using Singleton Services**

```csharp
using AnnotationServiceBuilder.Annotations.Singleton;

[SingletonService]
public class MySingletonService
{
    // Implementation...
}
```

### **3. Using Transient Services**

```csharp
using AnnotationServiceBuilder.Annotations.Transient_Services;

[TransientService]
public class MyTransientService
{
    // Implementation...
}
```

### **4. Using Refit Clients**

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

## Benefits of Using AnnotationServiceBuilder

### 1. **Automation of Service Registration**

Automatically register your services in the DI container without needing to manually add each service in `Startup.cs` or `Program.cs`. This reduces boilerplate code and makes your setup process much more streamlined.

### 2. **Clear and Organized Codebase**

Annotations define the lifetime of services (Singleton, Scoped, Transient), making your code more organized and easier to maintain.

### 3. **Time Efficiency**

Automating service registration saves time, especially in large projects. Developers can focus on building features instead of managing service registrations manually.

### 4. **Ease of Use**

The library provides a simple, intuitive API for registering services and Refit clients, making the process user-friendly.

### 5. **Caching for Performance**

Registered classes and interfaces are cached to improve performance and reduce the overhead of repeated reflection operations.

## Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss your ideas or report bugs.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.