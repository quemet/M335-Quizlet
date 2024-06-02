using Microsoft.VisualStudio.TestTools.UnitTesting;
using M335_QuizletLib.Models;
using M335_QuizletLib.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace M335_Quizlet.viewModels.Tests
{
    [TestClass()]
    public class GameTests
    {
        [TestMethod()]
        public void MelangerCartesTest()
        {
            Game game = new Game();
            game.cards.Add(new Card { Question = "Hello", Response = "ABC" });
            Assert.AreEqual(game.cards.Count, 1);
        }

        [TestMethod()]
        public void NextCardTest()
        {
            Game game = new Game();
            game.cards.Add(new Card { Question = "Hello", Response = "ABC" });
            game.cards.Add(new Card { Question = "1Hello", Response = "Hello" });

            game.NextCard();

            Assert.AreEqual(game.cardName, "1Hello");

        }

        [TestMethod()]
        public void EnableAccelerometerTest()
        {
            Game game = new Game();
            game.EnableAccelerometer();
            Assert.IsTrue(Accelerometer.Default.IsMonitoring);
            game.DisableAccelerometer();
        }
    }
}