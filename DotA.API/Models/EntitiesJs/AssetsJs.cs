using Newtonsoft.Json;

namespace DotA.API.Models.EntitiesJs
{
    public class AssetJs
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("bytes")]
        public byte[] Blob { get; set; }
    }
}