using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Stratego.Core;

namespace Stratego.UI
{
    public partial class MainWindow
    {
        // TODO: Remove
        public static MainWindow Instance;

        public MainWindow()
        {
            InitializeComponent();
            Instance = this;
        }

        void Cell_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((CellViewModel)((FrameworkElement)sender).DataContext).OnMouseOver();
        }

        void Cell_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            var frameworkElement = (FrameworkElement)sender;
            var cell = (CellViewModel)frameworkElement.DataContext;
            cell.OnClick(frameworkElement);
        }

        public void AnimateMove(FrameworkElement frameworkElement, CellViewModel cell, Position previousPosition)
        {
            if (frameworkElement == null)
                return;

            var point = frameworkElement.TranslatePoint(new Point(0, 0), LayerForAnimation);

            var view = new CellView
            {
                DataContext = new AnimatedCellViewModel
                {
                    Color = cell.Color,
                    PieceLongName = cell.PieceLongName,
                    PieceShortName = cell.PieceShortName
                },
                Width = frameworkElement.ActualWidth,
                Height = frameworkElement.ActualHeight,
                Background = Brushes.White,
                RenderTransform = new TranslateTransform(
                    frameworkElement.ActualWidth * (previousPosition.Column - cell.Cell.Position.Column),
                    frameworkElement.ActualHeight * (previousPosition.Row - cell.Cell.Position.Row))
            };
            LayerForAnimation.Children.Add(view);

            Canvas.SetLeft(view, point.X);
            Canvas.SetTop(view, point.Y);

            var storyboard = new Storyboard{SpeedRatio=4};

            var xAnimation = new DoubleAnimation { To = 0 };
            Storyboard.SetTarget(xAnimation, view);
            Storyboard.SetTargetProperty(xAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
            storyboard.Children.Add(xAnimation);

            var yAnimation = new DoubleAnimation { To = 0 };
            Storyboard.SetTarget(yAnimation, view);
            Storyboard.SetTargetProperty(yAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));
            storyboard.Children.Add(yAnimation);

            frameworkElement.Visibility = Visibility.Hidden;
            storyboard.Completed += (sender, args) =>
            {
                LayerForAnimation.Children.Remove(view);
                frameworkElement.Visibility = Visibility.Visible;
            };
            storyboard.Begin();
        }
    }
}