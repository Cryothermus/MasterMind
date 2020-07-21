using System;
using System.Linq;

namespace MasterMind
{
    class Program
    {
        static void WriteColor(ConsoleColor color, string text) //prints a line in the given color
        {
            Console.ForegroundColor = color;
            Console.WriteLine(text);
            Console.ResetColor();
        }
        
        static int takeRangeInput(string message, int min, int max) // takes input within a range
        {
            int result = min -  1;
            while (result < min || result > max)
            {
                Console.WriteLine("");
                Console.WriteLine(message);
                try
                {
                    result = Int32.Parse(Console.ReadLine());
                    if (result < min || result > max)
                    {
                        Console.WriteLine("Integer does not fall within range. Please try again.");
                    }
                }
                catch (Exception)
                {
                    WriteColor(ConsoleColor.Red, "ERROR: Cannot read integer from input.");
                }
               
            }
            return result;
        }

        class Combo
        {
            public int digits { get;}
            public int range { get; }
            public int tries { get; }
            public int[] correctCombo { get; }
            public int numCorrect { get; set; }
            public int numMisplaced { get; set; }
            public int numWrong { get; set; }
            public bool hasError { get; set; }
            public Combo(int _digits, int _range, int _tries)
            {
                digits = _digits; //no. of digits in the correct combo
                range = _range; // range of the digits
                tries = _tries; //number of tries the player has
                correctCombo = CreateCombo(digits, range); // the combo itself
                numCorrect = 0; // digits completely correct in the guess
                numMisplaced = 0; //digits that are correct, but misplaced in the guess
                numWrong = 0; // digits completely wrong
                hasError = false; //is true if the input causes an error

            }

            private int[] CreateCombo(int digits, int range) //generates the correct combo
            {
                var random = new Random();
                int[] result = new int[digits];
                for (int i = 0; i < result.Length; i++) // apparently, foreach can't change array contents- good to know
                {
                    result[i] = random.Next(0, range + 1);
                }
                return result;
            }

            public string GetCorrectCombo() // gets the correct combo as a string
            {
                return String.Join(" ", correctCombo);
            }
            
            public bool CheckGuess(int[] _guess) //checks a guess against the correct combo
            {
                hasError = false;
                // checks if the length is correct
                if (_guess.Length != correctCombo.Length)
                {
                    /*if (_guess.Length != 1)*/ Console.WriteLine($"Invalid guess length: must have precisely {digits} values!");
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
            //intro + some crucial inputs
            WriteColor(ConsoleColor.Yellow, "Welcome to Mastermind!");

            int numDigits = 0;
            int numRange = 0;
            int numTries = 0;

            // inputs the number of digits in the answer
            numDigits = takeRangeInput("Please specify the number of digits (1-10)", 1, 10);
           
            // Inputs the range of each digit in the answer
            numRange = takeRangeInput("Please specify the range of the digits from 0 (0-9)", 1, 9);
           
            //inputs the number of tries
            numTries = takeRangeInput("Please specify the number of tries: ", 1, 999);
            
            // generates the combination to be guessed
            Combo gameCombo = new Combo(numDigits, numRange, numTries);
            // sets up some crucial "main game" variables
            int[] guessedCombo = new int[numDigits];
            //for (int i = 0; i < guessedCombo.Length; i++) guessedCombo[i] = -1;
            bool isWon = false;

            //does the main game
            for (int i = 0; i < gameCombo.tries && !isWon; i++)
            {
                Console.WriteLine("");
                Console.WriteLine($"Tries left: {gameCombo.tries - i}");
                Console.WriteLine($"Please enter {gameCombo.digits} space-seperated values (0-{gameCombo.range}): ");

                //takes guess input
                try 
                {
                    guessedCombo = Array.ConvertAll(Console.ReadLine().Split(" "), s => int.Parse(s));
                    //Console.WriteLine(guessedCombo[0]);
                    isWon = gameCombo.CheckGuess(guessedCombo);
                }
                catch (Exception)
                {
                    WriteColor(ConsoleColor.Red, "ERROR: input has caused an exception and has been discarded.");
                    gameCombo.hasError = true;
                }
               


                // win condition
                if (isWon)
                {
                    WriteColor(ConsoleColor.Green, "All digits match correctly- YOU WIN!");
                }

                // if an error occurs
                else if (gameCombo.hasError)
                {
                    i--;
                    //Console.WriteLine("Try refunded."); // debug
                }

                //if the guess is incorrect
                else
                {
                    Console.WriteLine($"You have {gameCombo.numCorrect} digits correct, {gameCombo.numMisplaced} misplaced, and {gameCombo.numWrong} wrong.");

                    if (i == gameCombo.tries - 1) WriteColor( ConsoleColor.Yellow, $"GAME OVER! The correct combination is: {gameCombo.GetCorrectCombo()}");
                }


            }


        }
    }
}
