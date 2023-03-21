using System;

namespace Imagin.Core.Text;

[Serializable]
public enum Bullets
{
    [Abbreviation("■")]
    Square,
    [Abbreviation("□")]
    SquareOutline,
    [Abbreviation("●")]
    Circle,
    [Abbreviation("○")]
    CircleOutline,
    [Abbreviation("◆")]
    Diamond,
    [Abbreviation("◇")]
    DiamondOutline,
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