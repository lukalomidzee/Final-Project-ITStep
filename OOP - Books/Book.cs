using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OOP___Books
{
    // Book class with it's values
    public class Book
    {
        // Auto incremented ID
        private static int classId = 1;
        public static int maxId = classId;

        public Book() { }
        public Book(string title, string author, int releaseYear)
        {
            Id = classId;
            Title = title;
            Author = author;
            ReleaseYear = releaseYear;
            classId++;
        }

        public int Id { get; set; }
        public string Title { get; set; } = string.Empty; // Default to prevent null
        public string Author { get; set; } = string.Empty; // Default to prevent null

        public int ReleaseYear { get; set; }

        // Update when we read from json at the beginning to avoid overwriting ID's
        public static void UpdateClassId(int newId)
        {
            if (newId > classId)
            {
                classId = newId;
                maxId = newId;
            }
        }
    }
}
