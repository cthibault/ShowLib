using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entity;

namespace ShowLib.Data.Repository
{
    public class MocksShowRepository : IShowRepository
    {
        public async Task<IEnumerable<Show>> GetShows()
        {
            var shows = new List<Show>();

            shows.Add(this.CreateShow(1));
            shows.Add(this.CreateShow(2));
            shows.Add(this.CreateShow(3));
            shows.Add(this.CreateShow(4));
            shows.Add(this.CreateShow(5));

            await this.ExecuteLongRunningProcess();
            
            return shows;
        }

        public async Task<Show> GetShowDetail(int showId)
        {
            Show show = null;

            if (showId > 0)
            {
                show = this.CreateShow(showId);

                show = await this.GetShowDetail(show);
            }

            return show;
        }

        public async Task<Show> GetShowDetail(Show show)
        {
            if (show != null)
            {
                show.ShowDetail = this.CreateShowDetail(show.Id);
            }

            return show;
        }


        #region Public Test Methods
        public Func<Task> LongRunningProcess { get; set; }
        #endregion

        #region Private Methods
        private Show CreateShow(int showId)
        {
            return new Show(showId) { Title = "Show " + showId };
        }

        private ShowDetail CreateShowDetail(int showId)
        {
            var showDetail = new ShowDetail
            {
                ImdbId = "S" + showId,
                TvdbId = showId * 11,
                Directory = @"C:\Shows\Show " + showId + @"\"
            };

            showDetail.Parsers.Add(new Parser(ParserType.Season)
            {
                Pattern = "Show" + showId,
                ExcludedCharacters = "Show"
            });

            showDetail.Parsers.Add(new Parser(ParserType.Episode) 
            { 
                Pattern = "[Ee]dd.", 
                ExcludedCharacters = "Ee" 
            });

            return showDetail;
        }

        private async Task ExecuteLongRunningProcess()
        {
            if (this.LongRunningProcess != null)
            {
                await this.LongRunningProcess.Invoke();
            }
        }
        #endregion
    }
}
