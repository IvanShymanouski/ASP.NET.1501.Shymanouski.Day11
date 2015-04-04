using System;
using System.IO;
using SerialisationRepository;
using XmlHandlers;
using System.Collections.Generic;

namespace Books
{
    class Program
    {
        static void Main(string[] args)
        {

            BookListService books = new BookListService(new BinaryFileRepository(Directory.GetCurrentDirectory() + "\\default.txt"));

            #region WriteInNewRepositoryWhere
            IEnumerable<Book> selectBooks = books.WriteInNewRepositoryWhere(new BinaryFileRepository(Directory.GetCurrentDirectory() + "\\default2.txt"), (x) => { if (x.Author == "Joan Roling") return true; else return false; });

            Console.WriteLine("WriteInNewRepositoryWhere: x.Author == \"Joan Roling\":");
            foreach (var bookThatAuthor in selectBooks) Console.WriteLine(bookThatAuthor.Author + " " + bookThatAuthor.Title);
            Console.WriteLine();
            #endregion

            #region SerialisationTest
            BinarySerialisationRepository serialRepository = new BinarySerialisationRepository(Directory.GetCurrentDirectory() + "\\default3.txt");
            BookListService otherBooks = new BookListService(serialRepository);

            serialRepository.SaveBooks(books.GetRepositoryBooks());

            Console.WriteLine("Books in serialRepositiry:");
            foreach (var serialBook in otherBooks.GetRepositoryBooks()) Console.WriteLine(serialBook.Author + " " + serialBook.Title);
            Console.WriteLine();
            #endregion

            #region ExportXml
            XmlExporter xml = new XmlExporter();
            string xmlData;
            xml.Export(books.GetRepositoryBooks(), Directory.GetCurrentDirectory() + "\\Books.xml");
            xml.ReadXml(out xmlData, Directory.GetCurrentDirectory() + "\\Books.xml");       
            #endregion

            #region BookListXmlSave
            books.Export(books.GetRepositoryBooks(), Directory.GetCurrentDirectory() + "\\BookListService.xml");
            Console.WriteLine(File.ReadAllText(Directory.GetCurrentDirectory() + "\\BookListService.xml"));
            #endregion

            #region old Code
            Console.WriteLine("-------------");

            ShowBooks(books);
            Console.WriteLine("-------------");

            #region book Joan Roling
            Book book = new Book()
            {
                Author = "Joan Roling",
                PageCount = 133,
                Price = 10000,
                Title = "King of the kingdom"
            };

            try
            {
                Console.WriteLine("Adding " + book.Author + " " + book.Title);
                books.AddBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            #region book Jon Skeet
            book = new Book()
            {
                Author = "Jon Skeet",
                PageCount = 614,
                Price = 1500,
                Title = "C# in Depth"
            };

            try
            {
                Console.WriteLine("Adding " + book.Author + " " + book.Title);
                books.AddBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            #region Jon Skeet
            book = new Book()
            {
                Author = "Jon Skeet",
                PageCount = 644,
                Price = 15044,
                Title = "C# in Depth. Third edition"
            };
            try
            {
                Console.WriteLine("Adding " + book.Author + " " + book.Title);
                books.AddBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            Console.WriteLine("Jon Skeet books in repositiry:");
            foreach (var bookThatAuthor in books.GetBooksByAuthor("Jon Skeet"))
            {
                Console.WriteLine(bookThatAuthor.Title);
            }

            Console.WriteLine("-------------");

            #region book Jeffrey Richter removing
            book = new Book()
            {
                Author = "Jeffrey Richter",
                PageCount = 896,
                Price = 98762,
                Title = "CLR via C#"
            };

            try
            {
                Console.WriteLine("Removing " + book.Author + " " + book.Title);
                books.RemoveBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            #region book Jeffrey Richter adding
            book = new Book()
            {
                Author = "Jeffrey Richter",
                PageCount = 896,
                Price = 98762,
                Title = "CLR via C#"
            };

            try
            {
                Console.WriteLine("Adding " + book.Author + " " + book.Title);
                books.AddBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            #region book Jeffrey Richter adding
            book = new Book()
            {
                Author = "Jeffrey Richter",
                PageCount = 896,
                Price = 98762,
                Title = "CLR via C#"
            };

            try
            {
                Console.WriteLine("Adding " + book.Author + " " + book.Title);
                books.AddBook(book);
            }
            catch { }
            #endregion

            Console.WriteLine("-------------");

            ShowBooks(books);

            Console.WriteLine("\nBooks sorting\n");
            books.SortBooks((x, y) => x.CompareTo(y)*-1);

            ShowBooks(books);
            #endregion

        }

        static void ShowBooks(BookListService books)
        {
            Console.WriteLine("Books in repositiry:");
            foreach (var bookThatAuthor in books.GetRepositoryBooks())
            {

                Console.WriteLine(bookThatAuthor.Author + " " + bookThatAuthor.Title);
            }
        }
    }
}
