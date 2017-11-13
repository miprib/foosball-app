using System.Collections.Generic;
using System.Linq;
using MatchDetailsAPI.Interfaces;
using MatchDetailsAPI.Models;
using System;
using System.IO;
using Vid;

namespace MatchDetailsAPI.Services
{
    public class MatchDetailRepository : IMatchDetailRepository
    {
        GameList gameList;
        private List<MatchDetailItem> _matchList;
        public static String path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "All_games.txt");

        public MatchDetailRepository()
        {
            InitializeData();
        }

        public IEnumerable<MatchDetailItem> All
        {
            get { return _matchList; }
        }

        public bool DoesItemExist(string id)
        {
            return _matchList.Any(item => item.ID == id);
        }

        public MatchDetailItem Find(string id)
        {
            return _matchList.FirstOrDefault(item => item.ID == id);
        }

        public void Insert(MatchDetailItem item)
        {
            _matchList.Add(item);
        }

        public void Update(MatchDetailItem item)
        {
            var todoItem = this.Find(item.ID);
            var index = _matchList.IndexOf(todoItem);
            _matchList.RemoveAt(index);
            _matchList.Insert(index, item);
        }

        public void Delete(string id)
        {
            _matchList.Remove(this.Find(id));
        }

        private void InitializeData()
        {
            _matchList = new List<MatchDetailItem>();

            gameList = GameList.NewInstance();

            // Adding game history details to web service
            foreach (var game in gameList)
            {            
                var gameDataItem = new MatchDetailItem
                {
                    ID = game.id + 1 + "",
                    Date = game.date,
                    Team1 = game.Team1,
                    Team1Score = game.Team1Score,
                    Team2 = game.Team2,
                    Team2Score = game.Team1Score

                };
                _matchList.Add(gameDataItem);
            }
        }

    }
}