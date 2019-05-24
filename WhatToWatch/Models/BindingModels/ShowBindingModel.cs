using System.ComponentModel;

namespace WhatToWatch.Models.BindingModels
{
    public class ShowBindingModel : INotifyPropertyChanged
    {
        private string name;
        private int currentSeason;
        private int currentEpisode;
        private Status status;

        public ShowBindingModel()
        {
            CurrentEpisode = 1;
            CurrentSeason = 1;
        }

        public string Name
        {
            get { return this.name; }
            set
            {
                if (this.name != value)
                {
                    this.name = value;
                    this.NotifyPropertyChanged("Name");
                }
            }
        }

        public int CurrentSeason
        {
            get { return this.currentEpisode; }
            set
            {
                if (this.currentEpisode != value)
                {
                    this.currentEpisode = value;
                    this.NotifyPropertyChanged("CurrentSeason");
                }
            }
        }

        public int CurrentEpisode
        {
            get { return this.currentSeason; }
            set
            {
                if (this.currentSeason != value)
                {
                    this.currentSeason = value;
                    this.NotifyPropertyChanged("CurrentEpisode");
                }
            }
        }

        public Status Status
        {
            get { return this.status; }
            set
            {
                if (this.status != value)
                {
                    this.status = value;
                    this.NotifyPropertyChanged("Status");
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
    }
}
