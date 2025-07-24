using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// The simulation calculates end points for each active route. These are stored in this data structure.
    /// </summary>
    public class SimulationResults
    {
        public SimulationResults()
        { }

        public SimulationResults(bool dermalRouteInUse, bool inhalationRouteInUse, bool oralRouteInUse)
        {
            Dermal = new RouteResults(dermalRouteInUse);
            Inhalation = new RouteResults(inhalationRouteInUse);
            Oral = new RouteResults(oralRouteInUse);
            Integrated = new RouteResults(dermalRouteInUse || inhalationRouteInUse || oralRouteInUse);
        }

        [Key]
        [XmlIgnore]
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the number of iterations in the Monte Carlo simulation used to calculate this result.
        /// </summary>
        /// <value>
        /// The number of iterations.
        /// </value>
        public int NumberOfIterations { get; set; }

        public ScaleType OutputScale { get; set; }

        public RouteResults Dermal { get; set; }

        public RouteResults Inhalation { get; set; }

        public RouteResults Oral { get; set; }

        public RouteResults Integrated { get; set; }

        public static Type[] GetSerializationTypes()
        {
            return new Type[] { typeof(RouteResults), typeof(EndPointResults), typeof(Statistics), typeof(Histogram), typeof(Bin) };
        }

        [XmlIgnore]
        public Exception Error { get; set; }
    }
}