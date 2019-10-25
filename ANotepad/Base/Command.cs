using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public class Command<T> : CommandBase
    {
        public T Value { get; set; }
    }
}
