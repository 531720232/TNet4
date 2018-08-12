using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TNet.Com.Model;
using TNet.Com.Rank;

namespace TNet.Controllers
{
    
 
    public class SampleRank : RankController
    {
        public class SRanking : Ranking<RankingItem>
        {
            public SRanking(string key):base(key)
            {

            }
        

            protected override int ComparerTo(RankingItem x, RankingItem y)
            {
                return 1;
            }

            protected override IList<RankingItem> GetCacheList()
            {
                var list = new List<RankingItem>();
                list.Add(new RankingItem() { RankId = 5317202 });
                return list;
            }
        }
        public override IEnumerable<string> Get()
        {
            return new string[] { "fy", "sd" };
        }
        protected override IRanking GetRankingObject(int rankType)
        {
            var rank= new SRanking(rankType.ToString() + "/" + Guid.NewGuid());
            rank.TryAppend(new RankingItem { RankId = 2017 });
            rank.TryAppend(new RankingItem { RankId = 1 });
            return rank;
       //     throw new NotImplementedException();
        }

        protected override string RankingToHtml(IList<object> dataList)
        {
            throw new NotImplementedException();
        }

        protected override string RankingToJson(IList<object> dataList)
        {
            throw new NotImplementedException();
        }
    }
}
