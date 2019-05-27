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
using System.Windows.Navigation;
using System.Windows.Shapes;
using WhatToWatch.Controllers;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch.Pages
{
    /// <summary>
    /// Interaction logic for ShowsContainer.xaml
    /// </summary>
    public partial class ShowsContainer : Page
    {
        BaseController controller;

        public ShowsContainer(BaseController controller)
        {
            InitializeComponent();
            this.controller = controller;
            tvShows.ItemsSource = controller.GetShows();
        }

        private void NextEpisode_Click(object sender, RoutedEventArgs e)
        {
            string IdString =
                ((TextBlock)
                ((Grid)
                ((Grid)
                ((StackPanel)
                ((Border)
                ((Button)sender).Parent).Parent).Parent).Parent).Children[0]).Text;

            int id = int.Parse(IdString);

            controller.NextEpisode(id);
        }

        private void Previous_Click(object sender, RoutedEventArgs e)
        {
            string IdString =
                ((TextBlock)
                ((Grid)
                ((Grid)
                ((StackPanel)
                ((Border)
                ((Button)sender).Parent).Parent).Parent).Parent).Children[0]).Text;

            int id = int.Parse(IdString);

            //Do stuff here
        }

        private void NextSeason_Click(object sender, RoutedEventArgs e)
        {
            string IdString =
                ((TextBlock)
                ((Grid)
                ((Grid)
                ((StackPanel)
                ((Border)
                ((Button)sender).Parent).Parent).Parent).Parent).Children[0]).Text;

            int id = int.Parse(IdString);

            //Do stuf here
        }

        private void Edit_Click(object sender, RoutedEventArgs e)
        {
            // Get Id
            string IdString =
                ((TextBlock)
                ((Grid)
                ((StackPanel)
                ((Border)
                ((Button)sender).Parent).Parent).Parent).Children[0]).Text;

            int id = int.Parse(IdString);

            // Get episode
            ShowBindingModel model = controller.GetShow(id);

            // Edit info
            ChangeShow editWindow = new ChangeShow(model);

            editWindow.ShowDialog();

            if (editWindow.DialogResult == true)
            {
                if (!controller.EditShow(id, model))
                    MessageBox.Show("There was a problem. The show was not edited");
            }
        }

        private void Remove_Click(object sender, RoutedEventArgs e)
        {
            //Get Id
            string IdString =
                ((TextBlock)
                ((Grid)
                ((StackPanel)
                ((Border)
                ((Button)sender).Parent).Parent).Parent).Children[0]).Text;

            int id = int.Parse(IdString);

            //Remove show
            controller.RemoveShow(id);
        }
    }
}
