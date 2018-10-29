using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using WhatToWatch.Models;
using WhatToWatch.Models.BindingModels;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Models.ViewModels;
using WhatToWatch.Parsers;
using WhatToWatch.Utilities;

namespace WhatToWatch.Controllers
{
    public class BingeController : IControllable
    {
        private List<BingeShow> tvShows;
        private ObservableCollection<BingeViewModel> views;

        public BingeController()
        {
            tvShows = JSONParser<List<BingeShow>>.ReadFile(Constants.BingeFile);
            GenerateViews();
        }

        #region Safe
        private void SaveShows()
        {
            JSONParser<List<BingeShow>>.WriteJson(tvShows, Constants.BingeFile);
            GenerateViews();
        }
        #endregion

        #region Get
        public ShowBindingModel GetShow(int id)
        {
            throw new NotImplementedException();
        }

        public ObservableCollection<BingeViewModel> GetShows()
        {
            return views;
        }

        private bool AddEpisodeInfo(BingeViewModel model)
        {
            string epPath = String.Format(Constants.GetAllEpisodes, model.Id, model.CurrentSeason);
            EpisodeInfoRoot ep;

            string totalPath = String.Format(Constants.GetTotal, model.Id);
            SeasonDataRoot total;

            try
            {
                ep = WebParser<EpisodeInfoRoot>.GetInfo(epPath);
                total = WebParser<SeasonDataRoot>.GetInfo(totalPath);
            }
            catch (Exception)
            {
                return false;
            }

            model.UpdateEpisodeInfo(ep.data, total.data);

            return true;
        }

        private void GenerateViews()
        {
            views = new ObservableCollection<BingeViewModel>();

            for (int i = 0; i < tvShows.Count; i++)
            {
                BingeViewModel newModel = new BingeViewModel(tvShows[i]);

                AddEpisodeInfo(newModel);

                views.Add(newModel);
            }
        }
        #endregion

        #region Add
        public bool AddShow(ShowBindingModel show)
        {
            string urlPath = String.Format(Constants.GetSearch, GetSlug(show.Name));

            SearchInfoRoot data = WebParser<SearchInfoRoot>.GetInfo(urlPath);

            BingeShow newShow = new BingeShow()
            {
                Id = data.data[0].id,
                Title = data.data[0].seriesName,
                CurrentEpisode = show.CurrentEpisode,
                CurrentSeason = show.CurrentSeason,
            };

            BingeViewModel newView = new BingeViewModel(newShow);

            if (!AddEpisodeInfo(newView))
                return false;

            Thread save = new Thread(AddAndSave);
            save.Start(newShow);

            views.Add(newView);

            return true;
        }

        private string GetSlug(string title)
        {
            return String.Join("-", Regex.Split(title.ToLower(), @"\W+"));
        }

        private void AddAndSave(object show)
        {
            tvShows.Add(((BingeShow)show));
            SaveShows();
        }
        #endregion

        #region Edit
        public bool EditShow(int id, ShowBindingModel show)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region Remove
        public void RemoveShow(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

        #region MoveThrough the episodes
        public bool NextEpisode(int id)
        {
            throw new NotImplementedException();
        }

        public bool PreviousEpisode(int id)
        {
            throw new NotImplementedException();
        }

        public bool NextSeason(int id)
        {
            throw new NotImplementedException();
        }
        #endregion

    }
}
