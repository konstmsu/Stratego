using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace Stratego.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContextChanged += (sender, args) =>
            {
                if (args.OldValue != null)
                    throw new InvalidOperationException();

                var viewModel = (GameViewModel)args.NewValue;
                viewModel.Animate += (s, e) =>
                {
                    var cell = e.Cell;
                    var previousPosition = e.PreviousPosition;
                    var moveTo = (ContentPresenter)Board.ItemContainerGenerator.ContainerFromItem(e.Cell);
                    var frameworkElement = (Border)VisualTreeHelper.GetChild(moveTo, 0);
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

                    var storyboard = new Storyboard { SpeedRatio = 4 };

                    var xAnimation = new DoubleAnimation { To = 0 };
                    Storyboard.SetTarget(xAnimation, view);
                    Storyboard.SetTargetProperty(xAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
                    storyboard.Children.Add(xAnimation);

                    var yAnimation = new DoubleAnimation { To = 0 };
                    Storyboard.SetTarget(yAnimation, view);
                    Storyboard.SetTargetProperty(yAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));
                    storyboard.Children.Add(yAnimation);

                    frameworkElement.Visibility = Visibility.Hidden;
                    storyboard.Completed += (s2, a2) =>
                    {
                        LayerForAnimation.Children.Remove(view);
                        frameworkElement.Visibility = Visibility.Visible;
                    };
                    storyboard.Begin();
                };
            };

            InitializeComponent();
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
    }
}