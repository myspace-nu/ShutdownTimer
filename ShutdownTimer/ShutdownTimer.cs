using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Management;

namespace ShutdownTimer
{
    public partial class ShutdownTimerForm : Form
    {
        CommandLineParameters args;
        double ShutdownTime = 60;
        int progressHeight = 5;
        int progressWidth = 0;
        DateTime StartTime;
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
        public ShutdownTimerForm()
        {
            InitializeComponent();
            args = new CommandLineParameters();
            if (args.exists("-timer"))
                double.TryParse(args.get("-timer"), out ShutdownTime);
            ShutdownTime = (Math.Floor(ShutdownTime) > 0) ? Math.Floor(ShutdownTime) : 60;
            timerlabel.Text = TimeSpan.FromSeconds(ShutdownTime).ToString();
            timerprogress.Text = "";
            timerprogress.Height = progressHeight;
            timerprogress.Top = -progressHeight;
            StartTime = DateTime.Now;
            timer1.Enabled = true;
            timer1.Start();
            StopWatch.Start();
        }

        private void ShutdownTimer_Shown(object sender, EventArgs e)
        {
            timerprogress.Top = (int)(this.ClientSize.Height - progressHeight);
            progressWidth = (int)this.ClientSize.Width; // ShutdownTimerForm.ActiveForm.Width
            timerprogress.Width = progressWidth;
            timerprogress.Visible = !args.exists("-noprogress");
            pauseButton.Visible = !args.exists("-nopause");
        }
        private void timer1_Tick(object sender, EventArgs e)
        {
            double elapsedTime = StopWatch.ElapsedMilliseconds;
            double timeLeft = Math.Max(0,ShutdownTime*1000 - elapsedTime);
            timerlabel.Text = TimeSpan.FromSeconds(Math.Ceiling(timeLeft/1000)).ToString();
            double percent = Math.Max(0,((ShutdownTime * 1000 - elapsedTime) / (ShutdownTime * 1000)))*100;
            timerprogress.Width = (int)(Math.Floor((double)progressWidth * (percent/100)));
            // timerprogress.Text = Math.Floor(percent).ToString()+"%";
            if (timeLeft<=0) {
                timer1.Stop();
                timer1.Enabled = false;
                StopWatch.Stop();
                Shutdown(args.exists("-reboot"));
            }
        }
        private void pauseButton_Click(object sender, EventArgs e)
        {
            if (StopWatch.IsRunning)
            {
                StopWatch.Stop();
                pauseButton.Text = "4";
            } else
            {
                StopWatch.Start();
                pauseButton.Text = ";";
            }
        }
        void Shutdown(bool reboot = false)
        {
            ManagementBaseObject mboShutdown = null;
            ManagementClass MMCWin32 = new ManagementClass("Win32_OperatingSystem");
            MMCWin32.Get();
            // Shutdown requires elevated privileges
            MMCWin32.Scope.Options.EnablePrivileges = true;
            ManagementBaseObject MBOParams = MMCWin32.GetMethodParameters("Win32Shutdown");
            // Flags: 1 = shut down, 2 = reboot
            MBOParams["Flags"] = (reboot) ? "2" : "1";
            MBOParams["Reserved"] = "0";
            foreach (ManagementObject MObj in MMCWin32.GetInstances())
            {
                mboShutdown = MObj.InvokeMethod("Win32Shutdown", MBOParams, null);
            }
        }
    }
}
