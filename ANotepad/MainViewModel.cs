using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace ANotepad
{
    public class MainViewModel : ViewModelBase
    {
        #region Private Members
        
        /// <summary>
        /// File manager instance
        /// </summary>
        private readonly IFileManager _fileManager;

        private FileViewModel workingFile;

        private NotificationViewModel currentNotification;

        /// <summary>
        /// All notifications to display
        /// </summary>
        private Queue<NotificationViewModel> notificationsToDisplay;

        private event Func<Task> NotificationAdded;

        private bool isDisplayingNotification;

        #endregion

        #region Public Properties

        /// <summary>
        /// File currently being edited
        /// </summary>
        public FileViewModel WorkingFile
        {
            get => workingFile;
            set
            {
                // Check if value changes
                if (workingFile == value)
                    return;

                // If current file exist than set is working to false
                if(workingFile != null)
                {
                    workingFile.IsWorking = false;
                }

                // Set value
                workingFile = value;

                // If new file exist than set is working to true
                if (workingFile != null)
                {
                    workingFile.IsWorking = true;
                }

                UpdateProperty(nameof(WorkingFile));
            }
        }

        /// <summary>
        /// All files opened by user
        /// </summary>
        public ObservableCollection<FileViewModel> OpenedFiles { get; set; }
        
        /// <summary>
        /// Currently being displayed notification
        /// </summary>
        public NotificationViewModel CurrentNotification
        {
            get => currentNotification;
            set
            {
                currentNotification = value;

                if (currentNotification == null)
                {
                    IsDisplayingNotification = false;
                }
                else
                {
                    IsDisplayingNotification = true;
                }
                    
                UpdateProperty(nameof(CurrentNotification));
            }
        }

        public bool IsDisplayingNotification
        {
            get => isDisplayingNotification;
            set
            {
                isDisplayingNotification = value;
                UpdateProperty(nameof(IsDisplayingNotification));
            }
        }

        #endregion

        #region Constructors

        /// <summary>
        /// Default constructor
        /// </summary>
        public MainViewModel()
        {
            _fileManager = new FileManager();

            OpenedFiles = new ObservableCollection<FileViewModel>();
            notificationsToDisplay = new Queue<NotificationViewModel>();

            OpenFileCommand = new RelayCommand(OpenFileExe);
            SaveWorkingFileCommand = new RelayCommand(SaveWorkingFileExe, SaveWorkingFileCheck);
            SaveAsWorkingFileCommand = new RelayCommand(SaveAsWorkingFileExe, SaveAsWorkingFileCheck);
            CloseWorkingFileCommand = new RelayCommand(CloseWorkingFileExe, CloseWorkingFileCheck);
            SetFileAsWorkingCommand = new RelayCommand(SetFileAsWorkingExe);
            SaveFileFromListCommand = new RelayCommand(SaveFileFromListExe);
            NewFileCommand = new RelayCommand(NewFileExe);

            NotificationAdded += DisplayNotification;
        }

        #endregion

        #region Commands

        public ICommand NewFileCommand { get; set; }

        public ICommand OpenFileCommand { get; set; }

        public ICommand SaveWorkingFileCommand { get; set; }

        public ICommand SaveAsWorkingFileCommand { get; set; }

        public ICommand CloseWorkingFileCommand { get; set; }

        public ICommand SetFileAsWorkingCommand { get; set; }

        public ICommand SaveFileFromListCommand { get; set; }

        #endregion

        #region Command Methods

        void NewFileExe(object x)
        {
            var newFile = new FileViewModel()
            {
                Content = "",
                Name = "New File",
                IsSaved = false,
                IsNew = true,
                Path = "",
                IsChangedOutside = false,
                LastSaveTime = DateTime.Now
            };

            WorkingFile = newFile;
            OpenedFiles.Add(newFile);
        }

        void OpenFileExe(object x)
        {
            var commandResult = _fileManager.OpenFile();

            if(commandResult.Success == true)
            {
                var openedFile = commandResult.Value;

                // Check if file is already opened
                foreach (FileViewModel file in OpenedFiles)
                {
                    if(file.Path == openedFile.Path)
                    {
                        // Say file is already opened
                        Console.WriteLine("File is already opened");

                        // Set this file as working
                        WorkingFile = file;

                        return;
                    }
                }

                OpenedFiles.Add(openedFile);
                WorkingFile = openedFile;
            }
            else
            {
                // Say somnething go wrong
                var notyfication = new NotificationViewModel()
                {
                    Type = NotificationType.Error,
                    Message = "Something go wrong..."
                };
                AddNotification(notyfication);
            }
        }

        void SaveWorkingFileExe(object x)
        {
            var fileToSave = WorkingFile;

            SaveFileUniversal(fileToSave);
        }

        void SaveAsWorkingFileExe(object x)
        {
            var fileToSave = WorkingFile;

            SaveFileAsUniversal(fileToSave);
        }

        bool SaveAsWorkingFileCheck(object x)
        {
            var file = WorkingFile;
            if (SaveFileAsCheckUniversal(file) == false)
                return false;

            return true;
        }

        bool SaveWorkingFileCheck(object x)
        {
            var file = (FileViewModel)x;
            
            if (SaveFileCheckUniversal(file) == false)
                return false;

            return true;
        }

        void CloseWorkingFileExe(object x)
        {
            OpenedFiles.Remove(WorkingFile);
            WorkingFile = null;
        }

        bool CloseWorkingFileCheck(object x)
        {
            if (WorkingFile == null)
                return false;

            return true;
        }

        void SetFileAsWorkingExe(object x)
        {
            FileViewModel selectedFile = (FileViewModel)x;

            WorkingFile = selectedFile;
        }

        void SaveFileFromListExe(object x)
        {
            var fileToSave = (FileViewModel)x;

            SaveFileUniversal(fileToSave);
        }

        #endregion

        #region Notifications

        private void AddNotification(NotificationViewModel notification)
        {
            notificationsToDisplay.Enqueue(notification);
            NotificationAdded?.Invoke();

        }

        private async Task DisplayNotification()
        {
            if (IsDisplayingNotification == true)
                return;

            if(notificationsToDisplay.Count>0)
            {
                var notyfication = notificationsToDisplay.Dequeue();

                CurrentNotification = notyfication;

                await Task.Delay(3000);

                CurrentNotification = null;

                if(notificationsToDisplay.Count > 0)
                {
                    await Task.Delay(500);
                    DisplayNotification();
                }
            }
        }

        #endregion

        #region Helpers

        private void SaveFileUniversal(FileViewModel file)
        {
            var commandResult = _fileManager.SaveFile(file);

            if (commandResult.Success == false)
            {
                //...
            }
        }

        private bool SaveFileCheckUniversal(FileViewModel file)
        {
            if (file == null)
                return false;

            if (file.IsNew == true)
                return false;

            if (string.IsNullOrEmpty(file.Path))
                return false;

            return true;
        }

        private void SaveFileAsUniversal(FileViewModel file)
        {
            var commandResult = _fileManager.SaveFileAs(file);

            if(commandResult.Success == false)
            {
                AddNotification(new NotificationViewModel() { Message = "Something go wrong", Type = NotificationType.Information });
            }
        }

        private bool SaveFileAsCheckUniversal(FileViewModel file)
        {
            if(file == null)
                return false;

            return true;
        }

        private void CloseFileUniversal(FileViewModel file)
        {
            OpenedFiles.Remove(file);
            if(WorkingFile == file)
            {
                WorkingFile = null;
            }
        }

        #endregion
    }
}
