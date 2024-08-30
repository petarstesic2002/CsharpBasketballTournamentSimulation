using BasketballZadatak.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer
{
    public static class GroupStage
    {
        private static List<Match> matches = new List<Match>();
        public static void GenerateGroupMatches(Dictionary<string, List<TeamDTO>> groups)
        {
            if (groups == null)
                throw new ArgumentNullException(null,"Groups Dictionary is null");
            foreach (KeyValuePair<string, List<TeamDTO>> kvp in groups)
            {
                Console.WriteLine($"\tGrupa {kvp.Key}: ");
                if (kvp.Value.Count < 2)
                {
                    throw new ArgumentOutOfRangeException();
                }
                for (int i = 0; i < kvp.Value.Count; i++)
                {
                    for (int j = i + 1; j < kvp.Value.Count; j++)
                    {
                        Match m = new Match();
                        Console.WriteLine($"\t\t{m.PlayMatch(kvp.Value[i], kvp.Value[j])}");
                        matches.Add(m);
                    }
                }
            }
            Console.WriteLine("\n");
        }
        public static Dictionary<string,List<TeamDTO>> RankGroups(Dictionary<string, List<TeamDTO>> groups)
        {
            // U slucaju da treba rematch u slucaju jednakih poena u grupi
            //CheckAndRematchEqualInGroups(groups);

            Console.WriteLine("Konačan plasman u grupama: ");
            Dictionary<string, List<TeamDTO>> orderedGroupDictionary = new Dictionary<string, List<TeamDTO>>();
            foreach (KeyValuePair<string, List<TeamDTO>> kvp in groups)
            {
                List<TeamDTO> teams = kvp.Value.OrderByDescending(x => x.Points).ToList();
                teams = ResolveTies(teams);
                Console.WriteLine($"\tGrupa {kvp.Key} (Ime - pobede/porazi/bodovi/postignuti koševi/primljeni koševi/koš razlika)::");
                int i = 0;
                foreach (TeamDTO team in teams)
                {
                    string rankName = i + 1 + ". " + team.DisplayName();
                    Console.WriteLine($"\t{i+1,5}. {team.ToString()}");
                    i++;
                }
                orderedGroupDictionary[kvp.Key] = teams;
            }
            return orderedGroupDictionary;
        }

        //U slucaju jednakih poena
        public static List<TeamDTO> ResolveTies(List<TeamDTO> teams)
        {
            List<TeamDTO> sortedTeams = new List<TeamDTO>(teams);
            List<TeamDTO> samePoints=teams.GroupBy(t => t.Points)
                                          .Where(g => g.Count() > 1)
                                          .SelectMany(g => g)
                                          .Take(3)
                                          .ToList();
            if (samePoints.Count == 2) //AKO IH IMA 2
            {
                for (int i = 0; i < samePoints.Count; i++)
                {
                    for (int j = i + 1; j < samePoints.Count; j++)
                    {
                        if (sortedTeams[i].Points == sortedTeams[j].Points)
                        {
                            TeamDTO team1 = sortedTeams[i];
                            TeamDTO team2 = sortedTeams[j];

                            Match match = matches.FirstOrDefault(m => (m.Winner == team1 && m.Loser == team2) || (m.Winner == team2 && m.Loser == team1));

                            if (match != null)
                            {
                                if (match.Winner == team1)
                                {
                                    sortedTeams[i] = team2;
                                    sortedTeams[j] = team1;
                                }
                            }
                        }
                    }
                }
            }
            if (samePoints.Count == 3) //AKO IH IMA 3
            {
                int k = 0;
                while (k < samePoints.Count - 2)
                {
                    if (sortedTeams[k].Points == sortedTeams[k + 1].Points && sortedTeams[k + 2].Points == sortedTeams[k].Points)
                    {
                        TeamDTO team1 = sortedTeams[k];
                        TeamDTO team2 = sortedTeams[k + 1];
                        TeamDTO team3 = sortedTeams[k + 2];
                        List<TeamDTO> involvedTeams = new List<TeamDTO> { team1, team2, team3 };

                        double team1Diff = CalculatePointDifference(team1, involvedTeams);
                        double team2Diff = CalculatePointDifference(team2, involvedTeams);
                        double team3Diff = CalculatePointDifference(team3, involvedTeams);

                        var diffs = new List<(double, TeamDTO)>
                        {
                            (team1Diff, team1),
                            (team2Diff, team2),
                            (team3Diff, team3)
                        };
                        diffs.Sort((a, b) => b.Item1.CompareTo(a.Item1));

                        sortedTeams[k] = diffs[0].Item2;
                        sortedTeams[k + 1] = diffs[1].Item2;
                        sortedTeams[k + 2] = diffs[2].Item2;
                    }

                    k++;
                }
            }

            return sortedTeams;
        }

        private static double CalculatePointDifference(TeamDTO team, List<TeamDTO> teams)
        {
            double pointDifference = 0.0;
            foreach (var opponent in teams)
            {
                if (team != opponent)
                {
                    Match match = matches.FirstOrDefault(m => (m.Winner == team && m.Loser == opponent) || (m.Winner == opponent && m.Loser == team));
                    if (match != null)
                    {
                        if (match.Winner == team)
                        {
                            pointDifference += match.Team1Score - match.Team2Score;
                        }
                        else
                        {
                            pointDifference += match.Team2Score - match.Team1Score;
                        }
                    }
                }
            }
            return pointDifference;
        }

        // U slucaju da treba rematch u slucaju jednakih poena u grupi
        //private static void CheckAndRematchEqualInGroups(Dictionary<string, List<TeamDTO>> groups)
        //{
        //    Dictionary<string, List<TeamDTO>> tmpDictionary = new Dictionary<string, List<TeamDTO>>();
        //    List<TeamDTO> list = new List<TeamDTO>();
        //    foreach (KeyValuePair<string, List<TeamDTO>> kvp in groups)
        //    {
        //        if (kvp.Value.Count < 2)
        //        {
        //            throw new ArgumentOutOfRangeException();
        //        }
        //        list = kvp.Value.OrderByDescending(x => x.Points).ToList();
        //        List<TeamDTO> teamsWithSamePoints = list.GroupBy(t => t.Points)
        //                                                .Where(g => g.Count() > 1)
        //                                                .SelectMany(g => g)
        //                                                .Take(3)
        //                                                .ToList();
        //        if (teamsWithSamePoints.Any())
        //        {
        //            tmpDictionary.Add(kvp.Key, teamsWithSamePoints);
        //            Console.WriteLine($"Utakmice usled jednakih poena: ");
        //            GenerateGroupMatches(tmpDictionary);
        //            tmpDictionary.Clear();
        //        }
        //    }
        //}
    }
}
