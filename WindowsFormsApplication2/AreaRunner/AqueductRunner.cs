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


        protected override double MainAreaOrientation => 135;

        protected override double MaxAngle => 250;

        protected override double MinAngle => 10;

        protected override MapDirectionMoveInfo ProcessNextMarker(IEnumerable<MapDirectionMoveInfo> moveInfo) {
            if (!mapHistory.Any())
                return null;
            var sorted = moveInfo.OrderBy(a => a.Angle).Where(a => a.ContourID <= 1).ToList();
            var median = 0d;
            foreach (var info in sorted) {
                var angles = info.Angle.GetAllNearAngles(90);
                var vectors = mapHistory.Last().MoveInfos.Join(angles, history => history.Angle, a => a, (a, h) => new { Vector = a.Vector.Length(), Angle = a.Angle });
                var maxVector = vectors.Sum(a => a.Vector);
                median = vectors.ToList()[vectors.Count() / 2].Angle;
                if (maxVector > 60) {
                    logger.WriteLog(info.Angle.ToString());
                    return info;
                }
            }
            //logger.WriteLog($"no sol" + sorted.Last().Angle.ToString());
            //return sorted.Last();
            return new MapDirectionMoveInfo() { Angle = median + 180, CenterAngle = -1 };
        }
    }
}
