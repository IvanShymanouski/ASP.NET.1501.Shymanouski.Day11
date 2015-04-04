using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Books
{
    [Serializable]
    public class Book: IEquatable<Book>, IComparable<Book>
    {
        #region fields
        public string Author { get; set; }
        public string Title { get; set; }
        public Int32 PageCount { get; set;}
        public Int32 Price { get; set; }
        #endregion

        #region overrides
        public override bool Equals(object obj)
        {
            if (!(obj is Book)) return false;
            return Equals((Book)obj);
        }

        public override int GetHashCode()
        {
            return unchecked(Price*969864898+PageCount);
        }
        #endregion

        #region interface
        public bool Equals(Book other)
        {
            if (object.Equals(other, null)) return false;
            if ((Author == other.Author) &&
                (Title == other.Title) &&
                (PageCount == other.PageCount) &&
                (Price == other.Price)
               )
                return true;

            return false;
        }

        public int CompareTo(Book other)
        {
            if (Equals(other)) return 0;
            else if (object.Equals(other, null)) return 1;
            else
                return Price + PageCount - other.PageCount - other.Price;
        }
        #endregion
    }
}
