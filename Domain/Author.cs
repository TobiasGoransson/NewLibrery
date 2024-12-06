namespace Domain
{
    public class Author
    {
        public int Id { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public Author()
        {

        }
        public Author(int id, string name, string lastName)
        {
            Id = id;
            FirstName = name;
            LastName = lastName;
        }
        public Author(string name, string lastName)
        {
            FirstName = name;
            LastName = lastName;
        }


    }
}
