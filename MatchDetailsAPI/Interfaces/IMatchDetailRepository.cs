using System.Collections.Generic;
using MatchDetailsAPI.Models;


namespace MatchDetailsAPI.Interfaces
{
    public interface IMatchDetailRepository
    {
        bool DoesItemExist(string id);
        IEnumerable<MatchDetailItem> All { get; }
        MatchDetailItem Find(string id);
        void Insert(MatchDetailItem item);
        void Update(MatchDetailItem item);
        void Delete(string id);
    }
}