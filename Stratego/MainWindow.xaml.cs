using System.Windows;
using System.Windows.Input;

namespace Stratego
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        GameViewModel ViewModel => (GameViewModel)DataContext;

        void Cell_OnMouseEnter(object sender, MouseEventArgs e)
        {
            ((CellViewModel)((FrameworkElement)sender).DataContext).HighlightPossibleMoves();
        }
    }
}