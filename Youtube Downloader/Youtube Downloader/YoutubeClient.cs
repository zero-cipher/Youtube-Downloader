using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Youtube_Downloader
{
    class YoutubeClient
    {
        private static readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        private VideoLibrary.YouTube youtube = VideoLibrary.YouTube.Default;
        public IEnumerable<VideoLibrary.YouTubeVideo> AllVideos;


        public void GetVideoInfo(string url)
        {
            try
            {
                this.AllVideos = youtube.GetAllVideos(url);

                int index = 0;
                foreach(VideoLibrary.YouTubeVideo video in this.AllVideos)
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
        

    }
}
