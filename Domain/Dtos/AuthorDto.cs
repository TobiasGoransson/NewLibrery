using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public record AuthorDto(
        string FirstName, 
        string LastName);

    public record AuthorDtoWithId(
    int Id,
    string FirstName,
    string LastName);


}
