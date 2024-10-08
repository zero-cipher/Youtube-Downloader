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
using System.Diagnostics;
using System.Net;

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


        private const int DOWNLOAD_PACKET_SIZE = 10240;


        // 다운로드 대상 영상
        private YouTubeVideo VideoItem = null;
        private YouTubeVideo AudioItem = null;


        private bool VideoDownloadComplete;
        private bool AudioDownloadComplete;

        private int VideoDownloadPercent;
        private int AudioDownloadPercent;

        // 다운로드할 영상/음성 파일명
        private string VideoFileName = "";
        private string AudioFileName = "";

        // 최종 저장될 파일명
        //private string ConvertFileName = "convert.avi";
        private string SaveFileName = "";


        private Stopwatch timer = new Stopwatch();




        // 환경 옵션
        public string FFMpegOption { get; set; } = "";
        public int VideoQuality { get; set; }



        /// <summary>
        /// 대상 url의 모든 영상을 가져온 다음 다운로드 할 Video영상과 Audio영상을 선택한다.
        /// </summary>
        /// <param name="url"></param>
        public Downloader(string url, string savePath)
        {
            try
            {
                logger.Debug("Downloader Start.");
                logger.Debug("    URL = " + url + ", Destination Path = " + savePath);

                this.AllVideos = youtube.GetAllVideos(url);

                if (this.VideoQuality == 1)
                {
                    this.VideoItem = GetBestVideo();
                }
                else
                {
                    this.VideoItem = Get720pVideo();
                }
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

                logger.Debug("Video File Name = " + this.VideoFileName);
                logger.Debug("Audio File Name = " + this.AudioFileName);
                logger.Debug("Save File Name = " + this.SaveFileName);
            }
            catch (Exception)
            {
            }

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
        private YouTubeVideo GetBestVideo()
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

        private YouTubeVideo Get720pVideo()
        {
            if (this.AllVideos == null)
                return null;

            int highResolution = 0;
            YouTubeVideo highVideo = null;
            foreach (YouTubeVideo video in this.AllVideos)
            {
                if (video.AdaptiveKind.ToString().Equals(AdaptiveKind.Video.ToString()))
                {
                    // 720p일경우 해당 영상 리턴
                    if ((video.Resolution == 720) && (video.Format == VideoFormat.Mp4))
                        return video;

                    if (video.Resolution > highResolution)
                    {
                        highResolution = video.Resolution;
                        highVideo = video;
                    }
                }
            }

            // 720p 영상이 없을 경우 가장 높은 해상도 영상 리턴
            return highVideo;
        }


        


        /// <summary>
        /// 전체 음성에서 음질이 가장 좋은 영상을 가져온다.
        /// </summary>
        /// <returns></returns>
        private YouTubeVideo GetBestAudio()
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



        public void DownloadStart()
        {
            // 상태 초기화
            this.VideoDownloadComplete = false;
            this.AudioDownloadComplete = false;

            this.VideoDownloadPercent = 0;
            this.AudioDownloadPercent = 0;

            this.timer.Start();

            if ((this.VideoItem == null) && (this.AudioItem == null))
            {
                if (StatusChangeEvent != null)
                {
                    StatusChangeEvent("URL로부터 영상/음성을 가져오는데 실패하였습니다.");
                    if (DownloadComplete != null)
                    {
                        DownloadComplete();
                    }
                }

                return;
            }

            // 영상 다운로드 시작
            if (this.VideoItem == null)
            {
                this.VideoDownloadComplete = true;
                this.VideoDownloadPercent = 100;
            }
            else
            {
                VideoDownloadWorker();
            }

            // 음성 다운로드 시작
            if (this.AudioItem == null)
            {
                this.AudioDownloadComplete = true;
                this.AudioDownloadPercent = 100;
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

            // 영상파일이 완전히 다운로드되었는지 확인
            if (this.VideoDownloadPercent != 100)
            {
                StatusChangeEvent("영상 파일 다운로드에 실패하였습니다.");
                if (DownloadComplete != null)
                {
                    DownloadComplete();
                }
                return;
            }

            // 음성 파일이 완전히 다운로드 되었는지 확인
            if (this.AudioDownloadPercent != 100)
            {
                StatusChangeEvent("음성 파일 다운로드에 실패하였습니다.");
                if (DownloadComplete != null)
                {
                    DownloadComplete();
                }
                return;
            }

            // 영상 합치기 메세지 출력
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

            // ini 파일에 저장된 ffmpeg option 적용
            command += FFMpegOption + " ";

            // ffmpeg 출력 파일 설정
            command += "\"" + this.SaveFileName + "\"";


            Stopwatch convertTimer = new Stopwatch();
            convertTimer.Start();

            // ffmpeg 명령 실행
            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                // FFMpeg ConvertProgressEvent
                engine.ConvertProgressEvent += (s, e) =>
                {
                    //logger.Debug("영상/음성 합치기 : " + e.ProcessedDuration + " / " + e.TotalDuration);
                };

                // FFMpeg ConversionCompleteEvent
                engine.ConversionCompleteEvent += (s, e) =>
                {
                    logger.Debug("영상/음성 합치기 완료");
                };

                // execute FFMpeg command
                engine.CustomCommand(command);
            }

            logger.Debug("ffmpeg command : " + command);
            logger.Debug("원본 영상 파일 크기 : " + Utils.GetFileSize(this.VideoFileName).ToString("#,##0") + " bytes");
            logger.Debug("원본 음성 파일 크기 : " + Utils.GetFileSize(this.AudioFileName).ToString("#,##0") + " bytes");
            logger.Debug("동영상 파일 크기 : " + Utils.GetFileSize(this.SaveFileName).ToString("#,##0") + " bytes");
            logger.Debug("동영상 합치기 소요 시간 : " + convertTimer.Elapsed.ToString());

            // 다운로드 완료 메세지 출력
            if (StatusChangeEvent != null)
            {
                StatusChangeEvent("다운로드 완료");
            }

            // 다운로드 완료 이벤트 발생
            if (DownloadComplete != null)
            {
                DownloadComplete();
            }

            logger.Debug("동영상 다운로드 전체 소요 시간 : " + timer.Elapsed.ToString());
        }




        /// <summary>
        /// 영상 파일 다운로드
        /// </summary>
        private void VideoDownloadWorker()
        {
            var progress = new Progress<int>(percent =>
            {
                // 에러 발생. 다운로드 중지됨
                if (percent == -999)
                {
                    this.VideoDownloadComplete = true;
                    if (StatusChangeEvent != null)
                    {
                        StatusChangeEvent("영상 파일 다운로드중 오류가 발생했습니다.");
                    }
                }
                else
                {
                    this.VideoDownloadPercent = percent;

                    //logger.Debug(string.Format("영상 다운로드 : {0}%", percent));
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
                }
            });

            // 영상 다운로드 시작
            logger.Debug("영상 다운로드 Start");
            DownloadVideo(this.VideoItem, this.VideoFileName, progress);
        }

        /// <summary>
        /// 음성 파일 다운로드
        /// </summary>
        private void AudioDownloadWorker()
        {
            var progress = new Progress<int>(percent =>
            {
                // 에러 발생. 다운로드 중지됨
                if (percent == -999)
                {
                    this.AudioDownloadComplete = true;
                    if (StatusChangeEvent != null)
                    {
                        StatusChangeEvent("음성 파일 다운로드중 오류가 발생했습니다.");
                    }
                }
                else
                {
                    this.AudioDownloadPercent = percent;

                    //logger.Debug(string.Format("영상 다운로드 : {0}%", percent));
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
                }
            });

            // 영상 다운로드 시작
            logger.Debug("음성 다운로드 Start");
            DownloadVideo(this.AudioItem, this.AudioFileName, progress);
        }



        private void DownloadVideo(YouTubeVideo video, string outputPath, IProgress<int> progress)
        {
            using (var client = new WebClient())
            {
                client.DownloadProgressChanged += (s, e) =>
                {
                    progress.Report(e.ProgressPercentage);
                    Console.WriteLine($"Download progress: {e.ProgressPercentage}%");
                };

                client.DownloadFileCompleted += (s, e) =>
                {
                    if (e.Error != null)
                    {
                        progress.Report(-999);
                        logger.Error("download complete. but error occurred.");
                    }
                };

                client.DownloadFileAsync(new Uri(video.Uri), outputPath);
            }
        }


    }
}
