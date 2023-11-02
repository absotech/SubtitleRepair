﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace SubtitleRepair
{
    public partial class MainForm : Form
    {

        public const int WM_NCLBUTTONDOWN = 0xA1;
        public const int HT_CAPTION = 0x2;
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]

        public static extern int SendMessage(IntPtr hWnd, int Msg, int wParam, int lParam);
        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern bool ReleaseCapture();

        public MainForm()
        {
            InitializeComponent();
            timerFade.Start();
            if (Convert.ToInt32(Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").GetValue("AllMessages")) == 1)
                checkBoxAllMessages.Checked = true;
        }

        private void buttonClose_Click(object sender, EventArgs e)
        {
            Environment.Exit(0);
        }

        private void MainForm_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == (char)Keys.Enter)
                Environment.Exit(0);
        }

        private void timerFade_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + 0.02D;
            if (this.Opacity == 1.00D)
            {
                timerFade.Stop();
            }
        }

        private void labelTitle_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(Handle, WM_NCLBUTTONDOWN, HT_CAPTION, 0);
            }
        }

        private void checkBoxAllMessages_CheckedChanged(object sender, EventArgs e)
        {
            if (checkBoxAllMessages.Checked == true)
                Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").SetValue("AllMessages", 1);
            else
                Registry.CurrentUser.CreateSubKey("Software\\Abso Tech\\Subtitle Repair").SetValue("AllMessages", 0);
        }
    }
}
