using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___Books
{
    // BookManager class has all the function that library needs
    public class BookManager
    {
        // Main list
        public List<Book> books = new List<Book>();

        // Add book
        public void AddBook(Book book)
        {
            books.Add(book);
        }

        // Remove book
        public void RemoveBook(int id)
        {
            Book? bookToRemove = books.FirstOrDefault(b => b.Id == id);
            if (bookToRemove != null)
            {
                Console.WriteLine($"The book '{bookToRemove.Title} by {bookToRemove.Author}' with ID {id} removed.");
                books.Remove(bookToRemove);
            }
        }

        // Find book
        public void FindBooks()
        {
            Console.WriteLine("Find by:\n" + "1)ID\n" + "2)Title\n" + "3)Author\n" + "4)Year\n");
            // Get user key to identify find method (Using LINQ methods)
            ConsoleKey findKey = Console.ReadKey(true).Key;
            switch (findKey)
            {
                // Find by ID
                case ConsoleKey.D1:
                    Console.WriteLine("Please enter the ID to find the book");
                    int bookToFindId = Functions.GetNumberValidated(false, 1, books.Count);
                    Book? bookToFind = books.FirstOrDefault(b => b.Id == bookToFindId);
                    if (bookToFind == null)
                    {
                        Console.WriteLine("Book not found");
                    }
                    else
                    {
                        Console.WriteLine($"book that you was looking for is \n- Id: {bookToFind.Id}, Title: {bookToFind.Title}, Author: {bookToFind.Author}, Release year: {bookToFind.ReleaseYear}");
                    }
                    Console.WriteLine();
                    break;
                // Find by Title
                case ConsoleKey.D2:
                    Console.WriteLine("Please enter the Title to find the book");
                    string bookToFindTitle = Functions.GetStringValidated();
                    List<Book> booksByTitle = books
                    .Where(t => t.Title != null && t.Title.Contains(bookToFindTitle, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                    if (booksByTitle.Any())
                    {
                        Console.WriteLine($"Books that contain {bookToFindTitle} in title:");
                        foreach (var book in booksByTitle)
                        {
                            Console.WriteLine($"- Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Release year: {book.ReleaseYear}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found with provided title.");
                    }
                    break;
                // Find by Author
                case ConsoleKey.D3:
                    Console.WriteLine("Please enter the Author to find the book");
                    string bookToFindAuthor = Functions.GetStringValidated();
                    List<Book> booksByAuthor = books
                    .Where(a => a.Author != null && a.Author.Contains(bookToFindAuthor, StringComparison.OrdinalIgnoreCase))
                    .ToList();
                    if (booksByAuthor.Any())
                    {
                        Console.WriteLine($"Books by the author: {bookToFindAuthor}");
                        foreach (var book in booksByAuthor)
                        {
                            Console.WriteLine($"- Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Release year: {book.ReleaseYear}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found with provided author.");
                    }
                    break;
                // Find by Year
                case ConsoleKey.D4:
                    Console.WriteLine("Please enter the year to find the book");
                    int bookToFindYear = Functions.GetNumberValidated(false, 1, 2025);
                    List<Book> booksByYear = books
                    .Where(y => y.ReleaseYear == bookToFindYear)
                    .ToList();
                    if (booksByYear.Any())
                    {
                        Console.WriteLine($"Books that was released in: {bookToFindYear}");
                        foreach (var book in booksByYear)
                        {
                            Console.WriteLine($"- Id:  {book.Id} , Title:  {book.Title} , Author:  {book.Author} , Release year:  {book.ReleaseYear}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("No books found with provided year.");
                    }
                    break;
                default:
                    Console.WriteLine("Please select the proper function");
                    break;
            }
        }

        // Print out all the books using foreach loop
        public void FindAllBooks()
        {
            if (books.Count == 0)
            {
                Console.WriteLine("Books not found");
            }
            else
            {
                foreach (var book in books) Console.WriteLine($"Id: {book.Id}, Title: {book.Title}, Author: {book.Author}, Release year: {book.ReleaseYear}");
            }
        }
    }
}
