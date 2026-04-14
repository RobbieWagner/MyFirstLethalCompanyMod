using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace MyFirstLethalCompanyMod.Models
{
    [System.Serializable]
    [JsonConverter(typeof(StringEnumConverter))]
    public enum UWUWordTag
    {
        NONE = -1,
        HAPPY = 0,
        BASHFUL = 1,
        DEVIOUS = 2,
        SAD = 3,
        UPSET = 4,
        SERIOUS = 5
    }

    [System.Serializable]
    public class UWUWord
    {
        public string? word;

        [JsonProperty(ItemConverterType = typeof(StringEnumConverter))]
        public List<UWUWordTag> tags = new List<UWUWordTag>();
    }
}