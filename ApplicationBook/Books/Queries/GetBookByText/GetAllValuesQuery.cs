using Domain;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApplicationBook.Books.Queries.GetBook
{
    public class GetAllValuesQuery : IRequest<List<Book>>
    {
    }

}
