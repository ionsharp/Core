using System;
using System.Reflection;

namespace Imagin.Common.Linq
{
    public static partial class XObject
    {
        /// <summary>
        /// Returns object as specified type.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static Type As<Type>(this object input) => input is Type ? (Type)input : default;

        /// <summary>
        /// Get the value of the given field.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetFieldValue(this object input, string fieldName) => input.GetType().GetField(fieldName).GetValue(input);

        /// <summary>
        /// Get the value of the given property.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        public static object GetPropertyValue(this object input, string propertyName) => input.GetType().GetProperty(propertyName).GetValue(input, null);

        /// <summary>
        /// Calls the given action if the object is not <see langword="null"/>.
        /// </summary>
        public static void If<T>(this object input, Action<T> action)
        {
            if (input is T i)
                action(i);
        }

        /// <summary>
        /// Calls the given action if the object is not <see langword="null"/>.
        /// </summary>
        public static void If<T>(this object input, Predicate<T> @if, Action<T> then)
        {
            if (input is T i)
            {
                if (@if(i))
                    then.Invoke(i);
            }
        }

        /// <summary>
        /// Calls the given action if the object is not <see langword="null"/>.
        /// </summary>
        public static Output IfThen<Input, Output>(this object input, Func<Input, Output> action)
        {
            if (input is Input i)
                return action(i);

            return default;
        }

        /// <summary>
        /// Checks if given object's type implements interface (T).
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="source"></param>
        /// <returns></returns>
        public static bool Implements<Type>(this object input) where Type : class => (bool)input.GetType()?.GetTypeInfo()?.Implements<Type>();

        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool Is<T>(this object input) => input is T;

        /// <summary>
        /// Gets if <see cref="object.GetType"/> specifies the <see cref="SerializableAttribute"/>.
        /// </summary>
        public static bool IsSerializable(this object input) => input.GetType().GetAttribute<SerializableAttribute>() != null;
        
        /// <summary>
        /// Checks if specified object is of specified type.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static bool IsAny(this object input, params Type[] types)
        {
            var t = input.GetType();
            foreach (var i in types)
            {
                if (t == i || t.GetTypeInfo().IsSubclassOf(i))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// Gets whether or not the <see cref="object"/> is <see cref="System.Nullable"/>.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool Nullable(this object input) => input.Nullable<object>();

        /// <summary>
        /// Gets "null" if the object is <see langword="null"/> or "not null" if the object isn't.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static string NullString(this object input, Func<object, string> result = null) => input == null ? "null" : result?.Invoke(input) ?? "not null";

        /// <summary>
        /// Set value of given field.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="input"></param>
        /// <param name="fieldName"></param>
        /// <param name="value"></param>
        public static void SetFieldValue<Type>(this object input, string fieldName, Type value)
        {
            var field = input.GetType().GetField(fieldName, BindingFlags.Public | BindingFlags.Instance);
            field.SetValue(input, value);
        }

        /// <summary>
        /// Set value of given property.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="input"></param>
        /// <param name="propertyName"></param>
        /// <param name="value"></param>
        public static void SetPropertyValue<Type>(this object input, string propertyName, Type value)
        {
            var property = input.GetType().GetProperty(propertyName, BindingFlags.Public | BindingFlags.Instance);
            if (property?.CanWrite == true)
                property.SetValue(input, value, null);
        }

        /// <summary>
        /// Cast object to given type.
        /// </summary>
        /// <typeparam name="Type"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Type To<Type>(this object input) => (Type)input;

        //...

        public static decimal Decimal(this object i) => Convert.ToDecimal(i);

        public static double Double(this object i) => Convert.ToDouble(i);

        public static short Int16(this object i) => Convert.ToInt16(i);

        public static int Int32(this object i) => Convert.ToInt32(i);

        public static long Int64(this object i) => Convert.ToInt64(i);

        public static float Single(this object i) => Convert.ToSingle(i);
    }
}