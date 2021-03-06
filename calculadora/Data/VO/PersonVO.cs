using Calculadora.Hypermedia;
using Calculadora.Hypermedia.Abstract;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Calculadora.Data.VO
{
    public class PersonVO : ISupportHyperMedia
    {
        [JsonIgnore]
        public long Id { get; set; }
        [JsonPropertyName("name")]
        public string FirstName { get; set; }
        [JsonPropertyName("last_name")]
        public string LastName { get; set; }
        [JsonIgnore]
        public string Address { get; set; }
        [JsonPropertyName("sex")]
        public string Gender { get; set; }
        public List<HyperMediaLink> Links { get ; set; } = new List<HyperMediaLink>();
    }
}