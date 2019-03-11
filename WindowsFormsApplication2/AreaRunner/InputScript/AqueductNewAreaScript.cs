using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Native;

using PoeItemObjectModelLib.PickitEngine;

namespace WindowsFormsApplication2.AreaRunner.InputScript {

    public class ReturnToHideoutScript :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoInput(() => {
                Clipboard.SetText(HideoutCommand);
                return InputCodes.Return;
            }, 80);
            script.DoInputWithModifiers(() => Ctrl_V);
            script.DoInput(() => InputCodes.Return);
            return script;
        }
    }


    public class GameRunScript :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoDelay(() => 100);
            script.DoDelay(() => {
                NativeApiWrapper.InitGameInstance();
                return 1.5;
            });
            script.DoInput(() => InputCodes.Space);
            script.DoInput(() => InputCodes.Space);
            script.DoInput(() => InputCodes.Space);
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(344, 448)); //go to password box
            script.DoInputWithModifiers(() => {
                Clipboard.SetText(Settings.Password);
                return Ctrl_V;
            });
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(564, 425)); // click login
            script.DoInput(() => InputCodes.Return, 10);
            return script;
        }
    }


    public class UpGemsScript :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoDelay(() => {
                AvailableInput.InputSimulator.Keyboard.KeyDown(AvailableInput.InputToVirtualKeyCode(Settings.AttackInPlace));
                return 2;
            });
            for (int i = 0; i < 8; i++) {
                script.DoMouseMoveWithClick(() => new Point(766, 200));
            }
            script.DoDelay(() => {
                AvailableInput.InputSimulator.Keyboard.KeyUp(AvailableInput.InputToVirtualKeyCode(Settings.AttackInPlace));
                return 2;
            });
            return script;
        }
    }

    public class OpenGameWorldMapImmediately :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoDelay(() => 10);
            if (Settings.UpGems)
                script.DoScript(() => new UpGemsScript());
            script.DoMouseMoveWithClick(() => {
                return new Point(397, 302);
            });
            return script;
        }
    }





    public class RelogScript :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoInput(() => InputCodes.Escape);
            script.DoMouseMoveWithClick(() => new Point(396, 295));
            script.DoInput(() => InputCodes.Return, 30);
            script.DoInput(() => InputCodes.Return, 5);
            return script;
        }
    }

    public class ResetAuraSkills :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoInput(() => Settings.Skill7, 4);
            script.DoInput(() => Settings.Skill8);
            return script;
        }
    }

    public class StartAqueducScript :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoMouseMoveWithClick(() => new Point(149, 217), 10);
            script.DoMouseMoveWithClick(() => new Point(175, 141), 2);
            script.DoMouseMoveWithClick(() => new Point(175, 141), 2);
            script.DoMouseMoveWithClick(() => new Point(175, 141), 2);
            script.DoMouseMoveWithClick(() => new Point(175, 141), 2);
            script.DoMouseMoveWithClick(() => new Point(175, 141), 2);
            script.DoInput(() => InputCodes.Tab);
            return script;
        }
    }

    public class AqueductNewAreaFromNineAct :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoScript(() => new AqueductNewAreaBase(), 10);
            return script;
        }
    }

    public class AqueductNewAreFromTenAct :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoMouseMoveWithClick(() => new Point(224, 116), 30);
            script.DoScript(() => new AqueductNewAreaBase(), 3);
            return script;
        }
    }

    public class AqueductNewAreaBase :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoMouseMove(() => new System.Drawing.Point(155, 316));
            script.DoInputWithModifiers(() => new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey });
            script.DoDelay(() => 4);
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(128, 217));
            return script;
        }
    }


    public class HideoutTradeScript :InputScriptBase {

        protected override void DoRecord(Script script) {
            script.DoMouseMoveWithClick(() => new Point(428, 164), 10);
            script.DoMouseMoveWithClick(() => new Point(400, 166), 10);
            script.DoScript(() => new TradeScript(), 6);
            script.DoMouseMoveWithClick(() => new Point(220, 317), 10);
            script.DoScript(() => new PutAllItems());
            script.DoInput(() => InputCodes.Escape, 10);
            script.DoMouseMove(() => new Point(520, 324), 5);
            script.DoInput(() => Settings.MoveKey, 2);
        }
    }

    class PutAllItems :InputScriptBase {

        protected override void DoRecord(Script script) {
            int step = 30;
            int startX = 455;
            int endX = 786;
            int starty = 370;
            int endY = 491;
            script.DoDelay(() => 10);
            Queue<Point> capturedPoints = new Queue<Point>();

            for (int Y = starty; Y < endY; Y += step) {
                for (int X = startX; X < endX; X += step) {
                    capturedPoints.Enqueue(new Point(X, Y));
                    script.DoMouseMove(() => {
                        return capturedPoints.Dequeue();
                    }, 0.1);
                    script.DoScriptPart(() => {
                        return (Func<KeyWithModifier>)(() => new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey });
                    }, 0.1);
                }
            }
        }
    }

    class TradeScript :InputScriptBase {

        protected override void DoRecord(Script script) {
            int step = 30;
            int startX = 455;
            int endX = 786;
            int starty = 370;
            int endY = 491;
            script.DoDelay(() => 10);
            Queue<Point> capturedPoints = new Queue<Point>();

            for (int Y = starty; Y < endY; Y += step) {
                for (int X = startX; X < endX; X += step) {
                    capturedPoints.Enqueue(new Point(X, Y));
                    script.DoMouseMove(() => {
                        return capturedPoints.Dequeue();
                    }, 0.2);
                    script.DoInputWithModifiers(() => Ctrl_C, 0.2);
                    script.DoScriptPart(() => {
                        if (!TempPickit.Validate()) {
                            Console.WriteLine("not valid");
                            return (Func<KeyWithModifier>)(() => new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey });
                        }
                        Console.WriteLine("valid");
                        return null;
                    }, 0.2);
                }
            }
            script.DoMouseMove(() => new Point(75, 486), 3);
            if (Settings.IS_DEBUG_SELLING) {
                script.DoInput(() => InputCodes.Escape, 10);
            } else {
                script.DoInput(() => InputCodes.LButton, 10);
            }
        }
    }

    public class AqueductNewAreaScriptFromAriat :InputScriptBase {
        protected override Script StartRecord() {
            var script = new Script();
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(184, 97));
            script.DoMouseMoveWithClick(() => new System.Drawing.Point(223, 117));
            script.DoScript(() => new AqueductNewAreaBase());
            return script;
        }
    }

    class ZanaTradeScript :InputScriptBase {
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

    public class HideoutAreaScript :InputScriptBase {
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
            script.DoScript(() => new AqueductNewAreaScriptFromAriat());
            return script;
        }
    }


}
