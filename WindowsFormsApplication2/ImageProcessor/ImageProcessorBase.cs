using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.GameScreenReader;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.ImageProcessor {

    public abstract class ImageProcessorResult {

    }

    public class ImageProcessorEventArgs<T> where T : ImageProcessorResult, new() {
        public T ImageProcessorResult { get; }
        public ImageProcessorEventArgs(T imageProcessorResult) {
            ImageProcessorResult = imageProcessorResult;
        }
    }

    public abstract class ImageProcessorBase<T, Result>:IDisposable where T : GameScreenReaderBase, new() where Result : ImageProcessorResult, new() {
        public event EventHandler<ImageProcessorEventArgs<Result>> OnResult;

        Timer timer = new Timer();
        protected T reader = new T();
        protected abstract int Delay { get; }
        public ImageProcessorBase() {
            timer.Interval = Delay == -1 ? 1000 : Delay;
            timer.Start();
            timer.Tick += OnTick;
        }

        protected TMoveInfo PointToMoveInfo<TMoveInfo>(Point point) where TMoveInfo : MoveInfoBase, new() {
            VectorOfPoint vectorOfPoint = new VectorOfPoint();
            vectorOfPoint.Push(reader.PlayerCord.YieldToArray());
            vectorOfPoint.Push(point.YieldToArray());
            var angleLeft = reader.PlayerCord.AngleBetweenCenterAndPoint(point);
            var anglegradLeft = 180 * angleLeft / Math.PI;
            return new TMoveInfo() { Angle = anglegradLeft, Vector = vectorOfPoint };
        }


        protected abstract Result DoWhenTicket(Bitmap processImage);

        bool done = false;
        void OnTick(object sender, EventArgs e) {
            if (Delay == -1 && done) {
                return;
            }
            done = true;
            var result = DoWhenTicket(reader.ReadScreen());
            OnResult?.Invoke(this, new ImageProcessorEventArgs<Result>(result));
        }

        public void Dispose() {
            timer.Stop();
            timer.Tick -= OnTick;
            timer.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
