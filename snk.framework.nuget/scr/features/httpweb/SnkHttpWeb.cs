using System;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnkFramework.NuGet.Features
{
    namespace HttpWeb
    {
        public class SnkHttpWeb
        {
            public static async Task<(bool, string)> HttpGet(string uri)
            {
                var request = WebRequest.CreateHttp(uri);
                request.Method = "GET";
                try
                {
                    using (var response = await request.GetResponseAsync() as HttpWebResponse)
                    {
                        if (response.StatusCode == HttpStatusCode.OK)
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream != null)
                                {
                                    using (var streamReader = new StreamReader(stream, Encoding.UTF8))
                                    {
                                        var context = await streamReader.ReadToEndAsync();
                                        return (true, context);
                                    }
                                }
                            }

                        }
                    }
                }
                catch (Exception e)
                {
                    return (false, e.Message);
                    throw;
                }
                return (false, null);
            }

            public static async Task<bool> HttpDownload(string uri, string savePath)
            {
                try
                {
                    var request = WebRequest.CreateHttp(uri);
                    request.Method = "GET";
                    using (var response = await request.GetResponseAsync() as HttpWebResponse)
                    {
                        if (response.StatusCode != HttpStatusCode.OK)
                            return false;

                        var buffer = new byte[1024 * 1024 * 2];
                        var fileInfo = new FileInfo(savePath);
                        if (fileInfo.Exists)
                            fileInfo.Delete();
                        if (fileInfo.Directory.Exists == false)
                            fileInfo.Directory.Create();

                        using (var fileStream = new FileStream(fileInfo.FullName, FileMode.CreateNew))
                        {
                            using (var stream = response.GetResponseStream())
                            {
                                if (stream == null)
                                    return false;

                                var len = await stream.ReadAsync(buffer, 0, buffer.Length);
                                await fileStream.WriteAsync(buffer, 0, len);

                                await fileStream.FlushAsync();
                                fileStream.Close();
                            }
                        }
                    }
                    return true;

                }
                catch (Exception e)
                {
                    return false;
                }
            }
        }
    }
}