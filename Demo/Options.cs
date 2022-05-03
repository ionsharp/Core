using Imagin.Common;
using Imagin.Common.Collections;
using Imagin.Common.Collections.Serialization;
using Imagin.Common.Configuration;
using Imagin.Common.Controls;
using Imagin.Common.Models;
using Imagin.Common.Storage;
using System;
using System.Collections.Generic;

namespace Demo
{
    [Serializable]
    public class Options : DockViewOptions<MainViewModel>
    {
        #region (enum) Category

        enum Category
        {
            Explorer
        }

        #endregion

        #region Properties

        [Hidden]
        public override Uri DefaultLayout => Resources.Uri(nameof(Demo), Layouts.DefaultPath);

        [Hidden]
        public Favorites Favorites => explorerOptions.Favorites;

        #region Controls

        ExplorerOptions explorerOptions = new();
        [Category(Category.Explorer)]
        [DisplayName("Options")]
        public ExplorerOptions ExplorerOptions
        {
            get => explorerOptions;
            set => this.Change(ref explorerOptions, value);
        }

        #endregion

        #endregion

        #region Methods

        protected override IEnumerable<IWriter> GetData()
        {
            yield return ExplorerOptions.Favorites;
        }

        protected override void OnLoaded()
        {
            ExplorerOptions.Favorites = new Favorites($@"{ApplicationProperties.GetFolderPath(DataFolders.Documents)}\{nameof(Explorer)}", new Limit(250, Limit.Actions.RemoveFirst));
            base.OnLoaded();
        }

        #endregion
    }
}