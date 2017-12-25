using Imagin.Common.Mvvm;
using System;
using System.Reflection;
using System.Reflection.Emit;
using System.Windows;
using System.Windows.Input;
using System.Windows.Markup;
using Imagin.Common.Linq;

namespace Imagin.Common.Data
{
    /// <summary>
    /// 
    /// </summary>
    public class EventBindingExtension : MarkupExtension
    {
        /// <summary>
        /// 
        /// </summary>
        public string Command
        {
            get; set;
        }

        /// <summary>
        /// 
        /// </summary>
        public string CommandParameter
        {
            get; set;
        }

        Type GetEventHandlerType(MemberInfo memberInfo)
        {
            Type eventHandlerType = null;
            if (memberInfo is EventInfo)
            {
                var info = memberInfo as EventInfo;
                var eventInfo = info;
                eventHandlerType = eventInfo.EventHandlerType;
            }
            else if (memberInfo is MethodInfo)
            {
                var info = memberInfo as MethodInfo;
                var methodInfo = info;
                ParameterInfo[] pars = methodInfo.GetParameters();
                eventHandlerType = pars[1].ParameterType;
            }

            return eventHandlerType;
        }

        object CreateHandler(MemberInfo memberInfo, string cmdName, Type targetType)
        {
            Type eventHandlerType = GetEventHandlerType(memberInfo);

            if (eventHandlerType == null) return null;

            var handlerInfo = eventHandlerType.GetMethod("Invoke");
            var method = new DynamicMethod("", handlerInfo.ReturnType,
                new Type[]
                {
                    handlerInfo.GetParameters()[0].ParameterType,
                    handlerInfo.GetParameters()[1].ParameterType,
                });

            var gen = method.GetILGenerator();
            gen.Emit(OpCodes.Ldarg, 0);
            gen.Emit(OpCodes.Ldarg, 1);
            gen.Emit(OpCodes.Ldstr, cmdName);
            if (CommandParameter == null)
            {
                gen.Emit(OpCodes.Ldnull);
            }
            else
            {
                gen.Emit(OpCodes.Ldstr, CommandParameter);
            }
            gen.Emit(OpCodes.Call, getMethod);
            gen.Emit(OpCodes.Ret);

            return method.CreateDelegate(eventHandlerType);
        }

        static readonly MethodInfo getMethod = typeof(EventBindingExtension).GetMethod("HandlerIntern", new Type[] { typeof(object), typeof(object), typeof(string), typeof(string) });

        static void Handler(object sender, object args)
        {
            HandlerIntern(sender, args, "cmd", null);
        }

        internal static ICommand GetCommand(FrameworkElement target, string cmdName)
        {
            var vm = FindViewModel(target);
            if (vm == null) return null;

            var vmType = vm.GetType();
            var cmdProp = vmType.GetProperty(cmdName);
            if (cmdProp != null)
            {
                return cmdProp.GetValue(vm) as ICommand;
            }
#if DEBUG
            throw new Exception("EventBinding path error: '" + cmdName + "' property not found on '" + vmType + "' 'DelegateCommand'");
#endif

            return null;
        }

        internal static object GetCommandParameter(FrameworkElement target, object args, string commandParameter)
        {
            object ret = null;
            var classify = commandParameter.Split('.');
            switch (classify[0])
            {
                case "$e":
                    ret = args;
                    break;
                case "$this":
                    ret = classify.Length > 1 ? FollowPropertyPath(target, commandParameter.Replace("$this.", ""), target.GetType()) : target;
                    break;
                default:
                    ret = commandParameter;
                    break;
            }

            return ret;
        }

        internal static ViewModelBase FindViewModel(FrameworkElement target)
        {
            if (target == null) return null;

            var vm = target.DataContext as ViewModelBase;
            if (vm != null) return vm;

            return FindViewModel(target.GetParent() as FrameworkElement);
        }

        internal static object FollowPropertyPath(object target, string path, Type valueType = null)
        {
            if (target == null) throw new ArgumentNullException("target null");
            if (path == null) throw new ArgumentNullException("path null");

            Type currentType = valueType ?? target.GetType();

            foreach (string propertyName in path.Split('.'))
            {
                PropertyInfo property = currentType.GetProperty(propertyName);
                if (property == null) throw new NullReferenceException("property null");

                target = property.GetValue(target);
                currentType = property.PropertyType;
            }
            return target;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        public override object ProvideValue(IServiceProvider serviceProvider)
        {
            var targetProvider = serviceProvider.GetService(typeof(IProvideValueTarget)) as IProvideValueTarget;
            if (targetProvider == null)
            {
                throw new InvalidOperationException();
            }

            var targetObject = targetProvider.TargetObject as FrameworkElement;
            if (targetObject == null)
            {
                throw new InvalidOperationException();
            }

            var memberInfo = targetProvider.TargetProperty as MemberInfo;
            if (memberInfo == null)
            {
                throw new InvalidOperationException();
            }

            if (string.IsNullOrWhiteSpace(Command))
            {
                Command = memberInfo.Name.Replace("Add", "");
                if (Command.Contains("Handler"))
                {
                    Command = Command.Replace("Handler", "Command");
                }
                else
                {
                    Command = Command + "Command";
                }
            }

            return CreateHandler(memberInfo, Command, targetObject.GetType());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="args"></param>
        /// <param name="cmdName"></param>
        /// <param name="commandParameter"></param>
        public static void HandlerIntern(object sender, object args, string cmdName, string commandParameter)
        {
            var fe = sender as FrameworkElement;
            if (fe != null)
            {
                ICommand cmd = GetCommand(fe, cmdName);
                object commandParam = null;
                if (!string.IsNullOrWhiteSpace(commandParameter))
                {
                    commandParam = GetCommandParameter(fe, args, commandParameter);
                }
                if ((cmd != null) && cmd.CanExecute(commandParam))
                {
                    cmd.Execute(commandParam);
                }
            }
        }
    }
}