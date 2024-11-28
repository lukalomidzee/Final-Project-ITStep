using System.Numerics;
using System.Reflection;
using System.Text;
using System.Text.Json;
using static System.Reflection.Metadata.BlobBuilder;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace OOP___Books
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("--- OOP: Book library ---");
            // Get main directory to dinamically access file
            string mainDirectory = Functions.GetMainDirectory();
            // Get filename
            string path = $"{mainDirectory}\\books.json";

            // Create bookmanager instance
            BookManager bm = new BookManager();

            // If file already exists, read library from the file
            if (File.Exists(path))
            {
                // Open filestream
                using (FileStream fs = new FileStream(path, FileMode.Open, FileAccess.Read))
                {
                    // Read its content
                    byte[] buffer = new byte[fs.Length];
                    int bytesRead = fs.Read(buffer, 0, buffer.Length);
                    string readBooksJson = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    
                    // Deserialize json and insert it's content to temporary list
                    List<Book> readBooksList = JsonSerializer.Deserialize<List<Book>>(readBooksJson) ?? new List<Book>();
                    if (readBooksList != null && readBooksList.Any())
                    {
                        // Book class have classId that autoincrements whenever we create new book
                        // We need this step to find out what was last id in the file
                        // Update classId to one higher than the maximum ID
                        Book.UpdateClassId(readBooksList.Max(b => b.Id) + 1);

                        // Assign deserialized books to the main list from the temporary
                        bm.books = readBooksList;
                    }
                }
            }
            else
            {
                // If the file doesn't exist create new list
                Console.WriteLine("File not found. Creating new library");
                bm.books = new List<Book>();
            }
            // Main menu uses do-while loop until user presses the exit to leave
            do
            {
                Functions.ShowFunctions();
                Console.WriteLine();
                // Read user input
                ConsoleKey userKey = Console.ReadKey(true).Key;

                switch (userKey)
                {
                    // Add book - using BookManager AddBook function
                    case ConsoleKey.D1:
                        // Get book details
                        Console.WriteLine("Function - Add book");
                        Console.WriteLine("Enter the title");
                        string bookTitle = Functions.GetStringValidated();
                        Console.WriteLine("Enter the name of author");
                        string bookAuthor = Functions.GetStringValidated();
                        Console.WriteLine("Enter the release year");
                        int bookYear = Functions.GetNumberValidated(false, 0, 2025);
                        
                        // Add new book and log
                        bm.AddBook(new Book(bookTitle, bookAuthor, bookYear));
                        Console.WriteLine($"The book '{bookTitle}' written by {bookAuthor} ({bookYear}) has been added to the library");
                        Console.WriteLine();
                        break;
                    // Remove book - using BookManager RemoveBook function
                    case ConsoleKey.D2:
                        // Get book ID
                        if (bm.books.Count == 0)
                        {
                            Console.WriteLine("Library is empty");
                            break;
                        }
                        Console.WriteLine("Function - Remove book");
                        Console.WriteLine("Enter the ID to remove book");
                        // Checks lowest and highest range (range is determined by lowest and highest IDs)
                        // If the number is out of range, it will remind user to provide correct ID
                        int bookToFindId = Functions.GetNumberValidated(false, 1, Book.maxId - 1);
                        // If the id exists
                        if (bm.books.Any(b => b.Id == bookToFindId))
                        {
                            // Remove book with given ID
                            bm.RemoveBook(bookToFindId);
                            Console.WriteLine();
                        }
                        // If the number is in range but can't find ID
                        else
                        {
                            Console.WriteLine($"Couldn't find the book with the id: {bookToFindId}");
                            break;
                        }
                        break;
                    // Find specific book using ID, Title, Author or Year
                    case ConsoleKey.D3:
                        if (bm.books.Count == 0)
                        {
                            Console.WriteLine("Library is empty");
                            break;
                        }
                        Console.WriteLine("Function - Find specific book");
                        bm.FindBooks();
                        break;
                    // Show all books
                    case ConsoleKey.D4:
                        Console.WriteLine("Function - Show all books");
                        bm.FindAllBooks();
                        Console.WriteLine();
                        break;
                    // Leave main loop
                    case ConsoleKey.Enter:
                        break;
                    // Any other key than D1, D2, D3, D4 and Enter will write this line and restart loop
                    default:
                        Console.WriteLine("Please select the proper function");
                        break;
                }
                // Inform user that they left program
                if (userKey == ConsoleKey.Enter)
                {
                    Console.WriteLine("Leaving program...");
                    break;
                }
            } while (true);
            
            // Json Serialize final list and write it in the 'books.json' file
            // The file updates only when you exit program properly (using 'Enter' button)
            string booksJson = JsonSerializer.Serialize(bm.books);
            using(FileStream fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                byte[] buffer = new byte[booksJson.Length];
                buffer = Encoding.UTF8.GetBytes(booksJson);
                fs.Write(buffer);
            }
        }
    }
}
