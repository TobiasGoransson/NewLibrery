using Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructur.Database
{
    public class FakeDatabase
    {

        public List<Book> Books { get { return allBooksFromDB; } set { allBooksFromDB = value; } }
        public List<Author> Athors { get { return allAuthorsFromDB; } set { allAuthorsFromDB = value; } }

        private static List<Book> allBooksFromDB = new List<Book>
        {
        new Book(1, "TobyBook", "Book of Toby", new Author(1, "Toby", "Goransson")),
        new Book(2, "TobyBook2", "Book of Toby2", new Author(1, "Toby", "Goransson")),
        new Book(3, "TobyBook3", "Book of Toby3", new Author(2, "Toby2", "Goransson2")),
        new Book(4, "TobyBook4", "Book of Toby4", new Author(2, "Toby2", "Goransson2")),
        new Book(5, "TobyBook5", "Book of Toby5", new Author(1, "Toby", "Goransson"))
        };
        public Book AddNewBook(Book book)
        {
            allBooksFromDB.Add(book);
            return book;
        }

        public List<Author> Authors { get { return allAuthorsFromDB; } set { allAuthorsFromDB = value; } }

        private static List<Author> allAuthorsFromDB = new List<Author>
        {
            new Author(1, "Toby", "Goransson"),
            new Author(2, "Toby2", "Goransson2"),
        };
    }
}
