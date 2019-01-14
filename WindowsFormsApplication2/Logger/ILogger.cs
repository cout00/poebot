using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Logger {
    public interface ILogger {
        void WriteLog(string msg);       
    }
}
