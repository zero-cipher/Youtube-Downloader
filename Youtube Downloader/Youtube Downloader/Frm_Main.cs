﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Youtube_Downloader
{
    public partial class Frm_Main : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private YoutubeClient client = new YoutubeClient();

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            logger.Debug("Youtube Downloader started...");

            txt_URL.Text = "https://www.youtube.com/watch?v=huFzlGhhhC4";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txt_URL.Text;

            // Youtube 서버로부터 게시물 정보를 얻어온다.
            client.GetVideoInfo(url);
        }
    }
}
