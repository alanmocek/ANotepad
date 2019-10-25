using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public interface IFileManager
    {
        Command<FileViewModel> OpenFile();
        CommandBase SaveFile(FileViewModel file);
        CommandBase SaveFileAs(FileViewModel file);
        Command<DateTime> GetFileLastSaveTime();
    }
}
