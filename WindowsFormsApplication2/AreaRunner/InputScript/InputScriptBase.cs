using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.AreaRunner.LockedAction;
using WindowsFormsApplication2.Logger;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.InputScript {
    public abstract class InputScriptBase : ISupportLock {

        protected static KeyWithModifier Ctrl_C = new KeyWithModifier() { Key = InputCodes.C, Modifier = InputCodes.LControlKey };
        protected static KeyWithModifier Ctrl_V = new KeyWithModifier() { Key = InputCodes.V, Modifier = InputCodes.LControlKey };
        protected static KeyWithModifier CallNewInstanceWindow = new KeyWithModifier() { Key = InputCodes.LButton, Modifier = InputCodes.LControlKey };
        protected static string HideoutCommand = @"/hideout";

        private class FOR_INTERNAL_USE_ONLY : InputScriptBase {
            private readonly object func;

            public FOR_INTERNAL_USE_ONLY(object func) {
                this.func = func;
                script = StartRecord();
            }

            protected override Script StartRecord() {
                Script script = new Script();
                script.ScriptParts.Enqueue(func);
                return script;
            }
        }



        public const int STANDART_DELAY = 150;
        public const double MODIFIER = 1;


        protected class KeyWithModifier {
            public InputCodes Modifier { get; set; }
            public InputCodes Key { get; set; }
        }


        protected class Script {

            public Queue<object> ScriptParts = new Queue<object>();

            public void DoInput(Func<InputCodes> inputFunc, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                ScriptParts.Enqueue(inputFunc);
            }

            public void DoDelay(Func<double> inputFunc) {
                ScriptParts.Enqueue(inputFunc);
            }

            public void DoMouseMove(Func<Point> inputFuncXY, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                ScriptParts.Enqueue(inputFuncXY);
            }

            public void DoMouseMoveWithClick(Func<Point> inputFuncXY, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                ScriptParts.Enqueue(inputFuncXY);
                DoInput(() => InputCodes.LButton);
            }

            public void DoInputWithModifiers(Func<KeyWithModifier> inputWithModifiers, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                ScriptParts.Enqueue(inputWithModifiers);
            }

            public void DoScript(Func<InputScriptBase> inputScript, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                ScriptParts.Enqueue(inputScript);
            }

            public void DoScriptPart(Func<object> inputPart, double afterDelay = MODIFIER) {
                DoDelay(() => afterDelay);
                var someFunc = inputPart();
                FOR_INTERNAL_USE_ONLY fOR_INTERNAL_USE_ONLY = new FOR_INTERNAL_USE_ONLY(someFunc);
                DoScript(() => fOR_INTERNAL_USE_ONLY);
            }
        }

        Script script;
        Func<bool> validationPredicate;

        [Obsolete("Use DoAfter")]
        public event EventHandler Completed;

        public InputScriptBase() {
            script = StartRecord();
        }

        public bool Locked { get; set; }

        
        protected virtual void DoRecord(Script script) {

        }

        [Obsolete][EditorBrowsable(EditorBrowsableState.Never)]
        protected virtual Script StartRecord() {
            var script = new Script();
            DoRecord(script);
            return script;            
        }


        public void Run() {
            if (script == null)
                return;
            Timer timer = new Timer();
            timer.Tick += TimerTick;
            Locked = true;
            TimerTick(timer, null);
        }



        public void Validate(Func<bool> validationPredicate) {
            this.validationPredicate = validationPredicate;
        }

        public void DoAfter(Action afterAction) {
            Completed += (s, e) => afterAction();
        }

        bool IsDelay(object obj) {
            return obj is Func<double>;
        }

        bool IsInput(object obj) {
            return obj is Func<InputCodes>;
        }
        bool IsMouseMove(object obj) {
            return obj is Func<Point>;
        }

        bool IsModifiers(object obj) {
            return obj is Func<KeyWithModifier>;
        }

        bool IsScript(object obj) {
            return obj is Func<InputScriptBase>;
        }

        void TimerTick(object sender, EventArgs e) {
            var timer = sender as Timer;
            if (!script.ScriptParts.Any()) {
                Stop(timer);
                return;
            }
            var func = script.ScriptParts.Dequeue();
            if (IsInput(func)) {
                var scriptAction = (Func<InputCodes>)func;
                var input = scriptAction();
                AvailableInput.Input(input);
                timer.Stop();
                TimerTick(sender, e);
                return;
            }

            if (IsModifiers(func)) {
                var scriptAction = (Func<KeyWithModifier>)func;
                var input = scriptAction();
                AvailableInput.InputCombination(input.Modifier, input.Key);
                timer.Stop();
                TimerTick(sender, e);
                return;
            }

            if (IsDelay(func)) {
                var scriptAction = (Func<double>)func;
                var delay = (int)(scriptAction() * STANDART_DELAY * Settings.GlobalScriptDelayModifier);
                timer.Interval = delay;
                timer.Start();
                return;
            }

            if (IsScript(func)) {
                var scriptAction = (Func<InputScriptBase>)func;
                var script = scriptAction();
                timer.Stop();
                script.Completed = (s, args) => TimerTick(timer, null);
                script.Run();
                return;
            }

            if (IsMouseMove(func)) {
                var scriptAction = (Func<Point>)func;
                var input = scriptAction();
                AvailableInput.MouseMove(input);
                timer.Stop();
                TimerTick(sender, e);
                return;
            }
            Stop(timer);
        }

        void Stop(Timer timer) {
            timer.Stop();
            timer.Dispose();
            Locked = false;
            if (validationPredicate != null) {
                if (validationPredicate())
                    Completed?.Invoke(this, null);
                else
                    Run();
                return;
            }
            Completed?.Invoke(this, null);
        }
    }
}
