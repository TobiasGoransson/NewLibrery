using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public record BookDto(
        string Title,
        string Description,
        int AId,
        string AuthorFirstName,
        string AuthorLastName);

       
}
