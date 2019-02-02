using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.Parsers {


   
    public enum LocationID {        
        Aqueduct,
        Undercity_Hideout,
        The_Blood_Aqueduct,
    }


    internal class LogFileReader : LongFileReaderSafe {

        const string EXPR_FULL_MATCH = @".+You have entered |\.";
        const string EXPR = ".+You have entered ";

        LogFileReader(string filePath) : base(filePath) {
        }

        public LogFileReader():this(Path.Combine(NativeApiWrapper.GameFolder, "logs", "client.txt")) {

            var test = "2019/02/02 11:56:20 1942687 a21 [INFO Client 4464] @From NiddleSummon: Hi, I'd like to buy your 50 journeyman sextant for my 135 chaos in Standard.";
            var test2 = "2019/02/02 11:49:58 1560343 a21 [INFO Client 1360] : You have entered Undercity Hideout.";
            if (Regex.IsMatch(test, EXPR)) {
               var res = Regex.Replace(test, EXPR_FULL_MATCH, string.Empty);
            }

        }

        protected override void OnNewData(string newstr) {
            
        }
    }
}
