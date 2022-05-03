using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace Imagin.Common.Storage
{
    #region DropHandler<Element>

    public abstract class DropHandler<Element> : DropHandler<Element, Item> where Element : UIElement
    {
        public DropHandler(Element parent) : base(parent) { }

        //...

        protected override IEnumerable<Item> From(object source)
        {
            if (source is Item)
            {
                yield return (Item)source;
            }
            else
            if (source is IEnumerable<Item>)
            {
                if ((source as IEnumerable<Item>).Count() > 0)
                {
                    foreach (var i in (IEnumerable<Item>)source)
                        yield return i;
                }
            }
        }

        protected override IEnumerable<Item> From(DataObject dataObject)
        {
            if (dataObject.ContainsFileDropList())
            {
                foreach (var i in dataObject.GetFileDropList())
                    yield return Computer.GetItem(i);
            }
        }
    }

    #endregion

    //...

    #region PathBox

    public class PathBoxDropHandler : DropHandler<PathBox>
    {
        public PathBoxDropHandler(PathBox parent) : base(parent) { }

        protected override DragDropEffects DragOver(IEnumerable<Item> source, UIElement target) => DragDropEffects.Copy;

        protected override void Drop(IEnumerable<Item> source, UIElement target) => Parent.SetCurrentValue(PathBox.TextProperty, source.First().Path);
    }

    #endregion

    #region MultiPathBox

    public class MultiPathBoxDropHandler : DropHandler<MultiPathBox>
    {
        public MultiPathBoxDropHandler(MultiPathBox parent) : base(parent) { }

        protected override DragDropEffects DragOver(IEnumerable<Item> source, UIElement target) => DragDropEffects.Copy;

        protected override void Drop(IEnumerable<Item> source, UIElement target) => Parent.SetCurrentValue(MultiPathBox.SourceProperty, $"{Parent.Source};{source.Select(i => i.Path).ToString(";")}");
    }

    #endregion

    //...

    #region ExplorerDropHandler<T>

    public abstract class ExplorerDropHandler<T> : DropHandler<T> where T : UIElement, IExplorer
    {
        bool Modified => ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed();

        public ExplorerDropHandler(T parent) : base(parent) { }

        //...

        protected override DragDropEffects DragOver(IEnumerable<Item> source, UIElement target)
        {
            if (ModifierKeys.Control.Pressed() || ModifierKeys.Shift.Pressed())
                return DragDropEffects.Copy;

            return DragDropEffects.Move;
        }

        protected override void Drop(IEnumerable<Item> source, UIElement target)
        {
            if (XExplorer.GetWarnBeforeDrop(Parent))
            {
                var title
                    = Modified
                    ? XExplorer.GetCopyWarningTitle(Parent)
                    : XExplorer.GetMoveWarningTitle(Parent);

                var message
                    = Modified
                    ? XExplorer.GetCopyWarningMessage(Parent)
                    : XExplorer.GetMoveWarningMessage(Parent);

                if (Dialog.Show(title, message, DialogImage.Warning, Buttons.YesNo) == 1)
                    return;
            }
            Execute(source, GetTargetPath(target));
        }

        protected override bool Droppable(UIElement target)
        {
            if (target is FrameworkElement f)
            {
                if (f.DataContext is Item item)
                {
                    switch (item.Type)
                    {
                        case ItemType.Drive:
                            return false;

                        case ItemType.File:
                            return false;

                        case ItemType.Folder:
                            return true;

                        case ItemType.Shortcut:
                            return Shortcut.TargetsFolder(item.Path);
                    }
                }
                else if (f.DataContext is string path)
                    return Folder.Long.Exists(path);
            }
            return false;
        }

        //...

        protected virtual string GetTargetPath(UIElement target)
        {
            if (target is FrameworkElement element)
            {
                var item = element.DataContext;
                if (item is Item i)
                    return i.Path;

                if (item is string j && Folder.Long.Exists(j))
                    return j;
            }
            return default;
        }

        //...

        protected void Execute(IEnumerable<Item> source, string targetPath)
        {
            if (targetPath is not null)
            {
                if (Modified)
                    Computer.Copy(source, targetPath);

                else Computer.Move(source, targetPath);
            }
        }
    }

    #endregion

    //...

    #region Browser

    public class BrowserDropHandler : ExplorerDropHandler<Browser>
    {
        public BrowserDropHandler(Browser parent) : base(parent) { }

        protected override bool Droppable(UIElement target) => base.Droppable(target) || target is ListView;

        protected override string GetTargetPath(UIElement target)
        {
            if (target is ListView)
                return Parent.Path;

            return base.GetTargetPath(target);
        }
    }

    #endregion

    #region AddressBox

    public class AddressBoxDropHandler : ExplorerDropHandler<AddressBox>
    {
        public AddressBoxDropHandler(AddressBox parent) : base(parent) { }

        protected override bool Droppable(UIElement target) => base.Droppable(target) || target is ToolBar;
    }

    #endregion

    #region FolderBox

    public class FolderBoxDropHandler : ExplorerDropHandler<FolderBox>
    {
        public FolderBoxDropHandler(FolderBox parent) : base(parent) { }

        protected override bool Droppable(UIElement target)
        {
            var result = false;
            Try.Invoke(() =>
            {
                var targetPath = GetTargetPath(target);
                result = Shortcut.TargetsFolder(targetPath) || Folder.Long.Exists(targetPath);
            });
            return result;
        }

        protected override string GetTargetPath(UIElement target)
        {
            if (Parent.SelectedItem == null)
                return Parent.Path;

            return Parent.SelectedItem.As<Folder>()?.Path;
        }
    }

    #endregion

    #region FolderButton

    public class FolderButtonDropHandler : ExplorerDropHandler<FolderButton>
    {
        public FolderButtonDropHandler(FolderButton parent) : base(parent) { }

        protected override bool Droppable(UIElement target)
        {
            var result = false;
            Try.Invoke(() =>
            {
                var targetPath = GetTargetPath(target);
                result = Shortcut.TargetsFolder(targetPath) || Folder.Long.Exists(targetPath);
            });
            return result;
        }

        protected override string GetTargetPath(UIElement target) => Parent.Path;
    }

    #endregion

    #region Navigator

    public class NavigatorDropHandler : ExplorerDropHandler<Navigator>
    {
        public NavigatorDropHandler(Navigator parent) : base(parent) { }
    }

    #endregion
}