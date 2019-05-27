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
            request.Headers.Add("Authorization", "Bearer " + "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NTc1ODk5ODgsImlkIjoiIiwib3JpZ19pYXQiOjE1NTc1MDM1ODgsInVzZXJpZCI6NTExMzE0LCJ1c2VybmFtZSI6InBsYW1lbmEuaS50b2Rvcm92YWwyOCJ9.bY-nLbGp8NzerGH0zwOwCldh9I7dkTU7Jt3mUmSdmes8vrD6vyTGOYksIH60NPp7oLIqP9pWj1y-HuAiokp27NH6nVH9HuOMPfOgJH7scxaLWlpAK-p6GLm5x-bisqBEsOFRULrAyi4MR3M7jQ7xQ-as8qolIQqNAcep5OZ4ykA3Y7QlcABQk_M3ftTrQ326xiyTbN-HQ-_DSyuMeKCralEFtHKhA_acUIKhkslEzcl5xY1-dXEg239Qg0u2wo8EeJbbO_L1Bmp2ybTiThPBRxizNjuINajtLp4dFWOWvYPIn7mU5GIV8IcNXerT_tqFWQfJ8a1qtFq04i9wO5C6Ww");
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
