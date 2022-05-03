using Imagin.Common.Analytics;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Data;
using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Local;
using Imagin.Common.Serialization;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Windows;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    [DisplayName("Options")]
    [Serializable]
    public abstract class MainViewOptions : Base, IMainViewOptions
    {
        #region (enum) Category

        enum Category
        {
            General,
            Log,
            Notifications,
            Theme,
            Update,
            Window
        }

        #endregion

        #region Events

        [field: NonSerialized]
        public event EventHandler<EventArgs<string>> ThemeChanged;

        #endregion

        #region Properties

        #region General

        bool autoSave = false;
        [Featured, Style(BooleanStyle.Switch)]
        public bool AutoSave
        {
            get => autoSave;
            set => this.Change(ref autoSave, value);
        }

        bool saveWithDialog = true;
        [Category(Category.General)]
        public bool SaveWithDialog
        {
            get => saveWithDialog;
            set => this.Change(ref saveWithDialog, value);
        }

        #endregion

        #region Language

        Language language;
        [Category(nameof(Language))]
        [DisplayName("Language")]
        [Localize(false)]
        public Language Language
        {
            get => language;
            set => this.Change(ref language, value);
        }

        #endregion

        #region Log
        
        bool logEnabled = true;
        [Category(Category.Log)]
        [DisplayName("Enabled")]
        public bool LogEnabled
        {
            get => logEnabled;
            set => this.Change(ref logEnabled, value);
        }

        #endregion

        #region Theme

        [Category(Category.Theme)]
        [DisplayName("Theme")]
        [Index(1)]
        public ApplicationResources Themes => Get.Current<ApplicationResources>();

        string theme = $"{DefaultThemes.Light}";
        [Hidden]
        public string Theme
        {
            get => theme;
            set
            {
                this.Change(ref theme, value);
                Themes.LoadTheme(value);
                OnThemeChanged(value);
            }
        }

        bool autoSaveTheme = true;
        [Category(Category.Theme)]
        [DisplayName("AutoSave")]
        [Index(0)]
        public bool AutoSaveTheme
        {
            get => autoSaveTheme;
            set => this.Change(ref autoSaveTheme, value);
        }

        #endregion

        #region Update

        [field: NonSerialized]
        ICommand updateCheckCommand;
        [Category(Category.Update)]
        [DisplayName("Check")]
        public virtual ICommand UpdateCheckCommand => updateCheckCommand ??= new RelayCommand(() =>
        {

        }, false);

        [field: NonSerialized]
        ICommand updateCommand;
        [Category(Category.Update)]
        [DisplayName("Update")]
        public virtual ICommand UpdateCommand => updateCommand ??= new RelayCommand(() =>
        {

        }, false);

        #endregion

        #region Window

        protected virtual WindowState DefaultWindowState 
            => WindowState.Normal;

        bool windowShowInTaskBar = false;
        [Category(Category.Window)]
        [DisplayName("ShowInTaskbar")]
        public virtual bool WindowShowInTaskBar
        {
            get => windowShowInTaskBar;
            set => this.Change(ref windowShowInTaskBar, value);
        }

        double windowHeight = 900;
        [Hidden]
        public virtual double WindowHeight
        {
            get => windowHeight;
            set => this.Change(ref windowHeight, value);
        }

        double windowWidth = 1300;
        [Hidden]
        public virtual double WindowWidth
        {
            get => windowWidth;
            set => this.Change(ref windowWidth, value);
        }

        string windowState = null;
        [Hidden]
        public virtual WindowState WindowState
        {
            get
            {
                windowState ??= $"{DefaultWindowState}";
                return (WindowState)Enum.Parse(typeof(WindowState), windowState);
            }
            set => this.Change(ref windowState, $"{value}");
        }

        #endregion

        #endregion

        #region MainViewOptions

        public MainViewOptions() : base()
        {
            OnLoaded();
        }

        void OnApplicationLoaded(object sender, EventArgs e)
        {
            Get.Where<IApplication>().Loaded -= OnApplicationLoaded;
            OnApplicationLoaded();
        }

        #endregion

        #region Methods

        protected virtual void OnApplicationLoaded() { }

        [OnDeserialized]
        protected void OnDeserialized(StreamingContext input)
        {
            OnLoaded();
        }

        protected virtual IEnumerable<IWriter> GetData() => default;

        protected virtual void OnLoaded()
        {
            Get.Register(GetType(), this);
            Language.Set();

            Themes.LoadTheme(theme);
            GetData().If(i => i.Count() > 0, i => i.ForEach(j => j.Load()));

            Get.Where<IApplication>().Loaded += OnApplicationLoaded;
        }

        protected virtual void OnSaved() { }

        protected virtual void OnSaving() { }

        protected virtual void OnThemeChanged(string i) => ThemeChanged?.Invoke(this, new EventArgs<string>(i));

        //...

        public override void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            switch (propertyName)
            {
                case nameof(Language):
                    Language.Set();
                    break;
            }

            base.OnPropertyChanged(propertyName);
            if (AutoSave) Save();
        }

        //...

        public void Save()
        {
            OnSaving();

            if (AutoSaveTheme)
            {
                if (Storage.File.Long.Exists(theme))
                    Themes.SaveTheme(Path.GetFileNameWithoutExtension(theme));
            }

            BinarySerializer.Serialize(Get.Where<SingleApplication>().Properties.FilePath, this);
            GetData().If(i => i.Count() > 0, i => i.ForEach(j => j.Save()));

            OnSaved();
        }

        #endregion

        #region Commands

        [field: NonSerialized]
        ICommand clearLogCommand;
        [Category(nameof(Log))]
        [DisplayName("Clear")]
        [Index(99)]
        public virtual ICommand ClearLogCommand => clearLogCommand ??= new RelayCommand(() => Get.Where<ILog>().Clear(), () => Get.Where<ILog>()?.Count > 0);
        
        [field: NonSerialized]
        ICommand deleteThemeCommand;
        [Hidden]
        public virtual ICommand DeleteThemeCommand => deleteThemeCommand ??= new RelayCommand(() => Computer.Recycle(theme), () =>
        {
            var result = false;
            Try.Invoke(() => result = Storage.File.Long.Exists(theme));
            return result;
        });

        [field: NonSerialized]
        ICommand openLogCommand;
        [Category(nameof(Log))]
        [DisplayName("Open")]
        [Index(98)]
        public virtual ICommand OpenLogCommand => openLogCommand ??= new RelayCommand(() => Storage.File.Long.Open(Get.Where<BaseApplication>().Log.FilePath));

        [field: NonSerialized]
        ICommand resetCommand;
        [Hidden]
        public virtual ICommand ResetCommand => resetCommand ??= new RelayCommand(() =>
        {

        });

        [field: NonSerialized]
        ICommand saveCommand;
        [DisplayName("Save")]
        [Featured(AboveBelow.Below)]
        public virtual ICommand SaveCommand => saveCommand ??= new RelayCommand(Save);

        [field: NonSerialized]
        ICommand saveLogCommand;
        [Category(nameof(Log))]
        [DisplayName("Save")]
        [Index(99)]
        public virtual ICommand SaveLogCommand => saveLogCommand ??= new RelayCommand(() => Get.Where<ILog>().Save(), () => Get.Where<ILog>() != null);
        
        [field: NonSerialized]
        ICommand saveThemeCommand;
        [Hidden]
        public virtual ICommand SaveThemeCommand => saveThemeCommand ??= new RelayCommand(() =>
        {
            var inputWindow = new InputWindow
            {
                Placeholder = "File name without extension",
                Title = "Save theme..."
            };
            inputWindow.ShowDialog();
            if (!inputWindow.Input.NullOrEmpty())
            {
                Themes.SaveTheme(inputWindow.Input);

                theme = Themes.ThemePath(inputWindow.Input);
                this.Changed(() => Theme);

                Dialog.Show("Save theme", "Theme saved!", DialogImage.Information, Buttons.Ok);
            }
        },
        () => true);

        #endregion
    }
}