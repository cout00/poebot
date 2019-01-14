using System;
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
using Tesseract;

namespace Overlay
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            var path = @"D:\Безымянный.png";
            using (var engine = new TesseractEngine(@"D:\tessdata", "eng", EngineMode.Default)) {
                engine.SetVariable("tessedit_char_whitelist", "0123456789");
                using (var img = Pix.LoadFromFile(path)) {
                    using (var page = engine.Process(img)) {
                        var text = page.GetText();
                    }
                }
            }
        }
    }
}
