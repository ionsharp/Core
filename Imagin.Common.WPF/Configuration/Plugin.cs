using Imagin.Common.Models;
using System;
using System.Windows;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// A wrapper for an independent class library used to extend functionality of an application.
    /// </summary>
    [Serializable]
    public abstract class Plugin : Base, IPlugin
    {
        public event EventHandler<EventArgs> Disabled;

        public event EventHandler<EventArgs> Enabled;

        //...

        public AssemblyContext AssemblyContext { get; set; }

        //...

        bool isEnabled;
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
                    OnEnabled();
                }
                else OnDisabled();
            }
        }

        //...

        public abstract string Author { get; }

        public abstract string Description { get; }

        public abstract string Icon { get; }

        public abstract string Name { get; }

        public abstract string Uri { get; }

        public abstract Version Version { get;  }

        //...

        public Plugin() : base() { }

        //...

        public virtual void OnDisabled()
        {
            Disabled?.Invoke(this, new EventArgs());
        }

        public virtual void OnEnabled()
        {
            Enabled?.Invoke(this, new EventArgs());
        }
    }

    public abstract class PanelPlugin : Plugin
    {
        public abstract Panel Panel { get; }

        public abstract IPluginResources Resources { get; }

        DataTemplate template = null;
        public DataTemplate Template
        {
            get
            {
                if (template == null)
                {
                    foreach (var i in Resources.GetValues())
                    {
                        if (i is DataTemplate j)
                        {
                            if ((Type)j.DataType == Panel.GetType())
                            {
                                template = j;
                                break;
                            }
                        }
                    }
                }
                return template;
            }
        }

        public override void OnDisabled()
        {
            base.OnEnabled();
            Get.Where<IDockViewModel>().Panels.Remove(Panel);
        }

        public override void OnEnabled()
        {
            base.OnEnabled();
            Get.Where<IDockViewModel>().Panels.Add(Panel);
        }
    }
}