using System;
using Newtonsoft.Json;

namespace Authority.Operations.Configuration
{
    internal sealed class LogTargetConstants
    {
        public const string EventLog = "eventLog";
        public const string File = "file";
    }

    internal sealed class ModeConstants
    {
        public const string Single = "single";
        public const string Server = "server";
    }

    public sealed class AuthorityConfiguration
    {
        public static AuthorityConfiguration Default
        {
            get
            {
                return new AuthorityConfiguration
                {
                    LogTarget = LogTargetConstants.File,
                    Mode = ModeConstants.Single
                };
            }
        }

        public static AuthorityConfiguration FromJson(string json)
        {
            if (string.IsNullOrEmpty(json))
            {
                throw new ArgumentException("Invalid configuration JSON");
            }

            return JsonConvert.DeserializeObject<AuthorityConfiguration>(json);
        }

        [JsonProperty("logTarget")]
        public string LogTarget { get; set; }

        [JsonProperty("mode")]
        public string Mode { get; set; }
    }
}
