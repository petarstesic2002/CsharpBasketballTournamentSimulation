using BasketballZadatak.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer
{
    public static class EliminationPhase
    {
        private static Dictionary<string, List<QFRankDTO>> hats = new Dictionary<string, List<QFRankDTO>>()
                                                                {
                                                                    { "Šešir D", new List<QFRankDTO>() },
                                                                    { "Šešir E", new List<QFRankDTO>() },
                                                                    { "Šešir F", new List<QFRankDTO>() },
                                                                    { "Šešir G", new List<QFRankDTO>() }
                                                                };
        public static void GenerateHats(Dictionary<string, List<TeamDTO>> orderedGroups)
        {
            if (orderedGroups == null)
                throw new ArgumentNullException(null,"Ordered Groups Dictionary is null");
            RankTeams(orderedGroups);
            Console.WriteLine("Šeširi: ");
            foreach(KeyValuePair<string,List<QFRankDTO>> kvp in hats)
            {
                Console.WriteLine($"\t{kvp.Key}");
                foreach(QFRankDTO item in kvp.Value)
                {
                    Console.WriteLine($"\t\t{item.Team.DisplayName()}");
                }
            }
        }
        private static void RankTeams(Dictionary<string, List<TeamDTO>> orderedGroups)
        {
            List<QFRankDTO> firstRanks = new List<QFRankDTO>();
            List<QFRankDTO> secondRanks = new List<QFRankDTO>();
            List<QFRankDTO> thirdRanks = new List<QFRankDTO>();
            foreach(KeyValuePair<string,List<TeamDTO>> kvp in orderedGroups)
            {
                firstRanks.Add(new QFRankDTO
                {
                    Group = kvp.Key,
                    Team = kvp.Value[0],
                    GroupRank = 1
                });
            }
            foreach (KeyValuePair<string, List<TeamDTO>> kvp in orderedGroups)
            {
                secondRanks.Add(new QFRankDTO
                {
                    Group = kvp.Key,
                    Team = kvp.Value[1],
                    GroupRank = 2
                });
            }
            foreach (KeyValuePair<string, List<TeamDTO>> kvp in orderedGroups)
            {
                thirdRanks.Add(new QFRankDTO
                {
                    Group = kvp.Key,
                    Team = kvp.Value[2],
                    GroupRank=3
                });
            }
            firstRanks = firstRanks.OrderByDescending(x => x.Team.Points)
                                   .ThenByDescending(x => x.Team.ScoredNumber - x.Team.ScoredAgainstNumber)
                                   .ThenByDescending(x => x.Team.ScoredNumber)
                                   .ToList();
            secondRanks = secondRanks.OrderByDescending(x => x.Team.Points)
                                     .ThenByDescending(x => x.Team.ScoredNumber - x.Team.ScoredAgainstNumber)
                                     .ThenByDescending(x => x.Team.ScoredNumber)
                                     .ToList();
            thirdRanks = thirdRanks.OrderByDescending(x => x.Team.Points)
                                   .ThenByDescending(x => x.Team.ScoredNumber - x.Team.ScoredAgainstNumber)
                                   .ThenByDescending(x => x.Team.ScoredNumber)
                                   .ToList();
            MakeHatPairs(firstRanks, secondRanks, thirdRanks);
        }
        public static void MakeHatPairs(List<QFRankDTO> l1, List<QFRankDTO> l2, List<QFRankDTO> l3) //Generisanje sesira
        {
            if (l1 == null || l2 == null || l3 == null)
                throw new ArgumentNullException(null,"One of the lists is null.");
            List<QFRankDTO> allTeams = l1.Concat(l2).Concat(l3).ToList();
            foreach(QFRankDTO t in allTeams)
            {
                foreach (KeyValuePair<string, List<QFRankDTO>> kvp in hats)
                {
                    if (kvp.Value.Count < 2)
                    {
                        kvp.Value.Add(t);
                        break;
                    }
                }
            }
        }
        public static void Elimination()
        {
            //Uparivanje Timova D-G i E-F
            Random rnd = new Random();
            var matchups = new List<(QFRankDTO, QFRankDTO)>();
            List<(string, string)> hatMatchups = new List<(string, string)>()
                                            {
                                                ("Šešir D", "Šešir G"),
                                                ("Šešir E", "Šešir F")
                                            };
            foreach((string key1, string key2) in hatMatchups)
            {
                List<QFRankDTO> pairs1 = new List<QFRankDTO>(hats[key1]);
                List<QFRankDTO> pairs2 = new List<QFRankDTO>(hats[key2]);
                foreach(QFRankDTO team in pairs1)
                {
                    List<QFRankDTO> poteintialOpponents = pairs2.Where(t => t.Group != team.Group).ToList();
                    QFRankDTO opponent;

                    
                    if (poteintialOpponents.Any())
                    {
                        opponent = poteintialOpponents[rnd.Next(poteintialOpponents.Count)];
                    }
                    // Ako nema validnog protivnika, vracamo se na tim koji jos nije uparen
                    else if (pairs2.Any())
                    {
                        opponent = pairs2[rnd.Next(pairs2.Count)];
                    }
                    else
                    {
                        // Ako nema vise protivnika, menjamo protivnike sa drugim parom
                        continue;
                    }
                    matchups.Add((team, opponent));
                    pairs2.Remove(opponent);
                }
            }

            //U slucaju da neki timovi nisu upareni
            var unmatchedTeams = hats.SelectMany(h => h.Value).Where(t => !matchups.Any(m => m.Item1 == t || m.Item2 == t)).ToList();
            if (unmatchedTeams.Count > 0)
            {
                foreach (var team in unmatchedTeams)
                {
                    // Nalazi se par za uparivanje, uz pravilo da ne smeju biti iz iste grupe
                    var validPair = matchups.FirstOrDefault(pair => pair.Item1.Group != team.Group && pair.Item2.Group != team.Group);

                    if (validPair != default)
                    {
                        // Zamena protivnika
                        QFRankDTO newOpponent = validPair.Item2.Group != team.Group ? validPair.Item2 : validPair.Item1;
                        QFRankDTO otherOpponent = validPair.Item2 == newOpponent ? validPair.Item1 : validPair.Item2;

                        // Zamena starog para sa novim
                        matchups.Remove(validPair);
                        matchups.Add((team, newOpponent));
                        matchups.Add((validPair.Item1 == newOpponent ? validPair.Item2 : validPair.Item1, otherOpponent));
                    }
                }
            }

            //Ispis parova
            Console.WriteLine("\nEliminaciona faza: ");
            foreach ((QFRankDTO team1, QFRankDTO team2) in matchups){
                Console.WriteLine($"\t{team1.Team.DisplayName()} - {team2.Team.DisplayName()}");
            }

            //Generisanje utakmica u cetvrtfinalu
            Console.WriteLine("\nČetvrtfinale: ");
            List<QFRankDTO> winners = new List<QFRankDTO>();
            foreach ((QFRankDTO team1, QFRankDTO team2) in matchups)
            {
                Match m = new Match();
                Console.WriteLine($"\t{m.PlayMatch(team1.Team, team2.Team)}");
                winners.Add(m.Winner.Name == team1.Team.Name ? team1 : team2);
            }

            //Generisanje parova za polufinale
            matchups.Clear();
            List<QFRankDTO> set1 = winners.Where(w => hats["Šešir D"].Contains(w) || hats["Šešir G"].Contains(w)).ToList();
            List<QFRankDTO> set2 = winners.Where(w => hats["Šešir E"].Contains(w) || hats["Šešir F"].Contains(w)).ToList();
            for(int i = 0; i < set1.Count; i++)
            {
                QFRankDTO opponent1 = set1[i];
                QFRankDTO opponent2 = set2[rnd.Next(set2.Count)];
                matchups.Add((opponent1,opponent2));
                set2.Remove(opponent2);
            }

            //Generisanje utakmica u polufinalu
            Console.WriteLine("\nPolufinale: ");
            winners.Clear();
            List<QFRankDTO> losers = new List<QFRankDTO>();
            foreach((QFRankDTO team1, QFRankDTO team2) in matchups)
            {
                Match m = new Match();
                Console.WriteLine($"\t{m.PlayMatch(team1.Team, team2.Team)}");
                winners.Add(m.Winner.Name == team1.Team.Name ? team1 : team2);
                losers.Add(m.Winner.Name == team1.Team.Name ? team2 : team1);
            }

            if (losers.Count < 2)
            {
                throw new IndexOutOfRangeException();
            }
            Console.WriteLine("\nUtakmica za treće mesto: ");
            Match thirdPlace = new Match();
            Console.WriteLine($"\t{thirdPlace.PlayMatch(losers[0].Team, losers[1].Team)}");

            Console.WriteLine("\nFinale: ");
            Match finale = new Match();
            Console.WriteLine($"\t{finale.PlayMatch(winners[0].Team, winners[1].Team)}");

            Console.WriteLine("\nMedalje: ");
            Console.WriteLine($"\t1. {finale.Winner.DisplayName()}");
            Console.WriteLine($"\t2. {(finale.Winner.Name == winners[0].Team.Name ? winners[1].Team.DisplayName() : winners[0].Team.DisplayName())}");
            Console.WriteLine($"\t3. {thirdPlace.Winner.DisplayName()}");
        }
    }
}
