namespace Game.Core.Helpers.Services;

public interface IServiceLocator
{
    T Resolve<T>() where T : class;
    void Register<T>(T service) where T : class;
}
