using System.Diagnostics;

namespace GuessingGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            // New game loop with difficulty
            // Easy mode lets you guess indefinitely and number range is [1;50]
            // Normal mode lets you guess 20 times and number range is [1;100]
            // Hard mode lets you guess 10 times and number range is [1;500]
            // 
            // Game starts
            // Explain rules
            // Choose diff
            // Print how many tries left after any guess in RED
            // -- Only valid number guesses count.
            // -- Later can be improved by color-coding
            // The game ends when there are no more guesses left
            //  or the number is guessed correctly.
            
            const int MinNumber = 1;
            const int MaxNumber = 100;

            PrintGameRules();

            WaitForKeyPress();

            // Choose Difficulty
            
            StartGameLoop(MinNumber, MaxNumber);
        }

        private static void StartGameLoop(int rangeStart, int rangeEnd)
        {
            bool stopGame = false;
            do
            {
                bool forcedExit = !StartGame(rangeStart, rangeEnd);

                if (forcedExit)
                    stopGame = forcedExit;
                else
                    stopGame = StopGame();

            } while (!stopGame);
        }

        /// <summary>
        /// This method runs one game round.
        /// </summary>
        /// <returns>
        /// False if exit was forced e.g. input stream ended, True if game ended successfully
        /// </returns>
        private static bool StartGame(int rangeStart, int rangeEnd)
        {
            int numberToGuess = GenerateNumber(rangeStart, rangeEnd);
            int numberOfTries = 0;
            bool continueGuessing = true;
            bool correctlyGuessed = false;

            do
            {
                if (TryGetNumber(rangeStart, rangeEnd, out int guessedNumber))
                {
                    if (GuessNumber(guessedNumber, numberToGuess))
                    {
                        continueGuessing = false;
                        correctlyGuessed = true;
                    }
                    else
                    {
                        Console.WriteLine(Hint(numberToGuess, guessedNumber));
                    }
                    numberOfTries++;
                }
                else
                {
                    continueGuessing = false;
                }
            } while (continueGuessing);

            if (correctlyGuessed)
                Console.WriteLine($"Your guess was correct! Random number was {numberToGuess}. You guessed it in {numberOfTries} tries");

            return correctlyGuessed;
        }

        private static int GenerateNumber(int start, int end)
        {
            Random number = new();
            return number.Next(start, end + 1);
        }

        private static bool GuessNumber(int guessedNumber, int correctNumber)
            => guessedNumber == correctNumber;

        private static bool TryGetNumber(int rangeStart, int rangeEnd, out int number)
        {
            number = 0;
            bool parsedNumber = true;
            do
            {
                Console.Write($"Guess number from {rangeStart} to {rangeEnd}: ");
                string? answer = Console.ReadLine();

                if (answer is null)
                {
                    parsedNumber = false;
                    break;
                }

                if (!int.TryParse(answer, out int guessedNumber))
                {
                    Console.WriteLine("You've entered not a number. Please try again.");
                }
                else if (IsOutOfRange(guessedNumber, rangeStart, rangeEnd))
                {
                    Console.WriteLine("Out of Range");
                }
                else
                {
                    Console.WriteLine($"Number {guessedNumber} accepted. Checking...");
                    number = guessedNumber;
                    break;
                }
            } while (true);

            return parsedNumber;
        }

        private static string Hint(int number, int guessedNumber)
        {
            if (number > guessedNumber)
                return "Higher";

            if (number < guessedNumber)
                return "Lower";

            return string.Empty;
        }

        private static bool IsOutOfRange(int guessedNumber, int rangeStart, int rangeEnd)
            => (guessedNumber < rangeStart || guessedNumber > rangeEnd);

        private static bool StopGame()
        {
            bool stopGame;
            do
            {
                Console.Write("Try Again? (Y/N):");
                string? answer = Console.ReadLine();
                if (answer is null || answer.ToUpper().Equals("N"))
                {
                    stopGame = true;
                    break;
                }
                else if (answer.ToUpper().Equals("Y"))
                {
                    stopGame = false;
                    break;
                }
            } while (true);

            return stopGame;
        }

        private static DifficultyParameters GetDifficultyParameters(Difficulty difficulty)
            => difficulty switch
            {
                Difficulty.Easy => new (null, 1, 50, ConsoleColor.Green),
                Difficulty.Normal => new (20, 1, 100, ConsoleColor.Yellow),
                Difficulty.Hard => new (10, 1, 500, ConsoleColor.Red),
                _ => throw new UnreachableException($"Selected difficulty does not exist: {difficulty}")
            };

        private static void PrintGameRules()
        {
            Console.WriteLine($"In this game there are {Enum.GetValues<Difficulty>().Length} difficulty levels.");
            foreach (Difficulty difficulty in Enum.GetValues<Difficulty>())
            {
                WriteDifficultyRule(difficulty);
            }
        }

        private static void WriteColorText(string text, ConsoleColor consoleColor)
        {
            Console.ForegroundColor = consoleColor;
            Console.Write(text);
            Console.ResetColor();
        }

        private static void WriteDifficultyRule(Difficulty difficulty)
        {
            DifficultyParameters diffParams = GetDifficultyParameters(difficulty);
            string guessCount = (diffParams.GuessCount is null ? "unlimited" : $"{diffParams.GuessCount}");
            string difficultyName = difficulty.ToString();

            WriteColorText(difficultyName, diffParams.Color);
            Console.WriteLine($" - {guessCount} guesses, number from {diffParams.RangeStart} to {diffParams.RangeEnd}");
        }

        private static void WaitForKeyPress()
        {
            if (Console.IsInputRedirected)
                return;

            Console.WriteLine("\nPress any key to continue...");
            Console.ReadKey(intercept: true);
        }
    }
}
