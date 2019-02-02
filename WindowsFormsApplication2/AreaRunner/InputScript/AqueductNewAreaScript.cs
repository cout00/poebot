using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.InputScript {


    public class GameRunScript : InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoDelay(() => 15000);
            script.DoDelay(() => {
                NativeApiWrapper.InitGameInstance();
                return 200;
            });
            script.DoInput(() => InputCodes.Space);
            script.DoInput(() => InputCodes.Space);
            script.DoInput(() => InputCodes.Space);
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(344, 448)); //go to password box
            script.DoInputWithModifiers(() => {
                Clipboard.SetText(Settings.Password);
                return new KeyWithModifier() { Key=InputCodes.V, Modifier=InputCodes.LControlKey };
            });
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(564, 425)); // click login
            script.DoInput(() => InputCodes.Return, 1500);
            return script;
        }
    }



    public class AqueductNewAreaScript : InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(184, 97));
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(223, 117));
            script.DoMouseMove(() => new System.Drawing.Point(155, 316));
            script.DoInputWithModifiers(() => new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey });
            script.DoDelay(() => 600);
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(128, 217));
            return script;
        }
    }

    class ZanaTradeScript : InputScriptBase {
        protected override Script StartRecord() {
            int step = 30;
            int startX = 455;
            int endX = 786;
            int starty = 370;
            int endY = 491;
            var script = new Script();
            script.DoDelay(() => 2500);
            Queue<Point> capturedPoints = new Queue<Point>();

            for (int Y = starty; Y < endY; Y += step) {
                for (int X = startX; X < endX; X += step) {
                    capturedPoints.Enqueue(new Point(X, Y));
                    script.DoMouseMove(() => {
                        return capturedPoints.Dequeue();
                    });
                    script.DoScriptPart(() => {
                        if (true) {
                            return (Func<KeyWithModifier>)(() => new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey });
                        }
                    });
                }
            }



            return script;
        }
    }

    public class HideoutAreaScript : InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();

            script.DoDelay(() => 2500);


            script.DoMouseMoveWithClick(() => new System.Drawing.Point(230, 250)); //go to zana
            script.DoDelay(() => 400); //do somewith chest
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(393, 190)); //open trade
            script.DoDelay(() => 1200); // do with zana
            script.DoInput(() => InputCodes.Escape); //close trade
            script.DoInput(() => InputCodes.Escape); //close zana
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(579, 190)); //go to chest
            script.DoDelay(() => 1200);
            script.DoInput(() => InputCodes.Escape); //close chest
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(328, 433)); //go to way point
            script.DoDelay(() => 400);
            script.DoScript(() => new AqueductNewAreaScript());
            return script;
        }
    }


}
