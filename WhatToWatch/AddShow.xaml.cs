using System.Windows;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch
{
    public partial class AddShow : Window
    {
        private ShowBindingModel showBM;

        public AddShow()
        {
            InitializeComponent();

            showBM = new ShowBindingModel();
            this.DataContext = showBM;
        }

        public ShowBindingModel NewShowInfo
        {
            get { return this.showBM; }
        }

        private void Add_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
            {
                this.DialogResult = true;
            }
        }

        private bool IsValid()
        {
            if (showBM.Name != null && showBM.Name != "")
            {
                return true;
            }
            return false;
        }
    }
}