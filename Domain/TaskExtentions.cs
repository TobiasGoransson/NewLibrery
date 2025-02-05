using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public static class TaskExtensions
    {
        public static Task<T> AsTask<T>(this T value)
        {
            return Task.FromResult(value);
        }
    }

}
