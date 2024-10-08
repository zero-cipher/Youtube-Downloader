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



        private string ConfigFileName;

        public Frm_Main()
        {
            InitializeComponent();
        }

        private void Frm_Main_Load(object sender, EventArgs e)
        {
            logger.Debug("Youtube Downloader started...");

            txt_URL.Text = "https://www.youtube.com/watch?v=huFzlGhhhC4";
            txt_URL.Text = "https://www.youtube.com/watch?v=iEQ-akVZonc";

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
            downloader.VideoQuality = TextConfig.GetInteger(this.ConfigFileName, "default", "VideoQuality", 0);
            downloader.TargetFileExtension = TextConfig.GetString(this.ConfigFileName, "default", "TargetFileExtension", "");


            // 영상 파일 다운로드 진행 상황 Event 처리
            downloader.VideoProgressEvent += (percent) =>
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        pgb_Video.Value = percent;
                        lbl_VideoProgress.Text = string.Format("{0}%", percent);
                    }
                    catch (Exception)
                    {
                    }
                }));
            };

            // 음성 파일 다운로드 진행 상황 Event 처리
            downloader.AudioProgressEvent += (percent) =>
            {
                this.Invoke(new Action(() =>
                {
                    try
                    {
                        pgb_Audio.Value = percent;
                        lbl_AudioProgress.Text = string.Format("{0}%", percent);
                    }
                    catch (Exception)
                    {
                    }
                }));
            };
            

            // 다운로드 상태 변경 메세지 수신 Event 처리
            downloader.StatusChangeEvent += (message) =>
            {
                this.Invoke(new Action(() =>
                {
                    lbl_ProgressStatus.Text = message;
                }));
            };
            

            // 다운로드 완료 Event 처리
            downloader.DownloadComplete += () =>
            {
                this.Invoke(new Action(() =>
                {
                    btn_DownloadStart.Enabled = true;
                }));
            };

            // 다운로드 시작
            downloader.DownloadStart();

        }


        private void btn_SelectSavePath_Click(object sender, EventArgs e)
        {
            txt_SavePath.Text = Utils.SelectFolder(txt_SavePath.Text);
            TextConfig.SetValue(this.ConfigFileName, "default", "SavePath", txt_SavePath.Text);
        }

    }
}
