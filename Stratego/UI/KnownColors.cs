using System.Windows.Media;

namespace Stratego.UI
{
    public static class KnownColors
    {
        public static readonly SolidColorBrush[] Players = { Brushes.Blue, Brushes.Red };
        public static readonly SolidColorBrush Lake = Brushes.Aqua;
        public static readonly SolidColorBrush LakeMouseOver = Brushes.MediumAquamarine;
        public static readonly SolidColorBrush EmptyMouseOver = Brushes.Bisque;
        public static readonly SolidColorBrush PossibleMove = Brushes.PaleGreen;
        public static readonly SolidColorBrush PossibleAttack = Brushes.LightPink;
        public static readonly SolidColorBrush PlannedMoveStart = Brushes.DarkSeaGreen;
        public static readonly SolidColorBrush PossibleMoveMouseOver = Brushes.ForestGreen;
        public static readonly SolidColorBrush PossibleAttackMouseOver = Brushes.DarkOrange;
        public static readonly SolidColorBrush PlannedMoveStartMouseOver = Brushes.DarkOliveGreen;
    }
}
