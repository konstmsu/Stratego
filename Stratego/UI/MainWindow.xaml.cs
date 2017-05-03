using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using Stratego.Core;
using Stratego.Core.Utility;

namespace Stratego.UI
{
    public partial class MainWindow
    {
        Border GetCell(Position position)
        {
            var viewModel = (GameViewModel)DataContext;
            var cellViewModel = viewModel.Board.Cells.IndexOf(c => c.Cell.Position == position);
            var cellContainer = (ContentPresenter)Board.ItemContainerGenerator.ContainerFromIndex(cellViewModel);
            return (Border)VisualTreeHelper.GetChild(cellContainer, 0);
        }

        public MainWindow()
        {
            DataContextChanged += (sender, args) =>
            {
                if (args.OldValue != null)
                    throw new InvalidOperationException();

                var viewModel = (GameViewModel)args.NewValue;
                viewModel.Animate += moveResult =>
                {
                    var attackerView = GetCell(moveResult.InitialAttackerPosition);
                    var defenderView = GetCell(moveResult.DefenderPosition);
                    var previousPosition = moveResult.InitialAttackerPosition;
                    var frameworkElement = defenderView;
                    var point = frameworkElement.TranslatePoint(new Point(0, 0), LayerForAnimation);
                    var cell = (CellViewModel)defenderView.DataContext;

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
                            frameworkElement.ActualWidth * (previousPosition.Column - moveResult.DefenderPosition.Column),
                            frameworkElement.ActualHeight * (previousPosition.Row - moveResult.DefenderPosition.Row))
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
            cell.OnClick();
        }
    }
}