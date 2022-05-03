using Imagin.Common;
using Imagin.Common.Numbers;
using System;

namespace Imagin.Apps.Paint
{
    [DisplayName("Matrix")]
    [Icon(App.ImagePath + "Matrix.png")]
    [Serializable]
    public class Matrix : Namable<DoubleMatrix>
    {
        public Matrix() : base() { }

        public Matrix(string name, DoubleMatrix value) : base(name, value) { }
    }
}