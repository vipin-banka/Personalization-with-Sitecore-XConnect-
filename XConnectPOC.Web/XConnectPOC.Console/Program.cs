using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.WebApi;
using Sitecore.XConnect.Collection.Model;
using Sitecore.XConnect.Schema;
using Sitecore.Xdb.Common.Web;
using System;
using System.Collections.Generic;
using XConnectPOC.Collection.Model.CollectionModel;
using XConnectPOC.Collection.Model.Facets;

namespace XConnectPOC.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            GetContact();
        }

        public static void GetContact()
        {
            // Valid certificate thumbprints must be passed in
            CertificateWebRequestHandlerModifierOptions options =
                CertificateWebRequestHandlerModifierOptions.Parse("StoreName=My;StoreLocation=LocalMachine;FindType=FindByThumbprint;FindValue=DAEDD8105CFFAD6E95F25488E72EF0B5C6C9328B");

            var certificateModifier = new CertificateWebRequestHandlerModifier(options);

            // Optional timeout modifier
            List<IHttpClientModifier> clientModifiers = new List<IHttpClientModifier>();
            var timeoutClientModifier = new TimeoutHttpClientModifier(new TimeSpan(0, 0, 20));
            clientModifiers.Add(timeoutClientModifier);

            // This overload takes three client end points - collection, search, and configuration
            var collectionClient = new CollectionWebApiClient(new Uri("https://sc902.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var searchClient = new SearchWebApiClient(new Uri("https://sc902.xconnect/odata"), clientModifiers, new[] { certificateModifier });
            var configurationClient = new ConfigurationWebApiClient(new Uri("https://sc902.xconnect/configuration"), clientModifiers, new[] { certificateModifier });

            var cfg = new XConnectClientConfiguration(
                new XdbRuntimeModel(CollectionModel.Model, MySiteCollectionModel.Model), collectionClient, searchClient, configurationClient);

            try
            {
                cfg.Initialize();

            }
            catch (XdbModelConflictException ce)
            {
                System.Console.WriteLine("ERROR:" + ce.Message);
                return;
            }

            using (var client = new XConnectClient(cfg))
            {
                var reference = new IdentifiedContactReference("Website", "paulsmith@rightpoint.com");

                var contact = client.Get<Sitecore.XConnect.Contact>(reference, new ContactExpandOptions(PersonalInformation.DefaultFacetKey, BusinessIndustry.DefaultFacetKey, EmailAddressList.DefaultFacetKey)
                {
                    Interactions = new RelatedInteractionsExpandOptions(IpInfo.DefaultFacetKey)
                    {
                        StartDateTime = DateTime.MinValue,
                        EndDateTime = DateTime.MaxValue,
                        Limit = 3
                    }
                });
            }
        }
    }
}