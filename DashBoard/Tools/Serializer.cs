using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;

namespace DashBoard.Tools
{
    public class Serializer
    {
        public static T DeserializeJSon<T>(string jsonString)
        {
            try
            {
                var _Bytes = Encoding.UTF8.GetBytes(jsonString);
                using (MemoryStream _Stream = new MemoryStream(_Bytes))
                {

                    DataContractJsonSerializer _Serializer = new DataContractJsonSerializer(typeof(T));

                    return (T)_Serializer.ReadObject(_Stream);
                }
            }
            catch (Exception ex)
            {
                DeserializeJava<T>(jsonString);
            }
            return default(T);
        }

        public static T DeserializeJava<T>(string jsonString)
        {
            JavaScriptSerializer serializer = new JavaScriptSerializer();
            try
            {
                return serializer.Deserialize<T>(jsonString);
            }
            catch
            {
                return default(T);
            }
        }

        public static string SerializeJSon<T>(T t)
        {
            var serializer = new System.Web.Script.Serialization.JavaScriptSerializer();
            var objectAsJsonString = serializer.Serialize(t);
            return objectAsJsonString;

        }
    }
}