using System;
using System.Collections.Generic;

namespace Game.Core.Helpers.Services;

public class ServiceLocator : IServiceLocator
{
    private readonly Dictionary<Type, object> services = [];

    public void Register<T>(T service) where T : class => services[typeof(T)] = service;

    public T Resolve<T>() where T : class
    {
        if (services.TryGetValue(typeof(T), out var service)) return (T)service;

        throw new InvalidOperationException($"Service of type {typeof(T)} not registered.");
    }
}