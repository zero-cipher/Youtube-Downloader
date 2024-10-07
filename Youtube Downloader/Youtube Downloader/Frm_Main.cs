using MediaToolkit;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using VideoLibrary;

namespace Youtube_Downloader
{
    public partial class Frm_Main : Form
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private YoutubeClient client = new YoutubeClient();



        private string ConfigFileName;

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

            ConfigFileName = Path.Combine(Application.StartupPath, "config.ini");
            txt_SavePath.Text = TextConfig.GetString(this.ConfigFileName, "default", "SavePath", Application.StartupPath);
        }


        private void Frm_Main_FormClosing(object sender, FormClosingEventArgs e)
        {
            TextConfig.SetValue(this.ConfigFileName, "default", "SavePath", txt_SavePath.Text);
        }


        private void btn_DownloadStart_Click(object sender, EventArgs e)
        {
            btn_DownloadStart.Enabled = false;

            Downloader downloader = new Downloader(txt_URL.Text, txt_SavePath.Text);

            downloader.FFMpegOption = TextConfig.GetString(this.ConfigFileName, "default", "ffmpegoption", "");

            // 이벤트 설정
            downloader.VideoProgressEvent += new Downloader.ProgressEventHandler(OnVideoProgress);
            downloader.AudioProgressEvent += new Downloader.ProgressEventHandler(OnAudioProgress);
            downloader.StatusChangeEvent += new Downloader.StatusChangeEventHandler(OnStatusChange);
            downloader.DownloadComplete += new Downloader.DownloadCompleteEventHandler(OnDownloadComplete);

            // 다운로드 시작
            downloader.DownloadStart();

        }

        private void OnDownloadComplete()
        {
            btn_DownloadStart.Enabled = true;
        }

        private void OnStatusChange(string message)
        {
            this.Invoke(new Action(() =>
            {
                lbl_ProgressStatus.Text = message;
            }));
        }



        /// <summary>
        /// 영상 다운로드 진행상황을 ProgressBar에 표시
        /// </summary>
        /// <param name="percent"></param>
        private void OnVideoProgress(int percent)
        {
            this.Invoke(new Action(() =>
            {
                pgb_Video.Value = percent;
                lbl_VideoProgress.Text = string.Format("{0}%", percent);
            }));
        }

        /// <summary>
        /// 음성 다운로드 진행상황을 ProgressBar에 표시
        /// </summary>
        /// <param name="percent"></param>
        private void OnAudioProgress(int percent)
        {
            this.Invoke(new Action(() =>
            {
                pgb_Audio.Value = percent;
                lbl_AudioProgress.Text = string.Format("{0}%", percent);
            }));
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
            Downloader1 downloader = new Downloader1(txt_URL.Text);
            downloader.VideoProgressEvent += new Downloader1.ProgressEventHandler(OnVideoProgress);
            downloader.AudioProgressEvent += new Downloader1.ProgressEventHandler(OnAudioProgress);
            downloader.DownloadStart();
        }


        private void cmd_SelectSavePath_Click(object sender, EventArgs e)
        {
            txt_SavePath.Text = Utils.SelectFolder(txt_SavePath.Text);
            TextConfig.SetValue(this.ConfigFileName, "default", "SavePath", txt_SavePath.Text);
        }

        private void button9_Click(object sender, EventArgs e)
        {

            string command = "";

            // ffmpeg 입력 파일 설정
            command += "-i \"" + Path.Combine(Application.StartupPath, "video.webm") + "\" ";
            command += "-i \"" + Path.Combine(Application.StartupPath, "audio.mp4") + "\" ";

            // ffmpeg 옵션 지정
            //            command += "-preset veryfast ";     // preset - ultrafast, superfast, veryfast
            //command += "-c:v libx264 ";         // 비디오 - H.264 코덱
            //command += "-c:a mp3 ";             // 음성 - MP3 코덱

            command += TextConfig.GetString(this.ConfigFileName, "default", "ffmpegoption", "");
            command += " ";


            // ffmpeg 출력 파일 설정
            command += "\"" + Path.Combine(Application.StartupPath, "output.avi") + "\"";


            Stopwatch timer = new Stopwatch();
            timer.Start();

            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                engine.CustomCommand(command);
            }
            logger.Debug(command);
            logger.Debug("elsped time : " + timer.Elapsed.ToString());
            logger.Debug("file size = " + GetFileSize(Path.Combine(Application.StartupPath, "output.avi")));

        }

        private long GetFileSize(string path)
        {
            FileInfo fi = new FileInfo(path);
            return fi.Length;
        }
    }
}
