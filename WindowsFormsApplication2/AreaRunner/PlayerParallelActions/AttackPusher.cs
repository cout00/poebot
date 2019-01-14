using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.PlayerParallelActions {
    public class AttackPusher : PusherBase {
        public double CenterAngle { get; set; } = -1;

        public AttackPusher() {
            PusherInfo pusherInfo = new PusherInfo();
            pusherInfo.BeforePush = () => {
                if (CenterAngle == -1) {
                    return;
                }
                var point = NativeApiWrapper.GetScreenRotatedPoint((int)CenterAngle);
                AvailableInput.MouseMove(point);
            };
            pusherInfo.Code = Settings.MainAttack;
            pusherInfo.Delay = Settings.AttackDelay;
            CreatePusher(pusherInfo);
        }
    }
}
