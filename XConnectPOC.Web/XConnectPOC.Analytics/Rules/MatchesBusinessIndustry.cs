using Sitecore.Diagnostics;
using Sitecore.Rules;
using Sitecore.Rules.Conditions;
using XConnect.Personalization.Providers;
using XConnectPOC.Analytics.Providers;
using XConnectPOC.Collection.Model.Facets;

namespace XConnectPOC.Analytics.Rules
{
    public class MatchesBusinessIndustry<T> : WhenCondition<T> where T : RuleContext
    {
        public string Value { get; set; }

        private IContactFacetsProvider _contactFacetsProvider;

        public MatchesBusinessIndustry()
        {
            _contactFacetsProvider = new ContactFacetsProvider();
        }

        protected override bool Execute(T ruleContext)
        {
            Assert.ArgumentNotNull((object)ruleContext, nameof(ruleContext));
            System.Guid businessIndustryId;
            if (!string.IsNullOrEmpty(this.Value) && System.Guid.TryParse(this.Value, out businessIndustryId))
            {
                BusinessIndustry businessIndustry = this._contactFacetsProvider.GetFacet<BusinessIndustry>(BusinessIndustry.DefaultFacetKey);
                if (businessIndustry != null)
                {
                    return businessIndustryId.Equals(businessIndustry.Id);
                }
            }
            return false;
        }
    }
}