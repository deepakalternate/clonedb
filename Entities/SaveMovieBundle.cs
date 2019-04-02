using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloneDB.Entities
{
    [Serializable]
    public class SaveMovieBundle
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("movieTitle")]
        public string MovieTitle { get; set; }
        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }
        [JsonProperty("plot")]
        public string Plot { get; set; }
        [JsonProperty("posterPath")]
        public string PosterPath { get; set; }
        [JsonProperty("producerId")]
        public int ProducerId { get; set; }
        [JsonProperty("actorIds")]
        public IEnumerable<int> ActorIds { get; set; }
    }
}