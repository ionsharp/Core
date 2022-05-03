using Imagin.Common.Input;
using Imagin.Common.Linq;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media.Animation;
using System.Windows.Media.Media3D;

namespace Imagin.Common.Controls
{
    /// <summary>
    /// A <see cref="ContentControl"/> that provides the ability "flip" content in 3D.
    /// </summary>
    [TemplatePart(Name = nameof(PART_Viewport), Type = typeof(Viewport3D))]
    public class ContentControl3D : ContentControl
    {
        bool rotating;

        int rotationRequests;

        //...

        Viewport3D PART_Viewport;

        //...

        public static readonly DependencyProperty AnimationLengthProperty = DependencyProperty.Register(nameof(AnimationLength), typeof(int), typeof(ContentControl3D), new FrameworkPropertyMetadata(600, OnAnimationLengthChanged));
        /// <summary>
        /// Gets/sets the number of milliseconds it should take to rotate. Cannot set a value less than 10.
        /// </summary>
        public int AnimationLength
        {
            get => (int)GetValue(AnimationLengthProperty);
            set => SetValue(AnimationLengthProperty, value);
        }
        static void OnAnimationLengthChanged(DependencyObject i, DependencyPropertyChangedEventArgs e)
        {
            int value = (int)e.NewValue;
            if (value < 10)
                throw new ArgumentOutOfRangeException("AnimationLength", "AnimationLength cannot be less than 10 milliseconds.");
        }

        public static readonly DependencyProperty BackContentProperty = DependencyProperty.Register(nameof(BackContent), typeof(object), typeof(ContentControl3D));
        public object BackContent
        {
            get => (object)GetValue(BackContentProperty);
            set => SetValue(BackContentProperty, value);
        }

        public static readonly DependencyProperty BackContentTemplateProperty = DependencyProperty.Register(nameof(BackContentTemplate), typeof(DataTemplate), typeof(ContentControl3D), new FrameworkPropertyMetadata(null));
        public DataTemplate BackContentTemplate
        {
            get => (DataTemplate)GetValue(BackContentTemplateProperty); 
            set => SetValue(BackContentTemplateProperty, value); 
        }

        public static readonly DependencyProperty FlipProperty = DependencyProperty.Register(nameof(Flip), typeof(bool), typeof(ContentControl3D), new FrameworkPropertyMetadata(false, OnFlipChanged));
        public bool Flip
        {
            get => (bool)GetValue(FlipProperty);
            set => SetValue(FlipProperty, value);
        }
        static void OnFlipChanged(DependencyObject i, DependencyPropertyChangedEventArgs e) => (i as ContentControl3D).Rotate();
        
        static readonly DependencyPropertyKey IsFrontInViewPropertyKey;
        public static readonly DependencyProperty IsFrontInViewProperty;
        public bool IsFrontInView
        {
            get => (bool)GetValue(IsFrontInViewProperty); 
            private set => SetValue(IsFrontInViewPropertyKey, value); 
        }

        //...

        static ContentControl3D()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(ContentControl3D), new FrameworkPropertyMetadata(typeof(ContentControl3D)));
            
            IsFrontInViewPropertyKey 
                = DependencyProperty.RegisterReadOnly(nameof(IsFrontInView), typeof(bool), typeof(ContentControl3D), new FrameworkPropertyMetadata(true));
            IsFrontInViewProperty 
                = IsFrontInViewPropertyKey.DependencyProperty;
        }

        public ContentControl3D() : base() { }

        //...

        PerspectiveCamera CreateCamera()
        {
            return new PerspectiveCamera
            {
                Position 
                    = new System.Windows.Media.Media3D.Point3D(0, 0, 2),
                LookDirection 
                    = new Vector3D(0, 0, -2),
                FieldOfView 
                    = 90
            };
        }

        public void Rotate()
        {
            if (rotating)
            {
                ++rotationRequests;
                return;
            }
            else
            {
                rotating = true;
            }

            if (PART_Viewport != null)
            {
                // Find front rotation
                var backContentSurface 
                    = PART_Viewport.Children[1] as Viewport2DVisual3D;
                var backTransform 
                    = backContentSurface.Transform as RotateTransform3D;
                var backRotation 
                    = backTransform.Rotation as AxisAngleRotation3D;

                // Find back rotation
                var frontContentSurface 
                    = PART_Viewport.Children[2] as Viewport2DVisual3D;
                var frontTransform 
                    = frontContentSurface.Transform as RotateTransform3D;
                var frontRotation 
                    = frontTransform.Rotation as AxisAngleRotation3D;

                // Create a new camera each time, to avoid trying to animate a frozen instance.
                var camera 
                    = CreateCamera();
                PART_Viewport.Camera 
                    = camera;

                DoubleAnimation rotationAnim 
                    = new()
                    {
                    Duration = new Duration(TimeSpan.FromMilliseconds(this.AnimationLength)),
                    By = 180
                };
                Point3DAnimation cameraZoomAnim 
                    = new()
                    {
                    To = new System.Windows.Media.Media3D.Point3D(0, 0, 2.5),
                    Duration = new Duration(TimeSpan.FromMilliseconds(this.AnimationLength / 2)),
                    AutoReverse = true
                };
                cameraZoomAnim.Completed 
                    += OnRotationCompleted;

                frontRotation
                    .BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnim);
                backRotation
                    .BeginAnimation(AxisAngleRotation3D.AngleProperty, rotationAnim);
                camera
                    .BeginAnimation(PerspectiveCamera.PositionProperty, cameraZoomAnim);
            }
        }

        void OnRotationCompleted(object sender, EventArgs e)
        {
            IsFrontInView = !IsFrontInView;
            rotating = false;

            if (rotationRequests > 0)
            {
                --rotationRequests;
                Rotate();
            }
        }

        public override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            PART_Viewport = Template.FindName(nameof(PART_Viewport), this) as Viewport3D;
            PART_Viewport.If(i => i != null, i => i.Camera = CreateCamera());
        }

        //...

        ICommand rotateCommand;
        public ICommand RotateCommand => rotateCommand ??= new RelayCommand(Rotate);
    }
}