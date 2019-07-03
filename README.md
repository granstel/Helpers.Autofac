Autofac.Helpers
================
[![Build status](https://ci.appveyor.com/api/projects/status/bg07tb833tj2rmgu?svg=true)](https://ci.appveyor.com/project/granstel/helpers-autofac)
[![NuGet](https://buildstats.info/nuget/GranSteL.Helpers.Autofac)](https://www.nuget.org/packages/GranSteL.Helpers.Autofac)

SafeInvoker:
Represents a dependency that releases after using.
Depend on System.Func<Owned<T>> in order to create and dispose of other components as required.

Install
-------
It's available via [nuget package](https://www.nuget.org/packages/GranSteL.Helpers.Autofac/)  
PM> `Install-Package GranSteL.Helpers.Autofac`

Example Usage
-------------
```csharp
static void Main(string[] args)
{
    // Create your container
    var builder = new ContainerBuilder();

    builder.RegisterGeneric(typeof(SafeInvoker<>)).As(typeof(ISafeInvoker<>)).SingleInstance();

    // Register dependent component
    builder.RegisterType<InternalService>().As<IInternalService>();

    // Register dependency
    builder.RegisterType<WebServiceClient>().As<IWebServiceClient>();

    var container = builder.Build();
}

public class InternalService : IInternalService
{
    private readonly ISafeInvoker<IWebServiceClient> _webServiceClient;

    public InternalService(ISafeInvoker<IWebServiceClient> webServiceClient)
    {
        _webServiceClient = webServiceClient;
    }
    
    public async Task PingWebService()
    {
        await _webServiceClient.InvokeAsync(c => c.GetResponse("ping"));
    }
}
```
