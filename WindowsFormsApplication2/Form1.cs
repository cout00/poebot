using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Emgu.CV.Structure;
using Emgu.CV;
using Emgu.CV.Util;
using Emgu.CV.CvEnum;
using System.Runtime.InteropServices;
using System.Diagnostics;
using WindowsFormsApplication2.Native;
using WindowsFormsApplication2.GameScreenReader;
using WindowsFormsApplication2.ImageProcessor;
using WindowsFormsApplication2.AreaRunner;
using WindowsFormsApplication2.Logger;
using Process.NET;
using Process.NET.Memory;
using System.IO;
using WindowsFormsApplication2.AreaRunner.LockedAction;
using Microsoft.Win32;
using WindowsFormsApplication2.Parsers.GameLogParser;
using WindowsFormsApplication2.AreaRunner.InputScript;
using WindowsFormsApplication2.Parsers.ItemParser;
using PoeItemObjectModelLib;

namespace WindowsFormsApplication2 {

    //r-138 g-137 b-161 -Lowest
    //r-154 g-153 b-194 -H







    public partial class Form1 : Form {

        
        public Form1() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {

            //LogFileReader logFileReader = new LogFileReader();
            //logFileReader.NewData += LogFileReader_NewData;         
            //AvailableInput.MouseMove(new Point(50, 50));
            //AvailableInput.Input(InputCodes.LButton);
            //AvailableInput.Input(InputCodes.Return);
            //AvailableInput.InputCombination(InputCodes.ControlKey, InputCodes.V);

            ItemFactory itemFactory = new ItemFactory();
            var item = itemFactory.GetModel();
            var res = Settings.Pickit.IsValid(item);
        }

        private void LogFileReader_NewData(object sender, string e) {
            richTextBox1.Text += e;
        }

        private void OnResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            pictureBox1.Image = e.ImageProcessorResult.ResultBitmap;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void Form1_Shown(object sender, EventArgs e) {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            pictureBox1.Image = (Bitmap)listBox1.SelectedValue;
        }
    }
}
