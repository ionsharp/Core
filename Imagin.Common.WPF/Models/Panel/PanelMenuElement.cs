using Imagin.Common.Collections.Generic;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;

namespace Imagin.Common.Models
{
    public abstract class PanelMenuElement : Base
    {
        public PanelMenuElement() : base() { }

        static MenuItem Construct(PanelMenuCommand input)
        {
            var result = new MenuItem()
            {
                Command 
                    = input.Command,
                CommandParameter 
                    = input.CommandParameter,
                Header 
                    = input.Name,
                Icon 
                    = input.Icon
            };

            if (input.DefaultVisibility)
                result.Bind(MenuItem.VisibilityProperty, nameof(MenuItem.IsEnabled), result, BindingMode.OneWay, Imagin.Common.Converters.BooleanToVisibilityConverter.Default);

            return result;
        }

        static MenuItem Construct(PanelMenuEnumField input)
        {
            return new MenuItem()
            {
                Header = input.Name,
                IsCheckable = true
            };
        }

        public static CompositeCollection Construct(IEnumerable<PanelMenuElement> input)
        {
            var result = new CompositeCollection();
            foreach (var i in input)
            {
                if (i is PanelMenuCommand menuCommand)
                    result.Add(Construct(menuCommand));

                if (i is PanelMenuEnum menuEnum)
                {
                    foreach (Enum field in menuEnum.Type.GetEnumValues())
                        result.Add(Construct(new PanelMenuEnumField($"{field}")));
                }

                if (i is PanelMenuLine menuSeparator)
                {
                    var separator = new Separator();
                    XSeparator.SetHeader(separator, menuSeparator.Name);
                    result.Add(separator);
                }
            }
            return result;
        }
    }

    public abstract class NamablePanelMenuElement : PanelMenuElement
    {
        string name = default;
        public string Name
        {
            get => name;
            set => this.Change(ref name, value);
        }

        public NamablePanelMenuElement(string name) : base()
        {
            Name = name;
        }
    }

    //...

    public class PanelMenuCommand : NamablePanelMenuElement
    {
        Collection<PanelMenuElement> children = new Collection<PanelMenuElement>();
        public Collection<PanelMenuElement> Children
        {
            get => children;
            set => this.Change(ref children, value);
        }

        ICommand command = default;
        public ICommand Command
        {
            get => command;
            set => this.Change(ref command, value);
        }

        object commandParameter = default;
        public object CommandParameter
        {
            get => commandParameter;
            set => this.Change(ref commandParameter, value);
        }

        public bool DefaultVisibility { get; set; }

        Uri icon = default;
        public Uri Icon
        {
            get => icon;
            set => this.Change(ref icon, value);
        }

        public PanelMenuCommand(string name, ICommand command, Uri icon = null, params PanelMenuElement[] children) : base(name)
        {
            Command = command;
            Icon = icon;

            if (children?.Length > 0)
                children.ForEach(i => Children.Add(i));
        }

        public PanelMenuCommand(string name, ICommand command, object commandParameter, bool defaultVisibility, Uri icon = null) : this(name, command, commandParameter, icon)
        {
            DefaultVisibility = defaultVisibility;
        }

        public PanelMenuCommand(string name, ICommand command, object commandParameter, Uri icon = null, params PanelMenuElement[] children) : this(name, command, icon, children)
        {
            CommandParameter = commandParameter;
        }
    }

    public class PanelMenuEnum : PanelMenuElement
    {
        public readonly Type Type = default;

        public PanelMenuEnum(Type type) : base() => Type = type;
    }

    public class PanelMenuEnumField : NamablePanelMenuElement
    {
        public PanelMenuEnumField(string name) : base(name) { }
    }

    public class PanelMenuLine : NamablePanelMenuElement
    {
        public PanelMenuLine(string name = "") : base(name) { }
    }
}