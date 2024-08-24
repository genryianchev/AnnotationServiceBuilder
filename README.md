
# AnnationServiceBilder

AnnationServiceBilder is an ASP.NET Core library that simplifies dependency injection by using custom annotations to automatically register services in the DI container.

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

- Ensure that your .NET SDK is up-to-date. This project requires .NET 6.0 or later.

## Installation

1. Clone the repository to your local machine:

   ```bash
   git clone https://github.com/yourusername/AnnationServiceBilder.git
   cd AnnationServiceBilder
   ```

2. Open the project in Visual Studio or your preferred IDE.

3. Restore the necessary packages by building the solution.

## Configuration

### Setting up Annotations

1. **Create and Apply Annotations**:

   The project already includes the necessary annotations for service registration. They are located in the `Annotations` folder:

   - `SingletonServiceAttribute`: Register a service with a singleton lifetime.
   - `ScopedServiceAttribute`: Register a service with a scoped lifetime.
   - `TransientServiceAttribute`: Register a service with a transient lifetime.
   - `RefitClientAttribute`: Register an interface as a Refit client.

   Annotate your classes and interfaces accordingly:

   #### SingletonServiceAttribute Example

   ```csharp
   using AnnationServiceBilder.Annotations;

   [SingletonService]
   public class MySingletonService
   {
       // Implementation for a singleton service...
   }
   ```

   This class will be registered with a singleton lifetime, meaning only one instance of `MySingletonService` will be created and shared throughout the application's lifetime.

   #### ScopedServiceAttribute Example

   ```csharp
   using AnnationServiceBilder.Annotations;

   [ScopedService(typeof(IPostsRepository))]
   public class PostsRepository : IPostsRepository
   {
       // Implementation for a scoped service...
   }
   ```

   This service will be registered with a scoped lifetime, meaning an instance of `PostsRepository` will be created once per request.

   #### TransientServiceAttribute Example

   ```csharp
   using AnnationServiceBilder.Annotations;

   [TransientService]
   public class MyTransientService
   {
       // Implementation for a transient service...
   }
   ```

   This service will be registered with a transient lifetime, meaning a new instance of `MyTransientService` will be created every time it is requested.

   #### RefitClientAttribute Example

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

   This interface will be automatically registered as a Refit client, enabling you to make HTTP requests without manually configuring the client.

2. **Register Services Automatically**:

   In your `Startup.cs` or `Program.cs`, ensure that the services are registered by adding the following lines:

   ```csharp
   var assembly = Assembly.GetExecutingAssembly();
   services.AddAnnotatedSingletonServices(assembly);
   services.AddAnnotatedScopedServices(assembly);
   services.AddAnnotatedTransientServices(assembly);
   services.AddRefitClientsFromAttributes(assembly, "https://api.yourservice.com"); // Replace with your API base URL
   ```

   This will automatically register all services and Refit clients annotated with the custom attributes.

## Usage

Once your services are annotated and registered, you can use them anywhere in your application by injecting them into constructors:

```csharp
public class MyService
{
    private readonly IPostsRepository _postsRepository;

    public MyService(IPostsRepository postsRepository)
    {
        _postsRepository = postsRepository;
    }

    public async Task<List<Post>> GetPostsAsync()
    {
        return await _postsRepository.GetPostsAsync();
    }
}
```

For Refit clients, you can directly inject the interface and use it as follows:

```csharp
public class PostService
{
    private readonly IPostsApi _postsApi;

    public PostService(IPostsApi postsApi)
    {
        _postsApi = postsApi;
    }

    public async Task<List<Post>> FetchPostsAsync()
    {
        return await _postsApi.GetPostsAsync();
    }
}
```

## Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss your ideas or report bugs.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.

