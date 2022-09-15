namespace ShutdownTimer
{
    partial class ShutdownTimerForm
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
            this.components = new System.ComponentModel.Container();
            this.timerlabel = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.timerprogress = new System.Windows.Forms.Label();
            this.pauseButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // timerlabel
            // 
            this.timerlabel.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.timerlabel.AutoSize = true;
            this.timerlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 36F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.timerlabel.ForeColor = System.Drawing.Color.White;
            this.timerlabel.Location = new System.Drawing.Point(46, 28);
            this.timerlabel.Margin = new System.Windows.Forms.Padding(0);
            this.timerlabel.Name = "timerlabel";
            this.timerlabel.Size = new System.Drawing.Size(212, 55);
            this.timerlabel.TabIndex = 0;
            this.timerlabel.Text = "00:00:00";
            this.timerlabel.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // timerprogress
            // 
            this.timerprogress.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.timerprogress.BackColor = System.Drawing.Color.White;
            this.timerprogress.Location = new System.Drawing.Point(0, 128);
            this.timerprogress.Name = "timerprogress";
            this.timerprogress.Size = new System.Drawing.Size(304, 13);
            this.timerprogress.TabIndex = 1;
            this.timerprogress.Text = "100%";
            this.timerprogress.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pauseButton
            // 
            this.pauseButton.BackColor = System.Drawing.Color.White;
            this.pauseButton.FlatAppearance.BorderSize = 0;
            this.pauseButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.pauseButton.Font = new System.Drawing.Font("Webdings", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(2)));
            this.pauseButton.Location = new System.Drawing.Point(274, 106);
            this.pauseButton.Name = "pauseButton";
            this.pauseButton.Size = new System.Drawing.Size(25, 25);
            this.pauseButton.TabIndex = 2;
            this.pauseButton.Text = ";";
            this.pauseButton.UseVisualStyleBackColor = false;
            this.pauseButton.Click += new System.EventHandler(this.pauseButton_Click);
            // 
            // ShutdownTimerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Red;
            this.ClientSize = new System.Drawing.Size(304, 141);
            this.Controls.Add(this.pauseButton);
            this.Controls.Add(this.timerprogress);
            this.Controls.Add(this.timerlabel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.Name = "ShutdownTimerForm";
            this.Text = "ShutdownTimer";
            this.Shown += new System.EventHandler(this.ShutdownTimer_Shown);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label timerlabel;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.Label timerprogress;
        private System.Windows.Forms.Button pauseButton;
    }
}

