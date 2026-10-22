const int rangeStart = 1;
const int rangeEnd = 100;
int numberToGuess = GenerateNumber(rangeStart, rangeEnd, true);
int numberOfTries = 0;
bool continueGuessing = true;
do
{
    int guessedNumber = GetNumber();
    numberOfTries++;
    if (GuessNumber(guessedNumber, numberToGuess))
    {
        continueGuessing = false;
    }
    else
    {
        Console.WriteLine(Hint(numberToGuess, guessedNumber));
    }
    
} while (continueGuessing);

Console.WriteLine($"Your guess was correct! Random number was {numberToGuess}. You guessed it in {numberOfTries} tries");

int GenerateNumber(int start, int end, bool includeLastNum)
{
    Random number = new();
    return number.Next(start, includeLastNum ? end + 1 : end);
}

bool GuessNumber(int guessedNumber, int correctNumber)
    => guessedNumber == correctNumber;

int GetNumber()
{
    Console.Write("Guess number from 1 to 100: ");
    if (!int.TryParse(Console.ReadLine(), out int guessedNumber))
    {
        Console.WriteLine("You've entered not a number. Please try again.");
    }

    Console.WriteLine($"Number {guessedNumber} accepted. Checking...");

    return guessedNumber;
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