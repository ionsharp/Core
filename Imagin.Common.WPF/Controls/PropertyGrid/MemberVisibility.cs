using System;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// Whether or not members must implicitly or explicitly specify visibility with <see cref="HiddenAttribute"/> or <see cref="System.ComponentModel.BrowsableAttribute"/>.
    /// </summary>
    public enum MemberVisibility
    {        
        /// <summary>
        /// The member is visible unless <see cref="HiddenAttribute"/> or <see cref="VisibleAttribute"/> is otherwise specified.
        /// </summary>
        Implicit = 0,
        /// <summary>
        /// The member is hidden unless <see cref="HiddenAttribute"/> or <see cref="VisibleAttribute"/> is otherwise specified.
        /// </summary>
        Explicit = 1
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MemberVisibilityAttribute : Attribute
    {
        public readonly MemberVisibility Field;

        public readonly MemberVisibility Property;

        public MemberVisibilityAttribute(MemberVisibility Field = MemberVisibility.Implicit, MemberVisibility Property = MemberVisibility.Implicit) : base()
        {
            this.Field
                = Field;
            this.Property
                = Property;
        }
    }
}