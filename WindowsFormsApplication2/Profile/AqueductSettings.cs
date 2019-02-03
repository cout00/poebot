using System.Collections.Generic;

namespace WindowsFormsApplication2.Profile {
    public class AqueductSettings : ProfileSetting {
        public static readonly IList<int> SupportedActs = new List<int>() { 9, 10, 11 };

        public override string ProfileName => "Aqueduc settings";
        public int Act { get; set; }


        public override void ApplySettings() {
            foreach (var item in SettingsElements) {
                if (item.Key==nameof(Act)) {
                    Act = (int)item.Value;
                }
            }
        }

        protected override void InitSettings(List<GetPropertyDescriptor> properties) {
            properties.Add(new GetPropertyDescriptor() { SettingsName = "Player available act", Value = () => Act, Available = SupportedActs, Key = nameof(Act) });
        }
    }
}
