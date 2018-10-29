using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch.Models
{
    public class BingeShow
    {
        //Properties
        public int Id { get; set; }

        public string Title { get; set; }

        public int CurrentSeason { get; set; }

        public int CurrentEpisode { get; set; }

        public void UpdateData(ShowBindingModel model)
        {
            this.Title = model.Name;
            this.CurrentEpisode = model.CurrentEpisode;
            this.CurrentSeason = model.CurrentSeason;
        }
    }
}
