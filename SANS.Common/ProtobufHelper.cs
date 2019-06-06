using ProtoBuf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;


namespace SANS.Common
{
    public class ProtobufHelper
    {
        public static byte[] ObjectToBytes<T>(T instance)
        {
            try
            {
                byte[] array;
                if (instance == null)
                {
                    array = new byte[0];
                }
                else
                {
                    MemoryStream memoryStream = new MemoryStream();
                    Serializer.Serialize(memoryStream, instance);
                    array = new byte[memoryStream.Length];
                    memoryStream.Position = 0L;
                    memoryStream.Read(array, 0, array.Length);
                    memoryStream.Dispose();
                }

                return array;

            }
            catch (Exception ex)
            {

                return new byte[0];
            }
        }

        public static T BytesToObject<T>(byte[] bytesData, int offset, int length)
        {
            if (bytesData.Length == 0)
            {
                return default(T);
            }
            try
            {
                MemoryStream memoryStream = new MemoryStream();
                memoryStream.Write(bytesData, 0, bytesData.Length);
                memoryStream.Position = 0L;
                T result = Serializer.Deserialize<T>(memoryStream);
                memoryStream.Dispose();
                return result;
            }
            catch (Exception ex)
            {
                return default(T);
            }
        }

    }
}
