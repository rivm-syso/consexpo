using System.Collections.Generic;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// Generic class for end points for all routes. An endpoint can contain either the result of a deterministic calculation: a single point value, or the result of a Monte Carlo simulation: a list of point values.
    /// </summary>
    /// <typeparam name="E"></typeparam>
    /// <typeparam name="A"></typeparam>
    public class EndPoint<E, A>
        where E : ExposureOutcomeBase
        where A : AbsorptionOutcomeBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="EndPoint{E, A}"/> class.
        /// </summary>
        /// <param name="capacity">The capacity to reserve for iterations in distributed calculations. This value is used for optimization only.</param>
        public EndPoint(int capacity = 0)
        {
            Points = new SortedList<int, RouteOutcomes<E, A>>(capacity);
        }

        public RouteOutcomes<E, A> PointValue { get; set; }

        /// <summary>
        /// Gets or sets the points.
        /// </summary>
        /// <value>
        /// The points.
        /// </value>
        public SortedList<int, RouteOutcomes<E, A>> Points { get; set; }
    }
}