using System;
using System.Linq;

namespace MasterMind
{
    class Program
    {

        class Combo
        {
            public int digits { get; set; }
            public int range { get; set; }
            public int tries { get; set; }
            public int[] correctCombo { get; }
            public int numCorrect { get; set; }
            public int numMisplaced { get; set; }
            public int numWrong { get; set; }
            public bool hasError { get; set; }
            public Combo(int _digits, int _range, int _tries)
            {
                digits = _digits;
                range = _range;
                tries = _tries;
                correctCombo = CreateCombo(digits, range);
                numCorrect = 0;
                numMisplaced = 0;
                numWrong = 0;
                hasError = false;

            }

            private int[] CreateCombo(int digits, int range)
            {
                var random = new Random();
                int[] result = new int[digits];
                for (int i = 0; i < result.Length; i++) // apparently, foreach can't change array contents- good to know
                {
                    result[i] = random.Next(0, range + 1);
                }
                return result;
            }

            public string GetCorrectCombo()
            {
                return String.Join(" ", correctCombo);
            }

            public bool CheckGuess(int[] _guess)
            {
                hasError = false;
                // checks if the length is correct
                if (_guess.Length != correctCombo.Length)
                {
                    Console.WriteLine($"Invalid guess length: must have precisely {digits} values!");
                    hasError = true;
                    return false;
                }

                //checks that all digits are in range
                foreach (int i in _guess)
                {
                    if (i > range || i < 0)
                    {
                        Console.WriteLine($"Invalid guess: One or more values falls outside the specified range (0 - {range})");
                        hasError = true;
                        return false;
                    }
                }

                // calculates correct, wrong, and misplaced digits if all checks pass
                numCorrect = 0;
                numMisplaced = 0;
                numWrong = 0;
                int[] guessSearch = _guess;
                for (int i = 0; i < correctCombo.Length; i++)
                {
                    int index = Array.IndexOf(guessSearch, correctCombo[i]);

                    // if the value is not present anywhere
                    if (index == -1)
                    {
                        numWrong++;
                    }
                    // if the value is an exact match
                    else if (correctCombo[i] == guessSearch[i])
                    {
                        guessSearch[i] = -1;
                        numCorrect++;
                    }
                    // if the value is misplaced
                    else
                    {
                        guessSearch[index] = -1;
                        numMisplaced++;
                    }
                }

                if (numCorrect == digits) return true;
                else return false;
            }
        }
        static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("Welcome to Mastermind!");
            Console.ResetColor();

            int numDigits = 0;
            int numRange = 0;
            int numTries = 0;

            // inputs the number of digits in the answer
            while (numDigits < 1 || numDigits > 10)
            {
                Console.WriteLine("");
                Console.WriteLine("Please specify the number of digits (1-10):");
                try
                {
                    numDigits = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot read integer from input");
                    Console.ResetColor();
                }
                if (numDigits < 1 || numDigits > 10)
                {
                    Console.WriteLine("Integer does not fall within range. Please try again.");
                }
            }

            // Inputs the range of each digit in the answer
            while (numRange < 1 || numRange > 10)
            {
                Console.WriteLine("");
                Console.WriteLine("Please specify the range of digits from 0 (0-10):");
                try
                {
                    numRange = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot read integer from input");
                    Console.ResetColor();
                };

                if (numRange < 1 || numRange > 10)
                {
                    Console.WriteLine("Integer does not fall within range. Please try again.");
                }
            }

            //inputs the number of tries
            while (numTries < 1)
            {
                Console.WriteLine("");
                Console.WriteLine("Please specify the number of tries:");
                try
                {
                    numTries = Int32.Parse(Console.ReadLine());
                }
                catch (Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("ERROR: Cannot read integer from input");
                    Console.ResetColor();
                }
                if (numTries < 1)
                {
                    Console.WriteLine("Integer must be greater than zero.");
                }
            }

            // generates the combination to be guessed
            Combo gameCombo = new Combo(numDigits, numRange, numTries);
            // sets up some crucial "main game" variables
            int[] guessedCombo = new int[numDigits];
            bool isWon = false;

            for (int i = 0; i < gameCombo.tries && !isWon; i++)
            {
                Console.WriteLine("");
                Console.WriteLine($"Tries left: {gameCombo.tries - i}");
                Console.WriteLine($"Please enter {gameCombo.digits} space-seperated values (0-{gameCombo.range}): ");
                guessedCombo = Array.ConvertAll(Console.ReadLine().Split(" "), s => int.Parse(s));

                isWon = gameCombo.CheckGuess(guessedCombo);

                if (isWon)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine("All digits match correctly- YOU WIN!");
                    Console.ResetColor();
                }

                else
                {
                    Console.WriteLine($"You have {gameCombo.numCorrect} digits correct, {gameCombo.numMisplaced} misplaced, and {gameCombo.numWrong} wrong.");
                    if (gameCombo.hasError) i--;
                    if (i == gameCombo.tries - 1) Console.WriteLine($"GAME OVER! The correct combination is: {gameCombo.GetCorrectCombo()}");
                }


            }


        }
    }
}
