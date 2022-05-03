using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Models;
using System;

namespace Imagin.Apps.Paint
{
    public class NotesPanel : Panel
    {
        [Hidden]
        public override Uri Icon => Resources.ProjectImage("Note.png");

        Note note = default;
        [Hidden]
        public Note Note
        {
            get => note;
            set => this.Change(ref note, value);
        }

        [Hidden]
        public override string Title => "Notes";

        public NotesPanel() : base()
        {
            IsVisible = false;
            Get.Current<MainViewModel>().DocumentRemoved += OnDocumentRemoved;
        }

        void OnDocumentRemoved(object sender, EventArgs<Document> e)
        {
            Note = null;
        }
    }
}