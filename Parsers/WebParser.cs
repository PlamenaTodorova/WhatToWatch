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
            request.Headers.Add("Authorization", "Bearer " + "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjE1NDA1ODgwMDAsImlkIjoiIiwib3JpZ19pYXQiOjE1NDA1MDE2MDAsInVzZXJpZCI6NTExMzE0LCJ1c2VybmFtZSI6InBsYW1lbmEuaS50b2Rvcm92YWwyOCJ9.pMDdtTE-VQFYaY6WIDKM8beCz1aHe-CVqJ29btydx_ZV64QgxMeR5XSEtM58ir9ZhJxBCyfVA1uVU7HVLLX-u09KIXT5UjyswAGNJHfq3yg-8K2fYT5fFXj5xfoLJWGYAerAuPcnQNJRgsgTkTk4V010EqgXhOg8aa5lhahuXEFIWh1duvi5WD2PlKExrtBELHnTb5PF6TQj2weLclBQxU0sb3YA54amTvut3hq0BxNmfGSfKsuAyx1ab5tFqyoNcvyxD5PUOfOWd_2m-bKfpr3WKuVVubao_N0rl9KZ7H5X_t0XoEMYi3M8jfG-f9wRPZO14HmTf9Bscl8896vQeQ");
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
