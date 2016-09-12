using System;
using System.Net;
using System.IO;
using System.Text;
using System.Security.Cryptography.X509Certificates;
using System.Runtime.Serialization.Json;


namespace TestTask
{

    class Program
    {
        private static string key;
        private static string amount;
        private static void GetData()
        {
            try
            {
                Console.Write("Enter Service Key: ");
                key = (Convert.ToUInt64(Console.ReadLine())).ToString();
                Console.Write("Enter Amount in format #.##: ");
                amount = (Convert.ToDecimal(Console.ReadLine())).ToString("F2");
            }
            catch (Exception)
            {
                Console.WriteLine("Uncorrect input");
                GetData();
            }
        }

        static void Main()
        {
            GetData();
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Ssl3;  // Без этой строки не работает на Windows 10
                X509Certificate cert = X509Certificate.CreateFromSignedFile("a.dobrovidov@gmail.com-cert.pem");
                ServicePointManager.ServerCertificateValidationCallback += (sender, certificate, chain, sslPolicyErrors) => { return true; };
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create("https://109.167.200.164:37000");
                request.ClientCertificates.Add(cert);
                request.Credentials = CredentialCache.DefaultCredentials;
                request.ProtocolVersion = HttpVersion.Version10;
                request.Method = "POST";
                request.UserAgent = "1.5.0";
                request.Accept = "application/json";
                request.ContentType = "application/json";
                using (Stream out_stream = request.GetRequestStream())
                {
                    Data data = new Data(key, amount);
                    ServerProcessRequest spr = new ServerProcessRequest();
                    spr.single_process = data;
                    spr.UpToJson(out_stream);
                    out_stream.Flush();
                }
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        using (StreamReader in_stream = new StreamReader(stream))
                        {
                            string str = in_stream.ReadToEnd();
                            Console.WriteLine(str);
                            using (var memoryStream = new MemoryStream(Encoding.UTF8.GetBytes(str)))
                            {
                                ServerProcessAnswer spa = new ServerProcessAnswer();
                                DataContractJsonSerializer jsonAnswer = new DataContractJsonSerializer(typeof(ServerProcessAnswer));
                                spa = (ServerProcessAnswer)jsonAnswer.ReadObject(memoryStream);
                                Console.WriteLine(spa.single_process_answer.id.ToString());
                                using (StreamWriter file =
                                             new StreamWriter(@"D:\Downloads\Dobrovidov_Aleksej_Fyodorovich\id.txt", true))
                                {
                                    file.WriteLine(spa.single_process_answer.id.ToString());
                                }
                            }
                        }
                    }
                }
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                Console.ReadKey();
            }
        }
    }
}
