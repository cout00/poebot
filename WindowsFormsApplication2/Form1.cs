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
using WindowsFormsApplication2.Parsers.ItemParser;
using System.IO;
using WindowsFormsApplication2.AreaRunner.LockedAction;

namespace WindowsFormsApplication2 {

    //r-138 g-137 b-161 -Lowest
    //r-154 g-153 b-194 -H







    public partial class Form1 : Form {

        [DllImport("user32.dll")]
        [return: MarshalAs(UnmanagedType.Bool)]
        static extern bool GetWindowRect(IntPtr hWnd, out RECT lpRect);



        [StructLayout(LayoutKind.Sequential)]
        public struct RECT {
            public int Left { get; set; }
            public int Top { get; set; }
            public int Right { get; set; }
            public int Bottom { get; set; }
            public int Width { get { return Right - Left; } }
            public int Height { get { return Bottom - Top; } }
        }

        public Form1() {
            InitializeComponent();


            //Image<Bgr, byte> image = new Image<Bgr, byte>(@"C:\Users\michail\Pictures\Screenshots\Снимок экрана (8).png");
            //var range = image.InRange(new Bgr(157, 133, 133), new Bgr(200, 160, 160)).Dilate(1);
            //var range2 = image.InRange(new Bgr(185, 145, 70), new Bgr(195, 160, 85)).Dilate(1);
            ////range = range.Or(range2); 
            //CvInvoke.MorphologyEx(range, range, MorphOp.Dilate, new Mat(new Size(20, 20), DepthType.Cv8U, 1), new Point(-1, -1), 1, BorderType.Default, new MCvScalar(123123));
            //CvInvoke.MorphologyEx(range, range, MorphOp.Close, new Mat(new Size(20, 20), DepthType.Cv8U, 1), new Point(-1, -1), 1, BorderType.Default, new MCvScalar(123123));

            //var rangeClone = range.Clone();
            //Rectangle rectangle;
            //CvInvoke.FloodFill(rangeClone, null, new Point(0, 0), new MCvScalar(255), out rectangle,
            //new MCvScalar(0), new MCvScalar(255), Connectivity.FourConnected);
            //range = rangeClone.Not().Or(range);
            //pictureBox2.Image = range.ToBitmap();



            //VectorOfVectorOfPoint result = new VectorOfVectorOfPoint();
            //CvInvoke.FindContours(range, result, null, RetrType.List, ChainApproxMethod.LinkRuns);






            //CvInvoke.DrawContours(contours, result, -1, new MCvScalar(230, 230, 230), 2, LineType.EightConnected);

            //pictureBox1.Image = contours.ToBitmap();
        }

        const string notepad = "notepad++";
        const string game = "PathOfExile";

        private void button1_Click(object sender, EventArgs e) {
            //decimal\ gameMapFogProcessor = new MapScreenReader();
            //GameMapFogProcessor debugMapProcessor = new GameMapFogProcessor();
            //debugMapProcessor.OnResult += OnResult;

            //var result1 = 30d.GetAllNearAngles(130);
            //var result2 = 330d.GetAllNearAngles(130);
            //var result3 = 150d.GetAllNearAngles(130);
            //List<MoveInfo> moveInfos = new List<MoveInfo>();
            //foreach (var item in Extensions.AngleIterator(0, 359)) {
            //    moveInfos.Add(new MoveInfo() { Angle = item });
            //}
            //var vectors = moveInfos.Join(result1, history => history.Angle, a => a, (a, h) => a.);
            //LootProcessor gameMapProcessor = new LootProcessor();
            //gameMapProcessor.OnResult += OnResult;
            
            AqueductRunner aqueductRunner = new AqueductRunner(new DebugLogger(richTextBox1));

        }

        //private void DebugMapProcessor_OnResult(object sender, ImageProcessorEventArgs<HistogramResult> e) {
        //    listBox1.DisplayMember = "ID";
        //    listBox1.ValueMember = "ResultBitmap";
        //    listBox1.DataSource = e.ImageProcessorResult.Result;
        //    //chart1.Series[0].Points.Clear();
        //    //foreach (var item in e.ImageProcessorResult.HistogramVert) {
        //    //    chart1.Series[0].Points.AddXY(item.X, item.Y);
        //    //}
        //}

        private void LongFileReaderSafe_OnNewData(object sender, string e) {
            richTextBox1.Text += e;
        }

        private void OnResult(object sender, ImageProcessorEventArgs<GameMapProcessorResult<LootMoveResult>> e) {
            pictureBox1.Image = e.ImageProcessorResult.ResultBitmap;

            //if (!File.Exists(@"D:\test.bmp")) {
            //    e.ImageProcessorResult.ResultBitmap.Save(@"D:\test.bmp");
            //}
            //Image<Bgr, byte> image = new Image<Bgr, byte>(e.ImageProcessorResult.ResultBitmap);
            //var range = image.InRange(new Bgr((int)mB.Value, (int)mG.Value, (int)mR.Value), new Bgr((int)maxB.Value, (int)maxG.Value, (int)maxR.Value)).Dilate(1);
            //pictureBox1.Image = range.ToBitmap();
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e) {

        }

        private void Form1_Shown(object sender, EventArgs e) {
            //GameMapProcessor gameMapProcessor = new GameMapProcessor();
            //gameMapProcessor.OnResult += OnResult;
            //AqueductRunner aqueductRunner = new AqueductRunner(new DebugLogger(richTextBox1));
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            pictureBox1.Image = (Bitmap)listBox1.SelectedValue;
        }
    }
}
