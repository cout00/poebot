﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using WindowsFormsApplication2.Native;

namespace WindowsFormsApplication2.Parsers {
    public abstract class LongFileReaderSafe {
        int oldSize = 0;
        public event EventHandler<string> NewData;
        Timer _timer = new Timer();
        BinaryReader file;
        public LongFileReaderSafe(string filePath) {
            file = new BinaryReader(File.Open(filePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite));
            oldSize = (int)file.BaseStream.Length;
            _timer.Interval = 100;
            _timer.Tick += OnTick;
            _timer.Start();
        }

        protected abstract void OnNewData(string newstr);

        void OnTick(object sender, EventArgs e) {
            if (file.BaseStream.Length != oldSize) {
                var res = file.BaseStream.Seek(oldSize, SeekOrigin.Begin);
                var bytes = file.ReadBytes((int)file.BaseStream.Length - oldSize);
                var str = Encoding.UTF8.GetString(bytes);
                NewData?.Invoke(this, str);
                OnNewData(str);
            }
            oldSize = (int)file.BaseStream.Length;
        }
    }
}
