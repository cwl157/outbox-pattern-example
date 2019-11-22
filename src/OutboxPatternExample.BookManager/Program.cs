using System;
using System.Collections.Generic;

namespace OutboxPatternExample.BookManager
{
    class Program
    {
        static void Main(string[] args)
        {
            string inputKey = "";
            List<Book> books = new List<Book>();
            while (inputKey != "e")
            {
                Console.WriteLine("Menu. Press 'e' to exit.");
                Console.WriteLine("1. Add Book");
                Console.WriteLine("2. Load Books");
                Console.WriteLine("3. Display Books");
                Console.Write("Input: ");
                inputKey = Console.ReadLine();
                if (inputKey == "1")
                {
                    Book tmp = new Book();
                    Console.WriteLine("Title: ");
                    tmp.Title = Console.ReadLine();
                    Console.WriteLine("Author: ");
                    tmp.Author = Console.ReadLine();
                    books.Add(tmp);
                    DataAccess.SaveBook(tmp);
                    Console.WriteLine("Book Saved Successfully");
                    Console.WriteLine();
                }
                else if (inputKey == "2")
                {
                    books = DataAccess.LoadBooks();

                    Console.WriteLine("Books Loaded Successfully");
                }
                else if (inputKey == "3")
                {
                    foreach (Book b in books)
                    {
                        Console.WriteLine("Title: " + b.Title + ", Author: " + b.Author);
                    }
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("Invalid input");
                }
            }
        }
    }
}
