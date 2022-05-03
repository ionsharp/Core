using Imagin.Common.Input;
using Imagin.Common.Linq;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;

namespace Imagin.Common.Controls
{
    public class DirectionControl : Control
    {
        #region (class) DirectionModel

        public class DirectionModel : BaseNamable
        {
            readonly public int DefaultColumn;

            readonly public int DefaultRow;

            readonly CardinalDirection _direction = CardinalDirection.Unknown;
            public CardinalDirection Direction => _direction;

            int _column = 0;
            public int Column
            {
                get => _column;
                set => this.Change(ref _column, value);
            }

            ImageSource _icon = default;
            public ImageSource Icon
            {
                get => _icon;
                set => this.Change(ref _icon, value);
            }

            int _row = 0;
            public int Row
            {
                get => _row;
                set => this.Change(ref _row, value);
            }

            internal DirectionModel(int column, int row, int direction) : base()
            {
                DefaultColumn = Column = column;
                DefaultRow = Row = row;
                _direction = (CardinalDirection)direction;
            }
        }

        #endregion

        #region Properties

        static readonly DependencyPropertyKey DirectionsKey = DependencyProperty.RegisterReadOnly(nameof(Directions), typeof(ObservableCollection<DirectionModel>), typeof(DirectionControl), new FrameworkPropertyMetadata(null));
        public static readonly DependencyProperty DirectionsProperty = DirectionsKey.DependencyProperty;
        public ObservableCollection<DirectionModel> Directions
        {
            get => (ObservableCollection<DirectionModel>)GetValue(DirectionsProperty);
            private set => SetValue(DirectionsKey, value);
        }
        
        public static readonly DependencyProperty DirectionProperty = DependencyProperty.Register(nameof(Direction), typeof(CardinalDirection), typeof(DirectionControl), new FrameworkPropertyMetadata(CardinalDirection.Origin, OnDirectionChanged));
        public CardinalDirection Direction
        {
            get => (CardinalDirection)GetValue(DirectionProperty);
            set => SetValue(DirectionProperty, value);
        }
        static void OnDirectionChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionControl).OnDirectionChanged(new Value<CardinalDirection>(e));

        public static readonly DependencyProperty ELabelProperty = DependencyProperty.Register(nameof(ELabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string ELabel
        {
            get => (string)GetValue(ELabelProperty);
            set => SetValue(ELabelProperty, value);
        }

        public static readonly DependencyProperty EIconProperty = DependencyProperty.Register(nameof(EIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource EIcon
        {
            get => (ImageSource)GetValue(EIconProperty);
            set => SetValue(EIconProperty, value);
        }

        public static readonly DependencyProperty NLabelProperty = DependencyProperty.Register(nameof(NLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NLabel
        {
            get => (string)GetValue(NLabelProperty);
            set => SetValue(NLabelProperty, value);
        }

        public static readonly DependencyProperty NIconProperty = DependencyProperty.Register(nameof(NIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource NIcon
        {
            get => (ImageSource)GetValue(NIconProperty);
            set => SetValue(NIconProperty, value);
        }

        public static readonly DependencyProperty NELabelProperty = DependencyProperty.Register(nameof(NELabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NELabel
        {
            get => (string)GetValue(NELabelProperty);
            set => SetValue(NELabelProperty, value);
        }

        public static readonly DependencyProperty NEIconProperty = DependencyProperty.Register(nameof(NEIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource NEIcon
        {
            get => (ImageSource)GetValue(NEIconProperty);
            set => SetValue(NEIconProperty, value);
        }

        public static readonly DependencyProperty NWLabelProperty = DependencyProperty.Register(nameof(NWLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string NWLabel
        {
            get => (string)GetValue(NWLabelProperty);
            set => SetValue(NWLabelProperty, value);
        }

        public static readonly DependencyProperty NWIconProperty = DependencyProperty.Register(nameof(NWIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource NWIcon
        {
            get => (ImageSource)GetValue(NWIconProperty);
            set => SetValue(NWIconProperty, value);
        }

        public static readonly DependencyProperty OriginLabelProperty = DependencyProperty.Register(nameof(OriginLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string OriginLabel
        {
            get => (string)GetValue(OriginLabelProperty);
            set => SetValue(OriginLabelProperty, value);
        }

        public static readonly DependencyProperty OriginIconProperty = DependencyProperty.Register(nameof(OriginIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource OriginIcon
        {
            get => (ImageSource)GetValue(OriginIconProperty);
            set => SetValue(OriginIconProperty, value);
        }

        public static readonly DependencyProperty SLabelProperty = DependencyProperty.Register(nameof(SLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SLabel
        {
            get => (string)GetValue(SLabelProperty);
            set => SetValue(SLabelProperty, value);
        }

        public static readonly DependencyProperty SIconProperty = DependencyProperty.Register(nameof(SIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource SIcon
        {
            get => (ImageSource)GetValue(SIconProperty);
            set => SetValue(SIconProperty, value);
        }

        public static readonly DependencyProperty SELabelProperty = DependencyProperty.Register(nameof(SELabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SELabel
        {
            get => (string)GetValue(SELabelProperty);
            set => SetValue(SELabelProperty, value);
        }

        public static readonly DependencyProperty SEIconProperty = DependencyProperty.Register(nameof(SEIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource SEIcon
        {
            get => (ImageSource)GetValue(SEIconProperty);
            set => SetValue(SEIconProperty, value);
        }

        public static readonly DependencyProperty SWLabelProperty = DependencyProperty.Register(nameof(SWLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string SWLabel
        {
            get => (string)GetValue(SWLabelProperty);
            set => SetValue(SWLabelProperty, value);
        }

        public static readonly DependencyProperty SWIconProperty = DependencyProperty.Register(nameof(SWIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource SWIcon
        {
            get => (ImageSource)GetValue(SWIconProperty);
            set => SetValue(SWIconProperty, value);
        }

        public static readonly DependencyProperty WLabelProperty = DependencyProperty.Register(nameof(WLabel), typeof(string), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnLabelChanged));
        public string WLabel
        {
            get => (string)GetValue(WLabelProperty);
            set => SetValue(WLabelProperty, value);
        }

        public static readonly DependencyProperty WIconProperty = DependencyProperty.Register(nameof(WIcon), typeof(ImageSource), typeof(DirectionControl), new FrameworkPropertyMetadata(default(string), OnIconChanged));
        public ImageSource WIcon
        {
            get => (ImageSource)GetValue(WIconProperty);
            set => SetValue(WIconProperty, value);
        }

        static void OnLabelChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionControl).OnLabelChanged();

        static void OnIconChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as DirectionControl).OnIconChanged();

        #endregion

        #region DirectionControl

        /// <summary>
        /// Initializes an instance of <see cref="DirectionControl"/>.
        /// </summary>
        public DirectionControl()
        {
            Directions = new()
            {
                new DirectionModel(1, 1, 0),
                new DirectionModel(2, 1, 1),
                new DirectionModel(3, 1, 2),
                new DirectionModel(1, 2, 3),
                new DirectionModel(2, 2, 4),
                new DirectionModel(3, 2, 5),
                new DirectionModel(1, 3, 6),
                new DirectionModel(2, 3, 7),
                new DirectionModel(3, 3, 8)
            };
        }

        #endregion

        #region Methods

        /// <summary>
        /// Shift all relative to the specified <see cref="CardinalDirection"/>.
        /// </summary>
        /// <param name="direction"></param>
        void Shift(CardinalDirection direction)
        {
            var _direction = (int)direction;

            var shift = new int[,]
            {
                {-1, -1},
                {-1,  0},
                {-1,  1},
                { 0, -1},
                { 0,  0},
                { 0,  1},
                { 1, -1},
                { 1,  0},
                { 1,  1}
            };

            int x = shift[_direction, 1], y = shift[_direction, 0];

            if (x != 0 || y != 0)
            {
                foreach (var d in Directions)
                {
                    d.Column += x;
                    d.Row += y;
                }
            }
            else OnDirectionChanged(new Value<CardinalDirection>(CardinalDirection.Origin));
        }

        /// <summary>
        /// Set <see cref="Direction"/> relative to the specified <see cref="DirectionModel"/>.
        /// </summary>
        /// <param name="button"></param>
        void Set(DirectionModel button)
        {
            var origin = button.Direction == CardinalDirection.Origin ? button : Directions.First(x => x.Direction == CardinalDirection.Origin);
            var result = (CardinalDirection)new int[,]
            {
                { 0, 1, 2 },
                { 3, 4, 5 },
                { 6, 7, 8 }
            }
            [origin.Row - 1, origin.Column - 1];
            SetCurrentValue(DirectionProperty, result);
        }

        /// <summary>
        /// Occurs when <see cref="Direction"/> changes.
        /// </summary>
        /// <param name="value"></param>
        protected virtual void OnDirectionChanged(Value<CardinalDirection> input)
        {
            var positions = new int[,]
            {
                {0, 0},
                {0, 1},
                {0, 2},
                {1, 0},
                {1, 1},
                {1, 2},
                {2, 0},
                {2, 1},
                {2, 2}
            };

            int srow = positions[(int)input.New, 0],
                scolumn = positions[(int)input.New, 1];

            int y = srow, x = scolumn;

            foreach (var d in Directions)
            {
                if (x < scolumn + 3)
                {
                    d.Row = y;
                    d.Column = x++;
                    if (x == (scolumn + 3))
                    {
                        x = scolumn;
                        y++;
                    }
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWLabel"/>, <see cref="NLabel"/>, <see cref="NELabel"/>, <see cref="WLabel"/>, <see cref="OriginLabel"/>, <see cref="ELabel"/>, <see cref="SWLabel"/>, <see cref="SLabel"/>, or <see cref="SELabel"/> changes.
        /// </summary>
        protected virtual void OnLabelChanged()
        {
            foreach (var i in Directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Name = ELabel;
                        break;
                    case CardinalDirection.N:
                        i.Name = NLabel;
                        break;
                    case CardinalDirection.NE:
                        i.Name = NELabel;
                        break;
                    case CardinalDirection.NW:
                        i.Name = NWLabel;
                        break;
                    case CardinalDirection.Origin:
                        i.Name = OriginLabel;
                        break;
                    case CardinalDirection.S:
                        i.Name = SLabel;
                        break;
                    case CardinalDirection.SE:
                        i.Name = SELabel;
                        break;
                    case CardinalDirection.SW:
                        i.Name = SWLabel;
                        break;
                    case CardinalDirection.W:
                        i.Name = WLabel;
                        break;
                }
            }
        }

        /// <summary>
        /// Occurs when <see cref="NWIcon"/>, <see cref="NIcon"/>, <see cref="NEIcon"/>, <see cref="WIcon"/>, <see cref="OriginIcon"/>, <see cref="EIcon"/>, <see cref="SWIcon"/>, <see cref="SIcon"/>, or <see cref="SEIcon"/> changes.
        /// </summary>
        protected virtual void OnIconChanged()
        {
            foreach (var i in Directions)
            {
                switch (i.Direction)
                {
                    case CardinalDirection.E:
                        i.Icon = EIcon;
                        break;
                    case CardinalDirection.N:
                        i.Icon = NIcon;
                        break;
                    case CardinalDirection.NE:
                        i.Icon = NEIcon;
                        break;
                    case CardinalDirection.NW:
                        i.Icon = NWIcon;
                        break;
                    case CardinalDirection.Origin:
                        i.Icon = OriginIcon;
                        break;
                    case CardinalDirection.S:
                        i.Icon = SIcon;
                        break;
                    case CardinalDirection.SE:
                        i.Icon = SEIcon;
                        break;
                    case CardinalDirection.SW:
                        i.Icon = SWIcon;
                        break;
                    case CardinalDirection.W:
                        i.Icon = WIcon;
                        break;
                }
            }
        }

        ICommand shiftCommand;
        public ICommand ShiftCommand => shiftCommand ??= new RelayCommand<DirectionModel>(i =>
        {
            Shift(i.Direction);
            Set(i);
        },
        i => i is DirectionModel);

        #endregion
    }
}