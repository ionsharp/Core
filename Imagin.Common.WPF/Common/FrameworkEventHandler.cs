using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace Imagin.Common
{
    public class FrameworkEventHandler : DependencyObject
    {
        readonly List<Action<FrameworkElement>> Load = new();

        readonly Dictionary<object, Action<FrameworkElement>> LoadAttached = new();

        //...

        readonly List<Action<FrameworkElement>> Unload = new();

        readonly Dictionary<object, Action<FrameworkElement>> UnloadAttached = new();

        //...

        readonly Dictionary<object, bool> PreloadAttached = new();

        //...

        readonly FrameworkElement Element;

        //...

        public FrameworkEventHandler(FrameworkElement i)
        {
            Element = i;
            i.Loaded += OnLoaded;
        }

        //...

        internal void Add(Action load, Action unload)
        {
            if (load != null)
                Load.Add(i => load());

            if (unload != null)
                Unload.Add(i => unload());
        }

        internal void Add<T>(Action<T> load, Action<T> unload) where T : FrameworkElement
        {
            if (load != null)
                Load.Add(i => load((T)i));

            if (unload != null)
                Unload.Add(i => unload((T)i));
        }

        internal void AddAttached<T>(bool add, object key, Action<T> load, Action<T> unload) where T : FrameworkElement
        {
            if (UnloadAttached.ContainsKey(key))
                UnloadAttached[key](Element);

            else unload?.Invoke((T)Element);

            LoadAttached
                .Remove(key);
            UnloadAttached
                .Remove(key);

            PreloadAttached
                .Remove(key);

            if (add)
            {
                PreloadAttached.Add(key, false);
                if (load != null)
                {
                    if (Element.IsLoaded)
                    {
                        PreloadAttached[key] = true;
                        load((T)Element);
                    }
                    LoadAttached.Add(key, i => load((T)i));
                }

                if (unload != null)
                    UnloadAttached.Add(key, i => unload((T)i));
            }
        }

        //...

        void OnLoaded()
        {
            if (Element is Window window)
                window.Closed += OnWindowClosed;
            
            if (Element is not Window)
            {
                Element.Unloaded 
                    += OnUnloaded;
                Window.GetWindow(Element).If(i => i is not null, i => i.Closed += OnWindowClosed);
            }

            Load
                .ForEach(i => i(Element));
            LoadAttached
                .ForEach(i =>
                {
                    if (!PreloadAttached[i.Key])
                        i.Value(Element);
                });
        }

        void OnUnloaded()
        {
            if (Element is Window window)
                window.Closed -= OnWindowClosed;

            if (Element is not Window)
            {
                Element.Unloaded 
                    -= OnUnloaded;
                Window.GetWindow(Element).If(i => i is not null, i => i.Closed -= OnWindowClosed);
            }

            Unload
                .ForEach(i => i(Element));
            UnloadAttached
                .ForEach(i =>
                {
                    PreloadAttached[i.Key] = false;
                    i.Value(Element);
                });
        }

        //...

        void OnLoaded(object sender, RoutedEventArgs e) => OnLoaded();

        void OnUnloaded(object sender, RoutedEventArgs e) => OnUnloaded();

        //...

        void OnWindowClosed(object sender, EventArgs e)
        {
            Element.Loaded -= OnLoaded;
            OnUnloaded();
        }
    }
}