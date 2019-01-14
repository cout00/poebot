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
using WindowsFormsApplication2.AreaRunner.LockedAction;
using WindowsFormsApplication2.GameScreenReader;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.ImageProcessor {
    public class LootProcessor : ImageProcessorBase<LootScreenReader, GameMapProcessorResult<LootMoveResult>>, ISupportLock {
       
        public bool Locked { get; set; }

        protected override int Delay => NativeApiWrapper.StandartDelay;

        protected override GameMapProcessorResult<LootMoveResult> DoWhenTicket(Bitmap processImage) {
            Locked = true;
            Image<Bgr, byte> image = new Image<Bgr, byte>(processImage);
            var range = image.InRange(new Bgr(200, 0, 200), new Bgr(210, 0, 210)).Dilate(1).SmoothMedian(5).SmoothMedian(5);
            VectorOfVectorOfPoint vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
            IOutputArray hirarchy = null;
            List<LootMoveResult> moveInfos = new List<LootMoveResult>();
            CvInvoke.FindContours(range, vectorOfVectorOfPoint, hirarchy, RetrType.List, ChainApproxMethod.ChainApproxTc89Kcos);
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++) {
                if (CvInvoke.ContourArea(vectorOfVectorOfPoint[i]) < 25)
                    continue;
                var moments = CvInvoke.Moments(vectorOfVectorOfPoint[i]);
                var centerMoveInfo = PointToMoveInfo<LootMoveResult>(new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00)));
                centerMoveInfo.LootCord = reader.ClientPointToWindowPoint(centerMoveInfo.Vector[1]);
                moveInfos.Add(centerMoveInfo);
                CvInvoke.Line(range, centerMoveInfo.Vector[0], centerMoveInfo.Vector[1], new MCvScalar(255));
            }
            Locked = moveInfos.Any();            
            return new GameMapProcessorResult<LootMoveResult>() { ResultBitmap = range.ToBitmap(), VectorizationResult = moveInfos };
        }
    }
}
