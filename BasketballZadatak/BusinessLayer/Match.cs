using BasketballZadatak.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer
{
    public class Match
    {
        private int team1Score;
        private int team2Score;
        private int fieldGoals1;
        private int fieldGoals2;
        private TeamDTO winner;
        private TeamDTO loser;
        public Match()
        {
            team1Score = 0;
            team2Score = 0;
            fieldGoals1 = 0;
            fieldGoals2 = 0;
            winner = null;
            loser = null;
        }

        public TeamDTO Winner { get => winner; }
        public TeamDTO Loser { get => loser; }
        public int Team1Score { get => team1Score; }
        public int Team2Score { get => team2Score; }
        public string PlayMatch(TeamDTO Team1, TeamDTO Team2)
        {

            if (Team1 == null)
                throw new ArgumentNullException(null, "Team 1 is null");
            if (Team2 == null)
                throw new ArgumentNullException(null, "Team 2 is null");

            var random=new Random();
            
            int randomShots = random.Next(70, 90); //Nasumican broj koseva u utakmici
            int surrender = 0;
            for (int i = 0; i <= randomShots; i++)
            {
                int fieldGoal = random.Next(2, 3); //Nasumicno se generise da li je kos dvojka ili trojka
                bool team1Scores = random.NextDouble() < scoreProbability(Team1, Team2);

                //Ako vodeci tim ima 35+ poena vise od tima koji gubi, postoji sansa da ce se tim sa manje poena predati
                if (team1Score > team2Score + 35 && random.NextDouble() < 0.001)
                {
                    surrender = 1;
                    Team1.Points += 2;
                    Team2.Points += 0;
                }
                if (team2Score > team1Score + 35 && random.NextDouble() < 0.001)
                {
                    surrender = 2;
                    Team2.Points += 2;
                    Team1.Points += 0;
                }
                if (team1Scores)
                {
                    fieldGoals1++;
                    team1Score += fieldGoal;
                }
                else
                {
                    fieldGoals2++;
                    team2Score += fieldGoal;
                }
                
            }

            Team1.TotalScore += team1Score;
            Team1.ScoredNumber += fieldGoals1;
            Team1.ScoredAgainstNumber += fieldGoals2;

            Team2.TotalScore += team2Score;
            Team2.ScoredNumber += fieldGoals2;
            Team2.ScoredAgainstNumber += fieldGoals1;

            if (surrender == 1)
            {
                winner = Team1;
                loser = Team2;
                Team1.FormScore += 0.01;
                return $"{Team1.DisplayName()} - {Team2.DisplayName()} ({team1Score}:{team2Score}-Predaja)";
            }
            if (surrender == 2)
            {
                winner = Team2;
                loser = Team1;
                Team1.FormScore += 0.01;
                return $"{Team1.DisplayName()} - {Team2.DisplayName()} ({team1Score}-Predaja:{team2Score})";
            }

            if (team1Score == team2Score) //Ako je izjednaceno, nasumican tim pobedjuje
            {
                int wins = random.Next(1, 2);
                if (wins == 1)
                    team1Score += 2;
                else
                    team2Score += 2;
            }
            if (team1Score > team2Score)
            {
                Team1.Points += 2;
                Team2.Points ++;
                Team1.Wins++;
                Team2.Losses++;
                winner = Team1;
                loser = Team2;
                Team1.FormScore += 0.01 + (team1Score-team2Score) / 1000.0;
            }
            else
            {
                Team1.Points ++;
                Team2.Points += 2;
                Team2.Wins++;
                Team1.Losses++;
                winner = Team2;
                loser = Team1;
                Team2.FormScore += 0.01 + (team2Score - team1Score) / 1000.0;
            }

            return $"{Team1.DisplayName()} - {Team2.DisplayName()} ({team1Score}:{team2Score})";
        }

        private static double scoreProbability(TeamDTO t1, TeamDTO t2)
        {
            double probability = (double)t2.FibaRanking / (t1.FibaRanking + t2.FibaRanking);
            double formDifference = t1.FormScore - t2.FormScore;

            double biasTowardsWeakerTeam = 0.05;

            double chanceFactor = formDifference + biasTowardsWeakerTeam;

            chanceFactor = Math.Clamp(chanceFactor, 0.45, 0.65);

            return probability * (1 - chanceFactor) + (1 - probability) * chanceFactor;
        }

        //Bez uracunavanja forme
        //private static double scoreProbability(TeamDTO t1, TeamDTO t2)
        //{
        //    double probability = (double)t2.FibaRanking / (t1.FibaRanking + t2.FibaRanking);
        //    double chanceFactor = 0.45;
        //    return probability * (1 - chanceFactor) + (1 - probability) * chanceFactor;
        //}
    }
}
