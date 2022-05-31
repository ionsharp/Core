using Imagin.Core.Linq;
using Imagin.Core.Serialization;
using Imagin.Core.Storage;
using System;
using System.Collections;

namespace Imagin.Core.Collections
{
    [Serializable]
    public struct Limit : IEquatable<Limit>
    {
        [Serializable]
        public enum Actions
        {
            None,
            Clear,
            ClearAndArchive,
            RemoveFirst,
            RemoveLast,
        }

        public readonly Actions Action;

        public readonly int Value;

        //...

        public Limit(int value, Actions action = Actions.RemoveFirst)
        {
            Value = value;
            Action = action;
        }

        //...

        public static implicit operator Limit(int i) => new Limit(i);

        public static implicit operator int(Limit i) => i.Value;

        //...

        public bool Coerce(IList input)
        {
            if (input.Count > Value)
            {
                //Always >= 1
                var blocks = new string[(input.Count.Double() / Value.Double()).Floor().Int32()];
                switch (Action)
                {
                    case Actions.Clear:
                        for (var i = 0; i < (blocks.Length * Value); i++)
                            input.RemoveAt(0);

                        break;

                    case Actions.ClearAndArchive:

                        if (input is ISerialize serializer)
                        {
                            if (Value > 0)
                            {
                                for (var i = 0; i < blocks.Length; i++)
                                {
                                    var items = new object[Value];
                                    for (var j = 0; j < items.Length; j++)
                                    {
                                        items[j] = items[0];
                                        input.RemoveAt(0);
                                    }

                                    blocks[i] = StoragePath.Clone(serializer.FilePath, StoragePath.DefaultCloneFormat, j => System.IO.File.Exists(j));
                                    serializer?.Serialize(blocks[i], items);
                                }
                            }
                        }
                        else goto case Actions.Clear;
                        break;

                    case Actions.RemoveFirst:
                        for (var i = 0; i < (blocks.Length * Value); i++)
                            input.RemoveAt(0);

                        break;

                    case Actions.RemoveLast:
                        for (var i = 0; i < (blocks.Length * Value); i++)
                            input.RemoveAt(input.Count - 1);

                        break;
                }
                return true;
            }
            return false;
        }

        //...

        public override string ToString() => $"{Value}";

        //...

        public static bool operator ==(Limit left, Limit right) => left.EqualsOverload(right);

        public static bool operator !=(Limit left, Limit right) => !(left == right);

        public bool Equals(Limit i) => this.Equals<Limit>(i) && Action.Equals(i.Action) && Value.Equals(i.Value);

        public override bool Equals(object i) => Equals((Limit)i);

        public override int GetHashCode() => new { Action, Value }.GetHashCode();
    }
}