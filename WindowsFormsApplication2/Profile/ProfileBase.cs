using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.AreaRunner;
using WindowsFormsApplication2.AreaRunner.InputScript;
using WindowsFormsApplication2.Native;
using WindowsFormsApplication2.Parsers;

namespace WindowsFormsApplication2.Profile {

    public abstract class ProfileBase<TSettings> where TSettings : ProfileSetting {
        public abstract string Name { get; }
        public abstract TSettings Settings { get; }

        public void Run() {
            var logFileListener = new LogFileReader();
            logFileListener.RegisterListener(GetListener());
            NativeApiWrapper.RunGame();
        }

        protected abstract IGameLogListener GetListener();     
    }




    public class AqueducProfile : ProfileBase<AqueductSettings>, IGameLogListener {
        public override string Name => "Blood Aqueduct";
        const string LocationName = "The Blood Aqueduct";

        AqueductSettings settings = new AqueductSettings();


        public override AqueductSettings Settings {
            get {
                return settings;
            }
        }

        AqueductRunner aqueductRunner;

        void IGameLogListener.OnNewGameLocationData(string data) {
            if (data == LocationName) {
                aqueductRunner = new AqueductRunner();
                aqueductRunner.AreaEnded += AreaEnded;
            }
        }

        void AreaEnded(object sender, System.EventArgs e) {
            RelogScript relogScript = new RelogScript();
            relogScript.Run();
        }

        protected override IGameLogListener GetListener() {
            return this;
        }

        void IGameLogListener.OnHideoutEntered(string hideout) {
            OpenGameWorldMapImmediately openGameWorld = new OpenGameWorldMapImmediately();
            openGameWorld.Run();
        }

        void IGameLogListener.OnDead() {
            if (aqueductRunner!=null) {
                aqueductRunner.Abort();
                aqueductRunner = null;
                RelogScript relogScript = new RelogScript();
                relogScript.Run();
            }
        }
    }

}
