﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
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
    public class BingeController : BaseController
    {
        private List<BingeShow> tvShows;
        private ObservableCollection<BaseViewModel> views;

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
        public override ShowBindingModel GetShow(int id)
        {
            BingeShow chosen = tvShows.FirstOrDefault(e => e.Id == id);

            ShowBindingModel model = new ShowBindingModel()
            {
                Name = chosen.Title,
                CurrentEpisode = chosen.CurrentEpisode,
                CurrentSeason = chosen.CurrentSeason
            };

            return model;
        }

        public override ObservableCollection<BaseViewModel> GetShows()
        {
            return views;
        }

        private bool AddEpisodeInfo(BaseViewModel model)
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
            catch (HttpRequestException e)
            {
                if (e.HResult == 404)
                    return false;

                return true;
            }
            catch (Exception)
            {
                return false;
            }

            try
            {
                model.UpdateEpisodeInfo(ep.data, total.data);
            }
            catch (Exception)
            {
                return false;
            }
            
            return true;
        }

        public override void GenerateViews()
        {
            views = new ObservableCollection<BaseViewModel>();

            for (int i = 0; i < tvShows.Count; i++)
            {
                BingeViewModel newModel = new BingeViewModel(tvShows[i]);

                AddEpisodeInfo(newModel);

                views.Add(newModel);
            }
        }
        #endregion

        #region Add
        public override bool AddShow(ShowBindingModel show)
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

            if (BingeEnd(newView, newShow))
            {
                return false;
            }

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
        public override bool EditShow(int id, ShowBindingModel show)
        {
            BaseViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            BingeShow chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (toBeChanged != null)
            {
                views.Remove(toBeChanged);

                chosen.UpdateData(show);
                Thread save = new Thread(SaveShows);
                save.Start();

                toBeChanged = new BingeViewModel(chosen);

                if (BingeEnd(toBeChanged, chosen))
                    return false;

                return true;
            }

            return false;
        }
        #endregion

        #region Remove
        public override void RemoveShow(int id)
        {
            BaseViewModel toBeRemoved = views.FirstOrDefault(v => v.Id == id);

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
            BingeShow toBeRemoved = tvShows.FirstOrDefault(s => s.Id == id);

            tvShows.Remove(toBeRemoved);
            SaveShows();
        }
        #endregion

        #region MoveThrough the episodes
        public override bool NextEpisode(int id)
        {
            BaseViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            BingeShow chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (chosen != null)
            {
                chosen.CurrentEpisode++;
                toBeChanged.CurrentEpisode++;

                if (!AddEpisodeInfo(toBeChanged))
                {
                    chosen.CurrentEpisode = 1;
                    chosen.CurrentSeason++;
                    toBeChanged.CurrentEpisode = 1;
                    toBeChanged.CurrentSeason++;

                    if (BingeEnd(toBeChanged, chosen))
                        return false;
                }

                return true;
            }

            return false;
        }

        public override bool PreviousEpisode(int id)
        {
            throw new NotImplementedException();
        }

        public override bool NextSeason(int id)
        {
            BaseViewModel toBeChanged = views.FirstOrDefault(v => v.Id == id);
            BingeShow chosen = tvShows.FirstOrDefault(s => s.Id == id);

            if (chosen != null)
            {
                chosen.CurrentEpisode = 1;
                chosen.CurrentSeason++;
                toBeChanged.CurrentEpisode = 1;
                toBeChanged.CurrentSeason++;

                if (BingeEnd(toBeChanged, chosen))
                    return false;

                return true;
            }

            return false;
        }
        #endregion
        
        private bool BingeEnd(BaseViewModel model, BingeShow show)
        {
            if (!AddEpisodeInfo(model))
            {
                if (IsOngoing(show))
                {
                    MoveToFollow(show);
                    Thread remove = new Thread(PermanentlyRemove);
                    remove.Start(show.Id);
                }
                else
                {
                    Thread remove = new Thread(PermanentlyRemove);
                    remove.Start(show.Id);
                }

                return true;
            }

            return false;
        }

        private bool IsOngoing(BingeShow chosen)
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

        private void MoveToFollow(BingeShow show)
        {
            Show newShow = new Show()
            {
                Id = show.Id,
                CurrentEpisode = show.CurrentEpisode,
                CurrentSeason = show.CurrentSeason,
                Title = show.Title,
                Status = Status.Green
            };

            List<Show> followed = JSONParser<List<Show>>.ReadFile(Constants.FollowedFile);

            followed.Add(newShow);

            JSONParser<List<Show>>.WriteJson(followed, Constants.FollowedFile);
        }
    }
}
