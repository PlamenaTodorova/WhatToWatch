using System.Collections.Generic;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Utilities;
using System;

namespace WhatToWatch.Models.ViewModels
{
    public class ShowViewModel : IComparable<ShowViewModel>
    {
        public ShowViewModel(Show show)
        {
            this.Id = show.Id;
            this.Title = show.Title;
            this.CurrentEpisode = show.CurrentEpisode;
            this.CurrentSeason = show.CurrentSeason;
            this.Status = "None";
            this.RealStatus = show.Status.ToString();

            this.PosterSource = HelperFunctions.GetPosterSorce(show.Id, show.CurrentSeason);

        }

        public int Id { get; set; }

        public string Title { get; set; }

        public int CurrentSeason { get; set; }

        public int CurrentEpisode { get; set; }

        public string PosterSource { get; set; }

        public string Status { get; set; }

        public string EpisodeTitle { get; set; }

        public DateTime? ReleaseDate { get; set; }

        private string RealStatus { get; set; }

        public void UpdateEpisodeInfo(List<EpisodeInfoJson> data)
        {
            if (data != null && data.Count != 0)
            {
                EpisodeTitle = data[0].episodeName;
                if (data[0].firstAired != "" && data[0].firstAired != null)
                    ReleaseDate = DateTime.Parse(data[0].firstAired);
                else ReleaseDate = null;
            }

            if (IsDue())
                this.Status = this.RealStatus;
            else
                this.Status = "None";
        }

        public void UpdateEpisodeInfo()
        {
            this.EpisodeTitle = "";
            this.ReleaseDate = null;
            this.Status = "None";
        }

        public int CompareTo(ShowViewModel other)
        {
            if (this.ReleaseDate == null)
                return 1;

            if (other.ReleaseDate == null)
                return -1;

            if (this.IsDue() && other.IsDue())
            {
                if (this.Status == other.Status)
                {
                    if (this.ReleaseDate > other.ReleaseDate)
                        return 1;
                    return -1;
                }

                if (this.Status == "Red")
                    return -1;

                if (other.Status == "Red")
                    return 1;

                if (this.Status == "Yellow")
                    return 1;

                if (other.Status == "Yellow")
                    return -1;
            }

            if (this.ReleaseDate > other.ReleaseDate)
                return 1;

            return -1;
        }

        private bool IsDue()
        {
            if (ReleaseDate.HasValue)
                return ReleaseDate.Value.Date < DateTime.Now.Date;
            return false;
        }
    }
}