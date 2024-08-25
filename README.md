
# AnnationServiceBilder

**AnnationServiceBilder** is an ASP.NET Core library that simplifies dependency injection by using custom annotations to automatically register services in the DI container.

## Prerequisites

### Web Application

1. **.NET 6.0 or later**
2. **Visual Studio 2022** (or any other IDE with .NET support)
3. **Git** (for version control)

### Recommended Tools

- **Visual Studio Code** with extensions:
  - [C#](https://marketplace.visualstudio.com/items?itemName=ms-dotnettools.csharp) (for .NET development)
  - [Prettier](https://marketplace.visualstudio.com/items?itemName=esbenp.prettier-vscode) (for code formatting)
  - [EditorConfig](https://marketplace.visualstudio.com/items?itemName=EditorConfig.EditorConfig) (for consistent coding styles)

### Important Note

Ensure that your .NET SDK is up-to-date. This project requires .NET 6.0 or later.

## Setting up Annotations

The `Annotations` folder in your project contains the key attributes used for automatically registering services in the DI container. Below are the files and their purposes, along with examples of how they are used:

## Installing AnnationServiceBilder

### Step 1: Install the AnnationServiceBilder NuGet Package

You can install the AnnationServiceBilder package via NuGet.

#### Using Package Manager Console

```powershell
Install-Package AnnationServiceBilder
```

#### Using .NET Core CLI

```bash
dotnet add package AnnationServiceBilder
```

### Step 2: Set Up Annotations

Follow these steps to configure and use AnnationServiceBilder in your project.

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
using AnnationServiceBilder.Annotations.Scoped;

[ScopedService(typeof(IMyScopedService))]
public class MyScopedService : IMyScopedService
{
    // Implementation...
}
```

### **2. Using Singleton Services**

```csharp
using AnnationServiceBilder.Annotations.Singleton;

[SingletonService]
public class MySingletonService
{
    // Implementation...
}
```

### **3. Using Transient Services**

```csharp
using AnnationServiceBilder.Annotations.Transient_Services;

[TransientService]
public class MyTransientService
{
    // Implementation...
}
```

### **4. Using Refit Clients**

```csharp
using AnnationServiceBilder.Annotations.Refit;
using AnnationServiceBilder.Data.Models;
using Refit;

namespace AnnationServiceBilder.Network.Repositories
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

## Benefits of Using AnnationServiceBilder

### 1. **Automation of Service Registration**

With AnnationServiceBilder, you can automatically register your services in the DI container without needing to manually add each service in `Startup.cs` or `Program.cs`. This reduces boilerplate code and makes your setup process much more streamlined.

### 2. **Clear and Organized Codebase**

By using annotations to define the lifetime of services (Singleton, Scoped, Transient), your code becomes more organized. Each service's lifetime is clearly indicated in the class itself, making it easier to understand and maintain.

### 3. **Time Efficiency**

Automating service registration saves time, especially in large projects where there are many services to register. Developers can focus on building features instead of managing service registrations manually.

### 4. **Consistency Across the Project**

Annotations ensure that the same pattern is followed throughout the project. This consistency reduces the chances of errors or missed registrations, which can lead to runtime issues.

### 5. **Ease of Use**

The library provides a simple, intuitive API for registering services and Refit clients. Developers can easily annotate their services and interfaces, making the overall process very user-friendly.

### 6. **Integration with Refit**

AnnationServiceBilder also supports the registration of Refit clients, allowing you to easily integrate with HTTP APIs. This makes the project versatile and applicable to a wide range of scenarios, including microservices architecture.

### 7. **Improved Maintainability**

Because the service registration logic is centralized and automated, it becomes easier to maintain. When changes are needed, they can be made in a single place rather than updating multiple lines of registration code scattered throughout the project.

### 8. **Enhanced Readability**

Annotations make the code more self-explanatory. When you see a class annotated with `[SingletonService]`, it's immediately clear what its lifecycle is, enhancing the readability of your codebase.

### 9. **Scalability**

This approach scales well as your project grows. Whether you have a few services or hundreds, the process remains the same and just as efficient, allowing your project to scale without adding complexity to the service registration process.

### 10. **Fewer Errors**

By centralizing and automating service registration, the chances of missing a service registration or configuring it incorrectly are minimized, leading to fewer runtime errors and a more stable application.

## Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss your ideas or report bugs.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
