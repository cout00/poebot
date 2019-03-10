using Emgu.CV.Util;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.ImageProcessor {

    public enum IntersectionType {
        Wall,
        NewAreaFlag,
        Nothing,
    }

    public class MapMoveInfo: MoveInfoBase {        
        public IntersectionType IntersectionType { get; set; }
    }

    public class MapDirectionMoveInfo : MoveInfoBase {
        public int ContourID { get; set; }
        public double CenterAngle { get; set; }
    }

    abstract public class MoveInfoBase {
        public VectorOfPoint Vector { get; set; }
        public double Angle { get; set; }
    }

    public class LootMoveResult:MoveInfoBase {
        public Point LootCord { get; set; }
        public bool IsLootEndPoint { get; set; }
    }

    public static class Vectorizer {
        static MapMoveInfo GoToOrtogonalRight(byte[,,] data, int incX, int incY, Point playerCord) {
            VectorOfPoint vectorOfPoint = new VectorOfPoint();
            vectorOfPoint.Push(playerCord.YieldToArray());
            var angle = 0d;
            if (incX == 1 && incY == 0)
                angle = 0;

            if (incX == 2 && incY == 1)
                angle = 360 - 45 / 2d;

            if (incX == 2 && incY == -1)
                angle = 45 / 2d;

            if (incX == 1 && incY == 1)
                angle = 360 - 45;

            if (incX == 1 && incY == -1)
                angle = 45;

            if (incX == 1 && incY == -2)
                angle = 45 + 45 / 2d;

            if (incX == 1 && incY == 2)
                angle = 360 - 45 / 2d - 45;

            var yCord = playerCord.Y;
            List<IntersectionType> accum = new List<IntersectionType>();
            for (int i = playerCord.X; i < data.GetLength(1); i += incX) {
                yCord += incY;
                var convStep = CalculateConvolutionStep(data, yCord, i, 3);
                if (convStep != IntersectionType.Nothing)
                    accum.Add(convStep);
                if (accum.Any() && convStep == IntersectionType.Nothing) {
                    vectorOfPoint.Push(new Point(i, yCord).YieldToArray());
                    var wallsCount = accum.Where(a => a == IntersectionType.Wall).Count();
                    var newArea = accum.Where(a => a == IntersectionType.NewAreaFlag).Count();
                    return new MapMoveInfo() { Vector = vectorOfPoint, Angle = angle, IntersectionType = wallsCount > newArea ? IntersectionType.Wall : IntersectionType.NewAreaFlag };
                }
            }
            vectorOfPoint.Push(new Point(data.GetLength(1) - 1, yCord).YieldToArray());
            return new MapMoveInfo() { Angle = angle, IntersectionType = IntersectionType.Nothing, Vector = vectorOfPoint };
        }

        static MapMoveInfo GoToOrtogonalTop(byte[,,] data, Point playerCord) {
            VectorOfPoint vectorOfPoint = new VectorOfPoint();
            vectorOfPoint.Push(playerCord.YieldToArray());

            for (int i = playerCord.Y; i >= 0; i--) {
                var convStep = CalculateConvolutionStep(data, i, playerCord.X, 3);
                if (convStep != IntersectionType.Nothing) {
                    vectorOfPoint.Push(new Point(playerCord.X, i).YieldToArray());
                    return new MapMoveInfo() { Vector = vectorOfPoint, Angle = 90, IntersectionType = convStep };
                }
            }

            vectorOfPoint.Push(new Point(playerCord.X, 1).YieldToArray());
            return new MapMoveInfo() { Angle = 90, IntersectionType = IntersectionType.Nothing, Vector = vectorOfPoint };
        }

        static MapMoveInfo GoToOrtogonalLeft(byte[,,] data, int incX, int incY, Point playerCord) { //t
            VectorOfPoint vectorOfPoint = new VectorOfPoint();
            vectorOfPoint.Push(playerCord.YieldToArray());
            var angle = 0d;
            if (incX == 1 && incY == 0)
                angle = 180;

            if (incX == 2 && incY == 1)
                angle = 180 + 45 / 2d;

            if (incX == 2 && incY == -1)
                angle = 180 - 45 / 2d;

            if (incX == 1 && incY == 1)
                angle = 180 + 45;

            if (incX == 1 && incY == -1)
                angle = 180 - 45;

            if (incX == 1 && incY == -2)
                angle = 90 + 45 / 2d;

            if (incX == 1 && incY == 2)
                angle = 270 - 45 / 2d;

            var yCord = playerCord.Y;
            List<IntersectionType> accum = new List<IntersectionType>();
            for (int i = playerCord.X; i >= 0; i -= incX) {
                yCord += incY;
                var convStep = CalculateConvolutionStep(data, yCord, i, 3);
                if (convStep != IntersectionType.Nothing)
                    accum.Add(convStep);
                if (accum.Any() && convStep == IntersectionType.Nothing) {
                    vectorOfPoint.Push(new Point(i, yCord).YieldToArray());
                    var wallsCount = accum.Where(a => a == IntersectionType.Wall).Count();
                    var newArea = accum.Where(a => a == IntersectionType.NewAreaFlag).Count();
                    return new MapMoveInfo() { Vector = vectorOfPoint, Angle = angle, IntersectionType = wallsCount > newArea ? IntersectionType.Wall : IntersectionType.NewAreaFlag };
                }
            }
            vectorOfPoint.Push(new Point(1, yCord).YieldToArray());
            return new MapMoveInfo() { Angle = angle, IntersectionType = IntersectionType.Nothing, Vector = vectorOfPoint };
        }

        static MapMoveInfo GoToOrtogonalBottom(byte[,,] data, Point playerCord) { //t
            VectorOfPoint vectorOfPoint = new VectorOfPoint();
            vectorOfPoint.Push(playerCord.YieldToArray());

            for (int i = playerCord.Y; i < data.GetLength(0); i++) {
                var convStep = CalculateConvolutionStep(data, i, playerCord.X, 3);
                if (convStep != IntersectionType.Nothing) {
                    vectorOfPoint.Push(new Point(playerCord.X, i).YieldToArray());
                    return new MapMoveInfo() { Vector = vectorOfPoint, Angle = 270, IntersectionType = convStep };
                }
            }
            vectorOfPoint.Push(new Point(playerCord.X, data.GetLength(0) - 1).YieldToArray());
            return new MapMoveInfo() { Angle = 270, IntersectionType = IntersectionType.Nothing, Vector = vectorOfPoint };
        }

        static IntersectionType CalculateConvolutionStep(byte[,,] data, int Y, int X, int size) {
            if (size % 2 == 0)
                throw new NotSupportedException("whong size");
            var half = (int)(size / 2);
            var walls = 0;
            var newAreas = 0; ;
            for (int i = Y - half; i <= Y + half; i++) {
                for (int j = X - half; j <= X + half; j++) {
                    if (i < 0 || j < 0 || i >= data.GetLength(0) || j >= data.GetLength(1))
                        continue;
                    if (data[i, j, 0] == 80)
                        walls++;
                    if (data[i, j, 0] == 255)
                        newAreas++;
                }
            }
            if (walls == 0 && newAreas == 0)
                return IntersectionType.Nothing;
            if (walls > newAreas)
                return IntersectionType.Wall;
            else
                return IntersectionType.NewAreaFlag;
        }


        static List<Point> Normalize(List<Point> points, double threshold = 0.1d) {
            List<Point> result = new List<Point>();
            var max = points.Max(a => a.Y) * threshold;
            foreach (var point in points)
                if (point.Y > max)
                    result.Add(point);
                else
                    result.Add(new Point(point.X, 0));
            return result;
        }

        static IEnumerable<VectorOfPoint> Vectorize(List<Point> points) {
            List<VectorOfPoint> vectorOfPoints = new List<VectorOfPoint>();
            VectorOfPoint vpoints = new VectorOfPoint();
            for (int i = 1; i < points.Count; i++) {
                if (points[i].Y > 0 && points[i - 1].Y == 0) {
                    vpoints.Push(points[i].YieldToArray());
                }
                if (points[i].Y == 0 && points[i - 1].Y > 0) {
                    vpoints.Push(points[i].YieldToArray());
                    vectorOfPoints.Add(vpoints);
                    vpoints = new VectorOfPoint();
                }
            }
            return vectorOfPoints;
        }


        public static IList<Rectangle> DoLinearVectorizationToRectangles(byte[,,] data) {
            List<Point> horHist = new List<Point>();
            for (int i = 0; i < data.GetLength(0); i++) {
                var sum = 0;
                for (int j = 0; j < data.GetLength(1); j++) {
                    sum += data[i, j, 0];
                }
                horHist.Add(new Point(i, sum));
            }
            horHist = Normalize(horHist);
            var horRes = Vectorize(horHist);
            List<Point> vertHist = new List<Point>();
            for (int i = 0; i < data.GetLength(1); i++) {
                var sum = 0;
                for (int j = 0; j < data.GetLength(0); j++) {
                    sum += data[j, i, 0];
                }
                vertHist.Add(new Point(i, sum));
            }
            vertHist = Normalize(vertHist);
            var verticalRes = Vectorize(vertHist);
            List<Rectangle> vectorizationResult = new List<Rectangle>();
            foreach (var horVector in horRes) {
                Rectangle rect = new Rectangle(0, horVector[0].X, data.GetLength(1), horVector[1].X - horVector[0].X);
                foreach (var vertVector in verticalRes) {
                    Rectangle vertrect = new Rectangle(vertVector[0].X, 0, vertVector[1].X - vertVector[0].X, data.GetLength(0));
                    var intersect = Rectangle.Intersect(rect, vertrect);
                    if (intersect.Width * intersect.Height > 0) {
                        //var sum = 0;
                        //for (int i = intersect.Left; i < intersect.Left + intersect.Width; i++) {
                        //    for (int j = intersect.Top; j < intersect.Top + intersect.Height; j++) {
                        //        sum += data[i, j, 0];
                        //    }
                        //}
                        //if (sum > 0)
                        vectorizationResult.Add(intersect);
                    }
                }
            }
            return vectorizationResult;
        }

        public static IList<MapMoveInfo> DoLinearVectorization(byte[,,] data, Point playerCord) {
            List<MapMoveInfo> moveInfos = new List<MapMoveInfo>();
            moveInfos.Add(GoToOrtogonalRight(data, 1, 0, playerCord)); //0
            moveInfos.Add(GoToOrtogonalRight(data, 2, -1, playerCord)); // 22
            moveInfos.Add(GoToOrtogonalRight(data, 2, 1, playerCord)); // -22
            moveInfos.Add(GoToOrtogonalRight(data, 1, -1, playerCord)); // 45
            moveInfos.Add(GoToOrtogonalRight(data, 1, 1, playerCord)); // -45
            moveInfos.Add(GoToOrtogonalRight(data, 1, 2, playerCord)); //-67
            moveInfos.Add(GoToOrtogonalRight(data, 1, -2, playerCord)); // 67
            moveInfos.Add(GoToOrtogonalTop(data, playerCord)); // 90
            moveInfos.Add(GoToOrtogonalBottom(data, playerCord)); // 270
            moveInfos.Add(GoToOrtogonalLeft(data, 1, 0, playerCord)); //0
            moveInfos.Add(GoToOrtogonalLeft(data, 2, -1, playerCord)); // 22
            moveInfos.Add(GoToOrtogonalLeft(data, 2, 1, playerCord)); // -22
            moveInfos.Add(GoToOrtogonalLeft(data, 1, -1, playerCord)); // 45
            moveInfos.Add(GoToOrtogonalLeft(data, 1, 1, playerCord)); // -45
            moveInfos.Add(GoToOrtogonalLeft(data, 1, 2, playerCord)); //-67
            moveInfos.Add(GoToOrtogonalLeft(data, 1, -2, playerCord)); // 67
            return moveInfos;
        }
    }
}
