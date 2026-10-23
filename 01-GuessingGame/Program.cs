const int rangeStart = 1;
const int rangeEnd = 100;
int numberToGuess = GenerateNumber(rangeStart, rangeEnd, true);
int numberOfTries = 0;
bool continueGuessing = true;

do
{
    if (TryGetNumber(out int guessedNumber))
    {
        numberOfTries++;
        if (GuessNumber(guessedNumber, numberToGuess))
        {
            continueGuessing = false;
        }
        else
        {
            Console.WriteLine(Hint(numberToGuess, guessedNumber));
        }
    }

    if (!continueGuessing)
    {
        Console.WriteLine($"Your guess was correct! Random number was {numberToGuess}. You guessed it in {numberOfTries} tries");
        bool stopAsking = false;
        do
        {
            Console.Write("Try Again? (Y/N):");
            string answer = Console.ReadLine() ?? "";
            if (answer.ToUpper().Equals("Y"))
            {
                stopAsking = true;
                continueGuessing = true;
                numberToGuess = GenerateNumber(rangeStart, rangeEnd, true);
                numberOfTries = 0;
            }
            else if (answer.ToUpper().Equals("N"))
            {
                stopAsking = true;
            }
        } while (!stopAsking);
    }
} while (continueGuessing);


int GenerateNumber(int start, int end, bool includeLastNum)
{
    Random number = new();
    return number.Next(start, includeLastNum ? end + 1 : end);
}

bool GuessNumber(int guessedNumber, int correctNumber)
    => guessedNumber == correctNumber;

bool TryGetNumber(out int number)
{
    number = 0; // This is default value
    Console.Write($"Guess number from {rangeStart} to {rangeEnd}: ");
    if (!int.TryParse(Console.ReadLine(), out int guessedNumber))
    {
        Console.WriteLine("You've entered not a number. Please try again.");
        return false;
    }

    Console.WriteLine($"Number {guessedNumber} accepted. Checking...");
    number = guessedNumber;

    return true;
}

string Hint(int number, int guessedNumber)
{
    string answer = string.Empty;
    if (number > guessedNumber)
        answer = "Higher";

    if (number < guessedNumber)
        answer = "Lower";
    
    return answer;
}