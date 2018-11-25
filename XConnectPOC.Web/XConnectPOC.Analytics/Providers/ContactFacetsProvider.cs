using Sitecore.Analytics;
using Sitecore.Analytics.XConnect.Facets;
using Sitecore.XConnect;
using XConnect.Personalization.Providers;

namespace XConnectPOC.Analytics.Providers
{
    public class ContactFacetsProvider : IContactFacetsProvider
    { 
        public T GetFacet<T>(string facetName)
            where T : Facet
        {
            var xConnectFacet = Tracker.Current.Contact.GetFacet<IXConnectFacets>("XConnectFacets");
            var allFacets = xConnectFacet.Facets;
            if (allFacets == null)
            {
                return null;
            }

            if (!allFacets.ContainsKey(facetName))
            {
                return null;
            }

            return (T)allFacets?[facetName];
        }


    }
}

