using System;
using System.Globalization;
using System.IO;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Helper methods for XML.
    /// </summary>
    public class XmlHelper
    {
        /// <summary>
        /// Serializes a class instance to XML.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="extraTypes">The types of fields in the value that must be serialized as well.</param>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/1212555/serialize-c-sharp-class-directly-to-sql-server/4184313#4184313">Serialize C# class directly to SQL server?</see>
        public static string SerializeToXml(object value, Type[] extraTypes)
        {
            var serializer = new XmlSerializer(value.GetType(), extraTypes);

            using (StringWriter writer = new Utf8StringWriter(CultureInfo.InvariantCulture))
            {
                serializer.Serialize(writer, value);
                return writer.ToString();
            }
        }

        /// <summary>
        /// Deserializes to the specified type from and XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value">A string value containing the Xml.</param>
        /// <param name="extraTypes">The types of fields in the value that must be deserialized as well.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(string value, Type[] extraTypes)
        {
            using (var reader = new StringReader(value))
            {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Deserializes to the specified type from and XML string.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="stream">A stream object containing the Xml.</param>
        /// <param name="extraTypes">The types of fields in the value that must be deserialized as well.</param>
        /// <returns></returns>
        public static T DeserializeFromXml<T>(Stream stream, Type[] extraTypes)
        {
            stream.Seek(0, SeekOrigin.Begin);
            using (var reader = new StreamReader(stream))
            {
                var serializer = new XmlSerializer(typeof(T), extraTypes);
                return (T)serializer.Deserialize(reader);
            }
        }

        /// <summary>
        /// Validates the Xml against the specified schema and reports validation errors to the handler call back.
        /// </summary>
        /// <param name="assessmentXmlStream">The assessment XML stream.</param>
        /// <param name="validationHandler">The validation handler.</param>
        /// <param name="schemaFilePath">The schema file path.</param>
        public static void ValidateBySchema(Stream assessmentXmlStream, ValidationEventHandler validationHandler, string schemaFilePath)
        {
            XmlReaderSettings readerSettings = new XmlReaderSettings();

            readerSettings.Schemas.Add(null, schemaFilePath);
            readerSettings.ValidationType = ValidationType.Schema;
            readerSettings.ValidationFlags |= XmlSchemaValidationFlags.ReportValidationWarnings;
            readerSettings.ValidationEventHandler += validationHandler;

            assessmentXmlStream.Seek(0, SeekOrigin.Begin);

            using (var xmlReader = XmlReader.Create(assessmentXmlStream, readerSettings))
            {
                while (xmlReader.Read()) { }    // Validate XML file.
            }
        }
    }
}