using Newtonsoft.Json;

namespace Authority.Operations.Logging
{
    public sealed class LogConfiguration
    {
        [JsonProperty("target")]
        public string Target { get; set; }
    }
}
