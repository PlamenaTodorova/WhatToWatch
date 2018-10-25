using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using WhatToWatch.Models;
using WhatToWatch.Models.BindingModels;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Models.ViewModels.WhatToWatch.Models.ViewModels;
using WhatToWatch.Parsers;
using WhatToWatch.Utilities;

namespace WhatToWatch.Controllers
{
    public class FollowedController
    {
        private List<Show> tvShows;
        private ObservableCollection<ShowViewModel> views;

        public FollowedController()
        {
            tvShows = JSONParser<List<Show>>.ReadFile(Constants.FollowedFile);
        }

        #region Safe
        private void SaveShows()
        {
            JSONParser<List<Show>>.WriteJson(tvShows, Constants.FollowedFile);
        }
        #endregion

        #region ReturnAll
        public ObservableCollection<ShowViewModel> GetShows()
        {
            List<ShowViewModel> helper = new List<ShowViewModel>();

            foreach (Show show in tvShows)
            {
                helper.Add(CreateView(show));
            }

            helper.Sort();

            views = new ObservableCollection<ShowViewModel>(helper);

            return views;
        }

        public ShowBindingModel GetShow(int id)
        {
            Show chosen = tvShows.FirstOrDefault(e => e.Id == id);

            ShowBindingModel model = new ShowBindingModel()
            {
                Name = chosen.Title,
                CurrentEpisode = chosen.CurrentEpisode,
                CurrentSeason = chosen.CurrentSeason,
                Status = chosen.Status
            };

            return model;
        }

        private ShowViewModel CreateView(Show show)
        {
            string urlPath = String.Format(Constants.GetEpisode, show.Id, show.CurrentSeason, show.CurrentEpisode);
            EpisodeInfoRoot ep = WebParser<EpisodeInfoRoot>.GetInfo(urlPath);

            ShowViewModel model = new ShowViewModel(show);
            model.UpdateEpisodeInfo(ep.data);

            return model;
        }
        #endregion

        #region AddTVShow
        public bool AddShow(ShowBindingModel show)
        {
            string urlPath = String.Format(Constants.GetSearch, GetSlug(show.Name));

            SearchInfoRoot data = WebParser<SearchInfoRoot>.GetInfo(urlPath);

            if (data.data.Count == 0)
                return false; //Show not found

            Show newShow = new Show()
            {
                Id = data.data[0].id,
                Title = data.data[0].seriesName,
                CurrentEpisode = show.CurrentEpisode,
                CurrentSeason = show.CurrentSeason,
                Status = show.Status
            };

            Thread save = new Thread(AddAndSave);
            save.Start(newShow);

            ShowViewModel newView = CreateView(newShow);

            //Insert the view into the collection
            HelperFunctions.PutInTheRightPlace<ShowViewModel>(views, newView);

            return true;
        }

        private string GetSlug(string title)
        {
            return String.Join("-", Regex.Split(title.ToLower(), @"\W+"));
        }

        private void AddAndSave(object show)
        {
            tvShows.Add(((Show)show));
            SaveShows();
        }
        #endregion

        #region RemoveTVShow
        public void RemoveShow(int id)
        {
            ShowViewModel toBeRemoved = views.FirstOrDefault(v => v.Id == id);

            if (toBeRemoved != null)
            {
                views.Remove(toBeRemoved);

                Thread remove = new Thread(PermanentlyRemove);
                remove.Start(id);
            }
        }

        private void PermanentlyRemove(object param)
        {
            int id = (int)param;
            Show toBeRemoved = tvShows.FirstOrDefault(s => s.Id == id);

            tvShows.Remove(toBeRemoved);
            SaveShows();
        }
        #endregion

        #region EditTVShow
        public bool EditShow(int id, ShowBindingModel show)
        {
            ShowViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            Show chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (toBeChanged != null)
            {
                views.Remove(toBeChanged);

                chosen.UpdateData(show);
                Thread save = new Thread(SaveShows);
                save.Start();

                toBeChanged = CreateView(chosen);

                HelperFunctions.PutInTheRightPlace<ShowViewModel>(views, toBeChanged);

                return true;
            }

            return false;
        }
        #endregion

        #region NextEpisode
        public bool NextEposode(int id)
        {
            ShowViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            Show chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (toBeChanged != null)
            {
                views.Remove(toBeChanged);

                chosen.CurrentEpisode++;
                Thread save = new Thread(SaveShows);
                save.Start();

                toBeChanged = CreateView(chosen);

                HelperFunctions.PutInTheRightPlace<ShowViewModel>(views, toBeChanged);

                return true;
            }

            return false;
        }
        #endregion
    }
}