using System.IO;
using Sitecore.XConnect.Serialization;

namespace XConnectPOC.Collection.Model.Serialization
{
    public class Program
    {
        public static void Main()
        {
            Serialize();
        }

        public static void Serialize()
        {
            var model = XdbModelWriter.Serialize(XConnectPOC.Collection.Model.CollectionModel.MySiteCollectionModel.Model);
            File.WriteAllText(XConnectPOC.Collection.Model.CollectionModel.MySiteCollectionModel.Model.FullName + ".json", model);
        }
    }
}