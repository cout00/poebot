using Overlay.NET;
using Overlay.NET.Common;
using Overlay.NET.Wpf;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Windows;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Overlay
{

    public class DemoOverlaySettings
    {
        public int UpdateRate { get; set; }

        public string Author { get; set; }
        public string Description { get; set; }
        public string Identifier { get; set; }
        public string Name { get; set; }
        public string Version { get; set; }
    }

    [RegisterPlugin("WpfOverlayDemo-1", "Jacob Kemple", "WpfOverlayDemo", "0.0", "A basic demo of the WPF overlay.")]
    public class WpfOverlayDemoExample : WpfOverlayPlugin
    {
        public ISettings<DemoOverlaySettings> Settings { get; } = new SerializableSettings<DemoOverlaySettings>();
        private readonly TickEngine _tickEngine = new TickEngine();
        private Ellipse _ellipse;

        private bool _isDisposed;

        private bool _isSetup;
        private Line _line;
        private Polygon _polygon;
        private Rectangle _rectangle;

        public override void Enable()
        {
            _tickEngine.IsTicking = true;
            base.Enable();
        }

        public override void Disable()
        {
            _tickEngine.IsTicking = false;
            base.Disable();
        }

        public override void Initialize(IWindow targetWindow)
        {
            // Set target window by calling the base method
            base.Initialize(targetWindow);

            OverlayWindow = new OverlayWindow(targetWindow);

            // For demo, show how to use settings
            var current = Settings.Current;
            var type = GetType();

            current.UpdateRate = 1000 / 60;
            current.Author = GetAuthor(type);
            current.Description = GetDescription(type);
            current.Identifier = GetIdentifier(type);
            current.Name = GetName(type);
            current.Version = GetVersion(type);

            // File is made from above info
            Settings.Save();
            Settings.Load();

            _tickEngine.Interval = 500.Milliseconds();
            _tickEngine.PreTick += OnPreTick;
            _tickEngine.Tick += OnTick;
        }

        private void OnTick(object sender, EventArgs eventArgs)
        {
            if (OverlayWindow.IsVisible) {
                OverlayWindow.Update();
            }
        }

        private void OnPreTick(object sender, EventArgs eventArgs)
        {
            // Only want to set them up once.
            if (!_isSetup)
            {
                //SetUp();
                _isSetup = true;
            }

            var activated = TargetWindow.IsActivated;
            var visible = OverlayWindow.IsVisible;
            // Ensure window is shown or hidden correctly prior to updating
            if (!activated && visible)
            {
                OverlayWindow.Hide();
            }

            else if (activated && !visible)
            {
                OverlayWindow.Show();
            }
        }

        public override void Update() => _tickEngine.Pulse();

        // Clear objects
        public override void Dispose()
        {
            if (_isDisposed)
            {
                return;
            }

            if (IsEnabled)
            {
                Disable();
            }

            OverlayWindow?.Hide();
            OverlayWindow?.Close();
            OverlayWindow = null;
            _tickEngine.Stop();
            Settings.Save();

            base.Dispose();
            _isDisposed = true;
        }

        ~WpfOverlayDemoExample()
        {
            Dispose();
        }

        private void SetUp()
        {
            _polygon = new Polygon
            {
                Points = new PointCollection(5) {
                    new Point(100, 150),
                    new Point(120, 130),
                    new Point(140, 150),
                    new Point(140, 200),
                    new Point(100, 200)
                },
                Stroke = new SolidColorBrush(Color.FromRgb(0, 0, 255)),
                Fill =
                    new RadialGradientBrush(
                        Color.FromRgb(255, 255, 0),
                        Color.FromRgb(255, 0, 255))
            };

            OverlayWindow.Add(_polygon);

            // Create a line
            _line = new Line
            {
                X1 = 100,
                X2 = 300,
                Y1 = 200,
                Y2 = 200,
                Stroke = new SolidColorBrush(Color.FromRgb(0, 255, 0)),
                StrokeThickness = 2
            };

            OverlayWindow.Add(_line);

            // Create an ellipse (circle)
            _ellipse = new Ellipse
            {
                Width = 15,
                Height = 15,
                Margin = new Thickness(300, 300, 0, 0),
                Stroke =
                    new SolidColorBrush(Color.FromRgb(0, 255, 255))
            };

            OverlayWindow.Add(_ellipse);

            // Create a rectangle
            _rectangle = new Rectangle
            {
                RadiusX = 2,
                RadiusY = 2,
                Width = 50,
                Height = 100,
                Margin = new Thickness(400, 400, 0, 0),
                Stroke = new SolidColorBrush(Color.FromRgb(255, 0, 0)),
                Fill =
                    new SolidColorBrush(Color.FromArgb(100, 255, 255,
                        255))
            };

            OverlayWindow.Add(_rectangle);
        }
    }




    public class WpfOverlayExampleDemo
    {
        /// <summary>
        ///     The overlay
        /// </summary>
        private static OverlayPlugin _overlay;

        /// <summary>
        ///     The process sharp
        /// </summary>
        private static ProcessSharp _processSharp;

        /// <summary>
        ///     The work
        /// </summary>
        private static bool _work;

        /// <summary>
        ///     Starts the demo.
        /// </summary>
        public void StartDemo()
        {
            var processName = "PathOfExile";
            var process = System.Diagnostics.Process.GetProcessesByName(processName).FirstOrDefault();
            if (process == null)
            {
                return;
            }

            _processSharp = new ProcessSharp(process, MemoryType.Remote);
            _overlay = new WpfOverlayDemoExample();

            var wpfOverlay = (WpfOverlayDemoExample)_overlay;
            wpfOverlay.Initialize(_processSharp.WindowFactory.MainWindow);
            wpfOverlay.Enable();

            _work = true;
            var info = wpfOverlay.Settings.Current;
            wpfOverlay.OverlayWindow.Draw += OnDraw;
            while (_work)
            {
                _overlay.Update();
            }
        }

        private static void OnDraw(object sender, DrawingContext context)
        {
            // Draw a formatted text string into the DrawingContext.        
            context.DrawText(
                new FormattedText("Click Me!", CultureInfo.GetCultureInfo("en-us"), FlowDirection.LeftToRight,
                    new Typeface("Verdana"), 36, Brushes.BlueViolet), new Point(200, 116));

            context.DrawLine(new Pen(Brushes.Blue, 10), new Point(100, 100), new Point(10, 10));
        }
    }
}
