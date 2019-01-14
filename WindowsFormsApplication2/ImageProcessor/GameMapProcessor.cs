using Emgu.CV;
using Emgu.CV.CvEnum;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.GameScreenReader;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.ImageProcessor {



    public class GameMapProcessorResult<TResult> : ImageProcessorResult where TResult:MoveInfoBase {
        public Bitmap ResultBitmap { get; set; }
        public IEnumerable<TResult> VectorizationResult { get; set; }
        public GameMapProcessorResult(Bitmap resultBitmap, IEnumerable<TResult> vectorizationResult) {
            ResultBitmap = resultBitmap;
            VectorizationResult = vectorizationResult;
        }
        public GameMapProcessorResult() {

        }
    }
   
    public class GameMapProcessor : ImageProcessorBase<MapScreenReader, GameMapProcessorResult<MapMoveInfo>> {
        protected override int Delay => NativeApiWrapper.StandartDelay;
        
        protected override GameMapProcessorResult<MapMoveInfo> DoWhenTicket(Bitmap processImage) {
            Image<Bgr, byte> image = new Image<Bgr, byte>(processImage);
            var range = image.InRange(new Bgr(160, 110, 60), new Bgr(199, 170, 122)).Dilate(1);
            var range2 = image.InRange(new Bgr(167, 130, 122), new Bgr(205, 189, 203)).Dilate(1);
            range2 = range2.BlackColorToCustomColor(80);
            range = range2.Or(range);
            var rangeData = range.Copy().Data;
            var result = Vectorizer.DoLinearVectorization(rangeData, reader.PlayerCord);
            var image2 = new Image<Gray, byte>(rangeData);
            for (int i = 0; i < result.Count; i++) {
                CvInvoke.Line(image2, result[i].Vector[0], result[i].Vector[1], new MCvScalar(200), 2);
                CvInvoke.PutText(image2, result[i].IntersectionType.ToString()[0].ToString(), result[i].Vector[1], FontFace.HersheyPlain, 1, new MCvScalar(254));
            }
            return new GameMapProcessorResult<MapMoveInfo>(image2.ToBitmap(), result);
        }
    }
}
