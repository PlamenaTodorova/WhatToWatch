using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.JsonModels;

namespace WhatToWatch.Models.ViewModels
{
    public class BaseViewModel : IComparable<BaseViewModel>
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public int CurrentSeason { get; set; }

        public int CurrentEpisode { get; set; }

        public string PosterSource { get; set; }

        public string EpisodeTitle { get; set; }

        public DateTime? ReleaseDate { get; set; }

        public int CompareTo(BaseViewModel other)
        {
            throw new NotImplementedException();
        }

        public virtual void UpdateEpisodeInfo()
        {
        }

        public virtual void UpdateEpisodeInfo(List<EpisodeInfoJson> data)
        {
        }

        public virtual void UpdateEpisodeInfo(List<EpisodeInfoJson> episodeData, SeasonsDataJson totalData)
        {
        }
    }
}
