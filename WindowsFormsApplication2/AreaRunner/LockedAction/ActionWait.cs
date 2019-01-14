using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.AreaRunner.LockedAction {

    public class ActionWaitParameter : LockedActionParam {
        public int Interval { get; set; } = 1000;
    }

    public class ActionWait : ILockedAction<ActionWaitParameter> {
        public bool Locked { get; set; }

        public Func<Task> DoLockedAction(ActionWaitParameter param) {
            Locked = true;
            return () => Task.Delay(param.Interval);
        }
    }
}
