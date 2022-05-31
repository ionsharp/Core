using System;

namespace Imagin.Core.Local;

[Serializable]
public enum Language
{
    [Culture("en")]
    English,
    [Culture("fr-FR")]
    French,
    [Culture("it-IT")]
    Italian,
    [Culture("ja-JP")]
    Japanese
}