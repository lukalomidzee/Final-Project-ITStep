using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___Books
{
    // Custom functions for proper functionality
    public class Functions
    {
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

        // Function that shows main menu
        public static void ShowFunctions()
        {
            Console.WriteLine();
            Console.WriteLine("List of the functions:");
            Console.WriteLine("1) Add book");
            Console.WriteLine("2) Remove book");
            Console.WriteLine("3) Find specific book");
            Console.WriteLine("4) Find all books");
            Console.WriteLine("Press 'Enter' to exit");
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
