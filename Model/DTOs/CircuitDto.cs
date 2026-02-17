using Newtonsoft.Json;

namespace APIS_Test.Dtos
{
    public sealed class CircuitDto
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("length")]
        public double? Length { get; set; }
    }
}
