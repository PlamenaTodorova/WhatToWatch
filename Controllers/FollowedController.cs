using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Net.Http;
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
                ShowViewModel newModel = new ShowViewModel(show);

                AddEpisodeInfo(newModel);

                helper.Add(newModel);
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

        private bool AddEpisodeInfo(ShowViewModel model)
        {
            string urlPath = String.Format(Constants.GetEpisode, model.Id, model.CurrentSeason, model.CurrentEpisode);
            EpisodeInfoRoot ep;

            try
            {
                ep = WebParser<EpisodeInfoRoot>.GetInfo(urlPath);
            }
            catch (Exception)
            {
                return false;
            }

            model.UpdateEpisodeInfo(ep.data);

            return true;
        }
        #endregion

        #region AddTVShow
        public bool AddShow(ShowBindingModel show)
        {
            string urlPath = String.Format(Constants.GetSearch, GetSlug(show.Name));

            SearchInfoRoot data = WebParser<SearchInfoRoot>.GetInfo(urlPath);
            
            Show newShow = new Show()
            {
                Id = data.data[0].id,
                Title = data.data[0].seriesName,
                CurrentEpisode = show.CurrentEpisode,
                CurrentSeason = show.CurrentSeason,
                Status = show.Status
            };

            ShowViewModel newView = new ShowViewModel(newShow);

            if (!AddEpisodeInfo(newView))
                if (data.data[0].status != "Continuing")
                    return false;
            
            Thread save = new Thread(AddAndSave);
            save.Start(newShow);
            
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

                toBeChanged = new ShowViewModel(chosen);
                if (!AddEpisodeInfo(toBeChanged))
                    if (!IsOngoing(chosen))
                        return false;
                
                HelperFunctions.PutInTheRightPlace<ShowViewModel>(views, toBeChanged);

                return true;
            }

            return false;
        }

        private bool IsOngoing(Show chosen)
        {
            string path = String.Format(Constants.GetSeries, chosen.Id);

            try
            {
                ShowInfoRoot data = WebParser<ShowInfoRoot>.GetInfo(path);

                if (data.data.status == "Continuing")
                    return true;
            }
            catch (Exception)
            {
                //Unautorized
                return false;
            }

            return false;
        }
        #endregion

        #region NextEpisode
        public bool NextEposode(int id)
        {
            ShowViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            Show chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (chosen != null)
            {
                views.Remove(toBeChanged);

                chosen.CurrentEpisode++;
                Thread save = new Thread(SaveShows);
                save.Start();

                toBeChanged = new ShowViewModel(chosen);
                AddEpisodeInfo(toBeChanged);

                HelperFunctions.PutInTheRightPlace<ShowViewModel>(views, toBeChanged);

                return true;
            }

            return false;
        }
        #endregion
    }
}
