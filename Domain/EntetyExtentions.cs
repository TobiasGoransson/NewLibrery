

using Domain.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class EntetyExtentions
    {
        public static AuthorDto ToAuthorDto(this Author author)
        {
            return new AuthorDto(
            author.FirstName, 
            author.LastName);
           
        }
        public static BookDto ToBookDto(this Book book)
        {
            return new BookDto(
            book.Title,
            book.Description,
            book.AId,
            book.Author.FirstName,
            book.Author.LastName);

        }
        public static UserDto ToUserDto(this User user)
        {
            return new UserDto(
            user.UserName,
            user.Password);
        }
        
    }
}
