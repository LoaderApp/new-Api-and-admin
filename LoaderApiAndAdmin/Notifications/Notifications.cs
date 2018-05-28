using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace LoaderApiAndAdmin.Notifications
{
    public static class Notifications
    {
        const string url = "https://us-central1-load-9f46f.cloudfunctions.net/sendPushNotification";

        public static void SendPushNotification(string dev, string msg)
        {
            var json = Newtonsoft.Json.JsonConvert.SerializeObject(new
            {
                receiverDeviceId = dev,
                message = msg
            });
            var request = WebRequest.CreateHttp(url);
            request.Method = "POST";
            request.ContentType = "application/json";
            var buffer = Encoding.UTF8.GetBytes(json);
            request.ContentLength = buffer.Length;
            request.GetRequestStream().Write(buffer, 0, buffer.Length);
            var response = request.GetResponse();
            json = (new StreamReader(response.GetResponseStream())).ReadToEnd();
            // TODO: parse response (contained in `json` variable) as appropriate



        }

    }
}