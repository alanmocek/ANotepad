using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ANotepad
{
    public class FileViewModel : ViewModelBase
    {
        #region Private Members

        private bool isWorking;

        private string name;

        private string path;

        #endregion

        #region Public Properties

        /// <summary>
        /// File full path
        /// </summary>
        public string Path
        {
            get => path;
            set
            {
                if (path == value)
                    return;
                
                path = value;
                UpdateProperty(nameof(Path));
            }
        }

        /// <summary>
        /// File name
        /// </summary>
        public string Name
        {
            get => name;
            set
            {
                if (name == value)
                    return;

                name = value;
                UpdateProperty(nameof(Name));
            }
        }

        /// <summary>
        /// File text content
        /// </summary>
        public string Content { get; set; }

        /// <summary>
        /// Is file a new created file (not saved yet)
        /// </summary>
        public bool IsNew { get; set; }

        /// <summary>
        /// Is file currently being edited
        /// </summary>
        public bool IsWorking
        {
            get => isWorking;
            set
            {
                if (isWorking == value)
                    return;

                isWorking = value;
                UpdateProperty(nameof(IsWorking));
            }
        }

        /// <summary>
        /// Is file saved with all text content changes
        /// </summary>
        public bool IsSaved { get; set; }

        /// <summary>
        /// Is file changed by another app
        /// </summary>
        public bool IsChangedOutside { get; set; }

        /// <summary>
        /// Time of the last file save in app
        /// </summary>
        public DateTime LastSaveTime { get; set; }

        #endregion

        public FileViewModel()
        {
            Path = string.Empty;
            Name = "File name";
            Content = string.Empty;
            IsNew = false;
            IsWorking = false;
            IsSaved = false;
            IsChangedOutside = false;
            LastSaveTime = DateTime.Now;
        }
    }
}
