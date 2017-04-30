using Stratego.AI;
using Stratego.Core;

namespace Stratego.UI
{
    public class DesignGameViewModel : GameViewModel
    {
        public DesignGameViewModel()
            : base(GameFactory.CreateWithDefaultSetup())
        {
            Players[1] = new ComputerPlayer();
        }
    }
}