using System.Text.Json.Serialization;

namespace APIS_Test.Dtos
{
    public sealed class CompetitorProfileDto
    {
        [JsonPropertyName("competitor")]
        public CompetitorDto Competitor { get; set; }
    }

    public sealed class CompetitorDto
    {
        [JsonPropertyName("id")]
        public string Id { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("nationality")]
        public string Nationality { get; set; }

        [JsonPropertyName("date_of_birth")]
        public string DateOfBirth { get; set; }

        [JsonPropertyName("number")]
        public string Number { get; set; }

        [JsonPropertyName("abbreviation")]
        public string Abbreviation { get; set; }

        [JsonPropertyName("team")]
        public TeamReferenceDto Team { get; set; }
    }
}
