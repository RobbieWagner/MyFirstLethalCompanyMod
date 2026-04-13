using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace MyFirstLethalCompanyMod.Models
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    [System.Serializable]
    public enum UWUWordTag
    {
        NONE = -1,
        HAPPY = 0,
        BASHFUL = 1,
        DEVIOUS = 2,
        SAD = 3
    }

    [System.Serializable]
    public class UWUWord
    {
        public string? word;
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public List<UWUWordTag> tags = new List<UWUWordTag>();
    }
}
