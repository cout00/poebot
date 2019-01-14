using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.AreaRunner.LockedAction {

    public class ActionNewAreaParameter:LockedActionParam {

    }

    public class ActionNewArea : ILockedAction<ActionNewAreaParameter> {
        public bool Locked { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public Func<Task> DoLockedAction(ActionNewAreaParameter param) {
            throw new NotImplementedException();
        }
    }

}
