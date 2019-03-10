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
using WindowsFormsApplication2.AreaRunner.InputScript;
using PoeItemObjectModelLib;
using WindowsFormsApplication2.Parsers;
using WindowsFormsApplication2.Profile;

namespace WindowsFormsApplication2 {

    //r-138 g-137 b-161 -Lowest
    //r-154 g-153 b-194 -H







    public partial class Form1 : Form {

        
        public Form1() {
            InitializeComponent();
            var processes = System.Diagnostics.Process.GetProcessesByName(NativeApiWrapper.GameProcessName).Where(a => a.MainWindowHandle != IntPtr.Zero);
            comboBox1.DataSource = processes.ToList();
            comboBox1.DisplayMember = "MainWindowHandle";
            comboBox1.SelectedItem = processes.FirstOrDefault();
            comboBox1.SelectedIndexChanged += comboBox1_SelectedIndexChanged;
        }


        private void button1_Click(object sender, EventArgs e) {

            AqueducProfile aqueducProfile = new AqueducProfile();
            aqueducProfile.Settings.Act = 10;
            aqueducProfile.RunSafe(comboBox1.SelectedItem as System.Diagnostics.Process);

            //NativeApiWrapper.InitGameInstance();
            //HideoutTradeScript aqueductNewAreFromTenAct = new HideoutTradeScript();
            //aqueductNewAreFromTenAct.Run();
            ////AqueductNewAreaScript aqueductNewAreaScript = new AqueductNewAreaScript();
            ////aqueductNewAreaScript.Run();


            //LogFileReader logFileReader = new LogFileReader();
            //logFileReader.NewData += LogFileReader_NewData;         
            ////AvailableInput.MouseMove(new Point(50, 50));
            ////AvailableInput.Input(InputCodes.LButton);
            ////AvailableInput.Input(InputCodes.Return);
            ////AvailableInput.InputCombination(InputCodes.ControlKey, InputCodes.V);

            ////ItemFactory itemFactory = new ItemFactory();
            ////var item = itemFactory.GetModel();
            ////var res = Settings.Pickit.IsValid(item);
        }

        private void LogFileReader_NewData(object sender, string e) {
            //richTextBox1.Text += e;
        }

        private void OnResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            //pictureBox1.Image = e.ImageProcessorResult.ResultBitmap;
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void Form1_Shown(object sender, EventArgs e) {

        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            //pictureBox1.Image = (Bitmap)listBox1.SelectedValue;
        }

        private void button1_Click_1(object sender, EventArgs e) {
            AqueducProfile aqueducProfile = new AqueducProfile();
            aqueducProfile.Settings.Act = 9;
            aqueducProfile.RunSafe(comboBox1.SelectedItem as System.Diagnostics.Process);
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var handle = comboBox1.SelectedItem as System.Diagnostics.Process;
            NativeApiWrapper.FlashByWindow(handle.MainWindowHandle);
        }
        LootProcessor2 lootProcessor2;

        private void button2_Click(object sender, EventArgs e) {
            NativeApiWrapper.InitGameInstance(comboBox1.SelectedItem as System.Diagnostics.Process);
            lootProcessor2 = new LootProcessor2();
            lootProcessor2.OnResult += LootProcessor2_OnResult;
        }

        private void LootProcessor2_OnResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            if (string.IsNullOrEmpty(BMin.Text) || string.IsNullOrEmpty(GMin.Text) || string.IsNullOrEmpty(RMin.Text) || string.IsNullOrEmpty(BMax.Text) || string.IsNullOrEmpty(GMax.Text) || string.IsNullOrEmpty(RMax.Text)) {
                return;
            }
            //e.ImageProcessorResult.ResultBitmap.Save("test.bmp");
            lootProcessor2.bgrMin = new Bgr(int.Parse(BMin.Text), int.Parse(GMin.Text), int.Parse(RMin.Text));
            lootProcessor2.bgrMax = new Bgr(int.Parse(BMax.Text), int.Parse(GMax.Text), int.Parse(RMax.Text));
            pictureBox1.Image = e.ImageProcessorResult.ResultBitmap;
        }
    }
}
