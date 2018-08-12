using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TNet.Com.Model;
using TNet.Com.Rank;

namespace TNet.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public abstract class RankController : ControllerBase
    {
        // GET: api/Rank
        [HttpGet]
        public virtual IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Rank/5
        // 
        [HttpGet("{id}_{id2}", Name = "Get")]
        public virtual string Get(int id, int id2)
        {
            List<object> list = new List<object>();
            var ranking = GetRankingObject(id2);
            //if (ranking != null)
            //{
            //    int index = 0;
            //    var er = ranking.GetEnumerator();
            //    while (er.MoveNext())
            //    {
            //        if (index >= top)
            //        {
            //            break;
            //        }
            //        list.Add(er.Current);
            //        index++;
            //    }
            //}
            var stringbuilder = new System.Text.StringBuilder();
          foreach(var l in ranking)
            {
            if(l is RankingItem a)
                {
                    stringbuilder.AppendLine(a.RankId.ToString());
                }
            }
            stringbuilder.AppendLine(Runtime.GameZone.char_d);
            return stringbuilder.ToString();
           
        }
        /// <summary>
        /// Gets the ranking object.
        /// </summary>
        /// <returns>The ranking object.</returns>
        /// <param name="rankType">Rank type.</param>
        protected abstract IRanking GetRankingObject(int rankType);
        /// <summary>
        /// Rankings to json.
        /// </summary>
        /// <returns>The to json.</returns>
        /// <param name="dataList">Data list.</param>
        protected abstract string RankingToJson(IList<object> dataList);
        /// <summary>
        /// Rankings to html.
        /// </summary>
        /// <returns>The to html.</returns>
        /// <param name="dataList">Data list.</param>
        protected abstract string RankingToHtml(IList<object> dataList);

        // POST: api/Rank
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Rank/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
        [HttpGet("{rank}_{top}")]
        // GET: api/Rank/DoRanking/2017/888
        public IActionResult DoRanking(int rank,int top)
        {
            List<object> list = new List<object>();
            list.Add(rank);
            list.Add(top);
            return null;
        }
    }
}
