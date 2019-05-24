using System;
using System.Collections.ObjectModel;
using System.Linq;
using WhatToWatch.Models.JsonModels;
using WhatToWatch.Parsers;

namespace WhatToWatch.Utilities
{
    public static class HelperFunctions
    {
        public static void PutInTheRightPlace<T>(ObservableCollection<T> collection, T element)
            where T : IComparable<T>
        {
            int place = FindRightPlace(collection, element);

            collection.Insert(place, element);
        }

        private static int FindRightPlace<T>(ObservableCollection<T> collection, T element)
            where T : IComparable<T>
        {
            int start = 0;
            int end = collection.Count - 1;

            while(start <= end)
            {
                int mid = (start + end) / 2;

                if (collection[mid].CompareTo(element) > 0)
                    end = mid - 1;
                else
                    start = mid + 1;
            }

            return start;
        }

        public static string GetPosterSorce(int showId, int showSeason)
        {
            string srcBase = "https://www.thetvdb.com/banners/";

            while (showSeason > 0)
            {
                string path = String.Format(Constants.GetPoster, showId, showSeason);

                try
                {
                    ImageInfoRoot data = WebParser<ImageInfoRoot>.GetInfo(path);

                    data.data.OrderByDescending(p => p.ratingsInfo.average);

                    return srcBase + data.data[0].fileName;
                }
                catch (Exception)
                {
                    --showSeason;
                }
            }

            return "";
        }
    }
}
