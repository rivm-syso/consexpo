using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO
{
    /// <summary>
    /// The base class for all kinds of Name-Value Pairs
    /// </summary>
    public abstract class NameValuePairsBase
    {
        /// <summary>
        ///
        /// </summary>
        protected NameValuePairsBase()
        { }

        /// <summary>
        ///
        /// </summary>
        protected NameValuePairsBase(string value, string name)
        {
            Value = value;
            Name = name;
        }

        /// <summary>
        /// Gets or sets the Name
        /// </summary>
        [Required]
        public string Name { get; protected set; }

        /// <summary>
        ///
        /// </summary>
        [Required]
        public string Value { get; protected set; }
    }

    /// <summary>
    ///
    /// </summary>
    public class DecimalSeparator : NameValuePairsBase
    {
        public static readonly DecimalSeparator Point = new DecimalSeparator(".", "point (.)");

        public static readonly DecimalSeparator Comma = new DecimalSeparator(",", "comma (,)");

        [NotMapped]
        [XmlIgnore]
        public static IList<DecimalSeparator> AllDecimalSeparators
        {
            get
            {
                return new ReadOnlyCollection<DecimalSeparator>(new[]
                 {
                     Point,
                     Comma
                 });
            }
        }

        protected DecimalSeparator()
        { }

        protected DecimalSeparator(string value, string name)
            : base(value, name)
        { }
    }
}