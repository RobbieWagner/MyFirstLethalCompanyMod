using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace PompsUwuCompany.Utils
{
    public class ScrapUtils
    {
        private static ScrapCache? _cachedScrap;
        private const float CACHE_DURATION = 5f;

        private class ScrapCache
        {
            public List<GrabbableObject> Items { get; set; }
            public float Timestamp { get; set; }
            public HashSet<int> InstanceIDs { get; set; }

            public ScrapCache(List<GrabbableObject> items)
            {
                Items = items;
                Timestamp = Time.time;
                InstanceIDs = new HashSet<int>(items.Select(x => x.GetInstanceID()));
            }

            public bool IsValid()
            {
                if (Time.time - Timestamp > CACHE_DURATION)
                    return false;

                foreach (int instanceID in InstanceIDs)
                {
                    if (!DoesInstanceIDExist(instanceID))
                        return false;
                }

                return true;
            }

            private bool DoesInstanceIDExist(int instanceID)
            {
                GrabbableObject[] objects = Resources.FindObjectsOfTypeAll<GrabbableObject>(); // TODO: Test for performance
                return objects.Any(obj => obj.GetInstanceID() == instanceID && obj != null);
            }
        }

        public static List<GrabbableObject> GetScrapList(Func<GrabbableObject, bool>? additionalFilter = null)
        {
            List<GrabbableObject> scrap = GetAllScrap();

            IEnumerable<GrabbableObject> filtered = scrap.AsEnumerable();

            if (additionalFilter != null)
                filtered = filtered.Where(additionalFilter);

            return filtered.ToList();
        }

        private static List<GrabbableObject> GetAllScrap()
        {
            if (_cachedScrap != null && _cachedScrap.IsValid())
                return _cachedScrap.Items;

            List<GrabbableObject> freshScrap = GameObject.FindObjectsOfType<GrabbableObject>() // TODO: Test for performance
                .Where(x => x != null && x.itemProperties.isScrap)
                .ToList();

            _cachedScrap = new ScrapCache(freshScrap);
            return freshScrap;
        }
    }
}