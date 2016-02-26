using MSTest.Console.Extended.TRX;
using System.IO;
using System.Xml.Serialization;

namespace MSTest.Console.Extended.Utilities
{
    public static class FileSystemTools
    {
        public static void SerializeTestRun(TestRun updatedTestRun, string trxDest)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(TestRun));
            TextWriter writer = new StreamWriter(trxDest);
            using (writer)
            {
                serializer.Serialize(writer, updatedTestRun); 
            }
        }

        public static TestRun DeserializeTestRun(string trxPath)
        {
            TestRun testRun = null;
            if (File.Exists(trxPath))
            {
                XmlSerializer serializer = new XmlSerializer(typeof(TestRun));
                StreamReader reader = new StreamReader(trxPath);
                testRun = (TestRun)serializer.Deserialize(reader);
                reader.Close();
            }
            return testRun;
        }

        public static string GetTempTrxFile()
        {
            return Path.GetTempFileName().Replace(".tmp", ".trx");
        }
    }
}