using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer.DTO
{
    public class TeamDTO
    {
        [JsonPropertyName("Team")]
        public string Name { get; set; }
        [JsonPropertyName("ISOCode")]
        public string IsoCode { get; set; }
        [JsonPropertyName("FIBARanking")]
        public int FibaRanking { get; set; }
        public int Points { get; set; }
        public int TotalScore { get; set; }
        public int ScoredNumber { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public int ScoredAgainstNumber { get; set; }
        public double FormScore { get; set; }
        public override string ToString()
        {
            return $"{DisplayName(),-12}{Wins} / {Losses} / {Points} / {TotalScore} / {ScoredNumber} / {ScoredAgainstNumber} / {ScoreDifferenceFormat()}";
        }
        public string DisplayName()
        {
            return this.Name.Contains(" ") ? this.IsoCode : this.Name;
        }
        public string ScoreDifferenceFormat()
        {
            return ScoredNumber - ScoredAgainstNumber > 0 ? $"+{ScoredNumber - ScoredAgainstNumber}" : $"{ScoredNumber - ScoredAgainstNumber}";
        }
    }
}
