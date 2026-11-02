namespace GuessingGame
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            const int MinNumber = 1;
            const int MaxNumber = 100;
            
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
    }
}
