using System.Linq;
using Stratego.AI;
using Stratego.Core;

namespace Stratego.UI
{
    public class DesignGameViewModel : GameViewModel
    {
        public DesignGameViewModel()
            : base(GameFactory.CreateWithDefaultSetup())
        {
            Players = new IPlayer[]
            {
                new HumanPlayer(),
                new ComputerPlayer(), 
            }.ToList();
        }
    }
}