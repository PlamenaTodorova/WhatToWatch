using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WhatToWatch.Controllers;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch
{
    public partial class MainWindow : Window
    {
        private IControllable currentController;
        private FollowedController followController;
        private BingeController bingeController;

        public MainWindow()
        {
            InitializeComponent();

            followController = new FollowedController();
            bingeController = new BingeController();

            currentController = followController;
            tvShows.ItemsSource = followController.GetShows();
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            AddShow addWindow = new AddShow();

            addWindow.ShowDialog();

            if (addWindow.DialogResult == true)
            {
                ShowBindingModel newShow = addWindow.NewShowInfo;

                try
                {
                    if (!currentController.AddShow(newShow))
                        MessageBox.Show("The show was not added. Check your internet conection ot the spelling of the show's title");
                }
                catch (HttpRequestException ex)
                {
                    if (ex.HResult == 401)
                        MessageBox.Show("The api token is expired");
                    if (ex.HResult == 404)
                        MessageBox.Show("The show was not found. Check your spelling of the name.");
                }
            }
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

            currentController.NextEpisode(id);
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            tvShows.ItemsSource = followController.GetShows();
        }

        private void Following_Click(object sender, RoutedEventArgs e)
        {
            tvShows.ItemsSource = followController.GetShows();
            currentController = followController;
        }

        private void Binging_Click(object sender, RoutedEventArgs e)
        {
            tvShows.ItemsSource = bingeController.GetShows();
            currentController = bingeController;
        }

        private void ToWatch_Click(object sender, RoutedEventArgs e)
        {
            //Do something here?????????????
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
            ShowBindingModel model = currentController.GetShow(id);

            // Edit info
            EditShow editWindow = new EditShow(model);

            editWindow.ShowDialog();

            if (editWindow.DialogResult == true)
            {
                if (!currentController.EditShow(id, model))
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
            currentController.RemoveShow(id);
        }
    }
}
