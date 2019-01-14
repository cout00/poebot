using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.PlayerParallelActions {
    public class FlaskPusher:PusherBase {

        public FlaskPusher() {
            if (Settings.Flask1Delay!=int.MaxValue) {
                PusherInfo pusherInfoFlask1 = new PusherInfo();
                pusherInfoFlask1.Code = Settings.Flask1;
                pusherInfoFlask1.Delay = Settings.Flask1Delay;
                CreatePusher(pusherInfoFlask1);
            }

            if (Settings.Flask2Delay != int.MaxValue) {
                PusherInfo pusherInfoFlask2 = new PusherInfo();
                pusherInfoFlask2.Code = Settings.Flask2;
                pusherInfoFlask2.Delay = Settings.Flask2Delay;
                CreatePusher(pusherInfoFlask2);
            }

            if (Settings.Flask3Delay != int.MaxValue) {
                PusherInfo pusherInfoFlask3 = new PusherInfo();
                pusherInfoFlask3.Code = Settings.Flask3;
                pusherInfoFlask3.Delay = Settings.Flask3Delay;
                CreatePusher(pusherInfoFlask3);
            }

            if (Settings.Flask4Delay != int.MaxValue) {
                PusherInfo pusherInfoFlask4 = new PusherInfo();
                pusherInfoFlask4.Code = Settings.Flask4;
                pusherInfoFlask4.Delay = Settings.Flask4Delay;
                CreatePusher(pusherInfoFlask4);
            }

            if (Settings.Flask5Delay != int.MaxValue) {
                PusherInfo pusherInfoFlask5 = new PusherInfo();
                pusherInfoFlask5.Code = Settings.Flask5;
                pusherInfoFlask5.Delay = Settings.Flask5Delay;
                CreatePusher(pusherInfoFlask5);
            }

        }

    }
}
