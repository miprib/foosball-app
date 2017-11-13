using Newtonsoft.Json;
using System;
using System.IO;

namespace Foosball
{
    public class Game : IEquatable<Game>
    {
        public Game() { }

        public int id;
        public DateTime date;
        public String Team1;
        public String Team2;
        public int Team1Score;
        public int Team2Score;

        public bool Equals(Game other)
        {
            return this.id == other.id;
        }
    }
}
