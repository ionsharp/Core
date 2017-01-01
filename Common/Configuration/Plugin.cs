using Imagin.Common.Attributes;
using Imagin.Common.Mvvm;
using Imagin.Common.Tracing;
using System;
using System.ComponentModel;
using System.Windows;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// A wrapper for an independent class library used to extend functionality in an application.
    /// </summary>
    [Serializable]
    public abstract class Plugin : AbstractObject, IPlugin
    {
        #region Properties

        public event EventHandler<EventArgs> Enabled;

        /// <summary>
        /// Get reference to current application as IApp.
        /// </summary>
        protected IApp App
        {
            get
            {
                return Application.Current as IApp;
            }
        }

        protected IPaneViewModel PaneViewModel
        {
            get; private set;
        }

        /// <summary>
        /// Resources used by the plugin.
        /// </summary>
        protected ResourceDictionary Resources
        {
            get; private set;
        }

        string author;
        /// <summary>
        /// The individual who (or organization that) developed the plugin.
        /// </summary>
        [ReadOnly(true)]
        public string Author
        {
            get
            {
                return author;
            }
            private set
            {
                author = value;
                OnPropertyChanged("Author");
            }
        }

        string description;
        /// <summary>
        /// A short description explaining what the plugin does.
        /// </summary>
        [ReadOnly(true)]
        public string Description
        {
            get
            {
                return description;
            }
            private set
            {
                description = value;
                OnPropertyChanged("Description");
            }
        }

        string icon;
        /// <summary>
        /// Uri for an icon resource.
        /// </summary>
        [Browsable(false)]
        public string Icon
        {
            get
            {
                return icon;
            }
            private set
            {
                icon = value;
                OnPropertyChanged("Icon");
            }
        }

        bool isEnabled;
        /// <summary>
        /// Whether or not the plugin is enabled.
        /// </summary>
        [Browsable(false)]
        public bool IsEnabled
        {
            get
            {
                return isEnabled;
            }
            set
            {
                isEnabled = value;
                OnPropertyChanged("IsEnabled");

                if (value)
                {
                    this.Enable();
                    OnEnabled();
                }
                else this.Disable();
            }
        }

        string name;
        /// <summary>
        /// The name of the plugin.
        /// </summary>
        [Featured(true)]
        [ReadOnly(true)]
        public string Name
        {
            get
            {
                return name;
            }
            private set
            {
                name = value;
                OnPropertyChanged("Name");
            }
        }

        string uri;
        /// <summary>
        /// Website where the plugin is published.
        /// </summary>
        [ReadOnly(true)]
        public string Uri
        {
            get
            {
                return uri;
            }
            private set
            {
                uri = value;
                OnPropertyChanged("Uri");
            }
        }

        Version version;
        /// <summary>
        /// Plugin version.
        /// </summary>
        [ReadOnly(true)]
        public Version Version
        {
            get
            {
                return version;
            }
            private set
            {
                version = value;
                OnPropertyChanged("Version");
            }
        }

        #endregion

        #region Methods

        #region Abstract

        /// <summary>
        /// Get pane view model associated with the plugin (if there is one).
        /// </summary>
        protected abstract IPaneViewModel GetPaneViewModel();

        /// <summary>
        /// Get resources used by the plugin.
        /// </summary>
        protected abstract ResourceDictionary GetResources();

        protected abstract string GetAuthor();

        protected abstract string GetDescription();

        protected abstract string GetIcon();

        protected abstract string GetName();

        protected abstract string GetUri();

        protected abstract Version GetVersion();

        public abstract void Enable();

        public abstract void Disable();

        #endregion

        #region Protected

        /// <summary>
        /// Add pane view model.
        /// </summary>
        protected void MergePaneViewModel()
        {
            App.GetMainWindowViewModel().GetPanes().Add(PaneViewModel);
        }

        /// <summary>
        /// Merge plugin resources with application.
        /// </summary>
        protected void MergeResources()
        {
            App.MainWindow.Resources.MergedDictionaries.Add(this.Resources);
        }

        /// <summary>
        /// Remove pane view model.
        /// </summary>
        protected void ReleasePaneViewModel()
        {
            App.GetMainWindowViewModel().GetPanes().Remove(PaneViewModel);
        }

        /// <summary>
        /// Remove plugin resources from application.
        /// </summary>
        protected void ReleaseResources()
        {
            App.MainWindow.Resources.MergedDictionaries.Remove(this.Resources);
        }

        /// <summary>
        /// Write information to log.
        /// </summary>
        protected void Trace(WarningLevel WarningLevel, LogEntryStatus LogEntryStatus, string Source, string Message)
        {
            App.GetLog().Write(WarningLevel, LogEntryStatus, Source, Message);
        }

        #endregion

        #region Virtual

        protected virtual void OnEnabled()
        {
            if (Enabled != null)
                Enabled(this, new EventArgs());
        }

        #endregion

        #endregion

        #region Plugin

        public Plugin() : base()
        {
            Author = GetAuthor();
            Description = GetDescription();
            Icon = GetIcon();
            Name = GetName();
            Uri = GetUri();
            Version = GetVersion();
            PaneViewModel = GetPaneViewModel();
            Resources = GetResources();
        }

        #endregion
    }
}
