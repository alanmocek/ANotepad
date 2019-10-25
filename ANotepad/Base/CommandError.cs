using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public enum CommandError
    {
        /// <summary>
        /// Command successful
        /// </summary>
        None,
        
        /// <summary>
        /// User selected not existing file in Open File Dialog
        /// </summary>
        OpenDialog_FileNotExist,

        /// <summary>
        /// File path is empty or is not valid
        /// </summary>
        FileSave_WrongPath

    }
}
