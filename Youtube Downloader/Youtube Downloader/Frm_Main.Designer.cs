
namespace Youtube_Downloader
{
    partial class Frm_Main
    {
        /// <summary>
        /// 필수 디자이너 변수입니다.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 사용 중인 모든 리소스를 정리합니다.
        /// </summary>
        /// <param name="disposing">관리되는 리소스를 삭제해야 하면 true이고, 그렇지 않으면 false입니다.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form 디자이너에서 생성한 코드

        /// <summary>
        /// 디자이너 지원에 필요한 메서드입니다. 
        /// 이 메서드의 내용을 코드 편집기로 수정하지 마세요.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.txt_URL = new System.Windows.Forms.TextBox();
            this.pgb_Video = new System.Windows.Forms.ProgressBar();
            this.pgb_Audio = new System.Windows.Forms.ProgressBar();
            this.lbl_VideoProgress = new System.Windows.Forms.Label();
            this.lbl_AudioProgress = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txt_SavePath = new System.Windows.Forms.TextBox();
            this.btn_SelectSavePath = new System.Windows.Forms.Button();
            this.btn_DownloadStart = new System.Windows.Forms.Button();
            this.lbl_ProgressStatus = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "URL:";
            // 
            // txt_URL
            // 
            this.txt_URL.Location = new System.Drawing.Point(14, 24);
            this.txt_URL.Name = "txt_URL";
            this.txt_URL.Size = new System.Drawing.Size(357, 21);
            this.txt_URL.TabIndex = 1;
            // 
            // pgb_Video
            // 
            this.pgb_Video.Location = new System.Drawing.Point(14, 58);
            this.pgb_Video.Name = "pgb_Video";
            this.pgb_Video.Size = new System.Drawing.Size(357, 16);
            this.pgb_Video.TabIndex = 10;
            // 
            // pgb_Audio
            // 
            this.pgb_Audio.Location = new System.Drawing.Point(14, 80);
            this.pgb_Audio.Name = "pgb_Audio";
            this.pgb_Audio.Size = new System.Drawing.Size(357, 16);
            this.pgb_Audio.TabIndex = 11;
            // 
            // lbl_VideoProgress
            // 
            this.lbl_VideoProgress.AutoSize = true;
            this.lbl_VideoProgress.Location = new System.Drawing.Point(377, 62);
            this.lbl_VideoProgress.Name = "lbl_VideoProgress";
            this.lbl_VideoProgress.Size = new System.Drawing.Size(21, 12);
            this.lbl_VideoProgress.TabIndex = 12;
            this.lbl_VideoProgress.Text = "0%";
            // 
            // lbl_AudioProgress
            // 
            this.lbl_AudioProgress.AutoSize = true;
            this.lbl_AudioProgress.Location = new System.Drawing.Point(377, 84);
            this.lbl_AudioProgress.Name = "lbl_AudioProgress";
            this.lbl_AudioProgress.Size = new System.Drawing.Size(21, 12);
            this.lbl_AudioProgress.TabIndex = 13;
            this.lbl_AudioProgress.Text = "0%";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 151);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(85, 12);
            this.label2.TabIndex = 14;
            this.label2.Text = "파일 저장 위치";
            // 
            // txt_SavePath
            // 
            this.txt_SavePath.Location = new System.Drawing.Point(14, 166);
            this.txt_SavePath.Name = "txt_SavePath";
            this.txt_SavePath.Size = new System.Drawing.Size(554, 21);
            this.txt_SavePath.TabIndex = 15;
            // 
            // btn_SelectSavePath
            // 
            this.btn_SelectSavePath.Location = new System.Drawing.Point(574, 160);
            this.btn_SelectSavePath.Name = "btn_SelectSavePath";
            this.btn_SelectSavePath.Size = new System.Drawing.Size(32, 30);
            this.btn_SelectSavePath.TabIndex = 16;
            this.btn_SelectSavePath.Text = "...";
            this.btn_SelectSavePath.UseVisualStyleBackColor = true;
            this.btn_SelectSavePath.Click += new System.EventHandler(this.btn_SelectSavePath_Click);
            // 
            // btn_DownloadStart
            // 
            this.btn_DownloadStart.Location = new System.Drawing.Point(424, 12);
            this.btn_DownloadStart.Name = "btn_DownloadStart";
            this.btn_DownloadStart.Size = new System.Drawing.Size(182, 50);
            this.btn_DownloadStart.TabIndex = 17;
            this.btn_DownloadStart.Text = "다운로드";
            this.btn_DownloadStart.UseVisualStyleBackColor = true;
            this.btn_DownloadStart.Click += new System.EventHandler(this.btn_DownloadStart_Click);
            // 
            // lbl_ProgressStatus
            // 
            this.lbl_ProgressStatus.AutoSize = true;
            this.lbl_ProgressStatus.Location = new System.Drawing.Point(12, 113);
            this.lbl_ProgressStatus.Name = "lbl_ProgressStatus";
            this.lbl_ProgressStatus.Size = new System.Drawing.Size(57, 12);
            this.lbl_ProgressStatus.TabIndex = 18;
            this.lbl_ProgressStatus.Text = "진행상황:";
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(618, 202);
            this.Controls.Add(this.lbl_ProgressStatus);
            this.Controls.Add(this.btn_DownloadStart);
            this.Controls.Add(this.btn_SelectSavePath);
            this.Controls.Add(this.txt_SavePath);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.lbl_AudioProgress);
            this.Controls.Add(this.lbl_VideoProgress);
            this.Controls.Add(this.pgb_Audio);
            this.Controls.Add(this.pgb_Video);
            this.Controls.Add(this.txt_URL);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "Frm_Main";
            this.Text = "YouTube 영상 다운로드";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Frm_Main_FormClosing);
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_URL;
        private System.Windows.Forms.ProgressBar pgb_Video;
        private System.Windows.Forms.ProgressBar pgb_Audio;
        private System.Windows.Forms.Label lbl_VideoProgress;
        private System.Windows.Forms.Label lbl_AudioProgress;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txt_SavePath;
        private System.Windows.Forms.Button btn_SelectSavePath;
        private System.Windows.Forms.Button btn_DownloadStart;
        private System.Windows.Forms.Label lbl_ProgressStatus;
    }
}

