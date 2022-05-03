using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;

namespace Imagin.Common.Configuration
{
    public sealed class ApplicationResources : Resources, IPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public readonly string FolderPath;

        //...

        public ResourceDictionary CurrentTheme => MergedDictionaries.Count > 0 ? MergedDictionaries[0] : null;

        //...

        public PathCollection CustomThemes { get; private set; } = new PathCollection(new Filter(ItemType.File, "theme"));

        //...

        Collection<ThemeResource> DefaultResources { get; set; } = new Collection<ThemeResource>();

        class ThemeResource : Dictionary<DefaultThemes, Uri>
        {
            public ThemeResource(string assemblyName) => typeof(DefaultThemes).GetEnumValues().Cast<DefaultThemes>().ForEach(i => Add(i, Uri(assemblyName, ThemeKey.KeyFormat.F(i))));
        }

        //...

        public ApplicationResources(BaseApplication application) : base()
        {
            //... Themes
            DefaultResources.Add(new(InternalAssembly.Name));

            //... Styles
            application.Resources.MergedDictionaries.Add(this);
            typeof(StyleKeys).GetEnumValues().Cast<StyleKeys>().ForEach(i =>
            {
                switch (i)
                {
                    case StyleKeys.Member: break; //StyleKeys.PropertyGrid imports this already!
                    default:
                        application.Resources.MergedDictionaries.Add(New(InternalAssembly.Name, StyleKey.KeyFormat.F(i)));
                        break;
                }
            });

            //...

            FolderPath = $@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\Themes";
            if (!Folder.Long.Exists(FolderPath))
                System.IO.Directory.CreateDirectory(FolderPath);

            CustomThemes.Refresh(FolderPath);
        }

        //...

        public string ThemePath(string fileName) => $@"{FolderPath}\{fileName}.theme";

        //...

        public void LoadTheme(DefaultThemes theme) => LoadTheme($"{theme}");

        public void LoadTheme(string theme)
        {
            BeginInit();
            MergedDictionaries.Clear();

            if (!Enum.TryParse(theme, out DefaultThemes type))
            {
                XResourceDictionary.TryLoad(theme, out ResourceDictionary result);
                if (result != null)
                {
                    MergedDictionaries.Add(result);
                    goto End;
                }
            }
            foreach (var i in DefaultResources)
            {
                var result = new ResourceDictionary
                {
                    Source = i[type]
                };
                MergedDictionaries.Add(result);
            }

            End:
            {
                EndInit();
                this.Changed(() => CurrentTheme);
            }
        }

        public void SaveTheme(string nameWithoutExtension) => CurrentTheme.TrySave(ThemePath(nameWithoutExtension));

        //...

        public void OnPropertyChanged([CallerMemberName] string propertyName = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}