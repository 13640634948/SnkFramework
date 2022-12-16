using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace SnkFramework.Network.Web
{
    public class SnkWeb
    {
        public static async Task<(bool, string)> HttpGet(string uri)
        {
            var request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            using var response = await request.GetResponseAsync() as HttpWebResponse;

            if (response is { StatusCode: HttpStatusCode.OK })
            {
                await using var stream = response.GetResponseStream();
                if (stream != null)
                {
                    using var streamReader = new StreamReader(stream, Encoding.UTF8);
                    var context = await streamReader.ReadToEndAsync();
                    return (true, context);
                }
            }

            return (false, null);
        }

        public static async Task<bool> HttpDownload(string uri, string savePath)
        {
            var request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            using var response = await request.GetResponseAsync() as HttpWebResponse;
            if (response is not { StatusCode: HttpStatusCode.OK }) 
                return false;

            var buffer = new byte[1024 * 1024 * 2];
            var fileInfo = new FileInfo(savePath);
            if(fileInfo.Exists)
                fileInfo.Delete();
            if(fileInfo.Directory.Exists == false)
                fileInfo.Directory.Create();
            
            await using var fileStream = new FileStream(fileInfo.FullName, FileMode.CreateNew);
            await using var stream = response.GetResponseStream();
            if (stream == null)
                return false;
            
            var len = await stream.ReadAsync(buffer, 0, buffer.Length);
            await fileStream.WriteAsync(buffer, 0, len);
            
            await fileStream.FlushAsync();
            fileStream.Close();
            
            return true;
        }
    }
}