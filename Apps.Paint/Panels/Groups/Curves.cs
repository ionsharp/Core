using Imagin.Common;
using Imagin.Common.Collections.Generic;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using System;
using System.Collections.Generic;
using System.Reflection;

namespace Imagin.Apps.Paint
{
    #region DefaultCurves

    [Serializable]
    public class DefaultCurves : GroupCollection<Curve>
    {
        public DefaultCurves() : base("Default")
        {
            Assembly.GetEntryAssembly().GetDerivedTypes(typeof(Curve), App.DefaultName, true, true).ForEach(i =>
            {
                if (i != typeof(Curve))
                    Add(i.Create<Curve>());
            });
        }
    }

    #endregion

    #region CurvesPanel

    [MemberVisibility(Property: MemberVisibility.Explicit)]
    public class CurvesPanel : LocalGroupPanel<Curve>
    {
        public override Uri Icon => Resources.ProjectImage("Curve.png");

        public override string Title => "Curves";

        public CurvesPanel() : base(Get.Current<Options>().Curves) { }

        protected override IEnumerable<GroupCollection<Curve>> GetDefault()
            { yield return new DefaultCurves(); }
    }

    #endregion
}