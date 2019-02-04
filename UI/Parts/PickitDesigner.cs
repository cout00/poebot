using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace UI.Parts {
    public class PickitDesigner :Control {
        static PickitDesigner() {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(PickitDesigner),
                new FrameworkPropertyMetadata(typeof(PickitDesigner)));
        }

    }
}
