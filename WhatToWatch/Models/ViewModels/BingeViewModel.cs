using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Utilities;

namespace WhatToWatch.Models.ViewModels
{
    public class BingeViewModel : BaseViewModel
    {
        public BingeViewModel(BingeShow show)
        {
            this.Id = show.Id;
            this.Title = show.Title;
            this.CurrentEpisode = show.CurrentEpisode;
            this.CurrentSeason = show.CurrentSeason;

            this.PosterSource = HelperFunctions.GetPosterSorce(show.Id, show.CurrentSeason);
        }

        public int TotalSeason { get; set; }

        public int TotalEpisodes { get; set; }

        public override void UpdateEpisodeInfo(List<EpisodeInfoJson> episodeData, SeasonsDataJson totalData)
        {
            if (episodeData != null && episodeData.Count >= this.CurrentEpisode)
            {
                string firstAired = episodeData[this.CurrentEpisode - 1].firstAired;

                if (firstAired == "" && firstAired == null)
                    throw new Exception("No bingable episodes");

                if (firstAired != "" && firstAired != null)
                {
                    DateTime date = DateTime.Parse(firstAired);
                    if (date > DateTime.Today)
                        throw new Exception("No bingable episodes");
                }

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
