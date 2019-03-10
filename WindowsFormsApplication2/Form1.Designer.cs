namespace WindowsFormsApplication2 {
    partial class Form1 {
        /// <summary>
        /// Обязательная переменная конструктора.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Освободить все используемые ресурсы.
        /// </summary>
        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Код, автоматически созданный конструктором форм Windows

        /// <summary>
        /// Требуемый метод для поддержки конструктора — не изменяйте 
        /// содержимое этого метода с помощью редактора кода.
        /// </summary>
        private void InitializeComponent() {
            this.button1 = new System.Windows.Forms.Button();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.BMin = new System.Windows.Forms.TextBox();
            this.RMin = new System.Windows.Forms.TextBox();
            this.GMin = new System.Windows.Forms.TextBox();
            this.BMax = new System.Windows.Forms.TextBox();
            this.RMax = new System.Windows.Forms.TextBox();
            this.GMax = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(46, 37);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click_1);
            // 
            // comboBox1
            // 
            this.comboBox1.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(177, 41);
            this.comboBox1.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(105, 21);
            this.comboBox1.TabIndex = 1;
            // 
            // BMin
            // 
            this.BMin.Location = new System.Drawing.Point(471, 41);
            this.BMin.Name = "BMin";
            this.BMin.Size = new System.Drawing.Size(100, 20);
            this.BMin.TabIndex = 2;
            this.BMin.Text = "0";
            // 
            // RMin
            // 
            this.RMin.Location = new System.Drawing.Point(471, 119);
            this.RMin.Name = "RMin";
            this.RMin.Size = new System.Drawing.Size(100, 20);
            this.RMin.TabIndex = 3;
            this.RMin.Text = "0";
            // 
            // GMin
            // 
            this.GMin.Location = new System.Drawing.Point(471, 79);
            this.GMin.Name = "GMin";
            this.GMin.Size = new System.Drawing.Size(100, 20);
            this.GMin.TabIndex = 4;
            this.GMin.Text = "0";
            // 
            // BMax
            // 
            this.BMax.Location = new System.Drawing.Point(637, 40);
            this.BMax.Name = "BMax";
            this.BMax.Size = new System.Drawing.Size(100, 20);
            this.BMax.TabIndex = 5;
            this.BMax.Text = "255";
            // 
            // RMax
            // 
            this.RMax.Location = new System.Drawing.Point(637, 119);
            this.RMax.Name = "RMax";
            this.RMax.Size = new System.Drawing.Size(100, 20);
            this.RMax.TabIndex = 6;
            this.RMax.Text = "255";
            // 
            // GMax
            // 
            this.GMax.Location = new System.Drawing.Point(637, 79);
            this.GMax.Name = "GMax";
            this.GMax.Size = new System.Drawing.Size(100, 20);
            this.GMax.TabIndex = 7;
            this.GMax.Text = "255";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(539, 170);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(121, 23);
            this.button2.TabIndex = 8;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(471, 240);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(421, 287);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(904, 556);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.GMax);
            this.Controls.Add(this.RMax);
            this.Controls.Add(this.BMax);
            this.Controls.Add(this.GMin);
            this.Controls.Add(this.RMin);
            this.Controls.Add(this.BMin);
            this.Controls.Add(this.comboBox1);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox BMin;
        private System.Windows.Forms.TextBox RMin;
        private System.Windows.Forms.TextBox GMin;
        private System.Windows.Forms.TextBox BMax;
        private System.Windows.Forms.TextBox RMax;
        private System.Windows.Forms.TextBox GMax;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

