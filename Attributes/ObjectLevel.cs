using System;

namespace Imagin.Core
{
    public enum ObjectLevel
    {
        Low, High
    }

    public enum ObjectLayout
    {
        Horizontal, Vertical
    }

    public class ObjectAttribute : Attribute
    {
        public readonly ObjectLevel Level;

        public readonly ObjectLayout Orientation;

        public ObjectAttribute(ObjectLayout orientation) : this(ObjectLevel.High, orientation) { }

        public ObjectAttribute(ObjectLevel level = ObjectLevel.High, ObjectLayout orientation = ObjectLayout.Vertical)
        {
            Level
                = level;
            Orientation
                = orientation;
        }
    }
}