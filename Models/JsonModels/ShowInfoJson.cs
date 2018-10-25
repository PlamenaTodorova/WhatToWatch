namespace WhatToWatch.Models.JsonModels
{
    public class ShowInfoJson
    {
        public int id { get; set; }
        public string seriesName { get; set; }
        public string status { get; set; }
    }

    public class ShowInfoRoot
    {
        public ShowInfoJson data { get; set; }
    }
}
