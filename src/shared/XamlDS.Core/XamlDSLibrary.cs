using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace XamlDS;

public abstract class XamlDSLibrary
{
    private static readonly List<string> _registeredlibraries = new();

    public virtual void Register(HostApplicationBuilder hostBuilder)
    {
        string assemblyName = GetType().Assembly.FullName!;
        if (_registeredlibraries.Contains(assemblyName))
            return;
        _registeredlibraries.Add(assemblyName);

        AddServices(hostBuilder.Services);
    }

    protected abstract void AddServices(IServiceCollection services);
}