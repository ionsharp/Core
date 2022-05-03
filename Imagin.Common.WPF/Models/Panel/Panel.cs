using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System;
using System.Windows.Controls;

namespace Imagin.Common.Models
{
    public abstract class Panel : Content
    {
        #region Delegates

        internal delegate void SizeRequestHandler(Panel sender, double length);

        #endregion

        #region Events

        internal event SizeRequestHandler HeightRequested;

        internal event SizeRequestHandler WidthRequested;

        #endregion

        #region Fields

        public const SecondaryDocks DefaultDockPreference = SecondaryDocks.Center;

        public const Orientation DefaultOrientationPreference = Orientation.Horizontal;

        #endregion

        #region Properties

        /// <summary>
        /// Gets whether or not the panel can be hidden.
        /// </summary>
        [Hidden]
        public virtual bool CanHide { get; } = true;

        /// <summary>
        /// Gets whether or not the panel can live with other panels.
        /// </summary>
        [Hidden]
        public virtual bool CanShare { get; } = true;

        /// <summary>
        /// Gets the direction in which the panel prefers to dock.
        /// </summary>
        [Hidden]
        public virtual SecondaryDocks DockPreference { get; } = DefaultDockPreference;

        [Hidden]
        public double Height { set => HeightRequested?.Invoke(this, value); }

        [Hidden]
        public abstract Uri Icon { get; }

        bool isOptionsVisible = false;
        /// <summary>
        /// To do: Move logic to <see cref="DockControl"/>. Can't until everything is refactored to accomadate view models.
        /// </summary>
        [Hidden, Serialize(false)]
        public bool IsOptionsVisible
        {
            get => isOptionsVisible;
            set => this.Change(ref isOptionsVisible, value);
        }

        bool isSelected = false;
        [Hidden, Serialize(false)]
        public bool IsSelected
        {
            get => isSelected;
            set => this.Change(ref isSelected, value);
        }

        bool isVisible = true;
        [Hidden, Serialize(false)]
        public bool IsVisible
        {
            get => isVisible;
            set => this.Change(ref isVisible, value);
        }

        [Hidden]
        public virtual double MaxHeight { get; }

        [Hidden]
        public virtual double MaxWidth { get; }

        [Hidden]
        public virtual double MinHeight { get; }

        [Hidden]
        public virtual double MinWidth { get; }

        [Hidden]
        public string Name => GetType().Name;

        ControlLength pinHeight = default;
        [Hidden, Serialize(false)]
        public ControlLength PinHeight
        {
            get => pinHeight;
            set => this.Change(ref pinHeight, value);
        }

        ControlLength pinWidth = default;
        [Hidden, Serialize(false)]
        public ControlLength PinWidth
        {
            get => pinWidth;
            set => this.Change(ref pinWidth, value);
        }

        [Hidden]
        public ObservableCollection<PanelButton> TitleButtons { get; private set; } = new ObservableCollection<PanelButton>();

        [Hidden]
        public virtual bool TitleLocalized => true;

        [Hidden]
        public virtual bool TitleVisibility => true;

        [Hidden]
        public virtual TopBottom ToolBarPlacement => TopBottom.Top;

        [Hidden]
        public double Width { set => WidthRequested?.Invoke(this, value); }

        #endregion

        #region Panel

        public Panel() : base() { }

        #endregion

        #region Methods

        public static T Find<T>() where T : Panel => Get.Where<IDockViewModel>().Panels.FirstOrDefault<T>();

        #endregion
    }
}