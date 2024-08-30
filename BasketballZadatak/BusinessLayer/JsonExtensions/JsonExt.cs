using BasketballZadatak.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BasketballZadatak.BusinessLayer.JsonExtensions
{
    public static class JsonExt<T>
    {
        public static Dictionary<string,T> getObjectFromJson(string fileName)
        {
            string json = File.ReadAllText(Path.Combine(@"./DataLayer/", fileName));
            if (JsonSerializer.Deserialize<Dictionary<string, T>>(json) == null)
                throw new NullReferenceException();
            return JsonSerializer.Deserialize<Dictionary<string,T>>(json);
        }
    }
}
