using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.BindingModels;
using WhatToWatch.Models.ViewModels;

namespace WhatToWatch.Controllers
{
    public abstract class BaseController
    {
        public abstract ObservableCollection<BaseViewModel> GetShows();

        public abstract ShowBindingModel GetShow(int id);

        public abstract bool AddShow(ShowBindingModel show);

        public abstract bool EditShow(int id, ShowBindingModel show);

        public abstract void RemoveShow(int id);

        public abstract bool NextEpisode(int id);

        public abstract void GenerateViews();

        public virtual bool PreviousEpisode(int id)
        {
            throw new NotImplementedException();
        }

        public virtual bool NextSeason(int id)
        {
            throw new NotImplementedException();
        }
    }
}
