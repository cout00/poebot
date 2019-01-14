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
    public class GameMapFogProcessor : ImageProcessorBase<MapScreenReader, GameMapProcessorResult<MapDirectionMoveInfo>> {
        protected override int Delay => NativeApiWrapper.StandartDelay;
      
        protected override GameMapProcessorResult<MapDirectionMoveInfo> DoWhenTicket(Bitmap processImage) {
            Image<Bgr, byte> image = new Image<Bgr, byte>(processImage);
            var range = image.InRange(new Bgr(160, 110, 60), new Bgr(199, 170, 122)).Dilate(1).SmoothMedian(5).SmoothMedian(5);
            VectorOfVectorOfPoint vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
            IOutputArray hirarchy = null;
            List<MapDirectionMoveInfo> moveInfos = new List<MapDirectionMoveInfo>();
            CvInvoke.FindContours(range, vectorOfVectorOfPoint, hirarchy, RetrType.List, ChainApproxMethod.ChainApproxTc89Kcos);
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++) {
                if (CvInvoke.ContourArea(vectorOfVectorOfPoint[i]) < 25)
                    continue;
                var moveInfo = PointToMoveInfo<MapDirectionMoveInfo>(vectorOfVectorOfPoint[i].ToArray().OrderBy(a => a.X).Last());
                moveInfos.Add(moveInfo);
                var moments = CvInvoke.Moments(vectorOfVectorOfPoint[i]);
                var centerMoveInfo = PointToMoveInfo<MapDirectionMoveInfo>(new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00)));
                centerMoveInfo.CenterAngle = centerMoveInfo.Angle;
                moveInfo.CenterAngle = centerMoveInfo.Angle;
                moveInfos.Add(centerMoveInfo);
            }
            var sorted = moveInfos.OrderBy(a => a.Angle).ToList();
            var ID = 0;
            for (int i = 0; i < sorted.Count(); i+=2) {
                sorted[i].ContourID = ID;
                sorted[i+1].ContourID = ID;
                ID++;
            }
            return new GameMapProcessorResult<MapDirectionMoveInfo>() { ResultBitmap = range.ToBitmap(), VectorizationResult = moveInfos };
        }
    }
}
