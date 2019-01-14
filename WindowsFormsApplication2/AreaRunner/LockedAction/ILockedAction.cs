using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.AreaRunner.LockedAction {

    public abstract class LockedActionParam {

    }

    public class EmptyParam:LockedActionParam {

    }

    public interface ISupportLock {
        bool Locked { get; set; }
    }

    public interface ILockedAction<TParam>:ISupportLock where TParam : LockedActionParam {        
        Func<Task> DoLockedAction(TParam param);
    }


}
