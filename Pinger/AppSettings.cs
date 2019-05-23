using Newtonsoft.Json;

namespace Pinger
{
    [JsonObject("AppSettings")]
    class AppSettings
    {
        [JsonProperty("HostFileName")]
        public string HostFileName { get; set; }
        [JsonProperty("SectionName")]
        public string SectionName { get; set; }
    }
}
