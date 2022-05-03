using Imagin.Common.Models;
using System;
using System.ComponentModel;
using System.Windows;

namespace Imagin.Common.Controls
{
    public abstract class MainWindow : Window, IMainView
    {
        public event EventHandler<CancelEventArgs> ClosingFinal;

        public readonly IMainViewModel Model;

        public MainWindow() : base()
        {
            Model = Get.Where<IMainViewModel>();
            Model.View = this;
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            if (!e.Cancel)
                OnClosingFinal(e);
        }

        protected virtual void OnClosingFinal(CancelEventArgs e) => ClosingFinal?.Invoke(this, e);
    }
}