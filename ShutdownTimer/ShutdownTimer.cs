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
        int windowWidth = 320;
        int windowHeight = 180;
        int fontSize = 36;
        string font = "Microsoft Sans Serif";
        DateTime StartTime;
        System.Diagnostics.Stopwatch StopWatch = new System.Diagnostics.Stopwatch();
        public ShutdownTimerForm()
        {
            InitializeComponent();
            args = new CommandLineParameters();
            args.loadFromFile();
            if (args.exists("width"))
                int.TryParse(args.get("width"), out windowWidth);
            windowWidth = Math.Min(1000, Math.Max(260, windowWidth));
            if (args.exists("height"))
                int.TryParse(args.get("height"), out windowHeight);
            windowHeight = Math.Min(1000, Math.Max(90, windowHeight));
            if (args.exists("fontsize"))
                int.TryParse(args.get("fontsize"), out fontSize);
            fontSize = Math.Min(100, Math.Max(3, fontSize));
            if (args.exists("font"))
                font = args.get("font");
            if (args.exists("timer"))
            {
                string[] timeArr = args.get("timer").Split(':');
                Array.Reverse(timeArr);
                double[] mul = new double[] { 1, 60, 60*60 };
                ShutdownTime = 0;
                for (int i=0; i<timeArr.Length && i<mul.Length; i++)
                {
                    double t = 0;
                    double.TryParse(timeArr[i], out t);
                    ShutdownTime += t * mul[i];
                }
            } else if (args.exists("at")) {
                ShutdownTime = (double)GetSecondsUntilTime(args.get("at"));
            }
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
            if (args.exists("saveconfig"))
            {
                args.remove("saveconfig");
                args.saveToFile();
            }
            this.Width = windowWidth;
            this.Height = windowHeight;
            timerlabel.Font = new Font(font, fontSize);
            timerlabel.Top = (int)(this.ClientSize.Height / 2) - (timerlabel.Height / 2);
            timerlabel.Left = (int)(this.ClientSize.Width / 2) - (timerlabel.Width / 2);
            timerprogress.Top = (int)(this.ClientSize.Height - progressHeight);
            progressWidth = (int)this.ClientSize.Width;
            timerprogress.Width = progressWidth;
            timerprogress.Visible = !args.exists("noprogress");
            pauseButton.Visible = !args.exists("nopause");
            pauseButton.Left = (int)(this.ClientSize.Width - (pauseButton.Width + 5));
            pauseButton.Top = (int)(this.ClientSize.Height - (pauseButton.Height + 5 + ((timerprogress.Visible) ? progressHeight : 0)));
            if (args.exists("reboot")){
                actionlabel.Text = "Reboot in";
            } else {
                actionlabel.Text = "Shutdown in";
            }
            actionlabel.Font = new Font(font, fontSize / 2);
            actionlabel.Top = (int)(this.ClientSize.Height / 2) - (actionlabel.Height / 2) - (int)(timerlabel.Height / 1.5);
            actionlabel.Left = (int)(this.ClientSize.Width / 2) - (actionlabel.Width / 2);
            if (args.exists("background"))
            {
                try
                {
                    string color = args.get("background");
                    if (color.StartsWith("#"))
                    {
                        color = color.TrimStart('#');
                        int r = Convert.ToInt32(color.Substring(0, 2), 16);
                        int g = Convert.ToInt32(color.Substring(2, 2), 16);
                        int b = Convert.ToInt32(color.Substring(4, 2), 16);
                        this.BackColor = System.Drawing.Color.FromArgb(r, g, b);
                    }
                    else
                    {
                        this.BackColor = System.Drawing.Color.FromName(color);
                    }
                }
                catch { }
            }

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
                Shutdown(args.exists("reboot"));
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
        private int GetSecondsUntilTime(string timeString)
        {
            TimeSpan targetTime = TimeSpan.Parse(timeString);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan timeDifference;
            if (targetTime > currentTime) {
                timeDifference = targetTime - currentTime;
            } else {
                timeDifference = (TimeSpan.FromHours(24) - currentTime) + targetTime;
            }
            return (int)timeDifference.TotalSeconds;
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
