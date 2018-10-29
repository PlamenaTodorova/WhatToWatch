using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Models.JsonModels
{
    public class SeasonsDataJson
    {
        public List<string> airedSeasons { get; set; }
        public string airedEpisodes { get; set; }
    }

    public class SeasonDataRoot
    {
        public SeasonsDataJson data { get; set; }
    }
}
