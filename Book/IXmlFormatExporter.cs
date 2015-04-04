using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books
{
    public interface IXmlFormatExporter
    {
        void Export(IEnumerable<Book> books, string fileName);
    }
}
