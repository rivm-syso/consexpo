using System;
using System.Diagnostics;
using System.IO;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// A string reader that will count the lines read and can returns an object containing the line contents and the line number.
    /// </summary>
    /// <seealso cref="System.IO.StringReader" />
    public class CE4LineReader
    {
        private int lineNumber = 0;

        private StringReader stringReader;

        /// <summary>
        /// Initializes a new instance of the <see cref="StringReader" /> class.
        /// </summary>
        /// <param name="s">The string to which the <see cref="T:System.IO.StringReader" /> should be initialized.</param>
        public CE4LineReader(string s)
        {
            stringReader = new StringReader(s);
        }

        /// <summary>
        /// Reads a line of characters from the current string and returns the data as a string, from which leading or trailing whitespace is trimmed.
        /// </summary>
        /// <returns>
        /// The next line from the current string, or null if the end of the string is reached.
        /// </returns>
        public CE4Line ReadLine()
        {
            string line = stringReader.ReadLine();

            if (line == null)
            {
                Debug.WriteLine(String.Format("No more lines after line {0,3}.", lineNumber));
            }
            else
            {
                lineNumber++;
                line = line.Trim();

                Debug.WriteLine(String.Format("Read line {0,3}: {1}", lineNumber, line));
            }

            return new CE4Line()
            {
                Line = line,
                LineNumber = lineNumber
            };
        }
    }
}