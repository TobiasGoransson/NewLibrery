using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }
       
        public AuthorDto Author { get; set; }
        
    }
}
