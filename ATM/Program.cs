using ATM;
using System.IO;
using System.Runtime.Intrinsics.Arm;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Homework_13
{
    internal class Program
    {
        static void Main(string[] args)
        {
            // Get main directory to dinamically access file
            string mainDirectory = Functions.GetMainDirectory();
            // Get filename
            string path = $"{mainDirectory}\\cards.json";

            bool isLoggedIn = false;

            CardsManager cardsManager = new CardsManager();

            if (File.Exists(path))
            {
                // Open filestream
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Read its content
                    byte[] buffer = new byte[fs.Length];
                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                    string readCardsJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);

                    // Deserialize json and insert it's content to temporary list
                    List<CreditCard> readCardsList = JsonSerializer.Deserialize<List<CreditCard>>(readCardsJson) ?? new List<CreditCard>();
                    if (readCardsList != null && readCardsList.Any())
                    {
                        // Book class have classId that autoincrements whenever we create new book
                        // We need this step to find out what was last id in the file
                        // Update classId to one higher than the maximum ID
                        CreditCard.UpdateClassId(readCardsList.Max(b => b.Id) + 1);

                        // Assign deserialized books to the main list from the temporary
                        cardsManager.cards = readCardsList;
                    }
                }
            }
            else
            {
                // If the file doesn't exist create new list
                Console.WriteLine("File not found. Creating new library");
                cardsManager.cards = new List<CreditCard>();
            }




            Console.WriteLine("--- ATM ---");
            Console.WriteLine("Welcome to ATM machine:");
            Console.WriteLine();

            // Initialize CreditCard to assign it to the authorized card
            CreditCard? loggedInCard = null;

            // Do this cycle while the user is unauthorized
            // Unauthorized user can only create a new card or sign in to account
            do
            {
                Functions.ShowFunctionsUnauthorized();
                ConsoleKey userKey = Console.ReadKey(true).Key;
                switch (userKey)
                {
                    // Create a new card
                    case ConsoleKey.D1:
                        Console.WriteLine("To create a credit card, please provide the following details");
                        Console.WriteLine();

                        // Only card holder is needed to set up new card
                        Console.WriteLine("Card holder:");
                        string cardHolder = Functions.GetStringValidated();
                        Console.WriteLine();

                        // Add a new card to cards list
                        cardsManager.AddCard(cardHolder);

                        Console.WriteLine("Created a credit card with the following details:");
                        Console.WriteLine();

                        // Serialize cards list and write it into json
                        string cardsJson = JsonSerializer.Serialize(cardsManager.cards);

                        using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                        {
                            byte[] buffer = new byte[cardsJson.Length];
                            buffer = Encoding.UTF8.GetBytes(cardsJson);
                            fs.Write(buffer);
                        }
                        break;
                    // Sign in using card ID
                    case ConsoleKey.D2:
                        Console.WriteLine("Please select the card (Using ID)");
                        int cardId = Functions.GetNumberValidated(false, 1, 1000);
                        // Find card
                        CreditCard? selectedCard = cardsManager.cards.FirstOrDefault(card => card.Id == cardId);
                        if(selectedCard != null)
                        {
                            // Authorize using pincode
                            Console.WriteLine("Please enter the PIN:");
                            int success = selectedCard.CheckPin();
                            if (success == 0)
                            {
                                // Terminate unauthorized loop if user successfully logged in
                                isLoggedIn = true;
                                Console.WriteLine("Successfully logged in!");
                                loggedInCard = selectedCard;
                                Console.WriteLine();
                                break;
                            }
                            else
                            {
                                // Don't need to write incorrect pin because pinchecker method of the card already alerts user
                                Console.WriteLine();
                                break;
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Couldn't find the card with the given ID: {cardId}");
                            Console.WriteLine();
                            break;
                        }
                    // Exit program
                    case ConsoleKey.Enter:
                        break;
                    default:
                        Console.WriteLine("Please select proper operation");
                        break;
                }
                if (userKey == ConsoleKey.Enter)
                {
                    Console.WriteLine("Leaving program...");
                    break;
                }
            } while (!isLoggedIn);

            // Show main menu if user successfully logged in
            if (isLoggedIn)
            {
                // Do-While loop to choose functions
                do
                {
                    Functions.ShowFunctionsAuthorized();
                    Console.WriteLine("Please enter the function number");
                    // Custom function to select desired method
                    int functionNumber = Functions.GetFunctionValidated(false, 0, 6);
                    Console.WriteLine();
                    switch (functionNumber)
                    {
                        case 1:
                            Console.WriteLine("Function - TransferMoney");
                            loggedInCard?.TransferMoney();
                            break;
                        case 2:
                            Console.WriteLine("Function - WithdrawMoney");
                            loggedInCard?.WithdrawMoney();
                            break;
                        case 3:
                            Console.WriteLine("Function - AddMoney");
                            loggedInCard?.AddMoney();
                            break;
                        case 4:
                            Console.WriteLine("Function - ChangePin");
                            loggedInCard?.ChangePin();
                            break;
                        case 5:
                            Console.WriteLine("Function - PrintCardDetails");
                            loggedInCard?.PrintCardDetails();
                            break;
                        case 6:
                            Functions.ShowFunctionsAuthorized();
                            break;
                        case 0:
                            break;
                        default:
                            Console.WriteLine("Please enter the function number correctly");
                            Console.WriteLine();
                            break;
                    }
                    if (functionNumber == 0)
                    {
                        Console.WriteLine("Leaving program...");
                        break;
                    }
                } while (true);
                // Final step - writing current data into the JSON file
                // Find the card that we want to update
                CreditCard? cardToUpdate = cardsManager.cards.FirstOrDefault(c => c.Id == loggedInCard.Id);
                if (cardToUpdate != null)
                {
                    // Update details
                    cardToUpdate.Balance = loggedInCard.Balance;
                    cardToUpdate.Pincode = loggedInCard.Pincode;
                    Console.WriteLine("Card details saved successfully");
                }
                else
                {
                    Console.WriteLine("Something went wrong while updating card");
                }

                // Serialize final version of card list and write it to the json file
                string updatedJson = JsonSerializer.Serialize(cardsManager.cards);

                using (FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
                {
                    byte[] buffer = new byte[updatedJson.Length];
                    buffer = Encoding.UTF8.GetBytes(updatedJson);
                    fs.Write(buffer);
                }
            }
        }
    }

    // Credit card class
    public class CreditCard
    {
        // Autoincremented ID
        private static int cardId = 1;
        public static int maxId = cardId;
        public CreditCard() { }
        public CreditCard(string holder)
        {
            Id = cardId;
            CardNumber = CardNumberGenerator();
            CardHolder = holder.ToUpper();
            Validity = DateOnly.FromDateTime(DateTime.Now).AddYears(6);
            Pincode = PinGenerator();
            Balance = 25000;
            cardId++;
        }

        public int Id { get; set; }

        
        public string CardNumber { get; set; }

        
        public string CardHolder { get; set; }

        
        public DateOnly Validity { get; set; }

        
        public int Pincode { get; set; }

        
        public decimal Balance { get; set; }

        #region Public methods

        // Update class ID whenever we read from JSON file
        public static void UpdateClassId(int newId)
        {
            if (newId > cardId)
            {
                cardId = newId;
                maxId = newId;
            }
        }
        // Transfer money to another CreditCard
        public void TransferMoney()
        {
            // Check pin
            Console.WriteLine("Please enter the pincode to preceed:");
            int checker = CheckPin();
            if (checker == 0)
            {
                // Enter amount and check if it's possible to transfer
                Console.WriteLine("Please enter the transfer amount:");
                decimal transferAmount = GetMoneyValidated(false, 0, decimal.MaxValue);
                if (transferAmount > Balance)
                {
                    Console.WriteLine("Insufficient funds!");
                    Console.WriteLine();
                }
                else if (transferAmount <= Balance)
                {
                    Console.WriteLine("Please enter the card number to proceed");

                    string transferCard = Console.ReadLine() ?? "";

                    if (transferCard.Length == 16)
                    {
                        Balance = Balance - transferAmount;
                        Console.WriteLine($"{transferAmount} has been transferred from your balance to {transferCard.ToUpper()}");
                        Console.WriteLine();
                        Console.WriteLine($"Your balance is - {Balance}");
                        Console.WriteLine();
                    }
                    else
                    {
                        Console.WriteLine("Something went wrong, please try again");
                        Console.WriteLine();
                    }
                }
            }
        }

        // Withdraw money
        public void WithdrawMoney()
        {
            // Check pin
            Console.WriteLine("Please enter the pincode to preceed:");
            int checker = CheckPin();
            if (checker == 0)
            {
                // Enter the amount and check if it's possible
                Console.WriteLine("Please enter the withdraw amount:");
                decimal withdrawAmount = GetMoneyValidated(false, 0, decimal.MaxValue);
                if (withdrawAmount > Balance)
                {
                    Console.WriteLine("Insufficient funds!");
                    Console.WriteLine();
                }
                else if (withdrawAmount <= Balance)
                {
                    Balance = Balance - withdrawAmount;
                    Console.WriteLine($"{withdrawAmount} has been withdrawn from your balance");
                    Console.WriteLine();
                    Console.WriteLine($"Your balance is - {Balance}");
                    Console.WriteLine();
                }

            }
        }

        public void AddMoney()
        {
            // Check pin
            Console.WriteLine("Please enter the pincode to preceed:");
            int checker = CheckPin();
            if (checker == 0)
            {
                Console.WriteLine("Please enter the insert amount:");
                decimal insertAmount = GetMoneyValidated(false, 0, decimal.MaxValue);

                Balance = Balance + insertAmount;
                Console.WriteLine($"{insertAmount} has been added to your balance");
                Console.WriteLine();
                Console.WriteLine($"Your balance is - {Balance}");
                Console.WriteLine();
            }
        }

        // Change pincode
        public void ChangePin()
        {
            Console.WriteLine("Please enter the old pincode:");
            int checker = CheckPin();
            if (checker == 0)
            {
                Console.WriteLine("Please enter the new pincode:");
                int newPin = GetPinValidated(false, 1000, 9999);
                Pincode = newPin;
                Console.WriteLine($"Pincode changed, new pincode is {Pincode}");
                Console.WriteLine();
            }
        }

        // Print details
        public void PrintCardDetails()
        {
            Console.WriteLine($"Card id: {Id}");
            Console.WriteLine($"Card number: {CardNumber}");
            Console.WriteLine($"Card holder: {CardHolder}");
            Console.WriteLine($"Valid: {Validity}");
            Console.WriteLine($"Pin: {Pincode}");
            Console.WriteLine($"Balance: {Balance}");
            Console.WriteLine();
        }

        // Check pincode
        public int CheckPin()
        {
            int inputPin = GetPinValidated(false, 1000, 9999);
            if (inputPin == Pincode) return 0;
            else
            {
                Console.WriteLine("Incorrect Pincode");
                return 1;
            }
        }

        #endregion


        #region Private methods

        // Generate card number whenever we create new card
        private string CardNumberGenerator()
        {
            Random randomizer = new Random();
            StringBuilder generatedNumber = new StringBuilder();

            for (int i = 0; i < 16; i++)
            {
                generatedNumber.Append(randomizer.Next(0, 10));
            }

            return generatedNumber.ToString();
        }

        // Generate new pin
        private int PinGenerator()
        {
            Random random = new Random();
            return random.Next(0, 9999);
        }
        
        // Validate PIN input
        private int GetPinValidated(bool allowNegative, int minValue = 1000, int maxValue = 9999)
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

        // Validate Money amount
        private decimal GetMoneyValidated(bool allowNegative, decimal minValue = 1000, decimal maxValue = decimal.MaxValue)
        {
            bool isValid;
            decimal output;
            do
            {
                isValid = decimal.TryParse(Console.ReadLine(), out output);
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
        #endregion
    }
}
