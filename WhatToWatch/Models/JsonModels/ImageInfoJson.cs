using System.Collections.Generic;

namespace WhatToWatch.Models.JsonModels
{

    public class RatingsInfo
    {
        public double average { get; set; }
    }

    public class ImageInfoJson
    {
        public int id { get; set; }
        public string fileName { get; set; }
        public RatingsInfo ratingsInfo { get; set; }
    }

    public class ImageInfoRoot
    {
        public List<ImageInfoJson> data { get; set; }
    }
}
