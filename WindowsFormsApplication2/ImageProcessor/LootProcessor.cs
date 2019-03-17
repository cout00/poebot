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

    public class LootProcessor2 : ImageProcessorBase<LootScreenReader, GameMapProcessorResult<LootMoveResult>>, ISupportLock {

        public bool Locked { get; set; }

        protected override int Delay => NativeApiWrapper.StandartDelay;

        List<VectorOfPoint> GetCountours(Image<Gray, byte> rawImage) {
            List<VectorOfPoint> resultArray = new List<VectorOfPoint>();
            VectorOfVectorOfPoint vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
            Mat hirarchy = new Mat();
            List<LootMoveResult> moveInfos = new List<LootMoveResult>();
            CvInvoke.FindContours(rawImage, vectorOfVectorOfPoint, hirarchy, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++) {
                var area = CvInvoke.ContourArea(vectorOfVectorOfPoint[i]);
                if (area < 25)
                    continue;
                resultArray.Add(vectorOfVectorOfPoint[i]);
            }
            rawImage.Dispose();
            return resultArray;
        }


        List<VectorOfPoint> GetMarkers(Image<Gray, byte> rawImage, Image<Gray, byte> markerPattern) {
            VectorOfVectorOfPoint vectorOfVectorOfPoint = new VectorOfVectorOfPoint();
            VectorOfVectorOfPoint filtered = new VectorOfVectorOfPoint();
            Mat hirarchy = new Mat();
            List<LootMoveResult> moveInfos = new List<LootMoveResult>();
            CvInvoke.FindContours(rawImage, vectorOfVectorOfPoint, hirarchy, RetrType.Ccomp, ChainApproxMethod.ChainApproxSimple);
            for (int i = 0; i < vectorOfVectorOfPoint.Size; i++) {
                var area = CvInvoke.ContourArea(vectorOfVectorOfPoint[i]);
                if (area < 210 || area > 260)
                    continue;
                filtered.Push(vectorOfVectorOfPoint[i]);
            }
            Image<Gray, byte> result = new Image<Gray, byte>(markerPattern.Size);
            CvInvoke.DrawContours(result, filtered, -1, new MCvScalar(255), -1);
            var andResult = result.And(markerPattern);
            result.Dispose();
            return GetCountours(andResult);
        }


        public Bgr bgrMin { get; set; }
        public Bgr bgrMax { get; set; }

        protected override GameMapProcessorResult<LootMoveResult> DoWhenTicket(Bitmap processImage) {
            Locked = true;
            Image<Bgr, byte> image = new Image<Bgr, byte>(processImage);
            //var range = image.InRange(bgrMin, bgrMax).Dilate(1).SmoothMedian(5).SmoothMedian(5);
            var green = image.InRange(new Bgr(20, 220, 0), new Bgr(40, 255, 255)).Dilate(8).SmoothMedian(5).SmoothMedian(5);
            //var range = image.InRange(bgrMin, bgrMax).Dilate(1).SmoothMedian(5).SmoothMedian(5);
            var endPointRange = image.InRange(new Bgr(180, 0, 220), new Bgr(255, 40, 255)).Dilate(1).SmoothMedian(5).SmoothMedian(5);            
            var mapMarkers = image.InRange(new Bgr(0, 0, 0), new Bgr(1, 1, 1)).Dilate(1).SmoothMedian(5).SmoothMedian(5);
            var markers = GetMarkers(mapMarkers, green);
            List<LootMoveResult> moveInfos = new List<LootMoveResult>();
            var endPoints = GetCountours(endPointRange);

            if (markers.Any()  || endPoints.Any()) {
                if (endPoints.Any()) {
                    foreach (var endpoint in endPoints) {
                        var moments = CvInvoke.Moments(endpoint);
                        var centerMoveInfo = PointToMoveInfo<LootMoveResult>(new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00)));
                        centerMoveInfo.LootCord = reader.ClientPointToWindowPoint(centerMoveInfo.Vector[1]);
                        centerMoveInfo.IsLootEndPoint = true;
                        moveInfos.Add(centerMoveInfo);
                        //CvInvoke.Line(mapMarkers, centerMoveInfo.Vector[0], centerMoveInfo.Vector[1], new MCvScalar(255));
                    }
                }
                else {
                    foreach (var marker in markers) {
                        var moments = CvInvoke.Moments(marker);
                        var centerMoveInfo = PointToMoveInfo<LootMoveResult>(new Point((int)(moments.M10 / moments.M00), (int)(moments.M01 / moments.M00)));
                        //CvInvoke.Line(mapMarkers, centerMoveInfo.Vector[0], centerMoveInfo.Vector[1], new MCvScalar(255));
                        //CvInvoke.PutText(mapMarkers, CvInvoke.ContourArea(marker).ToString(), centerMoveInfo.Vector[1], FontFace.HersheyComplex, 1, new MCvScalar(255));
                        centerMoveInfo.IsLootEndPoint = false;
                        moveInfos.Add(centerMoveInfo);
                    }
                }
            }
            var resultBItmap = mapMarkers.ToBitmap();
            image.Dispose();
            green.Dispose();
            endPointRange.Dispose();
            mapMarkers.Dispose();
            Locked = moveInfos.Any();
            return new GameMapProcessorResult<LootMoveResult>() { ResultBitmap = resultBItmap, VectorizationResult = moveInfos };
        }
    }

}
