# Guess The Number Game

This project is a simple number-guessing game where the player attempts to guess a randomly generated number within a specified range.

## Project Structure

- **`Program.cs`**: Contains the main functionality of the game.

## How It Works

1. At the start, the player inputs the bounds for the guessing range (minimum and maximum numbers).
2. The program generates a random number within the specified range.
3. The player has **5 HP (health points)** to guess the correct number:
   - Each incorrect guess reduces the player's HP by 1.
   - If the player runs out of HP, it's **Game Over**.
4. If the player guesses the number correctly, the game ends, and the player wins.
5. At the end of the game, the final result is displayed.

## How to Play

1. Run the application.
2. Enter the minimum and maximum bounds for the guessing range.
3. Guess the number based on the prompts:
   - If the guess is incorrect, you'll lose 1 HP and receive feedback to guide your next guess.
   - If the guess is correct, the game ends with a victory message.
4. Continue guessing until you either win or run out of HP.
5. View the final result at the end of the game.