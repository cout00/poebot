using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Process.NET.Windows;
using WindowsFormsApplication2.ImageProcessor;
using WindowsFormsApplication2.Logger;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner {
    public class AqueductRunner : AreaRunnerBase {
        public AqueductRunner(ILogger logger) : base(logger) {

        }

        public AqueductRunner():base() {

        }

        protected override double MainAreaOrientation => 135;

        protected override double MaxAngle => 250;

        protected override double MinAngle => 10;

        protected override int MaxAreaTime => 80000;

        protected override MapDirectionMoveInfo ProcessNextMarker(IEnumerable<MapDirectionMoveInfo> moveInfo) {
            if (!mapHistory.Any())
                return null;
            var sorted = moveInfo.OrderBy(a => a.Angle).Where(a => a.ContourID <= 1).ToList();
            var median = -1d;
            foreach (var info in sorted) {
                var angles = info.Angle.GetAllNearAngles(90);
                var vectors = mapHistory.Last().MoveInfos.Join(angles, history => history.Angle, a => a, (a, h) => new { Vector = a.Vector.Length(), Angle = a.Angle });
                var maxVector = vectors.Sum(a => a.Vector);
                median = vectors.ToList()[vectors.Count() / 2].Angle;
                if (maxVector > 60) {
                    return info;
                }
            }
            return new MapDirectionMoveInfo() { Angle = median, CenterAngle = 180 };
        }
    }
}
