using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using System;
using System.Collections.Generic;

namespace Imagin.Apps.Paint
{
    #region DefaultMatrices

    [Serializable]
    public class DefaultMatrices : GroupCollection<Matrix>
    {
        public static double[,] Mean3x3 => new double[,]
        {
            { 1, 1, 1, },
            { 1, 1, 1, },
            { 1, 1, 1, },
        };

        public static double[,] Mean5x5 => new double[,]
        {
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1},
        };

        public static double[,] Mean7x7 => new double[,]
        {
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1},
        };

        public static double[,] Mean9x9 => new double[,]
        {
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
            { 1, 1, 1, 1, 1, 1, 1, 1, 1},
        };

        public static double[,] Gaussian3x3 => new double[,]
        {
            { 1, 2, 1, },
            { 2, 4, 2, },
            { 1, 2, 1, },
        };

        public static double[,] Gaussian5x5 => new double[,]
        {
            { 2, 04, 05, 04, 2 },
            { 4, 09, 12, 09, 4 },
            { 5, 12, 15, 12, 5 },
            { 4, 09, 12, 09, 4 },
            { 2, 04, 05, 04, 2 },
        };

        public static double[,] MotionBlur5x5 => new double[,]
        {
            { 1, 0, 0, 0, 1},
            { 0, 1, 0, 1, 0},
            { 0, 0, 1, 0, 0},
            { 0, 1, 0, 1, 0},
            { 1, 0, 0, 0, 1},
        };

        public static double[,] MotionBlur5x5At45Degrees => new double[,]
        {
            { 0, 0, 0, 0, 1},
            { 0, 0, 0, 1, 0},
            { 0, 0, 1, 0, 0},
            { 0, 1, 0, 0, 0},
            { 1, 0, 0, 0, 0},
        };

        public static double[,] MotionBlur5x5At135Degrees => new double[,]
        {
            { 1, 0, 0, 0, 0},
            { 0, 1, 0, 0, 0},
            { 0, 0, 1, 0, 0},
            { 0, 0, 0, 1, 0},
            { 0, 0, 0, 0, 1},
        };

        public static double[,] MotionBlur7x7 => new double[,]
        {
            { 1, 0, 0, 0, 0, 0, 1},
            { 0, 1, 0, 0, 0, 1, 0},
            { 0, 0, 1, 0, 1, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0},
            { 0, 0, 1, 0, 1, 0, 0},
            { 0, 1, 0, 0, 0, 1, 0},
            { 1, 0, 0, 0, 0, 0, 1},
        };

        public static double[,] MotionBlur7x7At45Degrees => new double[,]
        {
            { 0, 0, 0, 0, 0, 0, 1},
            { 0, 0, 0, 0, 0, 1, 0},
            { 0, 0, 0, 0, 1, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0},
            { 0, 0, 1, 0, 0, 0, 0},
            { 0, 1, 0, 0, 0, 0, 0},
            { 1, 0, 0, 0, 0, 0, 0},
        };

        public static double[,] MotionBlur7x7At135Degrees => new double[,]
        {
            { 1, 0, 0, 0, 0, 0, 0},
            { 0, 1, 0, 0, 0, 0, 0},
            { 0, 0, 1, 0, 0, 0, 0},
            { 0, 0, 0, 1, 0, 0, 0},
            { 0, 0, 0, 0, 1, 0, 0},
            { 0, 0, 0, 0, 0, 1, 0},
            { 0, 0, 0, 0, 0, 0, 1},
        };

        public static double[,] MotionBlur9x9 => new double[,]
        {
            {1, 0, 0, 0, 0, 0, 0, 0, 1,},
            {0, 1, 0, 0, 0, 0, 0, 1, 0,},
            {0, 0, 1, 0, 0, 0, 1, 0, 0,},
            {0, 0, 0, 1, 0, 1, 0, 0, 0,},
            {0, 0, 0, 0, 1, 0, 0, 0, 0,},
            {0, 0, 0, 1, 0, 1, 0, 0, 0,},
            {0, 0, 1, 0, 0, 0, 1, 0, 0,},
            {0, 1, 0, 0, 0, 0, 0, 1, 0,},
            {1, 0, 0, 0, 0, 0, 0, 0, 1,},
        };

        public static double[,] MotionBlur9x9At45Degrees => new double[,]
        {
            {0, 0, 0, 0, 0, 0, 0, 0, 1,},
            {0, 0, 0, 0, 0, 0, 0, 1, 0,},
            {0, 0, 0, 0, 0, 0, 1, 0, 0,},
            {0, 0, 0, 0, 0, 1, 0, 0, 0,},
            {0, 0, 0, 0, 1, 0, 0, 0, 0,},
            {0, 0, 0, 1, 0, 0, 0, 0, 0,},
            {0, 0, 1, 0, 0, 0, 0, 0, 0,},
            {0, 1, 0, 0, 0, 0, 0, 0, 0,},
            {1, 0, 0, 0, 0, 0, 0, 0, 0,},
        };

        public static double[,] MotionBlur9x9At135Degrees => new double[,]
        {
            {1, 0, 0, 0, 0, 0, 0, 0, 0,},
            {0, 1, 0, 0, 0, 0, 0, 0, 0,},
            {0, 0, 1, 0, 0, 0, 0, 0, 0,},
            {0, 0, 0, 1, 0, 0, 0, 0, 0,},
            {0, 0, 0, 0, 1, 0, 0, 0, 0,},
            {0, 0, 0, 0, 0, 1, 0, 0, 0,},
            {0, 0, 0, 0, 0, 0, 1, 0, 0,},
            {0, 0, 0, 0, 0, 0, 0, 1, 0,},
            {0, 0, 0, 0, 0, 0, 0, 0, 1,},
        };

        public DefaultMatrices() : base("Default")
        {
            Add(new(nameof(Mean3x3), 
                new (Mean3x3)));
            Add(new(nameof(Mean5x5),
                new(Mean5x5)));
            Add(new(nameof(Mean7x7),
                new(Mean7x7)));
            Add(new(nameof(Mean9x9),
                new(Mean9x9)));
            Add(new(nameof(Gaussian3x3),
                new(Gaussian3x3)));
            Add(new(nameof(Gaussian5x5),
                new(Gaussian5x5)));
            Add(new(nameof(MotionBlur5x5),
                new(MotionBlur5x5)));
            Add(new(nameof(MotionBlur5x5At45Degrees),
                new(MotionBlur5x5At45Degrees)));
            Add(new(nameof(MotionBlur5x5At135Degrees),
                new(MotionBlur5x5At135Degrees)));
            Add(new(nameof(MotionBlur7x7),
                new(MotionBlur7x7)));
            Add(new(nameof(MotionBlur7x7At45Degrees),
                new(MotionBlur7x7At45Degrees)));
            Add(new(nameof(MotionBlur7x7At135Degrees),
                new(MotionBlur7x7At135Degrees)));
            Add(new(nameof(MotionBlur9x9),
                new(MotionBlur9x9)));
            Add(new(nameof(MotionBlur9x9At45Degrees),
                new(MotionBlur9x9At45Degrees)));
            Add(new(nameof(MotionBlur9x9At135Degrees),
                new(MotionBlur9x9At135Degrees)));
        }
    }

    #endregion

    #region MatricesPanel

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class MatricesPanel : LocalGroupPanel<Matrix>
    {
        public override Uri Icon => Resources.ProjectImage("Matrix.png");

        public override string Title => "Matrices";

        public MatricesPanel() : base(Get.Current<Options>().Matrices) { }

        protected override IEnumerable<GroupCollection<Matrix>> GetDefault() 
            { yield return new DefaultMatrices(); }
    }

    #endregion
}