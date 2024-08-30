using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer.DTO
{
    public class ExhibitionDTO
    {
        [JsonPropertyName("Date")]
        public string Date { get; set; }
        [JsonPropertyName("Opponent")]
        public string Opponent { get; set; }
        [JsonPropertyName("Result")]
        public string Result { get; set; }
    }
}
