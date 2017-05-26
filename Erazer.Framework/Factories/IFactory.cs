namespace Erazer.Framework.Factories
{
    public interface IFactory<T> where T : class
    {
        T Build();
    }
}