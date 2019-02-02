using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.Util;
using PoeItemObjectModelLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Native {
    public static class Extensions {

        public static IEnumerable<T> Yield<T>(this T self) {
            List<T> list = new List<T>();
            list.Add(self);
            return list;
        }
        public static T[] YieldToArray<T>(this T self) {
            return self.Yield().ToArray();
        }

        public static Image<Gray, byte> BlackColorToCustomColor(this Image<Gray, byte> self, byte newVal) {
            for (int i = 0; i < self.Data.GetLength(0); i++) {
                for (int j = 0; j < self.Data.GetLength(1); j++) {
                    if (self.Data[i, j, 0] == 255) {
                        self.Data[i, j, 0] = newVal;
                    }
                }
            }
            return self;
        }

        public static double AngleBetweenCenterAndPoint(this Point center, Point rotatedPoint) {
            double deltaY = (-rotatedPoint.Y - (double)(-center.Y));
            double deltaX = (rotatedPoint.X - (double)(center.X));
            var a = Math.Atan2(deltaY, deltaX);
            while (a < 0.0) {
                a = a + Math.PI * 2;
            }
            return a;
        }

        public static IEnumerable<double> GetAllNearAngles(this double angle, double range) {
            List<double> result = new List<double>();
            var min = angle.ToAngle(-range / 2);
            var max = angle.ToAngle(range / 2);
            if ((angle - range / 2) < 0 || (angle + range / 2) > 360) {
                foreach (var nextAngle in AngleIterator(0, 359)) {
                    if (nextAngle <= max) {
                        result.Add(nextAngle);
                    }
                    if (nextAngle >= min) {
                        result.Add(nextAngle);
                    }
                }
            }
            else {
                foreach (var nextAngle in AngleIterator(0, 359)) {
                    if (nextAngle >= min && nextAngle <= max) {
                        result.Add(nextAngle);
                    }
                }
            }
            return result;
        }

        public static double Distance(this Point self, Point p1) {
            var r1 = self.X - p1.X;
            var r2 = self.Y - p1.Y;
            return Math.Sqrt(r1 * r1 + r2 * r2);
        }

        public static double ToAngle(this double self, double sum) {
            var result = self + sum;
            if (result >= 360) {
                return result - 360;
            }
            if (result < 0) {
                return 360 + result;
            }
            return result;
        }
        
        public static double ToAngle(this double self) {
            return self.ToAngle(0);
        }

        public static IEnumerable<double> AngleIterator(double start, double end, double step = NativeApiWrapper.AngleStep) {
            var startPos = start;
            if (end < start) {
                for (double i = start; i <= 360 + end; i += step) {
                    yield return i.ToAngle();
                }
            }
            else {
                for (double i = start; i <= end; i += step) {
                    yield return i;
                }
            }            
        }

        public static double Length(this VectorOfPoint vector) {
            if (vector.Size > 2)
                throw new NotSupportedException();
            var r1 = vector[0].X - vector[1].X;
            var r2 = vector[0].Y - vector[1].Y;
            return Math.Sqrt(r1 * r1 + r2 * r2);
        }
        
    }
}
