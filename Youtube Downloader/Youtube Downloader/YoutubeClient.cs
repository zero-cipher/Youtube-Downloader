using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using VideoLibrary;
using System.Windows.Forms;
using MediaToolkit;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Youtube_Downloader
{

    class YoutubeClient
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);




        private YouTube youtube = YouTube.Default;
        public IEnumerable<YouTubeVideo> AllVideos;


        public void GetVideoInfo(string url)
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



        public void DownloadBestVideo()
        {
            YouTubeVideo highVideo = GetBestVideo();
            if (highVideo != null)
            {
                DownloadYoutubeVideo(highVideo, Path.Combine(Application.StartupPath, "video.mp4"));
            }
        }

        public void DownloadBestAudio()
        {
            YouTubeVideo highVideo = GetBestAudio();

            if (highVideo != null)
            {
                DownloadYoutubeVideo(highVideo, Path.Combine(Application.StartupPath, "audio.mp4"));
            }
        }

        public void Combine()
        {

            string command = "-i \"" + Path.Combine(Application.StartupPath, "video.mp4") + "\" " +
                             "-i \"" + Path.Combine(Application.StartupPath, "audio.mp4") + "\" " +
                             "-preset veryfast  \"" + Path.Combine(Application.StartupPath, "final.mp4") + "\"";



            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                engine.ConvertProgressEvent += OnCombineProgress;
                engine.ConversionCompleteEvent += OnCombineComplete;
                engine.CustomCommand(command);

            }


            /*

            Process p = new Process();
            p.StartInfo.FileName = "ffmpeg.exe";
            p.StartInfo.Arguments = command;
            p.Start();
            p.WaitForExit();
            */
        }

        public void Combine2()
        {

            string command = "-i \"" + Path.Combine(Application.StartupPath, "video.mp4") + "\" " +
                             "-i \"" + Path.Combine(Application.StartupPath, "audio.mp4") + "\" " +
                             "-preset veryfast  \"" + Path.Combine(Application.StartupPath, "final.avi") + "\"";



            using (Engine engine = new Engine(Path.Combine(Application.StartupPath, "ffmpeg.exe")))
            {
                engine.ConvertProgressEvent += OnCombineProgress;
                engine.ConversionCompleteEvent += OnCombineComplete;
                engine.CustomCommand(command);

            }


            /*

            Process p = new Process();
            p.StartInfo.FileName = "ffmpeg.exe";
            p.StartInfo.Arguments = command;
            p.Start();
            p.WaitForExit();
            */
        }

        private void OnCombineProgress(object sender, ConvertProgressEventArgs e)
        {
            logger.Debug("combine progress. " + e.ProcessedDuration + " / " + e.TotalDuration);
        }

        private void OnCombineComplete(object sender, ConversionCompleteEventArgs e)
        {
            logger.Debug("combine complete");
        }


        public async Task DownloadVideoAsync(string url, string outputPath, IProgress<double> progress)
        {
            using (var client = new HttpClient())
            using (var response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var totalBytes = response.Content.Headers.ContentLength ?? -1L;
                var canReportProgress = totalBytes != -1;

                using (var stream = await response.Content.ReadAsStreamAsync())
                using (var fileStream = new FileStream(outputPath, FileMode.Create, FileAccess.Write, FileShare.None, 8192, true))
                {
                    var totalRead = 0L;
                    var buffer = new byte[8192];

                    while(true)
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
                            progress.Report((double)totalRead / totalBytes);
                        }
                    }
                }
            }
        }




        private void DownloadYoutubeVideo(YouTubeVideo y, string patch)
        {
            int total = 0;
            FileStream fs = null;
            Stream streamweb = null;
            WebResponse w_response = null;
            try
            {
                WebRequest w_request = WebRequest.Create(y.Uri);
                if (w_request != null)
                {
                    w_response = w_request.GetResponse();
                    if (w_response != null)
                    {
                        fs = new FileStream(patch, FileMode.Create);
                        byte[] buffer = new byte[128 * 1024];
                        int bytesRead = 0;
                        streamweb = w_response.GetResponseStream();
                        logger.Debug("Download Started");
                        do
                        {
                            bytesRead = streamweb.Read(buffer, 0, buffer.Length);
                            fs.Write(buffer, 0, bytesRead);
                            total += bytesRead;
                            logger.Debug($"\rDownloading ({Math.Round(((double)total / (int)y.ContentLength) * 100, 2)}%) {total}/{y.ContentLength}     ");
                        } while (bytesRead > 0);
                        logger.Debug("\nDownload Complete");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                Process.Start(Directory.GetCurrentDirectory());
            }
            finally
            {
                if (w_response != null) w_response.Close();
                if (fs != null) fs.Close();
                if (streamweb != null) streamweb.Close();
            }
        }
    }
}
