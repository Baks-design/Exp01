using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Core.Helpers.Services;

public sealed class ServiceLocator
{
    readonly Dictionary<Type, object> services = [];
    readonly Lock _lock = new();

    public void Register<T>(T service) where T : class
    {
        ArgumentNullException.ThrowIfNull(service);

        lock (_lock) { services[typeof(T)] = service; }
    }

    public void Register<TInterface, TImplementation>(TImplementation service)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        ArgumentNullException.ThrowIfNull(service);

        lock (_lock) { services[typeof(TInterface)] = service; }
    }

    public void RegisterFactory<T>(Func<T> factory) where T : class
    {
        ArgumentNullException.ThrowIfNull(factory);

        lock (_lock) { services[typeof(T)] = new Lazy<T>(factory); }
    }

    public T Resolve<T>() where T : class
    {
        if (TryResolve<T>(out var service)) return service;

        throw new InvalidOperationException($"Service of type {typeof(T).FullName} not registered.");
    }

    public bool TryResolve<T>(out T service) where T : class
    {
        lock (_lock)
        {
            if (services.TryGetValue(typeof(T), out var entry))
            {
                service = entry is Lazy<T> lazy ? lazy.Value : (T)entry;
                return true;
            }
        }

        service = null!;
        return false;
    }

    public bool IsRegistered<T>() where T : class
    {
        lock (_lock) { return services.ContainsKey(typeof(T)); }
    }

    public bool Unregister<T>() where T : class
    {
        lock (_lock) { return services.Remove(typeof(T)); }
    }

    public void Clear()
    {
        lock (_lock) { services.Clear(); }
    }
}