
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

   Annotate your service classes accordingly:

   ```csharp
   [ScopedService(typeof(IPostsRepository))]
   public class PostsRepository : IPostsRepository
   {
       // Implementation...
   }
   ```

2. **Register Services Automatically**:

   In your `Startup.cs` or `Program.cs`, ensure that the services are registered by adding the following lines:

   ```csharp
   var assembly = Assembly.GetExecutingAssembly();
   services.AddAnnotatedSingletonServices(assembly);
   services.AddAnnotatedScopedServices(assembly);
   services.AddAnnotatedTransientServices(assembly);
   ```

   This will automatically register all services annotated with the custom attributes.

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

## Contributing

We welcome contributions! Please submit a pull request or open an issue to discuss your ideas or report bugs.

## License

This project is licensed under the MIT License - see the [LICENSE](LICENSE) file for details.
