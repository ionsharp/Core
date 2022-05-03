using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using System.Collections.Specialized;

namespace Imagin.Common.Models
{
    public abstract class DockViewModel<View, Document> : MainViewModel<View>, IDockViewModel where View : MainWindow where Document : Models.Document
    {
        #region Events

        public event DefaultEventHandler<Content> 
            ActiveContentChanged;

        public event DefaultEventHandler<Document> 
            ActiveDocumentChanged;

        public event DefaultEventHandler<Panel> 
            ActivePanelChanged;

        public event DefaultEventHandler<Document> 
            DocumentAdded;

        public event DefaultEventHandler<Document>
            DocumentRemoved;

        #endregion

        #region Properties

        Content activeContent = null;
        [Hidden]
        public Content ActiveContent
        {
            get => activeContent;
            set
            {
                this.Change(ref activeContent, value);
                OnActiveContentChanged(value);
            }
        }

        [Hidden]
        Models.Document IDockViewModel.ActiveDocument 
            { get => ActiveDocument; set => ActiveDocument = value as Document; }

        Document activeDocument = null;
        [Hidden]
        public Document ActiveDocument
        {
            get => activeDocument;
            set
            {
                this.Change(ref activeDocument, value);
                OnActiveDocumentChanged(value);
            }
        }

        Panel activePanel = null;
        [Hidden]
        public Panel ActivePanel
        {
            get => activePanel;
            set
            {
                this.Change(ref activePanel, value);
                OnActivePanelChanged(value);
            }
        }

        readonly DocumentCollection documents = new();
        [Hidden]
        public DocumentCollection Documents => documents;

        readonly PanelCollection panels = new();
        [Hidden]
        public PanelCollection Panels => panels;

        #endregion

        #region DockViewModel

        public DockViewModel() : base()
        {
            Documents.CollectionChanged 
                += OnDocumentsChanged;
            Documents.Removing
                += OnDocumentRemoving;

            GetDefaultPanels()
                .ForEach(i => Panels.Add(i));
            GetPanels()
                .ForEach(i => Panels.Add(i));
        }

        #endregion

        #region Methods

        void OnDocumentsChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            switch (e.Action)
            {
                case NotifyCollectionChangedAction.Add:
                    ActiveContent = (Content)e.NewItems[0];
                    OnDocumentAdded((Document)e.NewItems[0]);
                    break;
                case NotifyCollectionChangedAction.Remove:
                    OnDocumentRemoved((Document)e.OldItems[0]);
                    break;
            }
        }

        void OnDocumentRemoving(object sender, CancelEventArgs<object> e)
        {
            if (!e.Cancel)
            {
                if (e.Value.As<Models.Document>().CanClose)
                {
                    OnDocumentRemoving(e.Value as Document);
                    if (e.Value.As<Models.Document>().IsModified)
                    {
                        var result = Dialog.Show("Close", "Are you sure you want to close?", DialogImage.Warning, Buttons.YesNo);
                        e.Cancel = result == 1;
                    }
                }
                else e.Cancel = true;
            }
        }

        //...

        protected virtual void OnActiveContentChanged(Content content)
            => ActiveContentChanged?.Invoke(this, new(content));

        protected virtual void OnActiveDocumentChanged(Document document)
            => ActiveDocumentChanged?.Invoke(this, new(document));

        protected virtual void OnActivePanelChanged(Panel panel)
            => ActivePanelChanged?.Invoke(this, new(panel));

        //...

        protected virtual void OnDocumentAdded(Document document)
        {
            DocumentAdded?.Invoke(this, new(document));
            document.Subscribe();
        }

        protected virtual void OnDocumentRemoved(Document document)
        {
            if (Documents.Count == 0)
                ActiveDocument = null;

            DocumentRemoved?.Invoke(this, new(document));
            document.Unsubscribe();
        }

        protected virtual void OnDocumentRemoving(Document document) { }

        //...

        public virtual IEnumerable<Panel> GetDefaultPanels()
        {
            yield return
                new LogPanel(Get.Where<BaseApplication>().Log);
            yield return
                new NotificationsPanel(Get.Where<BaseApplication>().Notifications);
            yield return
                new OptionsPanel();
            yield return
                new ThemePanel();
        }

        public virtual IEnumerable<Panel> GetPanels() => Enumerable.Empty<Panel>();

        #endregion

        #region Commands

        ICommand closeCommand;
        [Hidden]
        public ICommand CloseCommand => closeCommand ??= new RelayCommand(() => Documents.Remove(ActiveDocument), () => ActiveDocument != null);

        ICommand closeAllCommand;
        [Hidden]
        public ICommand CloseAllCommand => closeAllCommand ??= new RelayCommand(() => Documents.Clear(), () => Documents.Count > 0);

        ICommand closeAllButThisCommand;
        [Hidden]
        public ICommand CloseAllButThisCommand => closeAllButThisCommand ??= new RelayCommand(() =>
        {
            for (var j = Documents.Count - 1; j >= 0; j--)
            {
                if (!ActiveDocument.Equals(Documents[j]))
                    Documents.RemoveAt(j);
            }
        },
        () => Documents.Count > 0 && ActiveDocument != null);

        ICommand saveAllCommand;
        [Hidden]
        public ICommand SaveAllCommand => saveAllCommand ??= new RelayCommand(() => Documents.ForEach(i => i.Save()), () => Documents.Count > 0);

        #endregion
    }
}