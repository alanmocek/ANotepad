using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public class UserData
    {
        /// <summary>
        /// Setet font size
        /// </summary>
        public int FontSize { get; set; }

        /// <summary>
        /// All opened by user files
        /// </summary>
        public List<string> OpenedFiles { get; set; }
    }
}
