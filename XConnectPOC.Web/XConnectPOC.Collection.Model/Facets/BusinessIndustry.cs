using System;
using Sitecore.XConnect;

namespace XConnectPOC.Collection.Model.Facets
{
    [Serializable]
    [FacetKey(DefaultFacetKey)]
    public class BusinessIndustry : Sitecore.XConnect.Facet
    {
        public const string DefaultFacetKey = "MySite.Facet.BusinessIndustry";
        
        public Guid Id { get; set; }
    }
}