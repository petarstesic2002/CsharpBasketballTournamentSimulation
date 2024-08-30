using BasketballZadatak.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer
{
    public static class ExhibitionResults
    {
        private static List<TeamDTO> Teams=new List<TeamDTO>();
        public static void InitializeTeamForms(Dictionary<string,List<ExhibitionDTO>> exh, Dictionary<string,List<TeamDTO>> teams)
        {
            if (exh == null)
            {
                throw new ArgumentNullException(null, "Exhibitions Dictionary is null.");
            }
            if (teams == null)
            {
                throw new ArgumentNullException(null, "Groups Dictionary is null.");
            }
            foreach(KeyValuePair<string,List<TeamDTO>> kvp in teams)
            {
                foreach(TeamDTO t in kvp.Value)
                {
                    Teams.Add(t);
                }
                foreach(TeamDTO team in kvp.Value)
                {
                    if(exh.TryGetValue(team.IsoCode, out List<ExhibitionDTO> results))
                    {
                        team.FormScore = CalculateInitialFormScore(results);
                    }
                    else
                    {
                        team.FormScore = 1.0;
                    }
                }
            }
        }
        private static double CalculateInitialFormScore(List<ExhibitionDTO> results)
        {
            double formScore = 1.0;
            foreach(ExhibitionDTO result in results)
            {
                int scoreDiff = GetScoreDifference(result.Result);
                double opponentStr = GetOpponentStrength(result.Opponent);
                formScore += scoreDiff / 10.0 * opponentStr;
            }
            return formScore;
        }
        private static int GetScoreDifference(string result)
        {
            int[] scores = result.Split('-').Select(int.Parse).ToArray();
            return Math.Abs(scores[0] - scores[1]);
        }
        private static double GetOpponentStrength(string opponentCode)
        {
            var opponent = Teams.FirstOrDefault(t => t.IsoCode == opponentCode);
            return opponent != null ? 1.0 / opponent.FibaRanking : 1.0;
        }
    }
}
