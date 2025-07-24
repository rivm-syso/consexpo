using System;
using System.Collections.Generic;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Stores the simulation results for a route.
    /// </summary>
    public class RouteResults
    {
        [Obsolete("Added to solve \"RIVM.ConsExpo.DTO.Output.RouteResults cannot be serialized because it does not have a parameterless constructor.\"")]
        public RouteResults()
        { }

        public RouteResults(bool isEnabled = true)
        {
            IsEnabled = isEnabled;
        }

        public bool IsEnabled { get; set; }

        public List<EndPointResults> EndPointResults { get; } = new List<EndPointResults>();
    }
}