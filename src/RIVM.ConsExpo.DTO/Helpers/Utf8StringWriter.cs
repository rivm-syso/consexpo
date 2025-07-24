using System;
using System.IO;
using System.Text;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// A string writer that will automatically use UTF8 encoding.
    /// </summary>
    /// <seealso cref="System.IO.StringWriter"/>
    /// <see href="http://stackoverflow.com/questions/3862063/serializing-an-object-as-utf-8-xml-in-net/3862106#3862106">Serializing an object as UTF-8 XML in .NET</see>
    public class Utf8StringWriter : StringWriter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Utf8StringWriter"/> class.
        /// </summary>
        public Utf8StringWriter()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Utf8StringWriter"/> class.
        /// </summary>
        /// <param name="formatProvider">An <see cref="T:System.IFormatProvider" /> object that controls formatting.</param>
        public Utf8StringWriter(IFormatProvider formatProvider)
            : base(formatProvider)
        {
        }

        /// <summary>
        /// Gets the <see cref="T:System.Text.Encoding" /> in which the output is written: UTF8.
        /// </summary>
        public override Encoding Encoding { get { return Encoding.UTF8; } }
    }
}