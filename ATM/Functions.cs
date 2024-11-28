using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATM
{
    // Custom functions for proper functionality
    public class Functions
    {
        // Custom function that helps loggedin user select the method
        public static int GetFunctionValidated(bool allowNegative, int minValue = 1, int maxValue = 6)
        {
            bool isValid;
            int output;
            do
            {
                isValid = int.TryParse(Console.ReadLine(), out output);
                if (!isValid)
                {
                    Console.WriteLine("Please provide a number.");
                }
                else if (!allowNegative && output < 0)
                {
                    Console.WriteLine("Please provide a positive number.");
                    isValid = false;
                }
                else if (output < minValue)
                {
                    Console.WriteLine("Please provide a number greater than or equal to {0}.", minValue);
                    isValid = false;
                }
                else if (output > maxValue)
                {
                    Console.WriteLine("Please provide a number less than or equal to {0}", maxValue);
                    isValid = false;
                }
            } while (!isValid);
            return output;
        }
        public static int GetNumberValidated(bool allowNegative, int minValue = 1, int maxValue = 2025)
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
                else if (output > maxValue)
                {
                    Console.WriteLine("Please provide a number less than or equal to {0}.", maxValue);
                    isValid = false;
                }
            } while (!isValid);
            // Returns the number if it's validated
            return output;
        }

        // Custom function to validate string inputs
        public static string GetStringValidated()
        {
            // Initial variables to check input
            string input;
            bool isEmpty;
            do
            {
                // Get input from user
                input = Console.ReadLine() ?? "";
                // If the input is empty, it should print a warning and restart loop;
                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Please provide details");
                    isEmpty = true;
                    continue;
                }
                // If it's not empty, the program should continue
                isEmpty = false;
            } while (isEmpty);
            return input;
        }

        // Function for unauthorized users
        public static void ShowFunctionsUnauthorized()
        {
            Console.WriteLine("Choose the operation from the following list:");
            Console.WriteLine("1) Create card");
            Console.WriteLine("2) Insert card");
            Console.WriteLine("Press 'Enter' to exit");
            Console.WriteLine();
        }

        // Function for authorized users
        public static void ShowFunctionsAuthorized()
        {
            Console.WriteLine("Choose the operation from the following list:");
            Console.WriteLine("1) TransferMoney - Transfers the money to the another CreditCard (Must provide 16-letter Credit card number)");
            Console.WriteLine("2) WithdrawMoney - Withdraw money from balance");
            Console.WriteLine("3) AddMoney - Insert money to balance");
            Console.WriteLine("4) ChangePin - Change Pincode");
            Console.WriteLine("5) PrintCardDetails - Print details of the card");
            Console.WriteLine("6) Show functions again");
            Console.WriteLine("0) Enter '0' to exit");
            Console.WriteLine();
        }

        // Custom function to get directory path where json should be created
        public static string GetMainDirectory()
        {
            // Getting parent directorys to access OOP - Books directory
            // Program.cs and books.json should be located in the same folder
            string currentDirectory = Directory.GetCurrentDirectory();
            var parentDir = Directory.GetParent(currentDirectory);
            if (parentDir?.Parent?.Parent != null)
            {
                return parentDir.Parent.Parent.FullName;
            }
            return string.Empty;
        }
    }
}
