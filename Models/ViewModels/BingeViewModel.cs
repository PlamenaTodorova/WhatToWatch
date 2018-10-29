using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Utilities;

namespace WhatToWatch.Models.ViewModels
{
    public class BingeViewModel
    {
        public BingeViewModel(BingeShow show)
        {
            this.Id = show.Id;
            this.Title = show.Title;
            this.CurrentEpisode = show.CurrentEpisode;
            this.CurrentSeason = show.CurrentSeason;

            this.PosterSource = HelperFunctions.GetPosterSorce(show.Id, show.CurrentSeason);
        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int CurrentSeason { get; set; }

        public int TotalSeason { get; set; }

        public int CurrentEpisode { get; set;}

        public int TotalEpisodes { get; set; }

        public string PosterSource { get; set; }

        public string EpisodeTitle { get; set; }

        public void UpdateEpisodeInfo(List<EpisodeInfoJson> episodeData, SeasonsDataJson totalData)
        {
            if (episodeData != null && episodeData.Count >= this.CurrentEpisode)
            {
                EpisodeTitle = episodeData[this.CurrentEpisode - 1].episodeName;
                this.TotalEpisodes = episodeData.Count;

                this.TotalSeason = totalData.airedSeasons
                    .Select(e => int.Parse(e))
                    .OrderByDescending(e => e)
                    .FirstOrDefault();
            }
        }
    }
}
