using System;
using System.IO;
using System.Reflection;
using System.Text;

namespace Imagin.Common.Configuration
{
    /// <summary>
    /// Defines necessary utilites to load and unload plugins.
    /// </summary>
    public static class PluginManager
    {
        #region Private

        /// <summary>
        /// Return a context with newly created AppDomain.
        /// </summary>
        /// <param name="Path">Path to the .dll file.</param>
        static AppDomainContext GetContext(string Path)
        {
            var Id = Guid.NewGuid();

            AppDomain AppDomain = AppDomain.CreateDomain(Id.ToString());
            return new AppDomainContext(Id, AppDomain);
        }

        /// <summary>
        /// Create and return instance of plugin based on specified context.
        /// </summary>
        static IPlugin GetPlugin(AppDomainContext AppDomainContext, string Path)
        {
            var Instance = Activator.CreateInstance(GetPluginType(AppDomainContext.Assembly));

            var Result = (IPlugin)Instance;
            //Result.Context = AppDomainContext;

            return Result;
        }

        /// <summary>
        /// Search for and return plugin type defined in specified assembly.
        /// </summary>
        static Type GetPluginType(Assembly Assembly)
        {
            foreach (var i in Assembly.GetTypes())
            {
                if (typeof(IPlugin).IsAssignableFrom((Type)i))
                    return i;
            }
            throw new DllNotFoundException("Couldn't find plugin type in target assembly.");
        }

        #endregion

        #region Public

        /// <summary>
        /// Create and add plugin.
        /// </summary>
        public static Exception Install(string Path)
        {
            var Plugin = default(IPlugin);
            return Install(Path, out Plugin);
        }

        /// <summary>
        /// Create and add plugin.
        /// </summary>
        public static Exception Install(string Path, out IPlugin Result)
        {
            Result = default(IPlugin);
            try
            {
                var AppDomainContext = GetContext(Path);

                var Contents = File.ReadAllBytes(Path);
                AppDomainContext.Assembly = Assembly.Load(Contents);

                Result = GetPlugin(AppDomainContext, Path);
                return null;
            }
            catch (Exception e)
            {
                return e;
            }
        }

        /// <summary>
        /// Remove and dispose plugin.
        /// </summary>
        public static Exception Uninstall(IPlugin Plugin)
        {
            if (Plugin.IsEnabled)
                Plugin.Disable();

            //Unload AppDomain that hosts plugin

            return null;
        }

        #endregion
    }
}
