using System.ComponentModel.DataAnnotations;


namespace Domain
{
    public class Book
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "The Title is required.")]
        [StringLength(200, ErrorMessage = "The Title cannot exceed 200 characters.")]
        public string Title { get; set; }

        [Required(ErrorMessage = "The Description is required.")]
        [StringLength(1000, ErrorMessage = "The Description cannot exceed 1000 characters.")]
        public string Description { get; set; }

        [Required(ErrorMessage = "The Author is required.")]
        public Author Author { get; set; }


        public Book()
        {

        }

        public Book(int id, string title, string description, Author author)
        {
            Id = id;
            Title = title;
            Description = description;
            Author = author;
        }

        public Book(string title, string description, Author author)
        {
            Title = title;
            Description = description;
            Author = author;
        }
    }

}
