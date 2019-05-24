using Newtonsoft.Json;
using System;
using System.IO;
using WhatToWatch.Utilities;

namespace WhatToWatch.Parsers
{
    public class JSONParser<T>
        where T : new()
    {
        //Reading a Json file
        public static T ReadFile(string filePath)
        {
            T collection = new T();
            int attemtps = 5;

            for (int i = 0; i < attemtps; i++)
            {
                try
                {
                    string json = File.ReadAllText(filePath);

                    return ReadJson(json);
                }
                catch (Exception)
                {
                    continue;
                }
            }

            //Should reach here only on first run when there is no folder Files
            Directory.CreateDirectory(Directory.GetCurrentDirectory() + Constants.Folder);

            WriteJson(collection, filePath);

            return collection;
        }

        //Translating the json file into an object
        public static T ReadJson(string json)
        {
            T collection = JsonConvert.DeserializeObject<T>(json);

            if (collection == null)
                return new T();

            return collection;
        }

        public static void WriteJson(T collection, string filePath)
        {
            string text = JsonConvert.SerializeObject(collection);
            File.WriteAllText(filePath, text);
        }
    }
}