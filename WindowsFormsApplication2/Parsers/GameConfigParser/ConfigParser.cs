using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.Parsers {
    public class ConfigParser {




        public static Dictionary<ConfigKeys, string> Config = new Dictionary<ConfigKeys, string>() {
            { ConfigKeys.borderless_windowed_fullscreen, "false" },
            { ConfigKeys.directx_version, "9" },
            { ConfigKeys.dx11_antialias_mode, "2" },
            { ConfigKeys.dx9_antialias_mode, "0" },
            { ConfigKeys.dynamic_resolution_fps, "60" },
            { ConfigKeys.fullscreen, "false" },
            { ConfigKeys.global_illumination_detail, "0" },
            { ConfigKeys.light_quality, "0" },
            { ConfigKeys.max_PS_shader_model, "ps_3_0" },
            { ConfigKeys.max_VS_shader_model, "vs_3_0" },
            { ConfigKeys.maximize_window, "false" },
            { ConfigKeys.post_processing, "false" },
            { ConfigKeys.resolution_height, "600" },
            { ConfigKeys.resolution_width, "800" },
            { ConfigKeys.screen_shake, "false" },
            { ConfigKeys.screenspace_effects, "0" },
            { ConfigKeys.screenspace_effects_resolution, "0" },
            { ConfigKeys.shadow_type, "hardware_3_samples" },
            { ConfigKeys.texture_quality, "10" },
            { ConfigKeys.texture_filtering, "1" },
            { ConfigKeys.use_dynamic_resolution, "false" },
            { ConfigKeys.vsync, "true" },
            { ConfigKeys.water_detail, "0" },
            { ConfigKeys.username, Settings.UserName },
            { ConfigKeys.use_bound_skill1, "1" },
            { ConfigKeys.use_bound_skill2, "4" },
            { ConfigKeys.use_bound_skill3, "2" },
            { ConfigKeys.use_bound_skill4, "49" },
            { ConfigKeys.use_bound_skill5, "50" },
            { ConfigKeys.use_bound_skill6, "51" },
            { ConfigKeys.use_bound_skill7, "52" },
            { ConfigKeys.use_bound_skill8, "53" },
            { ConfigKeys.use_flask_in_slot1, "81" },
            { ConfigKeys.use_flask_in_slot2, "97" },
            { ConfigKeys.use_flask_in_slot3, "69" },
            { ConfigKeys.use_flask_in_slot4, "82" },
            { ConfigKeys.use_flask_in_slot5, "84" },
            { ConfigKeys.use_temporary_skill1, "5" },
            { ConfigKeys.use_temporary_skill2, "6" },
            { ConfigKeys.minimap_geometry_alpha, "0" },
            { ConfigKeys.minimap_walkability_alpha, "1" },
            { ConfigKeys.minimap_zoom, "5" },

        };

        public enum ConfigKeys {
            borderless_windowed_fullscreen,
            directx_version,
            dx11_antialias_mode,
            dx9_antialias_mode,
            dynamic_resolution_fps,
            fullscreen,
            global_illumination_detail,
            light_quality,
            max_PS_shader_model,
            max_VS_shader_model,
            maximize_window,
            post_processing,
            resolution_height,
            resolution_width,
            screen_shake,
            screenspace_effects,
            screenspace_effects_resolution,
            shadow_type,
            texture_filtering,
            texture_quality,
            use_dynamic_resolution,
            vsync,
            water_detail,
            username,
            use_bound_skill1,
            use_bound_skill2,
            use_bound_skill3,
            use_bound_skill4,
            use_bound_skill5,
            use_bound_skill6,
            use_bound_skill7,
            use_bound_skill8,
            use_flask_in_slot1,
            use_flask_in_slot2,
            use_flask_in_slot3,
            use_flask_in_slot4,
            use_flask_in_slot5,
            use_temporary_skill1,
            use_temporary_skill2,
            minimap_geometry_alpha,
            minimap_walkability_alpha,
            minimap_zoom
        }
        string path;
        public ConfigParser() {
            path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "My Games", "Path of Exile", "production_Config.ini");
        }


        public bool ConfigIsValid(Dictionary<ConfigKeys, string> config) {
            var allStrings = Regex.Split(File.ReadAllText(path), "\n");
            var avialKeys = Enum.GetNames(typeof(ConfigKeys));

            for (int i = 0; i < allStrings.Length; i++) {
                var key = avialKeys.FirstOrDefault(a => (allStrings[i] + "=").Contains(a + "="));
                if (key != null) {                    
                    var value = allStrings[i].Replace(key + "=", string.Empty);
                    var resKey = (ConfigKeys)Enum.Parse(typeof(ConfigKeys), key);
                    if (resKey==ConfigKeys.username)
                        continue;                    
                    var dicValue = config[resKey];
                    if (value!=dicValue) {
                        return false;
                    }
                }
            }
            return true;
        }

        public void SetConfig(Dictionary<ConfigKeys, string> config) {

            Directory.CreateDirectory("Temp");
            File.WriteAllText(@"Temp\OrigConfig.ini", File.ReadAllText(path));
            var allStrings = Regex.Split(File.ReadAllText(path), "\n");
            var avialKeys = Enum.GetNames(typeof(ConfigKeys));

            for (int i = 0; i < allStrings.Length; i++) {
                var key = avialKeys.FirstOrDefault(a => (allStrings[i] + "=").Contains(a + "="));
                if (key != null) {
                    var resKey = (ConfigKeys)Enum.Parse(typeof(ConfigKeys), key);
                    var dicValue = config[resKey];
                    allStrings[i] = resKey + "=" + dicValue;
                }
            }
            File.WriteAllText(path, string.Join("\n", allStrings));
        }

        public Dictionary<ConfigKeys, string> GetConfig() {
            Dictionary<ConfigKeys, string> resDictionary = new Dictionary<ConfigKeys, string>();
            var allStrings = Regex.Split(File.ReadAllText(path), "\n");
            var avialKeys = Enum.GetNames(typeof(ConfigKeys));
            foreach (string str in allStrings) {
                var key = avialKeys.FirstOrDefault(a => (str + "=").Contains(a + "="));
                if (key != null) {
                    var value = str.Replace(key + "=", string.Empty);
                    var resKey = (ConfigKeys)Enum.Parse(typeof(ConfigKeys), key);
                    resDictionary.Add(resKey, value);
                }
            }
            return resDictionary;
        }

    }
}
