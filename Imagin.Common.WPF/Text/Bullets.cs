using System;

namespace Imagin.Common.Text
{
    [Serializable]
    public enum Bullets
    {
        [Abbreviation("■ 9632")]
        Square,
        [Abbreviation("□ 9633")]
        SquareOutline1,
        [Abbreviation("▣ 9635")]
        SquareOutline2,
        [Abbreviation("● 9679")]
        Circle,
        [Abbreviation("○ 9675")]
        CircleOutline1,
        [Abbreviation("◉ 9673")]
        CircleOutline2,
        [Abbreviation("▰ 9648")]
        Dash1,
        [Abbreviation("▱ 9649")]
        Dash2,
        [Abbreviation("◆ 9670")]
        Diamond,
        [Abbreviation("◇ 9671")]
        DiamondOutline1,
        [Abbreviation("◈ 9672")]
        DiamondOutline2,
        [Abbreviation("► 9658")]
        RightTriangle1,
        [Abbreviation("▶ 9654")]
        RightTriangle2,
        /// <summary>
        /// A., B., C.
        /// </summary>
        LetterUpperPeriod,
        /// <summary>
        /// A), B), C)
        /// </summary>
        LetterUpperParenthesis,
        /// <summary>
        /// a., b., c.
        /// </summary>
        LetterLowerPeriod,
        /// <summary>
        /// a), b), c)
        /// </summary>
        LetterLowerParenthesis,
        /// <summary>
        /// 1., 2., 3.
        /// </summary>
        NumberPeriod,
        /// <summary>
        /// 1), 2), 3)
        /// </summary>
        NumberParenthesis,
        /// <summary>
        /// I., II., III
        /// </summary>
        RomanNumberUpperPeriod,
        /// <summary>
        /// I), II), III)
        /// </summary>
        RomanNumberUpperParenthesis,
        /// <summary>
        /// i., ii., iii.
        /// </summary>
        RomanNumberLowerPeriod,
        /// <summary>
        /// i), ii), iii)
        /// </summary>
        RomanNumberLowerParenthesis
    }
}