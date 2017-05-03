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
        public MainWindow()
        {
            DataContextChanged += (sender, args) =>
            {
                if (args.OldValue != null)
                    throw new InvalidOperationException();

                var viewModel = (GameViewModel)args.NewValue;
                viewModel.Animate += moveResult =>
                {
                    var attackerCell = GetCell(moveResult.InitialAttackerPosition);
                    var defenderCell = GetCell(moveResult.DefenderPosition);

                    var attackerView = new CellView
                    {
                        DataContext = new AnimatedCellViewModel
                        {
                            Color = viewModel.GetColor(moveResult.Attacker.Owner),
                            PieceLongName = moveResult.Attacker.Name,
                            PieceShortName = moveResult.Attacker.ShortDisplayName
                        },
                        Width = attackerCell.ActualWidth,
                        Height = attackerCell.ActualHeight,
                        RenderTransform = new TranslateTransform(
                            attackerCell.ActualWidth * (moveResult.InitialAttackerPosition.Column - moveResult.DefenderPosition.Column),
                            attackerCell.ActualHeight * (moveResult.InitialAttackerPosition.Row - moveResult.DefenderPosition.Row))
                    };
                    LayerForAnimation.Children.Add(attackerView);
                    var defenderLocation = defenderCell.TranslatePoint(new Point(0, 0), LayerForAnimation);
                    Canvas.SetLeft(attackerView, defenderLocation.X);
                    Canvas.SetTop(attackerView, defenderLocation.Y);

                    var storyboard = new Storyboard { SpeedRatio = 4 };

                    var xAnimation = new DoubleAnimation { To = 0 };
                    Storyboard.SetTarget(xAnimation, attackerView);
                    Storyboard.SetTargetProperty(xAnimation, new PropertyPath("RenderTransform.(TranslateTransform.X)"));
                    storyboard.Children.Add(xAnimation);

                    var yAnimation = new DoubleAnimation { To = 0 };
                    Storyboard.SetTarget(yAnimation, attackerView);
                    Storyboard.SetTargetProperty(yAnimation, new PropertyPath("RenderTransform.(TranslateTransform.Y)"));
                    storyboard.Children.Add(yAnimation);

                    CellView defenderView = null;

                    if (moveResult.HasAttackerDied)
                    {
                        var dieAnimation = new DoubleAnimation { To = 0 };
                        Storyboard.SetTarget(dieAnimation, attackerView);
                        Storyboard.SetTargetProperty(dieAnimation, new PropertyPath("Opacity"));
                        storyboard.Children.Add(dieAnimation);
                    }

                    if (moveResult.Defender != null)
                    {
                        defenderView = new CellView
                        {
                            DataContext = new AnimatedCellViewModel
                            {
                                Color = viewModel.GetColor(moveResult.Defender.Owner),
                                PieceLongName = moveResult.Defender.Name,
                                PieceShortName = moveResult.Defender.ShortDisplayName
                            },
                            Width = defenderCell.ActualWidth,
                            Height = defenderCell.ActualHeight,
                        };
                        LayerForAnimation.Children.Add(defenderView);
                        Canvas.SetLeft(defenderView, defenderLocation.X);
                        Canvas.SetTop(defenderView, defenderLocation.Y);

                        if (moveResult.HasDefenderDied)
                        {
                            var dieAnimation = new DoubleAnimation { To = 0 };
                            Storyboard.SetTarget(dieAnimation, defenderView);
                            Storyboard.SetTargetProperty(dieAnimation, new PropertyPath("Opacity"));
                            storyboard.Children.Add(dieAnimation);
                        }
                    }

                    defenderCell.Visibility = Visibility.Hidden;
                    storyboard.Completed += (s2, a2) =>
                    {
                        LayerForAnimation.Children.Remove(attackerView);
                        LayerForAnimation.Children.Remove(defenderView);
                        defenderCell.Visibility = Visibility.Visible;
                    };
                    storyboard.Begin();
                };
            };

            InitializeComponent();
        }

        Border GetCell(Position position)
        {
            var viewModel = (GameViewModel)DataContext;
            var cellViewModel = viewModel.Board.Cells.IndexOf(c => c.Cell.Position == position);
            var cellContainer = (ContentPresenter)Board.ItemContainerGenerator.ContainerFromIndex(cellViewModel);
            return (Border)VisualTreeHelper.GetChild(cellContainer, 0);
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