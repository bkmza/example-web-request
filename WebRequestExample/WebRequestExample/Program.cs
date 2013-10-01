using System;
using System.IO;
using System.Net;
using System.Text;

namespace WebRequestExample
{
    class Program
    {
        private static CookieCollection _cookies = new CookieCollection();
        private static string _sourceCode = string.Empty;

        static void Main(string[] args)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://www.facebook.com");
            request.CookieContainer = new CookieContainer();
            request.CookieContainer.Add(_cookies);
            //Get the response from the server and save the cookies from the first request..
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            _cookies = response.Cookies;


            string getUrl = "https://www.facebook.com/login.php?login_attempt=1";
            string postData = String.Format("email={0}&pass={1}", "ilya.usikov@gmail.com", "86w77t34");
            response = PostHttpWebResponse(getUrl, postData);
            _cookies = response.Cookies;


            string getUrlBar = "https://www.facebook.com/BlackhallBar";
            string postDataBar = "";
            response = GetHttpWebResponse(getUrlBar, postDataBar);
            using (StreamReader sr = new StreamReader(response.GetResponseStream()))
            {
                _sourceCode = sr.ReadToEnd();
            }
            File.WriteAllText(@"C:\access.html", _sourceCode);
        }

        public static HttpWebResponse PostHttpWebResponse(string getUrl, string postData)
        {
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            getRequest.CookieContainer = new CookieContainer();
            getRequest.CookieContainer.Add(_cookies); //recover cookies request
            getRequest.Method = WebRequestMethods.Http.Post;
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            getRequest.AllowWriteStreamBuffering = true;
            getRequest.ProtocolVersion = HttpVersion.Version11;
            getRequest.AllowAutoRedirect = false;
            getRequest.ContentType = "application/x-www-form-urlencoded";

            byte[] byteArray = Encoding.ASCII.GetBytes(postData);
            getRequest.ContentLength = byteArray.Length;

            Stream newStream = getRequest.GetRequestStream(); //open connection
            newStream.Write(byteArray, 0, byteArray.Length); // Send the data.
            newStream.Close();

            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            return getResponse;
        }

        public static HttpWebResponse GetHttpWebResponse(string getUrl, string postData)
        {
            HttpWebRequest getRequest = (HttpWebRequest)WebRequest.Create(getUrl);
            getRequest.CookieContainer = new CookieContainer();
            getRequest.CookieContainer.Add(_cookies); //recover cookies request
            getRequest.Method = WebRequestMethods.Http.Get;
            getRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            getRequest.AllowWriteStreamBuffering = true;
            getRequest.ProtocolVersion = HttpVersion.Version11;
            getRequest.AllowAutoRedirect = true;
            getRequest.ContentType = "application/x-www-form-urlencoded";

            HttpWebResponse getResponse = (HttpWebResponse)getRequest.GetResponse();
            return getResponse;
        }
    }
}
