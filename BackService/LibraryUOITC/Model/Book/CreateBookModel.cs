using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryUOITC.Model.Book
{
    public class CreateBookModel
    {
        public string Name { get; set; }
        public string Code { get; set; }
        public int SectionId { get; set; }
        public string Note { get; set; }
        public string AuthorName { get; set; }
        public string NamePrinting { get; set; }
        public DateTime Date { get; set; }
        public string Path { get; set; }
        public List<string> Photos { get; set; }
        public int ShelfNumber { get; set; }

    }
}
