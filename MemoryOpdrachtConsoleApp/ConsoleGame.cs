using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MemoryGame;
using MemoryGame.Data;
using MemoryGame.Models;
using MemoryGame.UI_s;

namespace MemoryOpdrachtConsoleApp
{
    public class ConsoleGame : Memory
    {
        private readonly int _amount;
        private Player _player;
        private Random rand = new();
        private Dictionary<string, char> guessedPostions = new();
        private char[,] memoryBoard;

        public ConsoleGame(int amount) : base(amount)
        {
            _amount = amount;
            //Checkt of de paren gemaakt kunnen worden
            if (amount % 2 != 0)
            {
                Console.WriteLine($"Can't make pairs with amount: {amount}"); //Exception
                return;
            }

            new DataAccess();
            MakeMemoryBoard(amount);
        }
        protected override void MakeCards(int amount)
        {
            int id = 1;
            int pairId = 0;
            while (pairId < amount / 2)
            {
                var character = MemoryCard.Characters[rand.Next(0, MemoryCard.Characters.Length)];
                bool containsChar = false;
                foreach (var c in Cards)
                    if (c.CardCharacter == character)
                    {
                        containsChar = true;
                        break;
                    }
                if (!containsChar)
                {
                    for (int i = 0; i < 2; i++)
                    {
                        Cards.Add(new MemoryCard(id, pairId + 1, character));
                        id++;
                    }

                    pairId++;
                }
            }
            foreach (var card in Cards.DistinctBy(p => p.CardPairID))
            {
                CardStatus.Add(card, false);
            }
            //TODO: Pas parameter aan
            ShuffleCards();
        }

        protected override void ShuffleCards()
        {
            Random rand = new Random();
            int n = Cards.Count;
            while (n > 1)
            {
                n--;
                int k = rand.Next(n + 1);
                MemoryCard value = Cards[k];
                Cards[k] = Cards[n];
                Cards[n] = value;
            }
        }

        public void Start()
        {
            //Print de highscores
            Console.WriteLine("Welcome to the console app version of Memory");
            Console.WriteLine("Position\tPlayername\t\tHighscore\tAmount of cards");
            foreach (var player in DataAccess.GetHighscores())
            {
                Console.WriteLine($"{player.Position}:\t\t{player.PlayerName.Trim()}\t\t{player.HighScore}\t\t{player.AmountOfCards}");
            }
            Console.Write("Enter Playername --> ");
            var playerName = Console.ReadLine();
            Console.Write("Press enter to start!");
            Console.ReadLine();
            _player = new Player(playerName.Trim(), 0, _amount);

            Console.Clear();

            //Spel begint
            Timer.Start(); //Timer start

            do
            {
                for (int i = 0; i < Cards.Count; i++)
                    Console.Write(Cards[i].CardCharacter + " ");
                Console.WriteLine();
                PrintBoard(_amount);

                Console.WriteLine();
                Console.Write("Make a choice: --> ");

                string position = "";
                try
                {
                    position = Console.ReadLine();
                    position = position.Trim();
                    position = position.ToUpper();
                    if (position == string.Empty)
                        throw new ArgumentNullException();
                    ConvertChar(position);
                }

                catch (ArgumentNullException)
                {
                    Console.WriteLine("Input needs to be a position!");
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }
                catch (ArgumentOutOfRangeException)
                {
                    Console.WriteLine("Input needs to be in the range of the board values!");
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                catch (FormatException)
                {
                    Console.WriteLine("Input needs to be a valid position, try again!");
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                catch (PostionIsChosen)
                {
                    Console.WriteLine("You've already chosen this position, try again!");
                    Console.Write("Press enter to continue...");
                    Console.ReadLine();
                    Console.Clear();
                    continue;
                }

                foreach (var value in guessedPostions.Where(p => position == p.Key).Select(c => c.Value))
                {
                    Console.WriteLine($"{position} is a {value}");
                    if (CheckChars() && guessedPostions.Count == 2)
                    {
                        Console.WriteLine("You guessed it right!");
                        AmountGuessed += 1;
                        UpdateMemory();
                        if (CheckIfWon())
                        {
                            Timer.Stop();
                            double score = Math.Pow(_amount, 2) / (Timer.Elapsed.Seconds * AmountGuessed) * 1000;

                            Console.WriteLine("You've won!");
                            Console.WriteLine($"Your score is {Math.Round(score)} points");
                            _player.HighScore = (int)Math.Round(score);
                            if (DataAccess.CheckIfIsHighscore(_amount, _player.PlayerName, _player.HighScore))
                            {
                                Console.WriteLine("Your score is a high score, Congratulations!");
                            }
                            else
                            {
                                Console.WriteLine("Unfortunately your score is not a high score... Better luck next time!");
                            }
                        }
                    }
                    else if (guessedPostions.Count == 2)
                    {
                        AmountGuessed += 1;
                        Console.WriteLine("Those are not the same... Try again!");
                        guessedPostions.Clear();
                    }
                }
                PrintBoard(_amount);
                Console.WriteLine();
                Console.Write("Press enter to continue...");
                Console.ReadLine();
                Console.Clear();

            }
            while (!CheckIfWon());
        }

        private bool CheckChars()
        {
            if (guessedPostions.Count == 2)
            {
                var listValues = guessedPostions.Select(c => c.Value).ToList();
                return listValues[0] == listValues[1];
            }
            return false;
        }

        private bool CheckIfWon()
        {
            return !CardStatus.ContainsValue(false);
        }
        private void UpdateMemory()
        {
            char value = '\0';
            foreach (var position in guessedPostions)
            {
                //Convert van unicode naar int
                int unicodeValue = position.Key[1];
                char character = (char)unicodeValue;
                string characterString = character.ToString();
                int postionRow = Int32.Parse(characterString) - 1;

                //Convert van Char naar index in character
                int postionColumn = Array.IndexOf(MemoryCard.Characters, position.Key[0]);

                memoryBoard[postionRow, postionColumn] = position.Value;
                value = position.Value;
            }

            var test = CardStatus.FirstOrDefault(cvalue => cvalue.Key.CardCharacter == value).Key;
            CardStatus[test] = true;
            guessedPostions.Clear();
        }

        private void MakeMemoryBoard(int amount)
        {
            int column = amount / 2;
            int row = amount / column;
            memoryBoard = new char[row, column];

            for (int r = 0; r < memoryBoard.GetLength(0); r++)
            {
                for (int c = 0; c < memoryBoard.GetLength(1); c++)
                {
                    memoryBoard[r, c] = '-';
                }
            }
        }

        private void PrintBoard(int amount)
        {
            char[] column = new char[amount / 2];
            int[] row = new int[amount / column.Length];

            for (int i = 0; i < column.Length; i++)
                column[i] = MemoryCard.Characters[i];
            for (int i = 0; i < row.Length; i++)
                row[i] = i + 1;

            Console.Write("  ");
            foreach (char c in column)
                Console.Write(c + " | ");
            Console.WriteLine();
            for (int i = 0; i < row.Length; i++)
            {
                Console.Write(row[i]);
                Console.Write(" ");
                for (int j = 0; j < column.Length; j++)
                {
                    Console.Write(memoryBoard[i, j] + " | ");
                }

                Console.WriteLine();
            }
        }

        private void ConvertChar(string position)
        {
            //Convert van unicode naar int
            int unicodeValue = position[1];
            char character = (char)unicodeValue;
            string characterString = character.ToString();
            int postionRow = Int32.Parse(characterString) - 1;

            //Convert van Char naar index in character
            int postionColumn = Array.IndexOf(MemoryCard.Characters, position[0]);

            int getPostionInList = postionColumn + 5 * postionRow;
            if (guessedPostions.ContainsKey(position))
                throw new PostionIsChosen();
            if (CardStatus.FirstOrDefault(k => k.Key.CardCharacter == Cards[getPostionInList].CardCharacter).Value)
                throw new PostionIsChosen();
            guessedPostions.Add(position, Cards[getPostionInList].CardCharacter);
        }
    }
}
