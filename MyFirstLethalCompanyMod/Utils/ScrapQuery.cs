using System;
using System.Collections.Generic;

namespace PompsUwuCompany.Utils
{
    public class ScrapQuery
    {
        private Func<GrabbableObject, bool> _predicate = _ => true;

        public ScrapQuery Where(Func<GrabbableObject, bool> predicate)
        {
            var previousPredicate = _predicate;
            _predicate = obj => previousPredicate(obj) && predicate(obj);
            return this;
        }

        public ScrapQuery Collected() => Where(x => x.isInShipRoom);
        public ScrapQuery InFactory() => Where(x => x.isInFactory);
        public ScrapQuery NotCollected() => Where(x => !x.isInShipRoom);
        public ScrapQuery NotInFactory() => Where(x => !x.isInFactory);
        public ScrapQuery Held() => Where(x => x.isHeld);
        public ScrapQuery OnGround() => Where(x => !x.isHeld && !x.isPocketed);
        public ScrapQuery Pocketed() => Where(x => x.isPocketed);
        public ScrapQuery MinValue(int min) => Where(x => x.scrapValue >= min);
        public ScrapQuery MaxValue(int max) => Where(x => x.scrapValue <= max);
        public ScrapQuery ValueBetween(int min, int max) => Where(x => x.scrapValue >= min && x.scrapValue <= max);
        public ScrapQuery TwoHanded() => Where(x => x.itemProperties.twoHanded);
        public ScrapQuery OneHanded() => Where(x => !x.itemProperties.twoHanded);
        public ScrapQuery NameContains(string text) =>
            Where(x => x.itemProperties.itemName.ToLower().Contains(text.ToLower()));
        public ScrapQuery NameEquals(string name) =>
            Where(x => x.itemProperties.itemName.Equals(name, StringComparison.OrdinalIgnoreCase));

        public List<GrabbableObject> Execute()
        {
            return ScrapUtils.GetScrapList(_predicate);
        }
    }
}