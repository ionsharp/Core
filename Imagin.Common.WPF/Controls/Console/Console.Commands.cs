using Imagin.Common.Linq;
using Imagin.Common.Numbers;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Imagin.Common.Controls
{
    public partial class Console
    {
        public abstract class BaseCommand : BaseNamable
        {
            internal Console Console { get; set; }

            //...

            public abstract string Description { get; }

            public virtual IEnumerable<char> Flags()
            {
                yield break;
            }

            public string AllNames => string.Join(", ", Names());

            new public virtual string Name => Names().First();

            public abstract IEnumerable<string> Names();

            public virtual IEnumerable<string> Usage() => Enumerable.Empty<string>();

            //...

            public string Path
            {
                get => Console.Path;
                set => Console.Path = value;
            }

            public string Output
            {
                get => Console.Output;
                set => Console.SetCurrentValue(Console.OutputProperty, value);
            }

            //...

            protected virtual string Outputs(params string[] arguments) => default;

            //...

            string[] Parse(string line, char separator) => line.Split(Array<char>.New(separator), 2, StringSplitOptions.RemoveEmptyEntries);

            protected virtual string[] Parse(string line) => line.NullOrEmpty() ? null : Array<string>.New(line);

            //...

            protected string ChildPath(string input) => FolderPath(Path, input);

            protected string FolderPath(string a, string b) => $@"{a}\{b}".Replace(@"\\", @"\");

            //...

            public virtual void Write(string line) => Console.Write($"{Path}> {line}");

            //...

            protected abstract void Execute(string name, string[] arguments, char[] flags);

            public void Execute(string line)
            {
                Write(line);

                var pieces = Parse(line, ' ');
            
                //The name always comes first
                var name
                    = pieces[0];

                //Arguments can be specified only once anywhere after name
                string arguments = null;

                //Flags can appear anywhere after name and before/after arguments
                var flags = new List<char>();

                var j = 0;
                foreach (var i in pieces)
                {
                    if (j > 0)
                    {
                        if (i.Length == 2 && i[0] == '-' && char.IsLetter(i[1]))
                        {
                            flags.Add(i[1]);
                        }
                        else if (arguments == null)
                        {
                            arguments = i;
                        }
                        else
                        {
                            throw new InvalidOperationException("Arguments can only be specified once.");
                        }
                    }
                    j++;
                }

                var validFlags = Flags();
                foreach (var i in flags)
                {
                    if (!validFlags.Contains(i))
                        throw new NotSupportedException($"The flag '{i}' is not supported.");
                }

                Execute(name, Parse(arguments) ?? Array<string>.New(), flags.ToArray());
            }
        }

        public abstract class ItemCommand : BaseCommand
        {
            protected virtual char Separator => '|';

            protected override string[] Parse(string line) => line?.Split(Array<char>.New(Separator), StringSplitOptions.RemoveEmptyEntries);
        }

        //...

        public class BackCommand : BaseCommand
        {
            public override string Description => "Go to the previous folder.";

            public override IEnumerable<string> Names()
            {
                yield return "Back";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (!Console.History.Undo(i => Console.handleFolder.Invoke(() => Console.Path = i)))
                    throw new InvalidOperationException();
            }
        }

        public class CurrentCommand : BaseCommand
        {
            public override string Description => "Print the current folder path.";

            public override IEnumerable<string> Names()
            {
                yield return "Current";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                var result = new StringBuilder();
                result.AppendLine(Path.PadLeft(' ', Console.LinePadding));
                Console.WriteBlock(result.ToString());
            }
        }

        public class NextCommand : BaseCommand
        {
            public override string Description => "Go to the next folder.";

            public override IEnumerable<string> Names()
            {
                yield return "Next";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (!Console.History.Redo(i => Console.handleFolder.Invoke(() => Console.Path = i)))
                    throw new InvalidOperationException();
            }
        }

        //...

        public class AttributesCommand : BaseCommand
        {
            public override string Description => "Show attribute(s) of specified file or folder, or show attribute(s) of current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Attributes";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} {{the current folder}}]";
                yield return $"[{Name}] [{{file or folder path in current folder}} or {{absolute file or folder path}}]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                string path = null;
                switch (arguments.Length)
                {
                    case 0:
                        path = Path;
                        break;

                    case 1:
                        path = arguments[0];
                        if (!Storage.File.Long.Exists(path) && !Storage.Folder.Long.Exists(path))
                            path = ChildPath(path);

                        break;

                    default: throw new ArgumentOutOfRangeException();
                }

                var attributes = Storage.File.Long.Attributes(path);

                var result = new StringBuilder();
                result.AppendLine(string.Empty);

                var line = attributes.ToString();
                line = line.PadLeft(line.Length + Console.LinePadding, ' ');

                result.AppendLine(line);
                Console.Write(result.ToString());
            }
        }

        public class ChangeFolderCommand : BaseCommand
        {
            public override string Description => "Set the current folder.";

            public override IEnumerable<string> Names()
            {
                yield return "..";
                yield return "Cd";
                yield return "Up";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[.. <the parent folder>]";
                yield return $"[Up <the parent folder>]";
                yield return $"[Cd] [{{folder name in current folder}} or {{absolute folder path}} or {{.., ..., etc.}}]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                switch (name.ToLower())
                {
                    case "cd":

                        if (arguments.Length != 1)
                            throw new ArgumentOutOfRangeException();

                        string folderPath = arguments[0];
                        //Check if it is a full path (rejects // and \\)
                        if (Storage.Folder.Long.Exists(folderPath))
                        {
                            Path = folderPath;
                        }
                        else
                        {
                            //Check if it only contains periods
                            if (folderPath.OnlyContains('.'))
                            {
                                //Supports '..', '...', '....', etc...
                                var up = folderPath.Repeats('.') - 1;
                                folderPath = Path;
                                while (up > 0)
                                {
                                    if (folderPath.NullOrEmpty())
                                    {
                                        folderPath = StoragePath.Root;
                                        break;
                                    }
                                    folderPath = System.IO.Path.GetDirectoryName(folderPath);
                                    up--;
                                }
                            }
                            //Adding a single / or \ to the end of a path seems to make it valid?
                            else if (folderPath.OnlyContains('/') || folderPath.OnlyContains('\\'))
                            {
                                throw new DirectoryNotFoundException(folderPath);
                            }
                            else
                            {
                                //Check if it is a child folder
                                folderPath = ChildPath(folderPath);
                            }

                            //If what we have at this point, exists...
                            if (Storage.Folder.Long.Exists(folderPath))
                                Path = Storage.Folder.Long.ActualPath(folderPath);

                            else throw new DirectoryNotFoundException(folderPath);
                        }
                        break;

                    case "..":
                    case "up":
                        if (arguments.Length == 0)
                        {
                            if (Path == StoragePath.Root)
                                return;

                            if (Computer.GetType(Path) == ItemType.Drive)
                            {
                                Path = StoragePath.Root;
                                break;
                            }

                            var parent = System.IO.Path.GetDirectoryName(Path);
                            if (!Storage.Folder.Long.Exists(parent))
                                throw new DirectoryNotFoundException();

                            Path = parent;
                            break;
                        }
                        throw new ArgumentOutOfRangeException();
                }
            }
        }

        public class ClearCommand : BaseCommand
        {
            public override string Description => "Clear a) the current output or b) history.";

            public override IEnumerable<string> Names()
            {
                yield return "Clr";
                yield return "Clear";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames}]";
                yield return $"[{AllNames}] [History]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments?.Length > 0)
                {
                    if (arguments[0].ToLower() == "history")
                    {
                        Console.History.Clear();
                        return;
                    }
                    throw new ArgumentOutOfRangeException();
                }
                Try.Invoke(() => Output = string.Empty);
            }
        }

        public class CountCommand : BaseCommand
        {
            public override string Description => "Displays the number of files and folders.";

            public override IEnumerable<char> Flags()
            {
                yield return 'h';
                yield return 'r';
            }

            public override IEnumerable<string> Names()
            {
                yield return "Count";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <current folder>]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length == 0)
                {
                    var files 
                        = Storage.Folder.Long.GetFiles(Path);
                    var folders 
                        = Storage.Folder.Long.GetFolders(Path);

                    var fileCount
                        = files.Count();
                    var folderCount
                        = folders.Count();

                    foreach (var i in files)
                    {
                        if ((flags.Contains('h') && !Computer.Hidden(i)) || (flags.Contains('r') && !Computer.ReadOnly(i)))
                            fileCount--;
                    }
                    foreach (var i in folders)
                    {
                        if ((flags.Contains('h') && !Computer.Hidden(i)) || (flags.Contains('r') && !Computer.ReadOnly(i)))
                            folderCount--;
                    }


                    Console.WriteIndent($"{fileCount} file{(fileCount != 1 ? "s" : "")} and {folderCount} folder{(folderCount != 1 ? "s" : "")}");
                    return;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public class EchoCommand : BaseCommand
        {
            public override string Description => "Print text to output.";

            public override IEnumerable<string> Names()
            {
                yield return "Echo";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name}] [(*)*]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length > 0)
                {
                    Console.WriteIndent(arguments[0]);
                    return;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public class ExitCommand : BaseCommand
        {
            public override string Description => "Exit application.";

            public override IEnumerable<string> Names()
            {
                yield return "Exit";
            }

            protected override void Execute(string name, string[] arguments, char[] flags) => Environment.Exit(0);
        }

        public class FavoriteCommand : BaseCommand
        {
            public override string Description => "Favorite or unfavorite the current folder.";

            public override IEnumerable<string> Names()
            {
                yield return "Favorite";
                yield return "Unfavorite";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames} <current folder>]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length == 0)
                {
                    Console.Favorites.Is(name == "favorite", Path);
                    return;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public class HelpCommand : BaseCommand
        {
            public override string Description => "Show list of available commands.";

            public override IEnumerable<string> Names()
            {
                yield return "?";
                yield return "Help";
            }

            protected override void Execute(string name, string[] arguments, char[] actualFlags)
            {
                var result = new StringBuilder();

                int left = Console.LinePadding;
                int right = 40;

                var index = 0;
                var commands = Console.Commands.OrderBy(x => x.Names().First());

                var count = commands.Count();
                foreach (var i in commands)
                {
                    string line1 = null;
                    i.Names().ForEach(j => line1 = line1 == null ? j.ToString() : $"{line1}, {j}");

                    line1 = line1.PadLeft(line1.Length + left, ' ');
                    line1 = line1.PadRight(right, ' ');
                    line1 = string.Concat(line1, i.Description);
                    result.AppendLine(line1);

                    var usage = i.Usage();
                    if (usage.Any())
                    {
                        result.AppendLine(string.Empty);
                        foreach (var j in usage)
                            result.AppendLine($"> {j}".PadLeft(j.Length + left + right, ' '));
                    }

                    var flags = i.Flags();
                    if (flags.Any())
                    {
                        result.AppendLine(string.Empty);

                        var flagText = string.Empty;
                        foreach (var j in flags)
                            flagText += $"-{j}, ";

                        flagText = flagText.TrimEnd(' ').TrimEnd(',');
                        result.AppendLine(flagText.PadLeft(left + right, ' '));
                    }

                    if (index < count - 1)
                        result.AppendLine(string.Empty);

                    index++;
                }
                Console.WriteBlock(result.ToString());
            }
        }

        public class HistoryCommand : BaseCommand
        {
            public override string Description => "Show list of visited folders.";

            public override IEnumerable<string> Names()
            {
                yield return "History";
                yield return "Visited";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (Console.History?.Count > 0)
                {
                    var result = new StringBuilder();
                    foreach (var i in Console.History)
                        result.AppendLine(i.PadLeft(i.Length + Console.LinePadding, ' '));

                    Console.WriteBlock(result.ToString());
                    return;
                }
                throw new NullReferenceException($"{nameof(Collections.ObjectModel.History)} does not exist");
            }
        }

        public class InCommand : BaseCommand
        {
            public override string Description => "Copy clipboard to output.";

            public override IEnumerable<string> Names()
            {
                yield return "In";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                var result = System.Windows.Clipboard.GetText();
                Console.Write(result.PadLeft(result.Length + Console.LinePadding, ' '));
            }
        }

        public class ListCommand : BaseCommand
        {
            public override string Description => "List names of all files and folders in the current folder (optionally, only include items with the specified extension).";

            public override IEnumerable<string> Names()
            {
                yield return "Ls";
                yield return "List";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames}]";
                yield return $"[{AllNames}] [.(*) {{a file extension}}]";
            }

            string Print(string folderPath, params string[] extensions)
            {
                var result = new StringBuilder();

                var j = 0;
                foreach (var i in Storage.Folder.Long.GetItems(folderPath))
                {
                    if (extensions?.Length > 0)
                    {
                        var include = false;
                        foreach (var k in extensions)
                        {
                            if (i.ToLower().EndsWith(k.ToLower()))
                            {
                                include = true;
                                break;
                            }
                        }
                        if (!include)
                            goto Continue;
                    }

                    var name = System.IO.Path.GetFileName(i);
                    result.AppendLine(name.PadLeft(' ', Console.LinePadding));
                    j++;

                    Continue: continue;
                }
                return j > 0 ? result.ToString() : throw new ItemsNotFoundException(folderPath);
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length <= 1)
                {
                    string[] extensions = null;

                    string folderPath = null;
                    if (arguments.Length == 0)
                    {
                        folderPath = Path;
                    }
                    else if (arguments[0].StartsWith("."))
                    {
                        folderPath = Path;
                        extensions = Array<string>.New(arguments[0]);
                    }
                    else if (Storage.Folder.Long.Exists(arguments[0]))
                    {
                        folderPath = arguments[0];
                    }
                    else
                    {
                        arguments[0] = ChildPath(arguments[0]);
                        if (Storage.Folder.Long.Exists(arguments[0]))
                            folderPath = arguments[0];

                        else throw new DirectoryNotFoundException("Directory not found.");
                    }

                    if (Storage.Folder.Long.Exists(folderPath))
                    {
                        var result = Print(folderPath, extensions);
                        Console.WriteBlock(result);
                    }

                    else throw new DirectoryNotFoundException($"Directory '{folderPath}' not found.");
                    return;
                }
                throw new ArgumentOutOfRangeException("Only one argument is allowed.");
            }
        }

        public class MathCommand : BaseCommand
        {
            readonly MathParser parser = new();

            public override string Description => "Solve a math equation [+, -, *, /, ()].";

            public override IEnumerable<string> Names()
            {
                yield return "Math";
                yield return "Solve";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames}] [a math equation]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments?.Length == 1)
                {
                    var result = new StringBuilder();

                    arguments[0] = $"{Equation.Clean(arguments[0])}";
                    var line = $"{arguments[0]} = {parser.Solve(arguments[0])}";
                    line = line.PadLeft(' ', Console.LinePadding);

                    result.AppendLine(line);
                    Console.WriteBlock(result.ToString());
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public class OutCommand : BaseCommand
        {
            public override string Description => "Copy output to clipboard.";

            public override IEnumerable<string> Names()
            {
                yield return "Out";
            }

            protected override void Execute(string name, string[] arguments, char[] flags) => System.Windows.Clipboard.SetText(Console.Output);
        }

        //...

        public class OpenCommand : ItemCommand
        {
            public override string Description => "Open a file (if specified); otherwise, open the specified folder in WindowsExplorer, or open the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Open";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>]";
                yield return $"[{Name}] [folder name in current folder]";
                yield return $"[{Name}] ([file name(s) in current folder]{Separator})*";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                switch (arguments.Length)
                {
                    case 0:
                        Computer.OpenInWindowsExplorer(Path);
                        break;

                    default:
                        var first = ChildPath(arguments[0]);
                        if (Storage.Folder.Long.Exists(first))
                            Computer.OpenInWindowsExplorer(first);

                        return;
                }
                foreach (var i in arguments)
                    Try.Invoke(() => Process.Start(ChildPath(i)), e => Write($"Command '{name}' failed: {e.Message}"));
            }
        }

        public class PromptCommand : BaseCommand
        {
            public override string Description => "Open Command Prompt.";

            public override IEnumerable<string> Names()
            {
                yield return "Cmd";
                yield return "Prompt";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length == 0)
                {
                    var p = new Process();
                    p.StartInfo.FileName = "cmd.exe";
                    p.StartInfo.WorkingDirectory = Path;
                    p.StartInfo.UseShellExecute = false;
                    p.Start();
                    return;
                }
                throw new ArgumentOutOfRangeException();
            }
        }

        public class PropertiesCommand : ItemCommand
        {
            public override string Description => "Open properties for the specified file(s) and/or folder(s), or open properties for the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Properties";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>]";
                yield return $"[{Name}] ([file and/or folder name(s) in current folder]{Separator})*";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length == 0)
                {
                    Computer.Properties.Show(Path);
                    return;
                }
                for (int i = 0; i < arguments?.Length; i++)
                    arguments[i] = ChildPath(arguments[i]);

                Computer.Properties.Show(arguments);
            }
        }

        //...

        public class CreateFileCommand : ItemCommand
        {
            public override string Description => "Create specified files.";

            public override IEnumerable<string> Names()
            {
                yield return "Mf";
                yield return "MakeFile";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames}] ([file name(s) in current folder]{Separator})*";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length > 0)
                {
                    foreach (var i in arguments)
                    {
                        Try.Invoke(() =>
                        {
                            var path = ChildPath(i);
                            System.IO.File.Create(path);
                            Write($"Created '{path}'");
                        }, 
                        e => Write(e.Message));
                    }
                }
                throw new Exception("No file name(s) were specified.");
            }
        }

        public class CreateFolderCommand : ItemCommand
        {
            public override string Description => "Create specified folders.";

            public override IEnumerable<string> Names()
            {
                yield return "Md";
                yield return "MkDir";
                yield return "MakeDirectory";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames}] ([folder name(s) in current folder]{Separator})*";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length > 0)
                {
                    foreach (var i in arguments)
                    {
                        Try.Invoke(() =>
                        {
                            var path = ChildPath(i);
                            Storage.Folder.Long.Create(path);
                            Write($"Created '{path}'");
                        }, 
                        e => Write(e.Message));
                    }
                }
                throw new Exception("No folder name(s) were specified.");
            }
        }

        /// <summary>
        /// Parsing will fail!
        /// </summary>
        public class CopyCommand : ItemCommand
        {
            protected override char Separator => ' ';

            public override string Description => "Copy specified files and/or folders, or copy the current folder if none are specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Copy";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>] [absolute path of folder to copy to]";
                yield return $"[{Name}] [file or folder name(s) in current folder]  [absolute path of folder to copy to]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length.Within(1, 2))
                {
                    switch (arguments.Length)
                    {
                        case 1:
                            if (Storage.Folder.Long.Exists(Path))
                            {
                                if (Storage.Folder.Long.Exists(arguments[0]))
                                {
                                    Computer.Copy(Path, arguments[0]);
                                    Write($"Copied '{Path}' to '{arguments[0]}'");
                                    break;
                                }
                                throw new DirectoryNotFoundException(arguments[0]);
                            }
                            throw new DirectoryNotFoundException(Path);

                        case 2:
                            if (Storage.Folder.Long.Exists(arguments[1]))
                            {
                                var source = ChildPath(arguments[0]);
                                if (Storage.File.Long.Exists(source) || Storage.Folder.Long.Exists(source))
                                {
                                    Computer.Copy(source, arguments[1]);
                                    Write($"Copied '{source}' to '{arguments[1]}'");
                                    break;
                                }
                                throw new ItemNotFoundException(source);
                            }
                            throw new DirectoryNotFoundException(arguments[1]);
                    }
                }
                throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
            }
        }

        public class DeleteCommand : ItemCommand
        {
            public override string Description => "Delete specified file(s) and/or folder(s), or delete the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Del";
                yield return "Delete";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames} <the current folder>]";
                yield return $"[{AllNames}] ([file or folder name(s) in current folder]{Separator})*";
            }

            protected virtual string Completed() => "Deleted";

            protected virtual void DeleteFile(string filePath) => Storage.File.Long.Delete(filePath);

            protected virtual void DeleteFolder(string folderPath) => Storage.Folder.Long.Delete(folderPath);

            protected virtual int Show() => Dialog.Show("Delete", "Are you sure you want to delete this?", DialogImage.Warning, Buttons.ContinueCancel);

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                var result = Show();
                if (result == 0)
                {
                    if (arguments.Length > 0)
                    {
                        foreach (var i in arguments)
                        {
                            Try.Invoke(() =>
                            {
                                var path = ChildPath(i);
                                if (Storage.File.Long.Exists(path))
                                {
                                    //DeleteFile(path);
                                    Write($"{Completed()} '{path}'");
                                }
                                else if (Storage.Folder.Long.Exists(path))
                                {
                                    //DeleteFolder(path);
                                    Write($"{Completed()} '{path}'");
                                }
                            },
                            e => Write(e.Message));
                        }
                    }
                    else
                    {
                        if (Path.EndsWith(@":\"))
                            throw new InvalidOperationException("Cannot delete a drives.");

                        var newFolder = System.IO.Path.GetDirectoryName(Path);
                        var oldFolder = Path;

                        if (!Storage.Folder.Long.Exists(newFolder))
                            throw new InvalidOperationException("Cannot delete the current folder.");

                        Path = newFolder;
                        //DeleteFolder(oldFolder);
                        Write($"{Completed()} '{oldFolder}'");
                    }
                }
            }
        }

        public class HideShowCommand : ItemCommand
        {
            public override string Description => "Hide/show the specified file(s) and/or folder(s), or hide/show the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Hide";
                yield return "Show";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{AllNames} <the current folder>]";
                yield return $"[{AllNames}] ([file or folder name(s) in current folder]{Separator})*";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                bool hide = name.ToLower() == "hide";

                string message(string i) => $"'{i}' is now {(hide ? "hidden" : "visible")}";

                if (arguments.Length > 0)
                {
                    foreach (var i in arguments)
                    {
                        Try.Invoke(() =>
                        {
                            var path = ChildPath(i);
                            Storage.File.Long.Hide(path, hide);
                            Write(message(path));
                        },
                        e => Write(e.Message));
                    }
                }
                else
                {
                    Storage.File.Long.Hide(Path, hide);
                    Write(message(Path));
                }
            }
        }

        /// <summary>
        /// Parsing will fail!
        /// </summary>
        public class MoveCommand : ItemCommand
        {
            protected override char Separator => ' ';

            public override string Description => "Move the specified file or folder.";

            public override IEnumerable<string> Names()
            {
                yield return "Move";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>] [absolute path of folder to move to]";
                yield return $"[{Name}] [file or folder name in current folder] [absolute path of folder to move to]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length.Within(1, 2))
                {
                    string oldPath = null, newPath = null;
                    switch (arguments.Length)
                    {
                        //Move the current folder
                        case 1:
                            oldPath = Path;
                            newPath = arguments[0];

                            if (Storage.Folder.Long.Exists(newPath))
                            {
                                Storage.Folder.Long.Move(oldPath, FolderPath(newPath, System.IO.Path.GetFileName(oldPath)));
                                Path = newPath;
                                Write($"Moved '{oldPath}' to '{newPath}'");
                                return;
                            }
                            throw new DirectoryNotFoundException(newPath);

                        //Move the specified file or folder
                        case 2:
                            oldPath = ChildPath(arguments[0]);
                            newPath = arguments[1];
                            if (Storage.Folder.Long.Exists(newPath))
                            {
                                newPath = FolderPath(newPath, System.IO.Path.GetFileName(oldPath));
                                if (Storage.File.Long.Exists(oldPath))
                                    Storage.File.Long.Move(oldPath, newPath);

                                else if (Storage.Folder.Long.Exists(oldPath))
                                    Storage.Folder.Long.Move(oldPath, newPath);

                                else throw new ItemNotFoundException(oldPath);
                                Write($"Moved '{oldPath}' to '{newPath}'");
                                return;
                            }
                            throw new DirectoryNotFoundException(newPath);
                    }
                }
                throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
            }
        }

        public class RecycleCommand : DeleteCommand
        {
            public override string Description => "Recycle specified file(s) and/or folder(s), or recycle the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Recycle";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>]";
                yield return $"[{Name}] ([file or folder name(s) in current folder]{Separator})*";
            }

            protected override string Completed() => "Recycled";

            protected override void DeleteFile(string filePath) => Computer.Recycle(filePath);

            protected override void DeleteFolder(string folderPath) => Computer.Recycle(folderPath);

            protected override int Show() => Dialog.Show("Recycle", "Are you sure you want to recycle this?", DialogImage.Warning, Buttons.ContinueCancel);
        }

        /// <summary>
        /// Parsing will fail!
        /// </summary>
        public class RenameCommand : ItemCommand
        {
            protected override char Separator => ' ';

            public override string Description => "Renames the specified file or folder, or renames the current folder if nothing is specified.";

            public override IEnumerable<string> Names()
            {
                yield return "Rename";
            }

            public override IEnumerable<string> Usage()
            {
                yield return $"[{Name} <the current folder>] [new folder name]";
                yield return $"[{Name}] [file or folder in current folder] [new file or folder name]";
            }

            protected override void Execute(string name, string[] arguments, char[] flags)
            {
                if (arguments.Length.Within(1, 2))
                {
                    string oldPath = null, newPath = null;
                    switch (arguments.Length)
                    {
                        //Rename the current folder
                        case 1:
                            oldPath = Path;
                            newPath = System.IO.Path.GetDirectoryName(oldPath);
                            Path = newPath;

                            newPath = $@"{newPath}\{arguments[0]}";
                            Write($"Renamed '{oldPath}' to '{newPath}'");
                            Folder.Long.Move(oldPath, newPath);
                            break;

                        //Rename the specified file or folder
                        case 2:
                            oldPath = ChildPath(arguments[0]);
                            newPath = ChildPath(arguments[1]);

                            Write($"Renamed '{oldPath}' to '{newPath}'");
                            if (Storage.File.Long.Exists(oldPath))
                                Storage.File.Long.Move(oldPath, newPath);

                            else if (Storage.Folder.Long.Exists(oldPath))
                                Storage.Folder.Long.Move(oldPath, newPath);

                            else throw new ArgumentException($"The file or folder '{arguments[0]}' does not exist.");
                            break;
                    }
                }
                throw new ArgumentOutOfRangeException($"Only one or two arguments can be specified. If renaming the current folder, specify the new name only. If renaming a containing file or folder, specify the old and new name of the file or folder to be renamed separated by '{Separator}'.");
            }
        }
    }
}