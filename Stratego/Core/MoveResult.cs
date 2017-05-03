namespace Stratego.Core
{
    public class MoveResult
    {
        public readonly Piece Attacker;
        public readonly Piece Defender;
        public readonly Position InitialAttackerPosition;
        public readonly Position DefenderPosition;
        public readonly bool HasAttackerDied;
        public readonly bool HasDefenderDied;

        public MoveResult(Piece attacker, Piece defender, Position initialAttackerPosition, Position defenderPosition, bool hasAttackerDied, bool hasDefenderDied)
        {
            Attacker = attacker;
            Defender = defender;
            InitialAttackerPosition = initialAttackerPosition;
            DefenderPosition = defenderPosition;
            HasAttackerDied = hasAttackerDied;
            HasDefenderDied = hasDefenderDied;
        }
    }
}