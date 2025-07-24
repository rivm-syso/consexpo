using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Output
{
    /// <summary>
    /// A list of dose measures, the various physical quantities in which model outcomes can be expressed.
    /// </summary>
    /// <remarks>The applicable dose measures vary by route.</remarks>
    public enum DoseMeasureType
    {
        /// <summary>
        /// The amount of product per unit of skin surface.
        /// </summary>
        [Display(Name = "Dermal load", Description = "Amount per cm² on the skin.")]
        DermalLoad,

        /// <summary>
        /// The amount per event of product on the skin, divided by the body weight.
        /// </summary>
        [Display(Name = "External event dose", Description = "The amount that can potentially be absorbed per kg body weight during one event.")]
        ExternalEventDose,

        /// <summary>
        /// The amount per event of product on the skin, divided by the body weight.
        /// </summary>
        [Display(Name = "External dose on day of exposure", Description = "The amount that can potentially be absorbed per kg body weight during one day.")]
        ExternalDayDose,

        /// <summary>
        /// The amount per event of product absorbed through the skin, divided by the body weight.
        /// </summary>
        [Display(Name = "Internal event dose", Description = "Absorbed dose per kg body weight during one exposure event.")]
        InternalEventDose,

        /// <summary>
        /// The amount per event of product absorbed through the skin, divided by the body weight.
        /// </summary>
        [Display(Name = "Internal dose on day of exposure", Description = "Absorbed dose per kg body weight during one day. Note: these can be higher than the ‘event dose’ for exposure frequencies larger than 1 per day.")]
        InternalDayDose,

        /// <summary>
        /// The amount of product per unit of time, absorbed through the skin during multiple events, divided by the body weight.
        /// </summary>
        [Display(Name = "Internal year average dose", Description = "Daily absorbed dose per kg body weight averaged over a year.")]
        InternalYearAverageDose,

        /// <summary>
        /// The mean air concentration during the event.
        /// </summary>
        [Display(Name = "Mean event concentration", Description = "Average air concentration of exposure event. Note that the mean event concentration depends strongly on exposure duration. In case the re-entry scenario has been selected; the mean event concentration is given as the maximum average air concentration for the exposure duration per day over the entire period of emission.")]
        MeanEventConcentration,

        /// <summary>
        /// The mean air concentration on day of exposure.
        /// </summary>
        [Display(Name = "Mean concentration on day of exposure", Description = "Average air concentration over the day (accounts for the number of events on one day).")]
        MeanDayConcentration,

        /// <summary>
        /// The mean year air concentration.
        /// </summary>
        [Display(Name = "Year average concentration", Description = "Mean daily air concentration averaged over a year.")]
        MeanYearConcentration,

        /// <summary>
        /// The peak air concentration during the event.
        /// </summary>
        [Display(Name = "Peak concentration (TWA 15 min)", Description = "Peak concentration (TWA 15 min) is the 15 minute time weighted average of the air concentration. In case the exposure duration is less than 15 minutes, the mean event air concentration is given instead.")]
        PeakAirConcentration,

        /// <summary>
        /// An exposure fraction. I.e., the amount of product the consumers are exposed to, divided by the amount of product used.
        /// </summary>
        [Display(Name = "Exposure fraction", Description = "")]
        ExposureFraction
    }
}