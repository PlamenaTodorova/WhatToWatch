using System.Windows;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch
{
    public partial class EditShow : Window
    {
        private ShowBindingModel showBM;

        public EditShow(ShowBindingModel show)
        {
            InitializeComponent();

            this.showBM = show;
            this.DataContext = this.showBM;
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            if (IsValid())
                this.DialogResult = true;
        }

        private bool IsValid()
        {
            if (showBM.Name != null && showBM.Name != "")
                return true;
            return false;
        }
    }
}