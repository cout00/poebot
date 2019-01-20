using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.Parsers.GameLogParser {


    public enum LocationID {
        Hideout,
        Aqueduct,

    }


    internal class LogFileReader : LongFileReaderSafe {
        LogFileReader(string filePath) : base(filePath) {
        }

        public LogFileReader():this(Path.Combine(NativeApiWrapper.GameFolder, "logs", "client.txt")) {

        }


    }
}
