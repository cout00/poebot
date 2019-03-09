using System;
using System.Collections.Generic;
using System.Linq;

namespace WindowsFormsApplication2.Profile {
    public abstract class ProfileSetting {
        protected class GetPropertyDescriptor {
            public string Key { get; set; }
            public string SettingsName { get; set; }
            public Func<object> Value { get; set; }
            public object Available { get; set; }
        }


        private IList<SettingsElement> _settingsElements;
        public abstract string ProfileName { get; }
        public IList<SettingsElement> SettingsElements {
            get {
                foreach (var item in UpdatePropertyTriggers) {
                    var propertyValue = item.Value();
                    var propertyName = _settingsElements.Where(a => a.Key == item.Key).FirstOrDefault();
                    if (propertyName == null) {
                        _settingsElements.Add(new SettingsElement() { SettingsName = item.SettingsName, Value = propertyValue, Available = item.Value, Key = item.Key });
                    }
                    else {
                        var index = _settingsElements.IndexOf(propertyName);
                        var value = _settingsElements[index].Value;
                        if (value != propertyValue) {
                            _settingsElements[index].Value = propertyValue;
                        }
                    }
                }
                return _settingsElements;
            }
            private set { _settingsElements = value; }
        }

        List<GetPropertyDescriptor> UpdatePropertyTriggers = new List<GetPropertyDescriptor>();

        public ProfileSetting() {
            SettingsElements = new List<SettingsElement>();
            UpdatePropertyTriggers.Add(new GetPropertyDescriptor() { Available = Enumerable.Range(1, 10), Key = nameof(MaxRunCountBeforeSellItems), SettingsName = "Max run count before sell items", Value = () => MaxRunCountBeforeSellItems });
            InitSettings(UpdatePropertyTriggers);
        }

        public int MaxRunCountBeforeSellItems { get; set; }

        protected abstract void InitSettings(List<GetPropertyDescriptor> properties);

        public virtual void ApplySettings() {
            foreach (var item in SettingsElements) {
                if (item.Key == nameof(MaxRunCountBeforeSellItems)) {
                    MaxRunCountBeforeSellItems = (int)item.Value;
                }
            }
        }
    }
}
