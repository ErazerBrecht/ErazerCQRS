namespace Erazer.Framework.Factories
{
    public interface IFactory<out T> where T : class
    {
        T Build();
    }
}