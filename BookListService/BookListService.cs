using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using NLog;

namespace Books
{
    public class BookListService : IXmlFormatExporter
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        private readonly IBookRepository repository;

        public BookListService(IBookRepository repository)
        {
            this.repository = repository;
        }

        public void AddBook(Book book)
        {
            if (book == null) throw new ArgumentNullException("Book must not be null");

            IEnumerable<Book> books = repository.LoadBooks();
            List<Book> newListBooks = new List<Book>();

            foreach (Book existBook in books)
            {
                newListBooks.Add(existBook);
                if (book.Equals(existBook))
                {
                    logger.Error("This book already exist in the repository "+DateTime.Now);
                    throw new ArgumentException("This book already exist in the repository");
                }
            }

            newListBooks.Add(book);
            repository.SaveBooks(newListBooks);
        }

        //отсортировать книги в хранилище по определенному признаку
        public void SortBooks(Comparison<Book> comparer)
        {
            if (comparer == null) throw new ArgumentNullException("Comparer must not be null");

            IEnumerable<Book> books = repository.LoadBooks();
            Book[] booksAray = books.ToArray();
            
            Array.Sort<Book>(booksAray, comparer);

            books = booksAray.ToList<Book>();

            repository.SaveBooks(books);
        }

        public void RemoveBook(Book book)
        {
            if (book == null) throw new ArgumentNullException("Book must not be null");

            IEnumerable<Book> books = repository.LoadBooks();
            List<Book> newListBooks = new List<Book>();

            bool flag = false;
            foreach (Book existBook in books)
            {
                if (book.Equals(existBook))
                {
                    flag = true;
                }
                else
                {
                    newListBooks.Add(existBook);
                }
            }

            if (flag)
            {
                repository.SaveBooks(newListBooks);
            }
            else
            {
                logger.Error("Book do not exist in the repository " + DateTime.Now);
                throw new ArgumentException("Book do not exist in the repository");
            }
        }

        public IEnumerable<Book> WriteInNewRepositoryWhere(IBookRepository newRepository, Func<Book, bool> predicate)
        {
            if (predicate == null) throw new ArgumentNullException("Book must not be null");
            if (newRepository == null) throw new ArgumentNullException("New repository must not be null");

            IEnumerable<Book> books = repository.LoadBooks();
            List<Book> newListBooks = new List<Book>();

            foreach (Book existBook in books)
            {
                if (predicate(existBook))
                {
                    newListBooks.Add(existBook);   
                }
            }

            newRepository.SaveBooks(newListBooks);
            return newListBooks;
        }

        public IEnumerable<Book> GetRepositoryBooks()
        {
            IEnumerable<Book> books = repository.LoadBooks();
            return books;
        }

        public IEnumerable<Book> GetBooksByAuthor(string author)
        {
            if (author == null) throw new ArgumentNullException("Author must not be null string");

            IEnumerable<Book> books = repository.LoadBooks();
            List<Book> newListBooks = new List<Book>();

            foreach (Book existBook in books)
            {
                if (existBook.Author == author)
                {
                    newListBooks.Add(existBook);
                }
            }

            return newListBooks;
        }

        public void Export(IEnumerable<Book> books, string xmlfileName)
        {
            XDeclaration declaration = new XDeclaration("1.0", "Utf-8", "yes");
            XElement bookList = new XElement("Books");

            foreach (Book book in books)
            {
                bookList.Add(new XElement("Book",
                    new XElement("Author", book.Author),
                    new XElement("Title", book.Title),
                    new XElement("PageCount", book.PageCount.ToString()),
                    new XElement("Price", book.Price.ToString())));
            }

            XDocument doc = new XDocument( declaration,bookList);
            doc.Save(xmlfileName);
        }
    }
}