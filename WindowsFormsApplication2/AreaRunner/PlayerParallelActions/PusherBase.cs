using Process.NET.Native.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.PlayerParallelActions {
    public abstract class PusherBase {

        class InstancePusherInfo : PusherInfo {
            public Timer Instance { get; set; }
        }

        protected class PusherInfo {
            public InputCodes Code { get; set; }
            public int Delay { get; set; }
            public Action BeforePush { get; set; }
            public Action AfterPush { get; set; }
        }

        List<InstancePusherInfo> ActivePushers { get; } = new List<InstancePusherInfo>();

        protected void CreatePusher(PusherInfo pusherInfo) {

            Timer timer = new Timer();
            timer.Interval = pusherInfo.Delay;
            timer.Tick += OnTick;
            timer.Start();
            ActivePushers.Add(new InstancePusherInfo() { AfterPush = pusherInfo.AfterPush, BeforePush = pusherInfo.BeforePush, Delay = pusherInfo.Delay, Code = pusherInfo.Code, Instance = timer });

        }

        private void OnTick(object sender, EventArgs e) {
            var key = ActivePushers.FirstOrDefault(a => a.Instance == sender);
            key.BeforePush?.Invoke();
            AvailableInput.Input(key.Code);
            key.AfterPush?.Invoke();
        }

        protected void DestroyPusher(InputCodes key) {
            if (ActivePushers.FirstOrDefault(a=>a.Code==key)!=null) {
                ActivePushers.FirstOrDefault(a => a.Code == key).Instance.Dispose();
            }
        }

    }
}
