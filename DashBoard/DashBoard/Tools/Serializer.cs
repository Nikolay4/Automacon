using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Web;

namespace DashBoard.Tools
{
    public class Serializer
    {
        public static T DeserializeJSon<T>(string jsonString)
        {
            try
            {
                var _Bytes = Encoding.Unicode.GetBytes(jsonString);
                using (MemoryStream _Stream = new MemoryStream(_Bytes))
                {

                    var _Serializer = new DataContractJsonSerializer(typeof(T));

                    return (T)_Serializer.ReadObject(_Stream);
                }
            }
            catch (Exception ex)
            {
                throw ex;
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