using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.ComponentModel;

namespace SlidingPivotDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        private bool isMapOpen;

        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            this.isMapOpen = false;
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (!App.ViewModel.IsDataLoaded)
            {
                App.ViewModel.LoadData();
            }
        }

        private void PivotItem_DoubleTap(object sender, System.Windows.Input.GestureEventArgs e)
        {
            resizePivot();
        }

        private void resizePivot()
        {
            double up = 0;
            double down = 478;

            double from, to;
            var easeAnimation = new CubicEase();

            if (!this.isMapOpen)
            {
                from = up;
                to = down;
                easeAnimation.EasingMode = EasingMode.EaseOut;
                this.isMapOpen = true;
            }
            else
            {
                from = down;
                to = up;
                easeAnimation.EasingMode = EasingMode.EaseOut;
                this.isMapOpen = false;
            }

            var translateTransform = new TranslateTransform();
            MainPivot.RenderTransform = translateTransform;

            Storyboard sb = new Storyboard();
            DoubleAnimation moveAnimation = new DoubleAnimation();
            moveAnimation.EasingFunction = easeAnimation;
            moveAnimation.From = from;
            moveAnimation.To = to;
            moveAnimation.Duration = new Duration(TimeSpan.FromSeconds(0.5));

            Storyboard.SetTarget(moveAnimation, translateTransform);
            Storyboard.SetTargetProperty(moveAnimation, new PropertyPath(TranslateTransform.YProperty));

            sb.Children.Add(moveAnimation);
            sb.Begin();
        }

        protected override void OnBackKeyPress(CancelEventArgs e)
        {
            if (isMapOpen)
            {
                resizePivot();
                e.Cancel = true;
            }
        }
    }
}