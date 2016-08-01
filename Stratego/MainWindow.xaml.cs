using System.Windows.Controls;
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
            ((CellViewModel)((Grid)sender).DataContext).HighlightPossibleMoves();
        }
    }
}