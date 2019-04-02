using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace CloneDB.Entities
{
    public class Movie
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("title")]
        public string Title { get; set; }
        [JsonProperty("plot")]
        public string Plot { get; set; }
        [JsonProperty("posterPath")]
        public string PosterPath { get; set; }
        [JsonProperty("actors")]
        public IEnumerable<Person> Actors { get; set; }
        [JsonProperty("producer")]
        public Person Producer { get; set; }
        [JsonProperty("releaseDate")]
        public DateTime ReleaseDate { get; set; }
    }
}