# cache-it
Framework for Direct and Simple Update of Cache Components in .NET

# Why Should I Use It?
If you want to use and refresh the cache in your application, it's a good choice to use it.

Since you only need to inherit our interface and call the dependency injection extension.

# Usage
1. Choose a component of your system that you want to have some type of information cached for quick access to data.

```dotnet
namespace MySuperApp.Services
{
    public class SuperRequestedService
    {
        ...
    }
}
```

2. Inherit from the [`ICacheable`](src\CacheIt\ICacheable.cs) interface and call your class functions that load or update your data as needed.

```dotnet
namespace MySuperApp.Services
{
    public class SuperRequestedService: ICacheable
    {
        ...

        public async Task Load()
        {
            await FillMySuperData();
        }

        public async Task Refresh()
        {
            await RefreshMySuperData();            
        }

    }
}
```

> The __Load__ Method is called once when your app starts. And the __Refresh__ Method from time to time.
> 
> By default, the refresh time is 1 minute. See the Custom Configuration topic for more details.

3. Call [`AddCachers`](src\CacheIt\Extensions\DependencyInjection\ServiceCollectionExtensions.cs) Service Collection Extension. 

```dotnet
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ...
    services.AddSingleton<SuperRequestedService>();
    ...
    services.AddCachers();
}
```

4. Start your app and Voilá!

__For simplicity the example class does not inherits from another interface but if your component/class does, there is no problem.__

# How It Works
The HostedService [`Handler`](src\CacheIt\Hosting\Handler.cs), registered via [`AddCachers`](src\CacheIt\Extensions\DependencyInjection\ServiceCollectionExtensions.cs) Service Collection Extension, retrieves by reflection any `Type` that Inherits from [`ICacheable`](src\CacheIt\ICacheable.cs).

t then uses the ServiceProvider to retrieve the registered services and then calls the __Load__ method if it is an Application Start, and over the life of the application and the defined refresh interval it calls the __Refresh__ method.

# Custom Configuration
Whether your update needs to be more frequent or not, you can use a custom setting in the `appsettings.json` of your project.

See the example below, that we want to refresh at each 10 minutes.

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Debug",
      "Microsoft": "Warning",
      "Microsoft.Hosting.Lifetime": "Debug"
    }
  },
  "AllowedHosts": "*",
  ...
  "CacheIt":{
    "RefreshIntervalMinutes":10
  }
  ...
}
```

# Contributing

Open a branch, code and open a PR to main and request review for any of the Contributors. For now this is the main flow.

# License

Licensed under the [MIT License](https://www.mit.edu/~amini/LICENSE.md).
