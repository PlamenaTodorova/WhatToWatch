using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch.Controllers
{
    public interface IControllable
    {
        ShowBindingModel GetShow(int id);

        bool AddShow(ShowBindingModel show);

        bool EditShow(int id, ShowBindingModel show);

        void RemoveShow(int id);

        bool NextEpisode(int id);
    }
}
