using System;
using Newtonsoft.Json;

namespace CloneDB.Entities
{
    public class Person
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("sex")]
        public Sex Sex { get; set; }
        [JsonProperty("dob")]
        public DateTime DOB { get; set; }
        [JsonProperty("bio")]
        public string Bio { get; set; }
    }
}