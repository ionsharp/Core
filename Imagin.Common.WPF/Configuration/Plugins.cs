using Imagin.Common.Input;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;

namespace Imagin.Common.Configuration
{
    public class Plugins : ItemCollection
    {
        #region Properties

        public event EventHandler<EventArgs<IPlugin>> Enabled;

        protected readonly Dictionary<string, IPlugin> Source = new();

        #endregion

        #region Plugins

        public Plugins(string path) : base(path, new Filter(ItemType.File, "dll")) { }

        #endregion

        #region Methods

        /// <summary>
        /// Search for and return plugin type defined in specified assembly.
        /// </summary>
        static Type Find(Assembly Assembly)
        {
            foreach (var i in Assembly.GetTypes())
            {
                if (i.Implements<IPlugin>())
                    return i;
            }
            throw new TypeLoadException();
        }

        static IPlugin Create(AssemblyContext assemblyContext)
        {
            var result = Find(assemblyContext.Assembly).Create<IPlugin>();
            result.AssemblyContext = assemblyContext;
            return result;
        }

        protected override void OnAdded(Item item)
        {
            base.OnAdded(item);
            IPlugin plugin = default;

            var result = Try.Invoke(() =>
            {
                var guid = Guid.NewGuid();

                var assemblyContext = new AssemblyContext(guid, Assembly.Load(System.IO.File.ReadAllBytes(item.Path)), AppDomain.CreateDomain(guid.ToString()));
                plugin = Create(assemblyContext);
            });

            if (!result)
            {
                Remove(item);
            }
            else
            {
                plugin.Enabled += OnEnabled;
                Source[item.Path] = plugin;
            }
        }

        protected override void OnRemoved(Item item)
        {
            base.OnAdded(item);
            if (Source.ContainsKey(item.Path))
            {
                var plugin = Source[item.Path];

                if (plugin.IsEnabled)
                    plugin.IsEnabled = false;

                plugin.Enabled -= OnEnabled;

                if (plugin.AssemblyContext?.AppDomain != null)
                    AppDomain.Unload(plugin.AssemblyContext.AppDomain);
                
                Source.Remove(item.Path);
            }
        }

        protected virtual void OnEnabled(object sender, EventArgs e)
        {
            Enabled?.Invoke(this, new EventArgs<IPlugin>(sender as IPlugin));
        }

        #endregion
    }
}