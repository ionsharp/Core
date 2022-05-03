using Imagin.Common;
using Imagin.Common.Input;
using Imagin.Common.Controls;
using Imagin.Common.Linq;
using Imagin.Common.Models;
using System;
using System.Windows.Input;

namespace Imagin.Apps.Paint
{
    [MemberVisibility(MemberVisibility.Explicit, MemberVisibility.Explicit)]
    public class ToolsPanel : Panel
    {
        public const double OneColumnWidth = 48;

        public const double TwoColumnWidth = 64;

        public event EventHandler<EventArgs<Tool>> ToolSelected;

        public override bool CanShare => false;

        public override SecondaryDocks DockPreference => SecondaryDocks.Left;

        public override Uri Icon => Resources.ProjectImage("Tools.png");

        public override string Title => "Tools";

        Tool tool;
        public Tool Tool
        {
            get => tool;
            set
            {
                this.Change(ref tool, value);
                OnToolSelected(value);
            }
        }

        //...

        public ToolsPanel() : base()
        {
            if (Get.Current<Options>().Tools == null)
                Get.Current<Options>().Tools = new ToolCollection();

            var tools = Get.Current<Options>().Tools;
            if (!Get.Current<Options>().Tools.Any())
            {
                tools.Add(new ToolGroup(new SelectionTool(), new EllipseSelectionTool(), new ColumnSelectionTool(), new RowSelectionTool()));
                tools.Add(new ToolGroup(new MoveTool(), new TransformTool()));
                tools.Add(new ToolGroup(new EyeTool()));
                tools.Add(new ToolGroup(new RulerTool(), new NoteTool(), new CountTool()));
                tools.Add(new ToolGroup(new LassoTool(), new PolygonalLassoTool()));
                tools.Add(new ToolGroup(new MagicWandTool()));
                tools.Add(new ToolGroup(new CropTool()));
                tools.Add(new ToolGroup(new CloneStampTool()));
                tools.Add(new ToolGroup(new BrushTool(), new EffectBrushTool()));
                tools.Add(new ToolGroup(new BlurTool(), new SharpenTool(), new SmudgeTool()));
                tools.Add(new ToolGroup(new DodgeTool(), new BurnTool()));
                tools.Add(new ToolGroup(new SpongeTool()));
                tools.Add(new ToolGroup(new BucketTool()));
                tools.Add(new ToolGroup(new GradientTool()));
                tools.Add(new ToolGroup(new EraserTool()));
                tools.Add(new ToolGroup(new LineTool(), new EllipseTool(), new RectangleTool(), new RoundedRectangleTool(), new PolygonTool(), new CustomShapeTool()));
                tools.Add(new ToolGroup(new PathTool(), new FreePathTool()));
                tools.Add(new ToolGroup(new TextTool()));
                tools.Add(new ToolGroup(new HandTool(), new HandRotateTool()));
                tools.Add(new ToolGroup(new ZoomTool()));
            }

            tools
                .ForAll(i => i.Selected += OnToolSelected);
            tools
                .ForEach(i => i.MenuShown += OnMenuShown);
        }

        //...

        void OnMenuShown(object sender, EventArgs e)
        {
            if (sender is ToolGroup group)
            {
                Get.Current<Options>().Tools.ForEach(i =>
                {
                    if (!ReferenceEquals(i, group))
                        i.IsMenuVisible = false;
                });
            }
        }

        void OnToolSelected(object sender, SelectedEventArgs e)
        {
            Tool = sender as Tool;
            Get.Current<Options>().Tools.ForAll(i =>
            {
                if (!ReferenceEquals(i, Tool))
                    i.IsSelected = false;
            });
        }

        //...

        void OnToolSelected(Tool value)
        {
            ToolSelected?.Invoke(this, new EventArgs<Tool>(value));
        }

        ICommand collapseCommand;
        public ICommand CollapseCommand => collapseCommand ??= new RelayCommand(() => Width = OneColumnWidth, () => true);
    }
}