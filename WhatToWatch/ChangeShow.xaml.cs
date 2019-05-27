using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch
{
    /// <summary>
    /// Interaction logic for ChangeShow.xaml
    /// </summary>
    public partial class ChangeShow : Window
    {
        private ShowBindingModel showBM;

        public ChangeShow()
            :this(new ShowBindingModel())
        {
        }

        public ChangeShow(ShowBindingModel show)
        {
            InitializeComponent();

            this.showBM = show;
            this.DataContext = this.showBM;
        }

        public ShowBindingModel NewShowInfo
        {
            get { return this.showBM; }
        }

        private void Save_Click(object sender, RoutedEventArgs e)
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
