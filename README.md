Autofac.Helpers
================

SafeInvoker:
Represents a dependency that can be released after using. 
Depend on System.Func<Owned<T>> in order to create and dispose of other components as required.

Install
-------
It's available via [nuget package](https://www.nuget.org/packages/topshelf.autofac)  
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
