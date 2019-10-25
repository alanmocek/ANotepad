using System;
using System.Windows.Forms;
using System.IO;

namespace ANotepad
{
    public class FileManager : IFileManager
    {
        public Command<DateTime> GetFileLastSaveTime()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens file selected in file dialog to <see cref="FileViewModel"/>
        /// </summary>
        /// <returns>Command with file as <see cref="FileViewModel"/></returns>
        public Command<FileViewModel> OpenFile()
        {
            // Create and remove file dialog
            using(OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                // Set dialog properties
                openFileDialog.Title = "Open file";
                openFileDialog.DefaultExt = "txt";
                openFileDialog.Filter = "Txt files (*txt) | *txt";

                // Show dialog
                openFileDialog.ShowDialog();

                // Check if user selected existing file
                if(openFileDialog.CheckFileExists == false || string.IsNullOrWhiteSpace(openFileDialog.FileName) == true)
                {
                    return new Command<FileViewModel>()
                    {
                        Value = null,
                        Success = false,
                        Error = CommandError.OpenDialog_FileNotExist
                    };
                }

                // Create and remove stream reader
                using(StreamReader reader = new StreamReader(openFileDialog.OpenFile()))
                {
                    var filePath = openFileDialog.FileName;
                    var fileName = openFileDialog.SafeFileName;
                    var fileContent = reader.ReadToEnd();

                    // Return opened file as fileViewModel
                    return new Command<FileViewModel>()
                    {
                        Value = new FileViewModel()
                        {
                            Path = filePath,
                            Name = fileName,
                            Content = fileContent,
                            IsNew = false,
                            IsSaved = true,
                            LastSaveTime = DateTime.Now,
                            IsChangedOutside = false,
                            IsWorking = false
                        },
                        Success = true,
                        Error = CommandError.None
                    };
                }
            }
        }

        /// <summary>
        /// Save file to provided in file path
        /// </summary>
        /// <param name="file">File to save</param>
        /// <returns></returns>
        public CommandBase SaveFile(FileViewModel file)
        {
            // Check if file path is valid
            if(string.IsNullOrWhiteSpace(file.Path))
            {
                return new CommandBase()
                {
                    Error = CommandError.FileSave_WrongPath,
                    Success = false
                };
            }

            // Create and remove stream writer
            using (StreamWriter writer = new StreamWriter(file.Path))
            {
                // Write file content to file
                writer.Write(file.Content);

                file.IsSaved = true;

                return new CommandBase()
                {
                    Error = CommandError.None,
                    Success = true
                };
            }
        }

        /// <summary>
        /// Save file to path provided in file dialog
        /// </summary>
        /// <param name="file">File to save</param>
        /// <returns></returns>
        public CommandBase SaveFileAs(FileViewModel file)
        {
            using(var saveFileDialog = new SaveFileDialog())
            {
                // Set filters
                saveFileDialog.Title = $"Save {file.Name} as";
                saveFileDialog.FileName = file.Name;
                saveFileDialog.DefaultExt = "txt";
                saveFileDialog.Filter = "Txt files (*txt) | *txt";

                // Show dialog
                var result = saveFileDialog.ShowDialog();

                // Checks
                if(result == DialogResult.OK && string.IsNullOrEmpty(saveFileDialog.FileName) == false)
                {
                    // Create and remove stream writer
                    using(var writer = new StreamWriter(saveFileDialog.OpenFile()))
                    {
                        // Change file path
                        file.Path = saveFileDialog.FileName;

                        // Change file name TODO =====================================##################################################################################################################################
                        file.Name = saveFileDialog.FileName;

                        // Write content to file
                        writer.Write(file.Content);

                        // Return command result
                        return new CommandBase()
                        {
                            Success = true,
                            Error = CommandError.None
                        };

                    }
                }else
                {
                    return new CommandBase()
                    {
                        Success = false,
                        Error = CommandError.FileSave_WrongPath
                    };
                }
            }
        }
    }
}
