# cache-it
Framework for Direct and Simple Update of Cache Components in .NET

# Why Should I Use It?
If you want to use and refresh cache in your application, it's a good choice to use it. 

Since, you just need to inherit from our Interface and call the dependency injection extension.

You can see below the details of how to use it.

# Usage
1. Chose one component of your system that you want to have some type of information cached for quick access to data.

```dotnet
namespace MySuperApp.Services
{
    public class SuperRequestedService
    {
        ...
    }
}
```

2. Inherits from [`ICacheable`](src\CacheIt\ICacheable.cs) interface, and call your class functions that load or refresh your data as you need.

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

> The __Load__ Method is called once, when your application starts. And the __Refresh__ Method on and off.
> By default, the refresh time is 1 minute. See xxxx for details.

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

Then it uses the ServiceProvider to retrieve the services registered, and then calls the __Load__ method if is a Application Start and over the application life and defined refresh interval it calls the __Refresh__ method.

# Custom Configuration
If your refresh does need to be more frequent or no, you can use a custom configuration at `appsettings.json` project. 

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
