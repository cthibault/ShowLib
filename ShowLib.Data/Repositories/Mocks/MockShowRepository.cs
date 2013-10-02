using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShowLib.Data.Entities;

namespace ShowLib.Data.Repositories
{
    public class MocksShowRepository : IShowRepository
    {
        public async Task<bool> SaveShow(Show show)
        {
            if (show != null)
            {
                var dbShow = this.Shows.SingleOrDefault(s => s.Id == show.Id);

                if (dbShow == null)
                {
                    int id = this.Shows.Max(s => s.Id) + 1;
                    show.Id = id;
                    this.Shows.Add(show);
                }
                else
                {
                    dbShow = show;
                }
            }

            return true;
        }


        public async Task<IList<Show>> LoadAll()
        {
            var shows = new List<Show>();

            shows.Add(this.CreateShow(1));
            shows.Add(this.CreateShow(2));
            shows.Add(this.CreateShow(3));
            shows.Add(this.CreateShow(4));
            shows.Add(this.CreateShow(5));

            await this.LongRunningProcess();

            return shows;
        }

        public async Task<Show> Load(int id)
        {
            Show show = null;

            if (id > 0)
            {
                show = this.CreateShow(id);
            }

            return show;
        }
        public Task<Show> Load(ValueType id)
        {
            throw new NotImplementedException();
        }
        public Task<IList<Show>> Load(IEnumerable<int> ids)
        {
            throw new NotImplementedException();
        }
        public Task<IList<Show>> Load(IEnumerable<ValueType> ids)
        {
            throw new NotImplementedException();
        }
        public Task<Show> Load(Show entity)
        {
            throw new NotImplementedException();
        }

        public async Task<KeyValuePair<bool, IList<Show>>> Save(Show entity)
        {
            return await this.Save(new List<Show> { entity });
        }
        public async Task<KeyValuePair<bool, IList<Show>>> Save(IEnumerable<Show> entities)
        {
            if (entities != null && entities.Any())
            {
                foreach (var entity in entities)
                {
                    var show = this.Shows.SingleOrDefault(s => s.Id == entity.Id);

                    if (show == null)
                    {
                        int id = this.Shows.Max(s => s.Id) + 1;
                        entity.Id = id;
                        this.Shows.Add(entity);
                    }
                    else
                    {
                        show = entity;
                    }
                }
            }

            return new KeyValuePair<bool, IList<Show>>(true, entities.ToList());
        }

        public async Task<bool> Delete(int id)
        {
            return await this.Delete(new List<int> { id });
        }
        public async Task<bool> Delete(ValueType id)
        {
            throw new NotImplementedException();
        }
        public async Task<bool> Delete(IEnumerable<int> ids)
        {
            bool success = false;

            if (ids != null && ids.Any())
            {
                this.Shows.RemoveAll(s => ids.Contains(s.Id));

                success = true;
            }

            return success;
        }
        public async Task<bool> Delete(IEnumerable<ValueType> ids)
        {
            throw new NotImplementedException();
        }


        #region Private Methods
        private Show CreateShow(int showId)
        {
            var show = this.Shows.SingleOrDefault(s => s.Id == showId);

            if (show == null)
            {
                show = new Show(showId) { Title = "Show " + showId };

                this.CreateShowDetail(show);

                this.Shows.Add(show);
            }
            
            return show;
        }

        private ShowDetail CreateShowDetail(Show show)
        {
            ShowDetail showDetail = show != null ? show.ShowDetail : null;

            if (showDetail == null && show != null)
            {
                showDetail = new ShowDetail
                {
                    ImdbId = "S" + show.Id,
                    TvdbId = show.Id * 11,
                    Directory = @"C:\Shows\Show " + show.Id + @"\"
                };

                showDetail.Parsers.Add(new Parser(ParserType.Season)
                {
                    Pattern = "Show" + show.Id,
                    ExcludedCharacters = "Show"
                });

                showDetail.Parsers.Add(new Parser(ParserType.Episode)
                {
                    Pattern = "[Ee]dd.",
                    ExcludedCharacters = "Ee"
                });
            }

            return showDetail;
        }

        private async Task LongRunningProcess()
        {
            await this.LongRunningProcess(2000);
        }
        private async Task LongRunningProcess(int milliseconds)
        {
            await Task.Delay(milliseconds);
        }
        #endregion

        #region Private Properties
        private List<Show> _shows;
        private List<Show> Shows
        {
            get
            {
                if (this._shows == null)
                {
                    this._shows = new List<Show>();
                }
                return this._shows;
            }
        }
        #endregion
    }
}
