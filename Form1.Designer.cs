namespace ConvNetForms
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.pictureBoxInput = new System.Windows.Forms.PictureBox();
            this.buttonRun = new System.Windows.Forms.Button();
            this.pictureBoxInput2 = new System.Windows.Forms.PictureBox();
            this.pictureBoxFilters = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.labelEpoch = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.labelError = new System.Windows.Forms.Label();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.pictureBoxFeatures = new System.Windows.Forms.PictureBox();
            this.pictureBoxFeatures2 = new System.Windows.Forms.PictureBox();
            this.listBox2 = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.listBox3 = new System.Windows.Forms.ListBox();
            this.label5 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInput)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInput2)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilters)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFeatures)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFeatures2)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBoxInput
            // 
            this.pictureBoxInput.BackColor = System.Drawing.Color.White;
            this.pictureBoxInput.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInput.Image")));
            this.pictureBoxInput.Location = new System.Drawing.Point(13, 13);
            this.pictureBoxInput.Name = "pictureBoxInput";
            this.pictureBoxInput.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxInput.TabIndex = 0;
            this.pictureBoxInput.TabStop = false;
            // 
            // buttonRun
            // 
            this.buttonRun.Location = new System.Drawing.Point(61, 13);
            this.buttonRun.Name = "buttonRun";
            this.buttonRun.Size = new System.Drawing.Size(75, 23);
            this.buttonRun.TabIndex = 1;
            this.buttonRun.Text = "Run";
            this.buttonRun.UseVisualStyleBackColor = true;
            this.buttonRun.Click += new System.EventHandler(this.buttonRun_Click);
            // 
            // pictureBoxInput2
            // 
            this.pictureBoxInput2.BackColor = System.Drawing.Color.White;
            this.pictureBoxInput2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBoxInput2.Image")));
            this.pictureBoxInput2.Location = new System.Drawing.Point(13, 51);
            this.pictureBoxInput2.Name = "pictureBoxInput2";
            this.pictureBoxInput2.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxInput2.TabIndex = 2;
            this.pictureBoxInput2.TabStop = false;
            // 
            // pictureBoxFilters
            // 
            this.pictureBoxFilters.BackColor = System.Drawing.Color.White;
            this.pictureBoxFilters.Location = new System.Drawing.Point(163, 13);
            this.pictureBoxFilters.Name = "pictureBoxFilters";
            this.pictureBoxFilters.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxFilters.TabIndex = 3;
            this.pictureBoxFilters.TabStop = false;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 105);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(38, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Epoch";
            // 
            // labelEpoch
            // 
            this.labelEpoch.AutoSize = true;
            this.labelEpoch.Location = new System.Drawing.Point(16, 122);
            this.labelEpoch.Name = "labelEpoch";
            this.labelEpoch.Size = new System.Drawing.Size(13, 13);
            this.labelEpoch.TabIndex = 5;
            this.labelEpoch.Text = "0";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(74, 105);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 6;
            this.label2.Text = "Error";
            // 
            // labelError
            // 
            this.labelError.AutoSize = true;
            this.labelError.Location = new System.Drawing.Point(77, 122);
            this.labelError.Name = "labelError";
            this.labelError.Size = new System.Drawing.Size(13, 13);
            this.labelError.TabIndex = 7;
            this.labelError.Text = "0";
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.Location = new System.Drawing.Point(380, 39);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(256, 342);
            this.listBox1.TabIndex = 8;
            // 
            // pictureBoxFeatures
            // 
            this.pictureBoxFeatures.BackColor = System.Drawing.Color.White;
            this.pictureBoxFeatures.Location = new System.Drawing.Point(163, 52);
            this.pictureBoxFeatures.Name = "pictureBoxFeatures";
            this.pictureBoxFeatures.Size = new System.Drawing.Size(32, 31);
            this.pictureBoxFeatures.TabIndex = 9;
            this.pictureBoxFeatures.TabStop = false;
            // 
            // pictureBoxFeatures2
            // 
            this.pictureBoxFeatures2.BackColor = System.Drawing.Color.White;
            this.pictureBoxFeatures2.Location = new System.Drawing.Point(163, 89);
            this.pictureBoxFeatures2.Name = "pictureBoxFeatures2";
            this.pictureBoxFeatures2.Size = new System.Drawing.Size(32, 32);
            this.pictureBoxFeatures2.TabIndex = 10;
            this.pictureBoxFeatures2.TabStop = false;
            // 
            // listBox2
            // 
            this.listBox2.FormattingEnabled = true;
            this.listBox2.Location = new System.Drawing.Point(16, 179);
            this.listBox2.Name = "listBox2";
            this.listBox2.Size = new System.Drawing.Size(166, 108);
            this.listBox2.TabIndex = 11;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(377, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(71, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Filter Weights";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(16, 160);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 13;
            this.label4.Text = "FC Weights";
            // 
            // listBox3
            // 
            this.listBox3.FormattingEnabled = true;
            this.listBox3.Location = new System.Drawing.Point(16, 317);
            this.listBox3.Name = "listBox3";
            this.listBox3.Size = new System.Drawing.Size(166, 95);
            this.listBox3.TabIndex = 14;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(16, 298);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(73, 13);
            this.label5.TabIndex = 15;
            this.label5.Text = "Node Outputs";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(688, 426);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.listBox3);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.listBox2);
            this.Controls.Add(this.pictureBoxFeatures2);
            this.Controls.Add(this.pictureBoxFeatures);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.labelError);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.labelEpoch);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.pictureBoxFilters);
            this.Controls.Add(this.pictureBoxInput2);
            this.Controls.Add(this.buttonRun);
            this.Controls.Add(this.pictureBoxInput);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInput)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxInput2)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFilters)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFeatures)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxFeatures2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBoxInput;
        private System.Windows.Forms.Button buttonRun;
        private System.Windows.Forms.PictureBox pictureBoxInput2;
        private System.Windows.Forms.PictureBox pictureBoxFilters;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label labelEpoch;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label labelError;
        private System.Windows.Forms.ListBox listBox1;
        private System.Windows.Forms.PictureBox pictureBoxFeatures;
        private System.Windows.Forms.PictureBox pictureBoxFeatures2;
        private System.Windows.Forms.ListBox listBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ListBox listBox3;
        private System.Windows.Forms.Label label5;
    }
}

