using System.Windows;
using System.Windows.Input;

namespace Stratego.UI
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        void Cell_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((CellViewModel)((FrameworkElement)sender).DataContext).OnMouseOver();
        }

        void Cell_OnMouseDown(object sender, MouseButtonEventArgs e)
        {
            ((CellViewModel)((FrameworkElement)sender).DataContext).ToggleAsPlannedMoveStart();
        }
    }
}