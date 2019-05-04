namespace WhatToWatch.Utilities
{
    public static class Constants
    {
        //Requests paths
        public static string GetSeries = "https://api.thetvdb.com/series/{0}";
        public static string GetEpisode = "https://api.thetvdb.com/series/{0}/episodes/query?airedSeason={1}&airedEpisode={2}";
        public static string GetAllEpisodes = "https://api.thetvdb.com/series/{0}/episodes/query?airedSeason={1}";
        public static string GetSearch = "https://api.thetvdb.com/search/series?slug={0}";
        public static string GetTotal = "https://api.thetvdb.com/series/{0}/episodes/summary";
        public static string GetPoster = "https://api.thetvdb.com/series/{0}/images/query?keyType=season&subKey={1}";

        //Files paths
        public static string Folder = @"\Files";
        public static string FollowedFile = @".\Files\Show.json";
        public static string BingeFile = @".\Files\Binge.json";
    }
}
