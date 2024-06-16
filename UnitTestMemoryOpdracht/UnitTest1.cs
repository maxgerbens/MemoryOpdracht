using System.Collections.ObjectModel;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryOpdrachtConsoleApp;
using Moq;


namespace UnitTestMemoryOpdracht
{
    public class Tests
    {
        [TestCase(10)]
        [TestCase(12)]
        [TestCase(14)]
        public void NoDuplicates(int amount)
        {
            ConsoleGame consoleGame = new ConsoleGame(amount);

            var duplicate = consoleGame.CardStatus.GroupBy(card => card.Key.CardCharacter).
                Where(group => group.Count() > 1)
                .Select(group => group.Key)
                .ToList();

            Assert.That(duplicate, Is.Empty);
        }

        [TestCase(10)]
        [TestCase(12)]
        [TestCase(14)]
        public void AIdenticalTwinForEachPair(int amount)
        {
            ConsoleGame game;
            int amountOfPairs = amount / 2;

            game = new ConsoleGame(amount);
            var twin = game.Cards.DistinctBy(c => c.CardPairID).ToList();

            Assert.That(amountOfPairs, Is.EqualTo(twin.Count));
        }

        [Test]
        public void CheckIfHighscore()
        {
            Player player;
            new DataAccess();

            player = new Player("UnitTest", 100, 10);
            if (!DataAccess.CheckIfIsHighscore(player.AmountOfCards, player.PlayerName, player.HighScore))
            {
                Assert.Pass();
            }
        }

        [Test]
        public void PositionInGetHighscore()
        {
            ObservableCollection<Player> highscores;
            new DataAccess();

            highscores = new ObservableCollection<Player>(DataAccess.GetHighscores());

            for (int i = 0; i < highscores.Count; i++)
            {
                Assert.That(i + 1, Is.EqualTo(highscores[i].Position));
            }
        }

        [TestCase(4, 10, 2, 800)]
        [TestCase(10, 20, 5, 1000)]
        [TestCase(4, 20, 2, 400)]
        [TestCase(4, 10, 3, 533)]
        public void TestScoreOutput(int amount, int seconds, int guess, int result)
        {
            int output = (int)Math.Round(Math.Pow(amount, 2) / (seconds * guess) * 1000);

            Assert.That(output, Is.EqualTo(result));
        }

        [Test]
        public void TestThatImageIsNullWithConsoleGame()
        {
            ConsoleGame consoleGame;

            consoleGame = new ConsoleGame(10);

            foreach (var card in consoleGame.Cards)
            {
                Assert.That(card.CardImage, Is.Null);
                Assert.That(card.CardImageFront, Is.Null);
            }
        }
    }
}