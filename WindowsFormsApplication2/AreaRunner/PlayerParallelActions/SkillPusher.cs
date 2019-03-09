using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.AreaRunner.PlayerParallelActions {
    public class SkillPusher:PusherBase {
        public SkillPusher() {
            PusherInfo pusherSkill5 = new PusherInfo();
            pusherSkill5.Code = Settings.Skill5;
            pusherSkill5.Delay = Settings.Skill5Delay;

            PusherInfo pusherSkill4 = new PusherInfo();
            pusherSkill4.Code = Settings.Skill4;
            pusherSkill4.Delay = Settings.Skill4Delay;

            CreatePusher(pusherSkill5);
            CreatePusher(pusherSkill4);
        }
    }
}
