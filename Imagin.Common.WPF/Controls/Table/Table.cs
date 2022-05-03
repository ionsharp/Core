using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using System.Xml;

namespace Imagin.Common.Controls
{
    public class Table : DataGrid
    {        
        bool handleSelectedCell;

        //...

        public static readonly DependencyProperty CellMouseEventProperty = DependencyProperty.Register(nameof(CellMouseEvent), typeof(MouseEvent), typeof(Table), new FrameworkPropertyMetadata(MouseEvent.DelayedMouseDown));
        public MouseEvent CellMouseEvent
        {
            get => (MouseEvent)GetValue(CellMouseEventProperty);
            set => SetValue(CellMouseEventProperty, value);
        }

        public static readonly DependencyProperty SelectedCellProperty = DependencyProperty.Register(nameof(SelectedCell), typeof(Cell), typeof(Table), new FrameworkPropertyMetadata(null, OnSelectedCellChanged));
        public Cell SelectedCell
        {
            get => (Cell)GetValue(SelectedCellProperty);
            set => SetValue(SelectedCellProperty, value);
        }
        static void OnSelectedCellChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Table table)
            {
                if (!table.handleSelectedCell)
                {
                    table.handleSelectedCell = true;
                    //Selected the actual cell
                    table.handleSelectedCell = false;
                }
            }
        }

        public static readonly DependencyProperty TextColumnsProperty = DependencyProperty.Register(nameof(TextColumns), typeof(string), typeof(Table), new FrameworkPropertyMetadata(string.Empty, OnTextColumnsChanged));
        public string TextColumns
        {
            get => (string)GetValue(TextColumnsProperty);
            set => SetValue(TextColumnsProperty, value);
        }
        static void OnTextColumnsChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (sender is Table table)
            {
                table.Columns.Clear();
                if (e.NewValue is string columns)
                {
                    var actualColumns = ParseColumns(columns);

                    var j = 0;
                    foreach (var i in actualColumns)
                    {
                        table.Columns.Add(new DataGridTemplateColumn()
                        {
                            Header = i,
                            CellTemplate = Create(i),
                            Width = new DataGridLength(1, DataGridLengthUnitType.Star)
                        });
                        j++;
                    }
                }
            }
        }

        //...

        public Table() : base()
        {
            TableData data = new();
            SetCurrentValue(ItemsSourceProperty, data);
            SetCurrentValue(TextColumnsProperty, data.Columns);

            InputBindings.Add(new KeyBinding()
            {
                Command = ClearCellCommand,
                CommandParameter = this,
                Key = Key.Delete
            });

            this.RegisterHandler(i =>
            {
                SelectedCellsChanged
                    += OnSelectedCellsChanged;
            }, i =>
            {
                SelectedCellsChanged
                    -= OnSelectedCellsChanged;
            });
        }

        //...

        //Background=""{Binding Value.Fill.Brush, Source={StaticResource Cell}}""
        //Foreground=""{Binding Value.FontColor.Brush, Source={StaticResource Cell}}""
        static string CellTemplate(string column) =>
        @"<DataTemplate 
            xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""
            xmlns:x=""http://schemas.microsoft.com/winfx/2006/xaml""
            xmlns:local=""clr-namespace:Imagin.Common.Controls;assembly=" + InternalAssembly.Name + @"""
            xmlns:Converters=""clr-namespace:Imagin.Common.Converters;assembly=" + InternalAssembly.Name + @"""
            xmlns:Data=""clr-namespace:Imagin.Common.Data;assembly=" + InternalAssembly.Name + @"""
            xmlns:Linq=""clr-namespace:Imagin.Common.Linq;assembly=" + InternalAssembly.Name + @"""
            xmlns:Markup=""clr-namespace:Imagin.Common.Markup;assembly=" + InternalAssembly.Name + @""">
            <Grid>
                <Grid.Resources>
                    <ResourceDictionary>
                        <local:CellReference x:Key=""Cell"" Source=""{Binding Cells}"" Column=""" + column + @""" />
                    </ResourceDictionary>
                </Grid.Resources>
                <TextBox
                    BorderThickness=""0""
                    FontFamily=""{Binding Cell.FontFamily, Converter={x:Static Converters:FontFamilyConverter.Default}, Source={StaticResource Cell}}""
                    FontSize=""{Binding Cell.FontSize, Source={StaticResource Cell}}""
                    HorizontalAlignment=""Stretch""
                    HorizontalContentAlignment=""{Binding Cell.Alignment.Horizontal, Source={StaticResource Cell}}""
                    Linq:XElement.Cursor=""{Markup:InternalUri Images/CrossOutline.png}""
                    Linq:XTextBox.CanLabel=""True""
                    Linq:XTextBox.EditMouseEvent=""{Data:Ancestor CellMouseEvent, {x:Type local:Table}}""                                
                    Linq:XTextBoxBase.TextTrimming=""{Binding Cell.Trim, Source={StaticResource Cell}}""
                    Padding =""0""
                    Text=""{Binding Cell.Text, Mode=TwoWay, Source={StaticResource Cell}, UpdateSourceTrigger=PropertyChanged}""
                    TextWrapping=""{Binding Cell.Wrap, Source={StaticResource Cell}}""
                    VerticalAlignment=""Stretch""
                    VerticalContentAlignment=""{Binding Cell.Alignment.Vertical, Source={StaticResource Cell}}"">
                    <Linq:XTextBox.Label>
                        <MultiBinding Converter=""{x:Static local:CellConverter.Default}"">
                            <Binding Path=""Cell.Text"" Source=""{StaticResource Cell}""/>
                            <Binding Path=""Cell.Format"" Source=""{StaticResource Cell}""/>
                            <Binding Path=""Cell.FormatText"" Source=""{StaticResource Cell}""/>
                        </MultiBinding>
                    </Linq:XTextBox.Label>
                </TextBox>
            </Grid>
        </DataTemplate>";

        static DataTemplate Create(string column)
        {
            StringReader stringReader = new(CellTemplate(column));
            XmlReader xmlReader = XmlReader.Create(stringReader);
            return XamlReader.Load(xmlReader) as DataTemplate;
        }

        //...

        void OnSelectedCellsChanged(object sender, SelectedCellsChangedEventArgs e)
        {
            if (!handleSelectedCell)
            {
                handleSelectedCell = true;
                Try.Invoke(() =>
                {
                    var dataGridCellInfo = e.AddedCells.Count > 0 ? e.AddedCells.First() : default;
                    var column = $"{dataGridCellInfo.Column.Header}";

                    var row = dataGridCellInfo.Item as Row;
                    SetCurrentValue(SelectedCellProperty, row?.Cells[column]);
                });
                handleSelectedCell = false;
            }
        }

        ICommand clearCellCommand;
        public ICommand ClearCellCommand => clearCellCommand ??= new RelayCommand<DataGrid>(i =>
        {
            foreach (var j in i.SelectedCells)
            {
                if (j.Item is Row k)
                    k.Cells[$"{j.Column.Header}"].Text = string.Empty;
            }
        });

        //...

        public static string[] ParseColumns(string input)
        {
            string[] result = null;
            Try.Invoke(() => result = input.Split(Array<char>.New(';'), StringSplitOptions.RemoveEmptyEntries));
            return result ?? new string[0];
        }
    }
}