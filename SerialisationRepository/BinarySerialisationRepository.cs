using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;
using System.Xml.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Books;
using NLog;
using System.Xml;

namespace SerialisationRepository
{
    public class BinarySerialisationRepository : IBookRepository
    {
        public string FileName { get; private set; }

        public BinarySerialisationRepository(string fileName)
        {
            FileName = fileName;
        }

        public IEnumerable<Book> LoadBooks()
        {
            if (File.Exists(FileName) == false) return new List<Book>();

            IEnumerable<Book> listOfBooks;

            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = File.Open(FileName, FileMode.Open))
            {
                listOfBooks = (IEnumerable<Book>)formatter.Deserialize(stream);
            }
            
            return listOfBooks;
        }

        public void SaveBooks(IEnumerable<Book> books)
        {
            IFormatter formatter = new BinaryFormatter();

            using (Stream stream = File.Open(FileName, FileMode.Create))
            {
                formatter.Serialize(stream, books);
            }
        }

    }
}
