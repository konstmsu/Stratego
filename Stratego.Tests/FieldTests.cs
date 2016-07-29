using Microsoft.VisualStudio.TestTools.UnitTesting;
using Stratego.Core;

namespace Stratego
{
    [TestClass]
    public class FieldTests
    {
        [TestMethod]
        public void ShouldMove()
        {
            var field = new Field();
            field.Cells[0][0].Piece = new Flag();
            field.Cells[0][0].Piece = new Flag();
        }
    }
}
