using Sitecore.XConnect;
using Sitecore.XConnect.Schema;
using XConnectPOC.Collection.Model.Facets;

namespace XConnectPOC.Collection.Model.CollectionModel
{
    public class MySiteCollectionModel
    {
        public static XdbModel Model { get; } = MySiteCollectionModel.BuildModel();

        private static XdbModel BuildModel()
        {
            XdbModelBuilder modelBuilder = new XdbModelBuilder("MySiteCollectionModel", new XdbModelVersion(1, 0));

            modelBuilder.ReferenceModel(Sitecore.XConnect.Collection.Model.CollectionModel.Model);
            modelBuilder.DefineFacet<Contact, BusinessIndustry>(BusinessIndustry.DefaultFacetKey);

            return modelBuilder.BuildModel();
        }
    }
}