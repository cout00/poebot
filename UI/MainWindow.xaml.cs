﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using MahApps.Metro.Controls;
using System.IO;
using System.Net;

using PoeItemObjectModelLib;
using PoeItemObjectModelLib.Elements;

namespace UI {

    public partial class MainWindow :MetroWindow {
        public MainWindow() {
            InitializeComponent();
            ItemFactory factory = new ItemFactory();
            var test = factory.GetModel();
            Pickit pickit=new Pickit();
            var valid = pickit.IsValid(test);

        }
    }
}
