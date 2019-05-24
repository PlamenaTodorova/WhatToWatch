using System.Collections.Generic;

namespace WhatToWatch.Models.JsonModels
{
    public class SearchInfoJson
    {
        public int id { get; set; }
        public string seriesName { get; set; }
        public string status { get; set; }
    }

    public class SearchInfoRoot
    {
        public List<SearchInfoJson> data { get; set; }
    }
}
