using System;

namespace Imagin.Core.Numerics;

/// <summary>Specifies a number operation.</summary>
[Serializable]
public enum NumberOperation
{
    /// <summary>Addition operation (+).</summary>
    Add,
    /// <summary>Division operation (/).</summary>
    Divide,
    /// <summary>Multiplication operation (*).</summary>
    Multiply,
    /// <summary>Subtraction operation (-).</summary>
    Subtract
}