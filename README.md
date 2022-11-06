<p align="center">
    <img src="https://github.com/duccl/cache-it/blob/main/resources/images/Just.CacheIt_Logo.png" alt="Just.CacheIt_Logo.png">
</p>


# Just.CacheIt
Framework for Direct and Simple Update of Cache or Reloadable Components in .NET

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

2. Inherit from the [`ICacheable`](/src/CacheIt/ICacheable.cs) interface and call your class functions that load or update your data as needed.

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

3. Call [`AddCacheIt`](src/CacheIt/Extensions/DependencyInjection/ServiceCollectionExtensions.cs) Service Collection Extension. 

```dotnet
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ...
    services.AddSingleton<SuperRequestedService>();
    ...
    services.AddCacheIt();
}
```

If you want to make custom configuration for a cacheable, call the overloaded [`AddCacheIt`](src/CacheIt/Extensions/DependencyInjection/ServiceCollectionExtensions.cs) Service Collection Extension method. This method receives a `Iconfiguration` object to setup the custom refresh.

```dotnet
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ...
    services.AddSingleton<SuperRequestedService>();
    ...
    services.AddCacheIt(_configuration);
}
```

See [Custom Configuration](#custom-configuration) Section for more details.

4. Start your app and Voilá!

__For simplicity the example class does not inherits from another interface but if your component/class does, there is no problem.__

# How It Works
The HostedService [`Handler`](src/CacheIt/Hosting/Handler.cs), registered via [`AddCacheIt`](src/CacheIt/Extensions/DependencyInjection/ServiceCollectionExtensions.cs) Service Collection Extension, retrieves by reflection any `Type` that Inherits from [`ICacheable`](src/CacheIt/ICacheable.cs).

It then uses the ServiceProvider to retrieve the registered services and then calls the __Load__ method if it is an Application Start, and over the life of the application and the defined refresh interval it calls the __Refresh__ method.

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
    "RefreshInterval":"00:10:00"
  }
  ...
}
```

If you want your cacheable component to be refreshed with a custom interval, you can use the follwing example.

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
    "RefreshInterval":"00:10:00",
    "CustomRefresh":{
      "RefreshTimesByCacheableName":{
        "MyCacheableComponentNameInjectedAtServices": "00:00:25",
        "IMySuperCacheableComponentNameInjectedAtServices": "00:05:00"
      }
    }
  }
  ...
}
```

Please if you are using versions lower than __2.1.3-preview__, use the following example

> before 2.1.3-preview the intervals were expressed in minutes.

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
    "RefreshIntervalMinutes":10,
    "CustomRefresh":{
      "RefreshTimesByCacheableName":{
        "MyCacheableComponentNameInjectedAtServices": 0.1,
        "IMySuperCacheableComponentNameInjectedAtServices": 5
      }
    }
  }
  ...
}
```

> **IMPORTANT** : the name must match with the class name injected at IServiceCollection!

And at your services configuration
```dotnet
public void ConfigureServices(IServiceCollection services)
{
    services.AddControllers();
    ...
    services.AddSingleton<MyCacheableComponentNameInjectedAtServices>();
    services.AddSingleton<IMySuperCacheableComponentNameInjectedAtServices,MySuperCacheableComponentNameInjectedAtServices>();
    ...
    services.AddCacheIt(_configuration);
}
```

> _configuration refers to the [IConfiguration dependency](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/?view=aspnetcore-6.0).

# Examples

1. [.NET Web Api With Just.CacheIt](/src/CacheIt.Tests/CacheIt.Tests.WebApi/)

# Changelog

Please go to this [guy](https://github.com/duccl/cache-it/blob/main/CHANGELOG.md)

# Contributing

Open a branch, code and open a PR to main and request review for any of the Contributors. For now this is the main flow.

# License

Licensed under the [MIT License](https://www.mit.edu/~amini/LICENSE.md).
