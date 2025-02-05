using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Dtos
{
    public class ResultDto<T>
    {
        public bool Success { get; set; } // Anger om operationen lyckades eller ej
        public T Data { get; set; } // Resultatdata från operationen (kan vara vilken typ som helst)
        public string ErrorMessage { get; set; } // Eventuellt felmeddelande om operationen misslyckades
    }

}
