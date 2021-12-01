using System.Threading.Tasks;

namespace CacheIt
{
    public interface ICacheable
    {
        Task Load();
        Task Refresh();
    }
}
