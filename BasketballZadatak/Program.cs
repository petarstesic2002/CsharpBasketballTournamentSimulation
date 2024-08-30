
using BasketballZadatak.BusinessLayer;
using BasketballZadatak.BusinessLayer.DTO;
using BasketballZadatak.BusinessLayer.JsonExtensions;
using System.Text;
using System;
class Program
{
    static void Main()
    {
        try
        {
            Console.OutputEncoding = UTF8Encoding.UTF8;

            //Dohvatanje groups.json
            Dictionary<string, List<TeamDTO>> dc = JsonExt<List<TeamDTO>>.getObjectFromJson("groups.json");

            //Dohvatanje exhibitions.json
            Dictionary<string, List<ExhibitionDTO>> ex = JsonExt<List<ExhibitionDTO>>.getObjectFromJson("exibitions.json");

            ExhibitionResults.InitializeTeamForms(ex,dc);

            //Simulacija utakmica u grupnoj fazi
            Console.WriteLine("Grupna faza - I kolo: ");
            GroupStage.GenerateGroupMatches(dc);

            //Rangiranje po grupama
            Dictionary<string, List<TeamDTO>> orderedGroups = GroupStage.RankGroups(dc);

            //Generisanje sesira za cetvrt finale
            EliminationPhase.GenerateHats(orderedGroups);

            //Eliminaciona faza
            EliminationPhase.Elimination();

        }
        catch (Exception e)
        {
            Console.WriteLine(e.Message);
        }
    }
}