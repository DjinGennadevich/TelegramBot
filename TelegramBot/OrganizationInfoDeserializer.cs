using System;
using System.IO;
using System.Runtime.Serialization.Json;

namespace TelegramBot
{
    internal static class OrganizationInfoDeserializer
    {
        public static RootObject Deserialize(string fileName)
        {
            using (FileStream fileStream = new FileStream(fileName, FileMode.Open))
            {
                try
                {
                    return new DataContractJsonSerializer(typeof(RootObject)).ReadObject(fileStream) as RootObject;
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        public static RootObject Deserialize(Stream jsonStream)
        {
            try
            {
                return new DataContractJsonSerializer(typeof(RootObject)).ReadObject(jsonStream) as RootObject;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
