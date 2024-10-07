using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
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

            pgb_Video.Minimum = 0;
            pgb_Video.Maximum = 100;
            pgb_Audio.Minimum = 0;
            pgb_Audio.Maximum = 100;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string url = txt_URL.Text;

            // Youtube 서버로부터 게시물 정보를 얻어온다.
            client.GetVideoInfo(url);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string value = Utils.SelectFolder("");
            logger.Debug("selected folder = " + value);
        }

        private void button3_Click(object sender, EventArgs e)
        {
            client.DownloadBestVideo();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            client.DownloadBestAudio();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            client.Combine();
        }

        private void button6_Click(object sender, EventArgs e)
        {
            client.Combine2();
        }

        private void button7_Click(object sender, EventArgs e)
        {

            VideoLibrary.YouTubeVideo video = client.GetBestVideo();


            var progress = new Progress<double>(percent =>
            {
                this.Invoke(new Action(() =>
                {
                    logger.Debug("downloaddig..... " + percent);
                }));
            });

            logger.Debug("start downloading");
            client.DownloadVideoAsync(video.Uri, System.IO.Path.Combine(Application.StartupPath, "video.mp4"), progress);
                //.GetAwaiter().GetResult();

        }

        private void button8_Click(object sender, EventArgs e)
        {
            Downloader downloader = new Downloader(txt_URL.Text);
            downloader.VideoProgressEvent += new Downloader.ProgressEventHandler(OnVideoProgress);
            downloader.AudioProgressEvent += new Downloader.ProgressEventHandler(OnAudioProgress);
            downloader.DownloadStart();
        }

        private void OnVideoProgress(int percent)
        {
            this.Invoke(new Action(() =>
            {
                pgb_Video.Value = percent;
                lbl_VideoProgress.Text = string.Format("{0}%", percent);
            }));
        }

        private void OnAudioProgress(int percent)
        {
            this.Invoke(new Action(() =>
            {
                pgb_Audio.Value = percent;
                lbl_AudioProgress.Text = string.Format("{0}%", percent);
            }));
        }

    }
}
