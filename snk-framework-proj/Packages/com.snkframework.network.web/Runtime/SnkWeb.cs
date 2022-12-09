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

        public static async Task<bool> HttpDownload(string uri, string dirPath)
        {
            var request = WebRequest.CreateHttp(uri);
            request.Method = "GET";
            using var response = await request.GetResponseAsync() as HttpWebResponse;
            if (response is not { StatusCode: HttpStatusCode.OK }) 
                return false;

            var buffer = new byte[1024 * 1024 * 2];
            var fileName = Path.GetFileName(uri);
            var localFilePath = Path.Combine(dirPath, fileName);
            if (System.IO.File.Exists(localFilePath))
                System.IO.File.Delete(localFilePath);
            
            await using var fileStream = new FileStream(localFilePath, FileMode.CreateNew);
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