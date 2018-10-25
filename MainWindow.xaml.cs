using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WhatToWatch.Controllers;
using WhatToWatch.Models.BindingModels;

namespace WhatToWatch
{
    public partial class MainWindow : Window
    {
        private FollowedController controller;

        public MainWindow()
        {
            InitializeComponent();

            controller = new FollowedController();
            tvShows.ItemsSource = controller.GetShows();
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

                if (!controller.AddShow(newShow))
                    MessageBox.Show("The show was not added. Check your internet conection ot the spelling of the show's title");
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

            controller.NextEposode(id);
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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            //Do something here?????????????
        }

        private void Following_Click(object sender, RoutedEventArgs e)
        {
            //Do something here?????????????
        }

        private void Binging_Click(object sender, RoutedEventArgs e)
        {
            //Do something here?????????????
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
            ShowBindingModel model = controller.GetShow(id);

            // Edit info
            EditShow editWindow = new EditShow(model);

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