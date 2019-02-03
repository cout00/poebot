using PoeItemObjectModelLib.Bases;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsFormsApplication2.AreaRunner.InputScript;
using WindowsFormsApplication2.Native;
using WindowsFormsApplication2.Profile;

namespace WindowsFormsApplication2.Parsers {
   
    public static class PublicTowns {
        public const string Lioneyes_Watch = "Lioneye's Watch";
        public const string The_Forest_Encampment = "The Forest Encampment";
        public const string The_Sarn_Encampment = "The Sarn Encampment";
        public const string Highgate = "Highgate";
        public const string Overseers_Tower = "Overseer's Tower";
        public const string The_Bridge_Encampment = "The Bridge Encampment";
        public const string Oriath_Docks = "Oriath Docks";
        public const string Oriath = "Oriath";

        public static bool IsPublicTown(string str) {
            return str.IsOneOf(Lioneyes_Watch, 
                The_Forest_Encampment, 
                The_Sarn_Encampment, 
                Highgate,
                Overseers_Tower, 
                The_Bridge_Encampment, 
                Oriath_Docks, 
                Oriath);
        }
    }

    public static class Hideouts {
        public const string Backstreet_Hideout = "Backstreet Hideout";
        public const string Baleful_Hideout = "Baleful Hideout";
        public const string Battle_scarred_Hideout = "Battle-scarred Hideout";
        public const string Cartographers_Hideout = "Cartographer's Hideout";
        public const string Coastal_Hideout = "Coastal Hideout";
        public const string Desert_Hideout = "Desert Hideout";
        public const string Enlightened_Hideout = "Enlightened Hideout";
        public const string Immaculate_Hideout = "Immaculate Hideout";
        public const string Lush_Hideout = "Lush Hideout";
        public const string Luxurious_Hideout = "Luxurious Hideout";
        public const string Overgrown_Hideout = "Overgrown Hideout";
        public const string Unearthed_Hideout = "Unearthed Hideout";
        public const string Undercity_Hideout = "Undercity Hideout";

        public static readonly List<string> SupportedHideout = new List<string>() { Undercity_Hideout };

        public static bool IsHideout(string str) {
            return str.IsOneOf(Backstreet_Hideout,
                Baleful_Hideout,
                Battle_scarred_Hideout,
                Cartographers_Hideout,
                Coastal_Hideout,
                Desert_Hideout,
                Enlightened_Hideout,
                Lush_Hideout,
                Luxurious_Hideout,
                Overgrown_Hideout,
                Unearthed_Hideout,
                Immaculate_Hideout);
        }
    }



    public interface IGameLogListener {        
        void OnNewGameLocationData(string data);
        void OnHideoutEntered(string hideout);
        void OnDead();
    }

    internal class LogFileReader : LongFileReaderSafe {

        const string EXPR_FULL_MATCH = @".+You have entered |\.";
        const string EXPR = ".+You have entered ";
        const string SLAIN = "has been slain";


        List<IGameLogListener> Listeners = new List<IGameLogListener>();

        LogFileReader(string filePath) : base(filePath) {
        }

        public LogFileReader():this(Path.Combine(NativeApiWrapper.GameFolder, "logs", "client.txt")) {
           
        }

        public void RegisterListener(IGameLogListener logListener) {
            Listeners.Add(logListener);
        }
       
        protected override void OnNewData(string newstr) {
            if (Regex.IsMatch(newstr, SLAIN)) {
                foreach (var item in Listeners) {
                    item.OnDead();
                }
            }
            if (Regex.IsMatch(newstr, EXPR)) {
                var res = Regex.Replace(newstr, EXPR_FULL_MATCH, string.Empty).Trim();
                foreach (var item in Listeners)
                    item.OnNewGameLocationData(res);                
                if (PublicTowns.IsPublicTown(res)) {                    
                    new ReturnToHideoutScript().Run();
                }
                if (Hideouts.IsHideout(res)) {
                    if (Hideouts.SupportedHideout.Contains(res)) {
                        foreach (var item in Listeners)
                            item.OnHideoutEntered(res);
                    }
                    else {
                        throw new NotSupportedException("Hideout" + res + "Not Supported!!!");
                    }
                    
                }
            }
        }
    }
}
