using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.AreaRunner;
using WindowsFormsApplication2.AreaRunner.InputScript;
using WindowsFormsApplication2.Native;
using WindowsFormsApplication2.Parsers;

namespace WindowsFormsApplication2.Profile {

    public abstract class ProfileBase<TSettings, TAreaRunner, TScriptStarter> : IGameLogListener where TSettings : ProfileSetting where TAreaRunner:AreaRunnerBase, new() where TScriptStarter:InputScriptBase, new() {
        public abstract string Name { get; }
        public abstract TSettings Settings { get; }
        protected TAreaRunner AreaRunner { get; private set; }
        protected TScriptStarter scriptStarter { get; private set; }
        bool needResetAuraSkills = false;

        protected abstract string LocationName { get; }
        int runCount = 0;

        public void Run() {
            var logFileListener = new LogFileReader();
            logFileListener.RegisterListener(GetListener());
            NativeApiWrapper.RunGame();
        }

        protected abstract IGameLogListener GetListener();

        public void RunSafe() {
            var logFileListener = new LogFileReader();
            logFileListener.RegisterListener(GetListener());
            NativeApiWrapper.InitGameInstance();
            RelogScript relogScript = new RelogScript();
            relogScript.Run();
        }

        public void RunSafe(System.Diagnostics.Process process) {
            var logFileListener = new LogFileReader();
            logFileListener.RegisterListener(GetListener());
            NativeApiWrapper.InitGameInstance(process);
            RelogScript relogScript = new RelogScript();
            relogScript.Run();
        }

        //System.Diagnostics.Process process

        protected virtual void OnNewGameLocationData(string data) {

        }

        protected virtual void OnHideoutProcessed(string hideout) {

        }

        protected virtual void OnDead() {

        }

        void IGameLogListener.OnNewGameLocationData(string data) {
            if (data == LocationName) {
                TScriptStarter scriptStarter = new TScriptStarter();
                scriptStarter.Completed += (s, e) => {
                    if (needResetAuraSkills) {
                        needResetAuraSkills = false;
                        ResetAuraSkills resetAuraSkills = new ResetAuraSkills();
                        resetAuraSkills.Run();
                    }
                    AreaRunner = new TAreaRunner();
                    AreaRunner.AreaEnded += AreaEnded;
                };
                scriptStarter.Run();
            }
        }

        void AreaEnded(object sender, System.EventArgs e) {
            RelogScript relogScript = new RelogScript();
            relogScript.Run();
        }

        void IGameLogListener.OnHideoutEntered(string hideout) {
            if (runCount == Settings.MaxRunCountBeforeSellItems) {
                runCount = 0;
                HideoutTradeScript hideoutTradeScript = new HideoutTradeScript();
                hideoutTradeScript.DoAfter(() => OnHideoutProcessed(hideout));
                hideoutTradeScript.Run();
            }
            else {
                runCount++;
                OnHideoutProcessed(hideout);
            }
        }

        void IGameLogListener.OnDead() {
            if (AreaRunner != null) {
                needResetAuraSkills = true;
                AreaRunner.Abort();
                AreaRunner = null;
                RelogScript relogScript = new RelogScript();
                relogScript.Run();
            }
            OnDead();
        }
    }




    public class AqueducProfile : ProfileBase<AqueductSettings, AqueductRunner, StartAqueducScript> {
        public override string Name => "Blood Aqueduct";
        protected override string LocationName => "The Blood Aqueduct";
        
        AqueductSettings settings = new AqueductSettings();


        public override AqueductSettings Settings {
            get {
                return settings;
            }
        }

             
        protected override IGameLogListener GetListener() {
            return this;
        }

        protected override void OnHideoutProcessed(string hideout) {
            OpenGameWorldMapImmediately openGameWorld = new OpenGameWorldMapImmediately();
            openGameWorld.Run();
            openGameWorld.Completed += (s, e) => {
                if (settings.Act == 9) {
                    AqueductNewAreaFromNineAct aqueductNewAreaFromNineAct = new AqueductNewAreaFromNineAct();
                    aqueductNewAreaFromNineAct.Run();
                }
                if (settings.Act == 10) {
                    AqueductNewAreFromTenAct aqueductNewAreFromTenAct = new AqueductNewAreFromTenAct();
                    aqueductNewAreFromTenAct.Run();
                }
            };
        }
    }

}
