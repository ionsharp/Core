using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.IO;

namespace Imagin.Common.Configuration
{
    public abstract class ApplicationProperties
    {
        public readonly DataFolders Folder;

        public readonly string FileName;

        public readonly string FileExtension;

        public string FilePath => GetFilePath($"{FileName}.{FileExtension}");

        public string FolderPath
        {
            get
            {
                var result = GetFolderPath(Folder);
                return Folder switch
                {
                    DataFolders.Documents => $@"{result}\{XAssembly.Title()}",
                    DataFolders.ExecutionPath => result,
                    _ => throw new NotSupportedException(),
                };
            }
        }

        public virtual Type MainViewOptions { get; }

        public virtual Type MainView { get; }

        public virtual Type MainViewModel { get; }

        public ApplicationProperties(DataFolders folder, string fileName, string fileExtension)
        {
            Folder 
                = folder;
            FileName 
                = fileName;
            FileExtension 
                = fileExtension;
        }

        public static string GetFolderPath(DataFolders folder)
        {
            switch (folder)
            {
                case DataFolders.Documents:
                    var i = Get.Where<SingleApplication>();
                    return $@"{Environment.SpecialFolder.MyDocuments.Path()}\{XAssembly.Company(InternalAssembly.Name)}";

                case DataFolders.ExecutionPath:
                    return Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
            }
            throw new NotSupportedException();
        }

        public string GetFilePath(string fileNameWithExtension) => $@"{FolderPath}\{fileNameWithExtension}";

        public string GetFilePath(string folderName, string fileNameWithExtension) => $@"{GetFolderPath(folderName)}\{fileNameWithExtension}";

        public string GetFolderPath(string folderName) => $@"{FolderPath}\{folderName}";
    }

    public class ApplicationProperties<TMainView, TMainViewModel, TMainViewOptions> : ApplicationProperties where TMainView : IMainView where TMainViewModel : IMainViewModel where TMainViewOptions : IMainViewOptions
    {
        public override Type MainView => typeof(TMainView);

        public override Type MainViewModel => typeof(TMainViewModel);

        public override Type MainViewOptions => typeof(TMainViewOptions);

        public ApplicationProperties(DataFolders folder = DataFolders.Documents, string fileName = "Options", string fileExtension = "data") : base(folder, fileName, fileExtension) { }
    }
}