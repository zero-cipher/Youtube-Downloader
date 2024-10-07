
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
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.button4 = new System.Windows.Forms.Button();
            this.button5 = new System.Windows.Forms.Button();
            this.button6 = new System.Windows.Forms.Button();
            this.button7 = new System.Windows.Forms.Button();
            this.button8 = new System.Windows.Forms.Button();
            this.pgb_Video = new System.Windows.Forms.ProgressBar();
            this.pgb_Audio = new System.Windows.Forms.ProgressBar();
            this.lbl_VideoProgress = new System.Windows.Forms.Label();
            this.lbl_AudioProgress = new System.Windows.Forms.Label();
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
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(14, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(199, 47);
            this.button1.TabIndex = 2;
            this.button1.Text = "Get Video Info";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(14, 373);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(199, 47);
            this.button2.TabIndex = 3;
            this.button2.Text = "Select Folder";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(219, 320);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(199, 47);
            this.button3.TabIndex = 4;
            this.button3.Text = "Download Video";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // button4
            // 
            this.button4.Location = new System.Drawing.Point(424, 320);
            this.button4.Name = "button4";
            this.button4.Size = new System.Drawing.Size(199, 47);
            this.button4.TabIndex = 5;
            this.button4.Text = "Download Audio";
            this.button4.UseVisualStyleBackColor = true;
            this.button4.Click += new System.EventHandler(this.button4_Click);
            // 
            // button5
            // 
            this.button5.Location = new System.Drawing.Point(424, 373);
            this.button5.Name = "button5";
            this.button5.Size = new System.Drawing.Size(199, 47);
            this.button5.TabIndex = 6;
            this.button5.Text = "Combine";
            this.button5.UseVisualStyleBackColor = true;
            this.button5.Click += new System.EventHandler(this.button5_Click);
            // 
            // button6
            // 
            this.button6.Location = new System.Drawing.Point(629, 373);
            this.button6.Name = "button6";
            this.button6.Size = new System.Drawing.Size(199, 47);
            this.button6.TabIndex = 7;
            this.button6.Text = "Combine (avi)";
            this.button6.UseVisualStyleBackColor = true;
            this.button6.Click += new System.EventHandler(this.button6_Click);
            // 
            // button7
            // 
            this.button7.Location = new System.Drawing.Point(219, 373);
            this.button7.Name = "button7";
            this.button7.Size = new System.Drawing.Size(199, 47);
            this.button7.TabIndex = 8;
            this.button7.Text = "Download Video Custom";
            this.button7.UseVisualStyleBackColor = true;
            this.button7.Click += new System.EventHandler(this.button7_Click);
            // 
            // button8
            // 
            this.button8.Location = new System.Drawing.Point(424, 123);
            this.button8.Name = "button8";
            this.button8.Size = new System.Drawing.Size(199, 47);
            this.button8.TabIndex = 9;
            this.button8.Text = "영상/음성 다운로드(Thread)";
            this.button8.UseVisualStyleBackColor = true;
            this.button8.Click += new System.EventHandler(this.button8_Click);
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
            this.lbl_VideoProgress.Size = new System.Drawing.Size(38, 12);
            this.lbl_VideoProgress.TabIndex = 12;
            this.lbl_VideoProgress.Text = "label2";
            // 
            // lbl_AudioProgress
            // 
            this.lbl_AudioProgress.AutoSize = true;
            this.lbl_AudioProgress.Location = new System.Drawing.Point(377, 84);
            this.lbl_AudioProgress.Name = "lbl_AudioProgress";
            this.lbl_AudioProgress.Size = new System.Drawing.Size(38, 12);
            this.lbl_AudioProgress.TabIndex = 13;
            this.lbl_AudioProgress.Text = "label3";
            // 
            // Frm_Main
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(840, 450);
            this.Controls.Add(this.lbl_AudioProgress);
            this.Controls.Add(this.lbl_VideoProgress);
            this.Controls.Add(this.pgb_Audio);
            this.Controls.Add(this.pgb_Video);
            this.Controls.Add(this.button8);
            this.Controls.Add(this.button7);
            this.Controls.Add(this.button6);
            this.Controls.Add(this.button5);
            this.Controls.Add(this.button4);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.txt_URL);
            this.Controls.Add(this.label1);
            this.Name = "Frm_Main";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Frm_Main_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txt_URL;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.Button button4;
        private System.Windows.Forms.Button button5;
        private System.Windows.Forms.Button button6;
        private System.Windows.Forms.Button button7;
        private System.Windows.Forms.Button button8;
        private System.Windows.Forms.ProgressBar pgb_Video;
        private System.Windows.Forms.ProgressBar pgb_Audio;
        private System.Windows.Forms.Label lbl_VideoProgress;
        private System.Windows.Forms.Label lbl_AudioProgress;
    }
}

