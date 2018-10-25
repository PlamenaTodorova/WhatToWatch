using System.ComponentModel;
using WhatToWatch.Models.BindingModels;
using WhatToWatch.Utilities;

namespace WhatToWatch.Models
{
    [TypeConverter(typeof(EnumDescriptionTypeConverter))]
    public enum Status
    {
        [Description("Must watch!")]
        Red,
        [Description("Let the episode gather.")]
        Yellow,
        [Description("I will get to it some day.")]
        Green
    }

    public class Show
    {
        //Properties
        public int Id { get; set; }

        public string Title { get; set; }

        public int CurrentSeason { get; set; }

        public int CurrentEpisode { get; set; }

        public Status Status { get; set; }

        public void UpdateData(ShowBindingModel model)
        {
            this.Title = model.Name;
            this.CurrentEpisode = model.CurrentEpisode;
            this.CurrentSeason = model.CurrentSeason;
            this.Status = model.Status;
        }
    }
}