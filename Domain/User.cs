namespace Domain
{



    public class User
    {
        public int UId { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public ICollection<Book> Books { get; set; } = new List<Book>();
        public ICollection<Author> Authors { get; set; } = new List<Author>();

        public User()
        {
        }

    }
}
