using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Foosball
{
    public class GameList : List<Game>
    {
        public static String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "All_games.txt");

        public GameList() { }

        public static GameList NewInstance()
        {
            if (!File.Exists(path))
            {
                File.Create(path).Close();
            }
            String json = File.ReadAllText(path);
            GameList gameList = JsonConvert.DeserializeObject<GameList>(json);
            if (gameList == null)
            {
                gameList = new GameList();
            }
            return gameList;
        }

        public void SaveList()
        {
            string json = JsonConvert.SerializeObject(this);
            File.WriteAllText(path, json);
        }

        public int NextId()
        {
            return this.Count;
        }
    }
}
