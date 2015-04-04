using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NLog;

namespace Books
{
    public class BinaryFileRepository : IBookRepository
    {
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();

        public string FileName { get; private set; }

        public BinaryFileRepository(string fileName)
        {
            FileName = fileName;
        }

        public IEnumerable<Book> LoadBooks()
        {
            if (File.Exists(FileName) == false) return new List<Book>();

            List<Book> listOfBooks = new List<Book>();
            using (BinaryReader reader = new BinaryReader(File.Open(FileName, FileMode.Open)))
            {
                while (reader.BaseStream.Position < reader.BaseStream.Length)
                {
                    try
                    {
                        listOfBooks.Add(new Book());
                        listOfBooks.Last<Book>().Author = reader.ReadString();
                        listOfBooks.Last<Book>().Title = reader.ReadString();
                        listOfBooks.Last<Book>().PageCount = reader.ReadInt32();
                        listOfBooks.Last<Book>().Price = reader.ReadInt32();
                    }
                    catch
                    {
                        logger.Warn("Broken file "+Path.GetFileName(FileName)+" "+DateTime.Now);
                        listOfBooks.Remove(listOfBooks.Last<Book>());
                    }
                }
            }
            return listOfBooks;
        }

        public void SaveBooks(IEnumerable<Book> books)
        {
            using (BinaryWriter writer = new BinaryWriter(File.Open(FileName, FileMode.Create)))
            {
                foreach (var book in books)
                {
                    writer.Write(book.Author);
                    writer.Write(book.Title);
                    writer.Write(book.PageCount);
                    writer.Write(book.Price);

                }
            }
        }
    }

}
