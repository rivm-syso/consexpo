using RIVM.ConsExpo.DTO.Extensions;
using RIVM.ConsExpo.DTO.Properties;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Schema;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Entities
{
    /// <summary>
    /// A model that contains the data of an assessment.
    /// </summary>
    [Serializable]
    [XmlRoot(Namespace = Constants.AssessmentCurrentSchemaNamespace)]
    public class AssessmentModel
    {
        [XmlIgnore]
        public int Id { get; set; }

        [NotMapped]
        [XmlAttribute(AttributeName = "schemaLocation", Namespace = XmlSchema.InstanceNamespace)]
        public string XsiSchemaLocation
        {
            get
            {
                return $"{Constants.AssessmentCurrentSchemaNamespace} {XsiSchemaLocationSchema}";
            }

#if(GenerateSchema)
#warning The project is build with Conditional Compilation Constant 'GenerateSchema'. This must only be done to be able to generate an XML schema. Use another build configuration for development and deployment.
#else
            set
            {
                //Required for xml serialization

                // When you want to generate a new schema, using xsd.exe, this setter must temporarily(!) be removed from code.
                // This is achieved by using Build Configuration 'GenerateSchema' and building this project.
                // Otherwise, you will get:
                // * Schema item 'attribute' named 'schemaLocation' from namespace 'http://www.w3.org/2001/XMLSchema-instance'. The target namespace of an attribute declaration, whether local or global, must not match http://www.w3.org/2001/XMLSchema-instance.
            }
#endif
        }

        public static string XsiSchemaLocationSchema { get; set; }

        [NotMapped]
        [XmlAttribute("versionNumber")]
        public int VersionNumber { get; set; } = VersionInfo.RevisionNumber;

        [NotMapped]
        [XmlAttribute("dateTimeGenerated")]
        public DateTime DateTimeGenerated { get; set; } = DateTime.UtcNow.Truncate(TimeSpan.FromSeconds(1));
     
        [XmlIgnore]
        public int UserId { get; set; }

        [XmlIgnore]
        public virtual UserModel User { get; set; }

        public const int MaxNameLength = 200;

        [Required(AllowEmptyStrings = false)]
        [StringLength(MaxNameLength)]
        public string Name { get; set; }

        public virtual SubstanceModel Substance { get; set; }

        public virtual PopulationModel Population { get; set; }

        public virtual ProductModel Product { get; set; }

        public AssessmentModel()
        { }

        public AssessmentModel(bool setDefaults)
        {
            if (setDefaults)
            {
                Substance = new SubstanceModel(setDefaults);
                Population = new PopulationModel(setDefaults);
                Product = new ProductModel(setDefaults);
            }
        }

        public virtual List<ScenarioModel> Scenarios { get; set; }

        /// <summary>
        /// This list of types is used by the XML-(de)serializer.
        /// </summary>
        /// <returns></returns>
        public static Type[] GetSerializationTypes()
        {
            return new Type[]
            {
                typeof(PopulationModel),
                typeof(ProductModel),
                typeof(SubstanceModel),
                typeof(ScenarioModel),
                typeof(DermalExposureModel),
                typeof(DermalAbsorptionModel),
                typeof(InhalationExposureModel),
                typeof(NonParametricSizeDistribution),
                typeof(NonParametricSizeBin),
                typeof(InhalationAbsorptionModel),
                typeof(OralExposureModel),
                typeof(OralAbsorptionModel)
            };
        }
    }
}