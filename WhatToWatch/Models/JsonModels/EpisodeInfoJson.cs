using System.Collections.Generic;

namespace WhatToWatch.Models.JsonModels
{
    public class EpisodeInfoJson
    {
        public int id { get; set; }
        public int airedSeason { get; set; }
        public int airedEpisodeNumber { get; set; }
        public string episodeName { get; set; }
        public string firstAired { get; set; }
        public int seriesId { get; set; }
    }

    public class EpisodeInfoRoot
    {
        public List<EpisodeInfoJson> data { get; set; }
    }
}
