using System;
using System.Windows;
using Imagin.Common.Extensions;

namespace Imagin.Controls.Extended
{
    public class ResourceDictionaryEditor : PropertyGrid
    {
        #region ResourceDictionaryEditor

        public ResourceDictionaryEditor() : base()
        {
        }

        #endregion

        #region Methods

        protected override void SetObject()
        {
            this.Properties.BeginFromResourceDictionary(this.SelectedObject.As<ResourceDictionary>());
        }

        #endregion
    }
}
