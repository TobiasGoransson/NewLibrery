namespace Domain
{
    public class Author
    {
        
        
            public int AId { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public ICollection<Book> Books { get; set; } = new List<Book>();
            public ICollection<User> Users { get; set; } = new List<User>();


            public Author()
            {

            }


        public Author(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
        }
        public Author(int aId, string firstName, string lastName)
        {
            AId = aId;
            FirstName = firstName;
            LastName = lastName;
        }



    }
}
