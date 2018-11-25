namespace XConnect.Personalization.Providers
{
    public interface IContactFacetsProvider
    {
        T GetFacet<T>(string facetName)
            where T : Sitecore.XConnect.Facet;
    }
}