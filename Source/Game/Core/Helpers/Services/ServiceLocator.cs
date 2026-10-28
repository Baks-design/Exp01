using System;
using System.Collections.Generic;
using System.Threading;

namespace Game.Core.Helpers.Services;

#nullable enable

public sealed class ServiceLocator : IDisposable
{
    readonly Dictionary<Type, object> services = [];
    readonly Lock _lock = new();
    bool disposed;

    public void Register<T>(T service) where T : class
    {
        ArgumentNullException.ThrowIfNull(service);
        RegisterInternal(typeof(T), service, replace: true);
    }

    public void Register<TInterface, TImplementation>(TImplementation service, bool replace = true)
        where TInterface : class
        where TImplementation : class, TInterface
    {
        ArgumentNullException.ThrowIfNull(service);
        RegisterInternal(typeof(TInterface), service, replace);
    }

    public void RegisterFactory<T>(Func<T> factory, bool replace = true) where T : class
    {
        ArgumentNullException.ThrowIfNull(factory);
        var lazy = new Lazy<T>(factory, LazyThreadSafetyMode.ExecutionAndPublication);
        RegisterInternal(typeof(T), lazy, replace);
    }

    public T GetOrRegister<T>(Func<T> factory) where T : class
    {
        ArgumentNullException.ThrowIfNull(factory);

        lock (_lock)
        {
            if (services.TryGetValue(typeof(T), out var existing))
                return existing is Lazy<T> lazy ? lazy.Value : (T)existing;

            var instance = factory();
            ArgumentNullException.ThrowIfNull(instance);
            services[typeof(T)] = instance;
            return instance;
        }
    }

    void RegisterInternal(Type type, object entry, bool replace)
    {
        lock (_lock)
        {
            ObjectDisposedException.ThrowIf(disposed, this);

            if (!replace && services.ContainsKey(type))
                throw new InvalidOperationException(
                    $"Service of type {type.FullName} is already registered. " +
                    "Pass replace: true to override.");

            if (services.TryGetValue(type, out var old) && old is IDisposable disposable)
                disposable.Dispose();

            services[type] = entry;
        }
    }

    public T Resolve<T>() where T : class
    {
        if (TryResolve<T>(out var service)) return service;
        throw new InvalidOperationException(
            $"Service of type {typeof(T).FullName} not registered.");
    }

    public T? ResolveOrDefault<T>() where T : class
        => TryResolve<T>(out var service) ? service : null;

    public bool TryResolve<T>(out T service) where T : class
    {
        object entry;
        lock (_lock)
        {
            ObjectDisposedException.ThrowIf(disposed, this);

            if (!services.TryGetValue(typeof(T), out entry!))
            {
                service = null!;
                return false;
            }
        }

        service = entry is Lazy<T> lazy ? lazy.Value : (T)entry;
        return true;
    }

    public bool IsRegistered<T>() where T : class
    {
        lock (_lock) { return !disposed && services.ContainsKey(typeof(T)); }
    }

    public bool Unregister<T>() where T : class
    {
        object? entry;
        lock (_lock)
        {
            if (!services.Remove(typeof(T), out entry))
                return false;
        }
        (entry as IDisposable)?.Dispose();
        return true;
    }

    public void Clear()
    {
        List<object> snapshot;
        lock (_lock)
        {
            snapshot = [.. services.Values];
            services.Clear();
        }
        foreach (var entry in snapshot)
        {
            if (entry is IDisposable disposable)
                disposable.Dispose();
            else if (entry is Lazy<object> lazy && lazy.IsValueCreated)
                (lazy.Value as IDisposable)?.Dispose();
        }
    }

    public void Dispose()
    {
        if (disposed) return;
        Clear();
        lock (_lock) { disposed = true; }
    }
}