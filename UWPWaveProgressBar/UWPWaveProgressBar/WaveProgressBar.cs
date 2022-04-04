using System;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Animation;

namespace UWPWaveProgressBar
{
    [TemplatePart(Name = LineSegmentTemplateName, Type = typeof(LineSegment))]
    [TemplatePart(Name = BezierSegmentTemplateName, Type = typeof(BezierSegment))]
    public class WaveProgressBar : Control
    {
        public static readonly DependencyProperty ProgressProperty = DependencyProperty.Register(
            nameof(Progress), typeof(double), typeof(WaveProgressBar), new PropertyMetadata(0d, OnProgressChanged));

        private const string BezierSegmentTemplateName = "PART_BezierSegment";
        private const string LineSegmentTemplateName = "PART_LineSegment";

        private BezierSegment _bezierSegment;
        private LineSegment _lineSegment;

        public WaveProgressBar()
        {
            DefaultStyleKey = typeof(WaveProgressBar);
        }

        public double Progress
        {
            get => (double)GetValue(ProgressProperty);
            set => SetValue(ProgressProperty, value);
        }

        protected override void OnApplyTemplate()
        {
            _lineSegment = (LineSegment)GetTemplateChild(LineSegmentTemplateName);
            _bezierSegment = (BezierSegment)GetTemplateChild(BezierSegmentTemplateName);

            PlayAnimation(true);
        }

        private static void OnProgressChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var obj = (WaveProgressBar)d;
            obj.PlayAnimation(false);
        }

        private void PlayAnimation(bool isInit)
        {
            if (_lineSegment == null || _bezierSegment == null)
            {
                return;
            }

            var targetY = 100 * (1 - Progress);
            var duration = new Duration(TimeSpan.FromSeconds(isInit ? 0 : 0.7));

            var storyboard = new Storyboard();

            var point1Animation = new PointAnimation
            {
                EnableDependentAnimation = true,
                Duration = duration,
                To = new Point(0, targetY),
                EasingFunction = new BackEase
                {
                    Amplitude = 0.5
                }
            };
            Storyboard.SetTarget(point1Animation, _lineSegment);
            Storyboard.SetTargetProperty(point1Animation, nameof(_lineSegment.Point));
            storyboard.Children.Add(point1Animation);

            var point2Animation = new PointAnimation
            {
                EnableDependentAnimation = true,
                Duration = duration,
                To = new Point(35, targetY),
                EasingFunction = new BackEase
                {
                    Amplitude = 1.5
                }
            };
            Storyboard.SetTarget(point2Animation, _bezierSegment);
            Storyboard.SetTargetProperty(point2Animation, nameof(_bezierSegment.Point1));
            storyboard.Children.Add(point2Animation);

            var point3Animation = new PointAnimation
            {
                EnableDependentAnimation = true,
                Duration = duration,
                To = new Point(65, targetY),
                EasingFunction = new BackEase
                {
                    Amplitude = 0.1
                }
            };
            Storyboard.SetTarget(point3Animation, _bezierSegment);
            Storyboard.SetTargetProperty(point3Animation, nameof(_bezierSegment.Point2));
            storyboard.Children.Add(point3Animation);

            var point4Animation = new PointAnimation
            {
                EnableDependentAnimation = true,
                Duration = duration,
                To = new Point(100, targetY),
                EasingFunction = new BackEase
                {
                    Amplitude = 0.5
                }
            };
            Storyboard.SetTarget(point4Animation, _bezierSegment);
            Storyboard.SetTargetProperty(point4Animation, nameof(_bezierSegment.Point3));
            storyboard.Children.Add(point4Animation);

            storyboard.Begin();
        }
    }
}