using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Process.NET;
using Process.NET.Memory;
using Process.NET.Windows;

namespace WindowsFormsApplication2.Native {
    public class GameInstance {
        readonly System.Diagnostics.Process gameProcess;

        public IntPtr GameInstanceHandle { get; }
        public IWindow GameWindow { get; }

        public GameInstance(System.Diagnostics.Process gameProcess) {
            this.gameProcess = gameProcess;
            GameInstanceHandle = gameProcess.MainWindowHandle;
            var processSharp = new ProcessSharp(gameProcess, MemoryType.Remote);
            GameWindow = processSharp.WindowFactory.MainWindow;
            GameWindow.Activate();
        }

    }
}
