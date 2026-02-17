using Newtonsoft.Json;

namespace APIS_Test.Dtos
{
    public sealed class TeamProfileDto
    {
        [JsonProperty("team")]
        public TeamDto Team { get; set; }
    }

    public sealed class TeamDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("nationality")]
        public string Nationality { get; set; }

        [JsonProperty("founded")]
        public int? Founded { get; set; }

        [JsonProperty("base")]
        public string Base { get; set; }
    }
}
