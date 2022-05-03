using Imagin.Common.Linq;
using System;
using System.Collections;

namespace Imagin.Common.Controls
{
    public class EntryModel : MemberModel
    {
        public override bool CanWrite
            => true;

        public override Type DeclaringType
            => Source?.First.GetType();

        public override string DisplayName
            => Name.SplitCamel();

        public override Type Type
            => Value?.GetType();

        //...

        public EntryModel(MemberData data) : base(data) { }

        //...

        protected override void Apply(MemberAttributes input) { }

        protected override object GetValue(object input) => (input as IDictionary)[Name];

        protected override void SetValue(object input, object value) => (input as IDictionary)[Name] = value;

        //...

        public override void Subscribe() { }

        public override void Unsubscribe() { }
    }
}