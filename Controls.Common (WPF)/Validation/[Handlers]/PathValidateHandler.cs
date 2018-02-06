using Imagin.Common;
using Imagin.Common.Linq;
using System.Windows;

namespace Imagin.Controls.Common
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class PathValidateHandler : DependencyObject, IValidate<object>
    {
        /// <summary>
        /// 
        /// </summary>
        public static DependencyProperty FileOrFolderProperty = DependencyProperty.Register("FileOrFolder", typeof(bool), typeof(PathValidateHandler), new FrameworkPropertyMetadata(false, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault));
        /// <summary>
        /// Gets or sets whether or not path represents a file or a folder.
        /// </summary>
        public bool FileOrFolder
        {
            get
            {
                return (bool)GetValue(FileOrFolderProperty);
            }
            set
            {
                SetValue(FileOrFolderProperty, value);
            }
        }

        /// <summary>
        /// Gets whether or not the file at given path exists.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected abstract bool FileExists(string Path);

        /// <summary>
        /// Gets whether or not the folder at given path exists.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected abstract bool FolderExists(string Path);

        /// <summary>
        /// Validates given path based on whether or not it is a file or a folder.
        /// </summary>
        /// <param name="Path"></param>
        /// <returns></returns>
        protected bool Validate(string Path)
        {
            return FileOrFolder ? FileExists(Path) : FolderExists(Path);
        }

        /// <summary>
        /// Validates path based on given arguments.
        /// </summary>
        /// <param name="Arguments"></param>
        /// <returns></returns>
        public virtual bool Validate(params object[] Arguments)
        {
            var Path = Arguments[0].ToString();

            //If not already specified, check if specified as argument
            if (Arguments.Length > 1)
                FileOrFolder = Arguments[1].To<bool>();

            return Validate(Path);
        }
    }
}
