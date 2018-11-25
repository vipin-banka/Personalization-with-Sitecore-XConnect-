using Sitecore.XConnect;
using System.Linq;
using System.Reflection;

namespace XConnectPOC.Analytics.Extensions
{
    public static class FacetExtensions
    {
        public static void Map(this Facet source, Facet target)
        {
            var properties = source.GetType().GetProperties(BindingFlags.DeclaredOnly | BindingFlags.Public | BindingFlags.Instance);

            if (properties != null && properties.Any())
            {
                foreach (var propertyInfo in properties)
                {
                    propertyInfo.SetValue(target, propertyInfo.GetValue(source));
                }
            }
        }
    }
}