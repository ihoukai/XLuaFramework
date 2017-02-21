using System;
using System.Diagnostics;
using System.Net;
using System.Threading;

namespace XLuaFramework
{
    public class ResDownloader
    {
        // 当前正在下载的资源数量
        private static int _curDownloadingCount = 0;
        public static int CurDownloadingCount
        {
            get { return _curDownloadingCount; }
        }

        public static void Download(string url,
                           string fileName,
                           Action<string> OnProgress,
                           Action OnSuccess,
                           Action<Exception> OnError
                           )
        {
            Interlocked.Increment(ref _curDownloadingCount);
            using (WebClient client = new WebClient())
            {
                Stopwatch sw = new Stopwatch();
                sw.Start();
                client.DownloadProgressChanged += new DownloadProgressChangedEventHandler(delegate (object sender, DownloadProgressChangedEventArgs e)
                {
                    string value = string.Format("{0} kb/s", (e.BytesReceived / 1024d / sw.Elapsed.TotalSeconds).ToString("0.00"));
                    if (OnProgress != null)
                    {
                        OnProgress(value);
                    }
                    if (e.ProgressPercentage == 100 && e.BytesReceived == e.TotalBytesToReceive)
                    {
                        sw.Reset();
                        if (OnSuccess != null)
                        {
                            OnSuccess();
                        }
                        Interlocked.Decrement(ref _curDownloadingCount);
                    }
                });
                try
                {
                    client.DownloadFileAsync(new System.Uri(url), fileName);
                }
                catch (Exception e)
                {
                    Interlocked.Decrement(ref _curDownloadingCount);
                    Log.Error(e);
                    if (OnError != null)
                    {
                        OnError(e);
                    }
                }
            }
        }
    }
}