using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public class CommandBase
    {
        public bool Success { get; set; }
        public CommandError Error { get; set; }

        public CommandBase()
        {
            Success = true;
            Error = CommandError.None;
        }
    }
}
