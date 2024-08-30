using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer.DTO
{
    public class QFRankDTO
    {
        public TeamDTO Team { get; set; }
        public int GroupRank {  get; set; }
        public string Group { get; set; }
    }
}
