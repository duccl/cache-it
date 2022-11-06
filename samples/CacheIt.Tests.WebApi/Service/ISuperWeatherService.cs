namespace CacheIt.Tests.WebApi.Service
{
    public interface ISuperWeatherService
    {
        string GetOne(int index);
        int GetSize();
    }
}