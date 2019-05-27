using System.Net.Http;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using WhatToWatch.Controllers;
using WhatToWatch.Models.BindingModels;
using WhatToWatch.Pages;

namespace WhatToWatch
{
    public partial class MainWindow : Window
    {
        private BaseController currentController;
        private FollowedController followController;
        private BingeController bingeController;

        public MainWindow()
        {
            InitializeComponent();
            this.DataContext = this;

            followController = new FollowedController();
            bingeController = new BingeController();

            currentController = followController;
            this.PageHolder.Navigate(new ShowsContainer(currentController));
        }

        private void NewCommand_CanExecute(object sender, CanExecuteRoutedEventArgs e)
        {
            e.CanExecute = true;
        }

        private void NewCommand_Executed(object sender, ExecutedRoutedEventArgs e)
        {
            ChangeShow addWindow = new ChangeShow();

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

        private void Refresh_Click(object sender, RoutedEventArgs e)
        {
            currentController.GenerateViews();
        }

        private void Following_Click(object sender, RoutedEventArgs e)
        {
            currentController = followController;
            PageHolder.Navigate(new ShowsContainer(currentController));
        }

        private void Binging_Click(object sender, RoutedEventArgs e)
        {
            currentController = bingeController;
            PageHolder.Navigate(new ShowsContainer(currentController));
        }

        private void ToWatch_Click(object sender, RoutedEventArgs e)
        {
            //Do something here?????????????
        }
    }
}