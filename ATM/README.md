# ATM Machine Application

This project is a simulation of an ATM machine with core functionalities such as managing credit cards, authorizing users, and performing various banking operations.

## Project Structure

- **`Program.cs`**: Contains the main entry point and core functionality of the application.
- **`Functions.cs`**: Includes custom functions used throughout the application.
- **`CardsManager.cs`**: Manages the creation and handling of the credit card list.

## How It Works

1. When the program starts, the user is unauthorized and has access to two basic options:
   - **Create a new credit card**
   - **Sign into an existing credit card**

2. Credit card information is stored in the `cards.json` file.

3. Once the user is successfully authorized, the following main functionalities are available:

   1. **TransferMoney**: Transfers money to another credit card (requires a valid 16-digit credit card number).
   2. **WithdrawMoney**: Withdraws money from the account balance.
   3. **AddMoney**: Adds money to the account balance.
   4. **ChangePin**: Changes the PIN code of the card.
   5. **PrintCardDetails**: Displays the details of the current card.
   6. **Show Functions Again**: Displays the list of functions again.
   7. **Exit**: Enter `0` to exit the program.

4. When the program ends execution, updated credit card information is automatically saved back to the `cards.json` file.

## Usage

1. Run the application.
2. Follow the prompts to either create a new card or sign in to an existing one.
3. Perform desired operations using the menu options.
4. Exit the program to save any changes to the credit card data.