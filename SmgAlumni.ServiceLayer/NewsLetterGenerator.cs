using SmgAlumni.Data.Interfaces;
using SmgAlumni.ServiceLayer.Interfaces;
using SmgAlumni.ServiceLayer.Models;
using SmgAlumni.Utils.Settings;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace SmgAlumni.ServiceLayer
{
    public class NewsLetterGenerator : INewsLetterGenerator
    {
        private readonly IAppSettings _appSettings;

        public NewsLetterGenerator(IAppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public string GenerateNewsLetter(BiMonthlyNewsLetterDto newsLetterModel)
        {
            XslCompiledTransform transform = new XslCompiledTransform();

            var path = _appSettings.NewsLetterSettings.BiWeeklyNewsLetterTemplatePath;
            transform.Load(path, XsltSettings.TrustedXslt, new XmlUrlResolver());

            var messageBuilder = new StringBuilder();
            var messageWriter = new StringWriter(messageBuilder);

            using (var wri = new XmlTextWriter(messageWriter))
            {
                var xmlIn = new XmlDocument();
                xmlIn.LoadXml(SerializeToXml(newsLetterModel));
                transform.Transform(xmlIn, wri);
                wri.Flush();
                wri.Close();
            }

            return messageBuilder.ToString();
        }

        private string SerializeToXml(object objectToSerialize)
        {
            var serializer = new XmlSerializer(objectToSerialize.GetType());
            var ms = new MemoryStream();

            serializer.Serialize(ms, objectToSerialize);

            byte[] bytes = ms.ToArray();
            ms.Close();

            return System.Text.Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
