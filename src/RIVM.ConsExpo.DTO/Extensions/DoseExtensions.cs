using System;
using RIVM.ConsExpo.DTO.Output;

namespace RIVM.ConsExpo.DTO.Extensions
{
    /// <summary>
    /// Extension methods related to Dose, dose unit, etc.
    /// </summary>
    public static class DoseExtensions
    {
        /// <summary>
        /// Gets the dose unit for the specified dose measure (end point).
        /// </summary>
        /// <param name="doseMeasureType">Type of the dose measure.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static DoseUnits GetDoseUnit(this DoseMeasureType doseMeasureType)
        {
            DoseUnits doseUnit;

            switch (doseMeasureType)
            {
                case DoseMeasureType.DermalLoad:
                    doseUnit = DoseUnits.MgPerSquareCentimetre;
                    break;

                case DoseMeasureType.ExternalEventDose:
                case DoseMeasureType.InternalEventDose:
                    doseUnit = DoseUnits.MgPerKgBodyWeight;
                    break;

                case DoseMeasureType.ExternalDayDose:
                case DoseMeasureType.InternalDayDose:
                case DoseMeasureType.InternalYearAverageDose:
                    doseUnit = DoseUnits.MgPerKgBodyWeightPerDay;
                    break;

                case DoseMeasureType.PeakAirConcentration:
                case DoseMeasureType.MeanEventConcentration:
                case DoseMeasureType.MeanDayConcentration:
                case DoseMeasureType.MeanYearConcentration:
                    doseUnit = DoseUnits.MgPerCubicMetre;
                    break;

                case DoseMeasureType.ExposureFraction:
                    doseUnit = DoseUnits.Fraction;
                    break;

                default:
                    throw new NotSupportedException($"Unsupported dose measure type '{doseMeasureType}'");
            }
            return doseUnit;
        }
    }
}