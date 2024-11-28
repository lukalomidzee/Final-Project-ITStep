namespace GuessTheNumber
{
    internal class Program
    {
        // Global variable to avoid hard-coded numbers
        private const int InitialHealthPoints = 5;
        static void Main(string[] args)
        {
            Console.WriteLine("--- Guess the number ---");

            // Get the minimum number for range
            Console.WriteLine("Please enter the minimum number to determine range:");
            int minNumber = GetNumberValidated(true, int.MinValue);
            Console.WriteLine();

            // Get the maximum number for range
            Console.WriteLine("Please enter the maximum number to determine range:");
            // NOTE: Custom validation gives you the ability to set the minimum line while validating the number
            // In this case maxNumber can't be less than minNumber + 2
            // It will ask you to enter the number until you provide it correctyl
            int maxNumber = GetNumberValidated(true, minNumber + 2);
            Console.WriteLine();

            // Randomizer to generate random int
            Random random = new Random();
            int number = random.Next(minNumber, maxNumber);

            // Initial variables
            int healthPoints = InitialHealthPoints;
            int tries = 0;

            Console.WriteLine($"The number that you have to guess is between {minNumber}(inclusive) and {maxNumber}(exclusive)");
            Console.WriteLine();

            Console.WriteLine("You have 5HP. Everytime you type the wrong number, the health decreases. Now, guess the number...");
            Console.WriteLine();
            
            // Do this cycle while healthPoints is more than 0
            do
            {
                // Get user guess
                Console.WriteLine("Enter the number...");
                int userTry = GetNumberValidated(true, int.MinValue);
                Console.WriteLine();
                // Check if the guess is more than main number
                if (userTry > number)
                {
                    // Decrease HP
                    healthPoints--;
                    // Increase tries
                    tries++;
                    if(userTry > maxNumber)
                    {
                        Console.WriteLine("Your guess is out of bounds");
                    }
                    Console.WriteLine($"Lower... You have {healthPoints}HPs left");
                    Console.WriteLine();
                }
                // Check if the guess is less than main number
                else if (userTry < number)
                {
                    // Decrease HP
                    healthPoints--;
                    // Increase tries
                    tries++;
                    if (userTry < minNumber)
                    {
                        Console.WriteLine("Your guess is out of bounds");
                    }
                    Console.WriteLine($"Higher... You have {healthPoints}HPs left");
                    Console.WriteLine();
                }
                // If the number is not more or less than main number, then it means that you have guessed it correctly
                else
                {
                    tries++;
                    Console.WriteLine($"Wow! You guessed the number {number} correctly in {tries} try!");
                    // Break the loop if guessed correctyl
                    break;
                }
            } while (healthPoints > 0);
            // There are only two options to exit the loop, 1) You loose or 2) You win
            // If you loose, program prints the Game Over paragraph
            // I check it once after you exit the loop to avoid it checking over and over again during the loop
            if(healthPoints == 0)
            {
                Console.WriteLine($"You couldn't guess the number {number} correctly in {tries} tries. Game over...");
            }
        }

        static int GetNumberValidated(bool allowNegative, int minValue = 1)
        {
            // Boolean to check over the cycle
            bool isValid;
            // Validated output
            int output;
            // Do this cycle until user inputs the correct number
            do
            {
                // Check if the input is valid
                isValid = int.TryParse(Console.ReadLine(), out output);
                if (!isValid)
                {
                    Console.WriteLine("Please provide a number.");
                }
                // Check if the input is negative
                else if (!allowNegative && output < 0)
                {
                    Console.WriteLine("Please provide a positive number.");
                    isValid = false;
                }
                // Check if the input is greater than minValue
                else if (output < minValue)
                {
                    Console.WriteLine("Please provide a number greater than or equal to {0}.", minValue);
                    isValid = false;
                }
            } while (!isValid);
            // Returns the number if it's validated
            return output;
        }
    }
}
