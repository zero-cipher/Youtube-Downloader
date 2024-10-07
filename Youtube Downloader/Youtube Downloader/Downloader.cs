using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using System.Windows.Forms;
using System.IO;
using System.Threading;
using System.Net.Http;
using MediaToolkit;

namespace Youtube_Downloader
{
    class Downloader
    {

        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private YouTube youtube = new YouTube();
        private IEnumerable<YouTubeVideo> AllVideos;


        public delegate void ProgressEventHandler(int percent);
        public event ProgressEventHandler VideoProgressEvent;
        public event ProgressEventHandler AudioProgressEvent;
        public delegate void StatusChangeEventHandler(string message);
        public event StatusChangeEventHandler StatusChangeEvent;
        public delegate void DownloadCompleteEventHandler();
        public event DownloadCompleteEventHandler DownloadComplete;


        private const int DOWNLOAD_PACKET_SIZE = 64 * 1024;


        // 다운로드 대상 영상
        private YouTubeVideo VideoItem = null;
        private YouTubeVideo AudioItem = null;


        private bool VideoDownloadComplete;
        private bool AudioDownloadComplete;


        public string FFMpegOption { get; set; } = "";

        // 다운로드할 영상/음성 파일명
        private string VideoFileName = "";
        private string AudioFileName = "";

        // 최종 저장될 파일명
        //private string ConvertFileName = "convert.avi";
        private string SaveFileName = "";

        /// <summary>
        /// 대상 url의 모든 영상을 가져온 다음 다운로드 할 Video영상과 Audio영상을 선택한다.
        /// </summary>
        /// <param name="url"></param>
        public Downloader(string url, string savePath)
        {
            try
            {
                this.AllVideos = youtube.GetAllVideos(url);


                this.VideoItem = GetBestVideo();
                this.AudioItem = GetBestAudio();

#if DEBUG
                logger.Debug("[영상 파일 정보]");
                GetVideoInformation(this.VideoItem);
                logger.Debug("[음성 파일 정보]");
                GetVideoInformation(this.AudioItem);
#endif

                // 대상폴더가 없으면 생성
                DirectoryInfo df = new DirectoryInfo(savePath);
                if (!df.Exists)
                    df.Create();

                // 영상 정보가 있으면
                if (this.VideoItem != null)
                {
                    this.VideoFileName = Path.Combine(Application.StartupPath, "video" + this.VideoItem.FileExtension);
                    this.SaveFileName = Path.Combine(savePath, this.VideoItem.FullName.Replace(this.VideoItem.FileExtension, ".avi"));  // 파일 확장자를 ".avi"로 변경
                }
                if (this.AudioItem != null)
                {
                    this.AudioFileName = Path.Combine(Application.StartupPath, "audio.mp4");
                    if (string.IsNullOrEmpty(this.SaveFileName))
                    {
                        this.SaveFileName = Path.Combine(savePath, this.AudioItem.FullName + ".avi");
                    }
                }
            }
            catch (Exception)
            {
            }

        }


        public void DownloadStart()
        {
            // 상태 초기화
            this.VideoDownloadComplete = false;
            this.AudioDownloadComplete = false;


            if ((this.VideoItem == null) && (this.AudioItem == null))
            {
                if (StatusChangeEvent != null)
                {
                    StatusChangeEvent("URL로부터 영상/음성을 가져오는데 실패하였습니다.");
                }

                return;
            }

            // 영상 다운로드 시작
            if (this.VideoItem == null)
            {
                this.VideoDownloadComplete = true;
            }
            else
            {
                VideoDownloadWorker();
            }

            // 음성 다운로드 시작
            if (this.AudioItem == null)
            {
                this.AudioDownloadComplete = true;
            }
            else
            {
                AudioDownloadWorker();
            }


            if (StatusChangeEvent != null)
            {
                StatusChangeEvent("영상/음성 파일 다운로드를 시작합니다.");
            }

            Thread thread = new Thread(ConvertWorker);
            thread.IsBackground = true;
            thread.Start();
        }


        /// <summary>
        /// 영상/음성 파일 다운로드가 완료될때까지 대기한 뒤 영상과 음성 파일을 합쳐 최종 파일을 생성한다.
        /// </summary>
        private void ConvertWorker()
        {
            // 영상/음성파일이 다운로드 될때까지 대기
            while (true)
            {
                if (this.VideoDownloadComplete && this.AudioDownloadComplete)
                    break;

                Thread.Sleep(1);
            }

            if (StatusChangeEvent != null)
            {
                StatusChangeEvent("영상과 음성파일을 하나의 파일로 합치는 중입니다.");
            }

            string command = "";
            
            // ffmpeg 입력 파일 설정
            if (this.VideoItem != null)
                command += "-i \"" + this.VideoFileName + "\" ";
            if (this.AudioItem != null)
                command += "-i \"" + this.AudioFileName + "\" ";

            // ffmpeg 옵션 지정
            //            command += "-preset veryfast ";     // preset - ultrafast, superfast, veryfast
            //command += "-c:v libx264 ";         // 비디오 - H.264 코덱
            //command += "-c:a mp3 ";             // 음성 - MP3 코덱

            command += FFMpegOption + " ";


            // ffmpeg 출력 파일 설정
            command += "\"" + this.SaveFileName + "\"";


            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                engine.ConvertProgressEvent += OnCombineProgress;
                engine.ConversionCompleteEvent += OnCombineComplete;
                engine.CustomCommand(command);
            }

            if (StatusChangeEvent != null)
            {
                StatusChangeEvent("다운로드 완료");
            }

            if (DownloadComplete != null)
            {
                DownloadComplete();
            }

        }


        private void OnCombineProgress(object sender, ConvertProgressEventArgs e)
        {
            logger.Debug("영상/음성 합치기 : " + e.ProcessedDuration + " / " + e.TotalDuration);
        }

        private void OnCombineComplete(object sender, ConversionCompleteEventArgs e)
        {
            logger.Debug("영상/음성 합치기 완료");
        }


        /// <summary>
        /// 영상 파일 다운로드
        /// </summary>
        private void VideoDownloadWorker()
        {
            var progress = new Progress<int>(percent =>
            {
                logger.Debug(string.Format("영상 다운로드 : {0}%", percent));
                if (VideoProgressEvent != null)
                    VideoProgressEvent(percent);

                if (percent == 100)
                {
                    this.VideoDownloadComplete = true;
                    if (StatusChangeEvent != null)
                    {
                        StatusChangeEvent("영상 파일 다운로드를 완료했습니다.");
                    }
                }
                    
            });

            // 영상 다운로드 시작
            logger.Debug("영상 다운로드 Start");
            DownloadVideoAsync(this.VideoItem.Uri, this.VideoFileName, progress);
        }

        /// <summary>
        /// 음성 파일 다운로드
        /// </summary>
        private void AudioDownloadWorker()
        {
            var progress = new Progress<int>(percent =>
            {
                logger.Debug(string.Format("음성 다운로드 : {0}%", percent));
                if (AudioProgressEvent != null)
                    AudioProgressEvent(percent);

                if (percent == 100)
                {
                    this.AudioDownloadComplete = true;
                    if (StatusChangeEvent != null)
                    {
                        StatusChangeEvent("음성 파일 다운로드를 완료했습니다.");
                    }
                }
                
            });

            // 영상 다운로드 시작
            logger.Debug("음성 다운로드 Start");
            DownloadVideoAsync(this.AudioItem.Uri, this.AudioFileName, progress);
        }


        private void GetVideoInformation(YouTubeVideo video)
        {
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


        /// <summary>
        /// 전체 영상에서 해상도가 가장 높은 영상을 가져온다.
        /// </summary>
        /// <returns></returns>
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

        /// <summary>
        /// 전체 음성에서 음질이 가장 좋은 영상을 가져온다.
        /// </summary>
        /// <returns></returns>
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



        /// <summary>
        /// 비동기 방식으로 지정한 영상을 다운로드 한다.
        /// </summary>
        /// <param name="url"></param>
        /// <param name="outputPath"></param>
        /// <param name="progress"></param>
        /// <returns></returns>
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
                            progress.Report((int)(totalRead * 100 / totalBytes));
                        }
                    }
                    if (totalRead != totalBytes)
                    {
                        if (StatusChangeEvent != null)
                        {
                            StatusChangeEvent("다운로드중 오류가 발생하였습니다.");
                        }
                    }
                }
            }
        }
    }
}
