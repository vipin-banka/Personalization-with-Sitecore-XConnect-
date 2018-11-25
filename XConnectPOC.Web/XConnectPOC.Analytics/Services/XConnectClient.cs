using Sitecore.XConnect;
using Sitecore.XConnect.Client;
using Sitecore.XConnect.Client.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using XConnectPOC.Analytics.Extensions;

namespace XConnectPOC.Analytics.Services
{
    public class XConnectClient
    {
        private Sitecore.Analytics.Tracking.Contact _contact;
        private Sitecore.Analytics.Tracking.ContactManager _manager;
        private IDictionary<string, Facet> facets = new Dictionary<string, Facet>();

        public XConnectClient(Sitecore.Analytics.Tracking.Contact contact)
        {
            this._manager = Sitecore.Configuration.Factory.CreateObject("tracking/contactManager", true) as Sitecore.Analytics.Tracking.ContactManager;

            this._contact = contact;
        }

        public XConnectClient SetFacet<T>(T data, string key) where T : Facet
        {
            if (facets.ContainsKey(key))
            {
                facets[key] = data;
            }
            else
            {
                facets.Add(key, data);
            }

            return this;
        }

        public void Submit()
        {
            using (var client = SitecoreXConnectClientConfiguration.GetClient())
            {
                try
                {
                    bool submit = false;

                    var contactReference = this.GetIdentifiedContactReference();
                    var xContact = client.Get(contactReference, new ContactExpandOptions(facets.Keys.ToArray()));
                    if (xContact == null)
                        return;

                    foreach (var facet in facets)
                    {
                        var facetValue = xContact.GetFacet<Facet>(facet.Key);

                        if (facetValue != null)
                        {
                            facet.Value.Map(facetValue);
                        }
                        else
                        {
                            facetValue = facet.Value;
                        }

                        if (this.SetFacet(facetValue, facet.Key, xContact, client))
                        {
                            submit = true;
                        }
                    }

                    if (submit)
                    {
                        client.Submit();
                        this.UpdateTracker();
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private IdentifiedContactReference GetIdentifiedContactReference()
        {
            if (this._contact.IsNew || this._contact.Identifiers == null)
            {
                return null;
            }
            var identifier = this._contact.Identifiers.FirstOrDefault();
            if (identifier != null)
                return new IdentifiedContactReference(identifier.Source, identifier.Identifier);

            return null;
        }

        private bool IsNew()
        {
            return this._contact.IsNew;
        }

        private bool SetFacet<T>(T data, string key, Contact contact, Sitecore.XConnect.Client.XConnectClient client) where T : Facet
        {
            try
            {
                var r = client.SetFacet<Facet>(contact, key, data);
                return true;
            }
            catch (Exception e)
            {
            }

            return false;
        }

        private void UpdateTracker()
        {
            this._manager.RemoveFromSession(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
            Sitecore.Analytics.Tracker.Current.Session.Contact = this._manager.LoadContact(Sitecore.Analytics.Tracker.Current.Contact.ContactId);
        }
    }
}