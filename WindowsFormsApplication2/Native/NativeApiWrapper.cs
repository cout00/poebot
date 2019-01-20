using Microsoft.Win32;
using Process.NET;
using Process.NET.Memory;
using Process.NET.Windows;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static WindowsFormsApplication2.Native.NativeAPI;

namespace WindowsFormsApplication2.Native {
    public static class NativeApiWrapper {
        public static IntPtr GameMainWindowHandle;
        public const string GameProcessName = "PathOfExile";
        public const double AngleStep = 22.5d;
        public const int StandartDelay = 200;
        public static string GameFolder;

        public static IWindow GameWindow { get; private set; }

        public static Point PlayerLocalCord {
            get {
                return new Point(402, 281);
            }
        }
        public static Point PlayerGlobalCord {
            get {
                return GetScreenPoint(PlayerLocalCord);
            }
        }

        static NativeApiWrapper() {            
            var processes = System.Diagnostics.Process.GetProcessesByName(GameProcessName).Where(a => a.MainWindowHandle != IntPtr.Zero);
            if (!processes.Any())
                throw new Exception($"Process not found {GameProcessName}");
            if (processes.Count() > 1)
                throw new NotSupportedException($"only 1 instance of {GameProcessName}");
            var process = processes.FirstOrDefault();
            GameMainWindowHandle = process.MainWindowHandle;
            var processSharp = new ProcessSharp(process, MemoryType.Remote);
            GameWindow = processSharp.WindowFactory.MainWindow;

            string InstallPath = (string)Registry.GetValue(@"HKEY_CURRENT_USER\Software\GrindingGearGames\Path of Exile", "InstallLocation", null);
            if (InstallPath != null) {
                GameFolder = InstallPath;
            }

        }

        public static Rectangle GetGameWindowRectange() {
            RECT windowRect = new RECT();
            GetClientRect(GameMainWindowHandle, out windowRect);
            Point point = new Point();
            NativeAPI.ClientToScreen(GameMainWindowHandle, ref point);
            return new Rectangle(point, new Size(windowRect.Width, windowRect.Height));
        }

        public static Point ClientToScreen() {
            Point result = new Point();
            NativeAPI.ClientToScreen(GameMainWindowHandle, ref result);
            return result;
        }

        public static bool GameWindowIsActive() {
            return GetActiveWindow() == GameMainWindowHandle;
        }

        public static Point GetScreenPoint(Point pt) {
            MapWindowPoints(GameMainWindowHandle, IntPtr.Zero, ref pt, 1);
            return pt;
        }

        static Point RotatePoint1(Point p1, Point p2, double angle) {

            double radians = ConvertToRadians(angle);
            double sin = Math.Sin(radians);
            double cos = Math.Cos(radians);
            p1.X -= p2.X;
            p1.Y -= p2.Y;
            double xnew = p1.X * cos - p1.Y * sin;
            double ynew = p1.X * sin + p1.Y * cos;

            Point newPoint = new Point((int)xnew + p2.X, (int)ynew + p2.Y);
            return newPoint;
        }

        static double ConvertToRadians(double angle) {
            return (Math.PI / 180) * angle;
        }

        public static Point GetScreenRotatedPoint(int angle) {
            var playerCord = PlayerLocalCord;
            var offset = 200;
            return RotatePoint1(new Point(playerCord.X + offset, playerCord.Y), playerCord, 360 - angle);
        }

    }
}
