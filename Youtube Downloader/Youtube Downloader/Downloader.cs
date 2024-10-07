using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoLibrary;
using System.Windows.Forms;
using MediaToolkit;

namespace Youtube_Downloader
{
    class Downloader
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private YouTube youtube = new YouTube();
        private IEnumerable<YouTubeVideo> AllVideos;
        private Thread thread = null;

        private const int DOWNLOAD_PACKET_SIZE = 64 * 1024;

        public delegate void ProgressEventHandler(int percent);
        public event ProgressEventHandler VideoProgressEvent;
        public event ProgressEventHandler AudioProgressEvent;




        public bool FoundVideo { get; set; } = false;
        public bool FoundAudio { get; set; } = false;

        public bool VideoDownloadComplete { get; set; } = false;
        public bool AudioDownloadComplete { get; set; } = false;

        public Downloader(string url)
        {
            try
            {
                this.AllVideos = youtube.GetAllVideos(url);

                int index = 0;
                foreach (YouTubeVideo video in this.AllVideos)
                {
                    logger.Debug("index : " + index++);
                    logger.Debug("    AdaptiveKind = " + video.AdaptiveKind);
                    logger.Debug("    AudioBitrate = " + video.AudioBitrate);
                    logger.Debug("    AudioFormat = " + video.AudioFormat);
                    logger.Debug("    ContentLength = " + video.ContentLength);
                    logger.Debug("    FileExtension = " + video.FileExtension);
                    logger.Debug("    Format = " + video.Format);
                    logger.Debug("    FormatCode = " + video.FormatCode);
                    logger.Debug("    Fps = " + video.Fps);
                    logger.Debug("    FullName = " + video.FullName);
                    logger.Debug("    IsAdaptive = " + video.IsAdaptive);
                    logger.Debug("    IsEncrypted = " + video.IsEncrypted);
                    logger.Debug("    Resolution = " + video.Resolution);
                    logger.Debug("    Title = " + video.Title);
                    logger.Debug("    Uri = " + video.Uri);
                    logger.Debug("    WebSite = " + video.WebSite);
                }

            }
            catch (Exception)
            {
            }
        }

        public void DownloadStart()
        {
            thread = new Thread(DownloadWorker);
            thread.IsBackground = true;
            thread.Start();
        }


        public void DownloadWorker()
        {
            string videoFileName = Path.Combine(Application.StartupPath, "video.mp4");
            string audioFileName = Path.Combine(Application.StartupPath, "audio.mp4");
            string finalFileName = Path.Combine(Application.StartupPath, "final.avi");
            string targetFileName = "";

            YouTubeVideo video = GetBestVideo();
            YouTubeVideo audio = GetBestAudio();

            // 영상 다운로드 체크
            this.FoundVideo = (video != null ? true : false);
            this.VideoDownloadComplete = (video != null ? false : true);        // 영상이 없는 경우 다운로드 완료 처리

            // 음성 다운로드 체크
            this.FoundAudio = (audio != null ? true : false);
            this.AudioDownloadComplete = (audio != null ? false : true);        // 음성이 없는 경우 다운로드 완료 처리


            if (!this.FoundVideo && !this.FoundAudio)
            {
                logger.Error("영상/음성을 찾을 수 없습니다.");
                return;
            }

            if (this.FoundVideo)
            {
                targetFileName = Path.Combine(Application.StartupPath, video.FullName.Replace(video.FileExtension, ".avi"));
            }
            else
            {
                targetFileName = Path.Combine(Application.StartupPath, audio.FullName.Replace(audio.FileExtension, ".avi"));
            }


            var videoprogress = new Progress<int>(percent =>
            {
                logger.Debug(string.Format("영상 다운로드 : {0}%", percent));
                if (percent == 100)
                    this.VideoDownloadComplete = true;

                if (VideoProgressEvent != null)
                    VideoProgressEvent(percent);
            });

            var audioprogress = new Progress<int>(percent =>
            {
                logger.Debug(string.Format("음성 다운로드 : {0}%", percent));
                if (percent == 100)
                    this.AudioDownloadComplete = true;

                if (AudioProgressEvent != null)
                    AudioProgressEvent(percent);
            });


            // 영상 다운로드 시작
            if (this.FoundVideo)
            {
                logger.Debug("영상 다운로드 Start");
                DownloadVideoAsync(video.Uri, videoFileName, videoprogress);
            }
                

            // 음성 다운로드 시작
            if (this.FoundAudio)
            {
                logger.Debug("음성 다운로드 Start");
                DownloadVideoAsync(audio.Uri, audioFileName, audioprogress);
            }
                

            // 영상/음성 다운로드 완료 확인
            while (true)
            {
                if (this.VideoDownloadComplete && this.AudioDownloadComplete)
                    break;

                Thread.Sleep(1);
            }

            logger.Debug("영상/음성 다운로드 완료");

            // Merge
            logger.Debug("영상/음성 merge start");
            string command = "";
            if (this.FoundVideo)
                command += "-i \"" + videoFileName + "\" ";
            if (this.FoundAudio)
                command += "-i \"" + audioFileName + "\" ";
            command += "-preset veryfast ";
            command += "\"" + finalFileName + "\"";

            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                engine.ConvertProgressEvent += OnCombineProgress;
                engine.ConversionCompleteEvent += OnCombineComplete;
                engine.CustomCommand(command);
            }

            logger.Debug("최종 파일 적용");
            FileInfo sf = new FileInfo(finalFileName);
            FileInfo tf = new FileInfo(targetFileName);
            if (tf.Exists)
                tf.Delete();
            sf.MoveTo(targetFileName);
            
            File.Delete(videoFileName);
            File.Delete(audioFileName);

            logger.Debug("작업 완료");

            thread = null;
        }
        private void OnCombineProgress(object sender, ConvertProgressEventArgs e)
        {
            logger.Debug("영상/음성 합치기 : " + e.ProcessedDuration + " / " + e.TotalDuration);
        }

        private void OnCombineComplete(object sender, ConversionCompleteEventArgs e)
        {
            logger.Debug("영상/음성 합치기 완료");
        }


        public YouTubeVideo GetBestVideo()
        {
            if (this.AllVideos == null)
                return null;

            int highResolution = 0;
            YouTubeVideo highVideo = null;
            foreach (YouTubeVideo video in this.AllVideos)
            {
                if (video.AdaptiveKind.ToString().Equals(AdaptiveKind.Video.ToString()))
                    if (video.Resolution > highResolution)
                    {
                        highResolution = video.Resolution;
                        highVideo = video;
                    }
            }

            return highVideo;
        }

        public YouTubeVideo GetBestAudio()
        {
            if (this.AllVideos == null)
                return null;

            int highBitrate = 0;
            YouTubeVideo highVideo = null;
            foreach (YouTubeVideo video in this.AllVideos)
            {
                if (video.AdaptiveKind.ToString().Equals(AdaptiveKind.Audio.ToString()))
                    if (video.AudioBitrate > highBitrate)
                    {
                        highBitrate = video.AudioBitrate;
                        highVideo = video;
                    }
            }

            return highVideo;
        }


        public async Task DownloadVideoAsync(string url, string outputPath, IProgress<int> progress)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var canReportProgress = totalBytes != -1;

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, DOWNLOAD_PACKET_SIZE, true))
                {
                    var totalRead = 0L;
                    var buffer = new byte[DOWNLOAD_PACKET_SIZE];

                    while (true)
                    {
                        var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        if (read == 0)
                        {
                            break;
                        }

                        await fileStream.WriteAsync(buffer, 0, read);

                        totalRead += read;
                        if (canReportProgress)
                        {
                            progress.Report((int) (totalRead * 100 / totalBytes));
                        }
                    }
                    if (totalRead != totalBytes)
                    {
                        logger.Error("다운로드 실패");
                    }
                }
            }
        }


    }
}
