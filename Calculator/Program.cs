using System.Collections.Generic;
using System.ComponentModel;

namespace Calculator
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- Calculator ---");
            // Create calculator instance to use it's functions
            Calculator calc = new Calculator();

            // Create dictionary that matches the input key (function numbers)
            // Whenever the user presses any of the following button, the function invokes
            var operations = new Dictionary<ConsoleKey, Action<double, double>>
            {
                { ConsoleKey.D1, calc.Add},
                { ConsoleKey.D2, calc.Minus },
                { ConsoleKey.D3, calc.Multiply },
                { ConsoleKey.D4, calc.Divide }
            };
            
            // Do this loop while any of the state breaks it
            do
            {
                Console.WriteLine("Please select the function:");
                ShowFunctions();

                // Get user input
                ConsoleKey functionNumber = Console.ReadKey(true).Key;

                // Break if user presses 'Enter'
                if (functionNumber == ConsoleKey.Enter)
                {
                    Console.WriteLine("Exited calculator");
                    Console.WriteLine();
                    break;
                }

                // Restart the loop if user presses the random key using 'continue' statement
                if (!operations.ContainsKey(functionNumber))
                {
                    Console.WriteLine("Select a proper function.");
                    Console.WriteLine();
                    continue;
                }
                
                // Get the first number using custom validated function
                Console.WriteLine("Please provide the first number:");
                double firstNumber = GetNumberValidated(true, double.MinValue);
                Console.WriteLine();

                // Get the second number using custom validated function
                Console.WriteLine("Please provide the second number:");
                double secondNumber = GetNumberValidated(true, double.MinValue);
                Console.WriteLine();

                // Invoke the function mapped to the user input key
                operations[functionNumber](firstNumber, secondNumber);

            } while (true);
        }

        // Function that writes description in console
        static void ShowFunctions()
        {
            Console.WriteLine();
            Console.WriteLine("List of the functions:");
            Console.WriteLine("1) Addition");
            Console.WriteLine("2) Subtraction");
            Console.WriteLine("3) Multiplication");
            Console.WriteLine("4) Division");
            Console.WriteLine("Press 'Enter' to exit");
            Console.WriteLine();
        }

        // Custom function to validate numeric inputs - Takes two arguments 1) allowNegative and 2) minValue
        // It validates the number depending on these arguments
        static double GetNumberValidated(bool allowNegative, double minValue = 1.00)
        {
            // Boolean to check over the cycle
            bool isValid;
            // Validated output
            double output;
            // Do this cycle until user inputs the correct number
            do
            {
                // Check if the input is valid
                isValid = double.TryParse(Console.ReadLine(), out output);
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

    // Calculator class that has basic functions (+, -, *, /)
    // No input or output is stored. It just prints out the result to save memory from unnecessary variables
    public class Calculator
    {
        public Calculator()
        {
            Console.WriteLine("Calculator created");
        }
        public void Add(double firstNumber, double secondNumber)
        {
            Console.WriteLine($"Sum: {firstNumber} + {secondNumber} = {Math.Round(firstNumber + secondNumber,2)}");
            Console.WriteLine();
        }

        public void Minus(double firstNumber, double secondNumber)
        {
            Console.WriteLine($"Difference: {firstNumber} - {secondNumber} = {Math.Round(firstNumber - secondNumber, 2)}");
            Console.WriteLine();
        }

        public void Multiply(double firstNumber, double secondNumber)
        {
            Console.WriteLine($"Multiply: {firstNumber} * {secondNumber} = {Math.Round(firstNumber * secondNumber, 2)}");
            Console.WriteLine();
        }

        public void Divide(double firstNumber, double secondNumber)
        {
            // Check if the secondNumber is '0' to avoid DivideByZeroException
            if (secondNumber == 0)
            {
                Console.WriteLine("Cannot divide by Zero");
                Console.WriteLine();
            }
            else
            {
                Console.WriteLine($"Divide: {firstNumber} / {secondNumber} = {Math.Round(firstNumber / secondNumber, 2)}");
                Console.WriteLine();
            }
        }
    }
}
