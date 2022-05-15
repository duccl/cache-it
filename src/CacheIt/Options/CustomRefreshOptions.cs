using System.Collections.Generic;

namespace CacheIt.Options
{
    internal class CustomRefreshOptions
    {
        public const string SectionName = "CacheIt:CustomRefresh";
        public Dictionary<string, double> RefreshTimesByCacheableName { get; set; } = new Dictionary<string, double>();
    }
}