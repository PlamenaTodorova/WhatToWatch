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
    public interface IControllable
    {
        ObservableCollection<ShowViewModel> GetShows();

        ShowBindingModel GetShow(int id);

        bool AddShow(ShowBindingModel show);

        bool EditShow(int id, ShowBindingModel show);

        void RemoveShow(int id);

        bool NextEpisode(int id);

        void GenerateViews();
    }
}
