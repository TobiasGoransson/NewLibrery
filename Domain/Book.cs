using System.ComponentModel.DataAnnotations;


namespace Domain
{
    public class Book
    {
        public int BId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int AId { get; set; }
        public Author Author { get; set; }
        public ICollection<User> Users { get; set; } = new List<User>();

        public Book()
        {

        }
        //public Book(string title, string genre, Author author)
        //{
        //    Title = title;
        //    this.Genre = genre;
        //    Author = author;


        //}
        //public Book(int bId, string title, string genre, Author author)
        //{
        //    BId = bId;
        //    Title = title;
        //    Genre = genre;
        //    Author = author;
        //}

    }
}
