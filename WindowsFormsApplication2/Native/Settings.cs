using PoeItemObjectModelLib;
using Process.NET.Native.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2.Native {
    public static class Settings {

        public static Pickit Pickit { get; } = new Pickit();
        public static ItemFactory ItemFactory { get; } = new ItemFactory();
        public static string UserName = "aizik.frost2@bk.ru";
        public static string Password = "justforhuk123";

        public static InputCodes MoveKey { get; } = InputCodes.D3;
        public static InputCodes MainAttack { get; } = InputCodes.RButton;


        public static InputCodes Flask1 { get; } = InputCodes.Q;
        public static InputCodes Flask2 { get; } = InputCodes.W;
        public static InputCodes Flask3 { get; } = InputCodes.E;
        public static InputCodes Flask4 { get; } = InputCodes.R;
        public static InputCodes Flask5 { get; } = InputCodes.T;

        public static int Flask1Delay { get; } = 6000;
        public static int Flask2Delay { get; } = 4000;
        public static int Flask3Delay { get; } = 7000;
        public static int Flask4Delay { get; } = 3000;
        public static int Flask5Delay { get; } = 3000;


        public static int Skill5Delay { get; } = 4000;

        public static InputCodes Skill1 { get; } = InputCodes.LButton;
        public static InputCodes Skill2 { get; } = InputCodes.MButton;
        public static InputCodes Skill3 { get; } = InputCodes.RButton;
        public static InputCodes Skill4 { get; } = InputCodes.D1;
        public static InputCodes Skill5 { get; } = InputCodes.D2;
        public static InputCodes Skill6 { get; } = InputCodes.D3;
        public static InputCodes Skill7 { get; } = InputCodes.D4;
        public static InputCodes Skill8 { get; } = InputCodes.D5;

        public static int AttackDelay { get; } = 1500;
        public static double GlobalScriptDelayModifier { get; } = 1;
    }
}
