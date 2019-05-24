using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace WhatToWatch.Parsers
{
    public static class WebParser<T> where T : new()
    {
        public static T GetInfo(string path)
        {
            HttpWebRequest request =
                (HttpWebRequest)WebRequest.Create(string.Format(path));

            request.Method = "GET";
            request.PreAuthenticate = true;

            //FixThis!!!!!!!!!!!!!!!
            request.Headers.Add("Authorization", "Bearer " + "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NTcwNzI4NTAsImlkIjoiIiwib3JpZ19pYXQiOjE1NTY5ODY0NTAsInVzZXJpZCI6NTExMzE0LCJ1c2VybmFtZSI6InBsYW1lbmEuaS50b2Rvcm92YWwyOCJ9.Uu3LPe-P0oxuR4cJzqC8WqBt0T6Ft6j3DVVb6LXdpMMtlPjpnW1lvDGSOYdzcrzzUmD9NRwRSxlCh6LdM0u3PBJqPDWSXuxNnUPjU6kY5_I88w9YRyzVqEfK9J-j2vHzYAQP_4EYWTkB6vPtaf69MXL9vTkiJiekraMrI4DfmAeB1yvdr9V5bHnmjUnNLu3JaFEMlk9cU9b50Xi-9u0SdfLTpv05FCJZVY7UZ06Vi7odqCtda5jRFNue-DEr9DjLJZu6PW5l4kNRuLxN4npAMpnW012URFtuR3TLk4oTRI_h_FobdgG7Evjq55zii4HOd0BAPsBWYiSBnP6ThVQgxg");
            request.Accept = "application /json";

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            string jsonString;
            using (Stream stream = response.GetResponseStream())   //modified from your code since the using statement disposes the stream automatically when done
            {
                StreamReader reader = new StreamReader(stream, System.Text.Encoding.UTF8);
                jsonString = reader.ReadToEnd();
            }

            return JSONParser<T>.ReadJson(jsonString);
        }
    }
}
