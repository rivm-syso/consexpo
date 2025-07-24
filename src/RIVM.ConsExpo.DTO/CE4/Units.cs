using RIVM.ConsExpo.DTO.PhysicalUnits;
using System;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.CE4
{
    /// <summary>
    /// Converts values expressed in CE4 units to CE web values/units.
    /// </summary>
    public class Units
    {
        #region unit string values, copied from CE4 source: units.h.

        private const string MicrogramString = "microgram";
        private const string MilligramString = "milligram";
        private const string GramString = "gram";
        private const string KilogramString = "kilogram";

        private const string SecondString = "second";
        private const string MinuteString = "minute";
        private const string HourString = "hour";
        private const string DayString = "day";
        private const string WeekString = "week";
        private const string MonthString = "month";
        private const string YearString = "year";

        private const string MilliGramPerSecString = "mg/sec";
        private const string MilliGramPerMinString = "mg/min";
        private const string MicroGramPerMinString = "µg/min";
        private const string MicroGramPerSecString = "µg/sec";
        private const string GramPerMinString = "g/min";
        private const string GramPerSecString = "g/sec";
        private const string GramPerDayString = "g/day";

        private const string CentimeterString = "centimeter";
        private const string MicrometerString = "micrometer";
        private const string MillimeterString = "millimetre";
        private const string DecimeterString = "decimeter";
        private const string MeterString = "meter";

        private const string MillimeterSquaredString = "mm2";
        private const string CentimeterSquaredString = "cm2";
        private const string DecimeterSquaredString = "dm2";
        private const string MeterSquaredString = "m2";

        private const string MillimeterCubedString = "mm3";
        private const string CentimeterCubedString = "cm3";
        private const string DecimeterCubedString = "dm3";
        private const string LiterString = "liter";
        private const string MeterCubedString = "m3";

        private const string TimesString = "times";

        private const string MilligramPerLiter = "mg/l";
        private const string GramPerLiter = "g/l";
        private const string MilligramPerCm3 = "mg/cm3";
        private const string GramPerCm3 = "g/cm3";

        private const string MilligramPerCm3String = "mg/cm3";
        private const string KilogramPerLiterString = "kg/liter";
        private const string GramPerCm3String = "g/cm3";
        private const string MicrogramPerCm3String = "µg/cm3";
        private const string GramPerM3String = "g/m3";
        private const string MilligramPerM3String = "mg/m3";
        private const string GramPerLiterString = "g/liter";
        private const string MilligramPerLiterString = "mg/liter";

        private const string MeterPerSecond = "m/s";
        private const string CentimeterPerSecond = "cm/s";
        private const string MeterPerMinute = "m/min";
        private const string CentimeterPerMinute = "cm/min";
        private const string CentimeterPerHour = "cm/hr";

        private const string Cm2PerSecString = "cm2/sec";
        private const string Cm2PerMinuteString = "cm2/min";
        private const string Cm2PerHourString = "cm2/hr";
        private const string M2PerMinuteString = "m2/min";
        private const string M2PerHourString = "m2/hr";

        private const string Cm3PerSecondString = "cm3/sec";
        private const string Cm3PerMinuteString = "cm3/min";
        private const string LiterPerMinuteString = "liter/min";
        private const string LiterPerHourString = "liter/h";
        private const string M3PerSecondString = "m3/sec";
        private const string M3PerMinuteString = "m3/min";
        private const string M3PerHourString = "m3/hour";
        private const string M3PerDayString = "m3/day";
        private const string LiterPerDayString = "liter/day";

        #endregion unit string values, copied from CE4 source: units.h.

        public static MolecularWeightUnits ParseMolecularWeightUnit(string unitName)
        {
            const string GramPerMolString = "g/mol";
            const string MilligramGramPerMolString = "mg/mol";

            unitName = unitName.Trim();

            switch (unitName)
            {
                case GramPerMolString:
                    return MolecularWeightUnits.GramPerMol;

                case MilligramGramPerMolString:
                    return MolecularWeightUnits.MilliGramPerMol;

                default:
                    throw new NotSupportedException(string.Format("Unsupported ConsExpo 4 molecular weight unit '{0}'", unitName));
            }
        }

        public static FrequencyUnits ParseFrequencyUnit(string unitName, out double conversionFactor)
        {
            const string PerSecondString = "1/sec";
            const string PerMinuteString = "1/min";
            const string PerHourString = "1/hr";
            const string PerDayString = "1/day";
            const string PerWeekString = "1/week";
            const string PerMonthString = "1/month";
            const string PerYearString = "1/year";

            unitName = unitName.Trim();

            switch (unitName)
            {
                case PerDayString:
                    conversionFactor = 1.0;
                    return FrequencyUnits.Daily;

                case PerWeekString:
                    conversionFactor = 1.0;
                    return FrequencyUnits.Weekly;

                case PerMonthString:
                    conversionFactor = 1.0;
                    return FrequencyUnits.Monthly;

                case PerYearString:
                    conversionFactor = 1.0;
                    return FrequencyUnits.Yearly;

                case PerSecondString:
                    conversionFactor = ConversionFactors.SecondsPerDay;
                    return FrequencyUnits.Daily;

                case PerMinuteString:
                    conversionFactor = ConversionFactors.MinutesPerDay;
                    return FrequencyUnits.Daily;

                case PerHourString:
                    conversionFactor = ConversionFactors.HoursPerDay;
                    return FrequencyUnits.Daily;

                default:
                    throw new NotSupportedException(string.Format("Unsupported ConsExpo 4 frequency unit '{0}'", unitName));
            }
        }

        public static TemperatureUnits ParseTemperatureUnit(string unitName)
        {
            const string Celsius = "Celsius";
            const string Kelvin = "Kelvin";

            switch (unitName)
            {
                case Kelvin:
                    return TemperatureUnits.Kelvin;

                case Celsius:
                    return TemperatureUnits.Celsius;

                default:
                    throw new NotSupportedException(string.Format("Unsupported temperature unit '{0}'", unitName));
            }
        }

        public static Dimensionless DimensionlessUnit(string unitName)
        {
            const string LinearString = "linear";
            const string Log10String = "10Log";

            switch (unitName)
            {
                case LinearString:
                    return Dimensionless.Linear;

                case Log10String:
                    return Dimensionless.Log10;

                default:
                    throw new NotSupportedException(string.Format("Unsupported dimensionless unit '{0}'", unitName));
            }
        }

        public static PressureUnits PressureUnit(string unitName)
        {
            const string MillibarString = "millibar";
            const string BarString = "bar";
            const string PascalString = "Pascal";
            const string AtmosphereString = "atmosphere";
            const string MmHgString = "mmHg";

            switch (unitName)
            {
                case MillibarString:
                    return PressureUnits.Millibar;

                case MmHgString:
                    return PressureUnits.MmHg;

                case AtmosphereString:
                    return PressureUnits.Atmosphere;

                case BarString:
                    return PressureUnits.Bar;

                case PascalString:
                    return PressureUnits.Pascal;

                default:
                    throw new NotSupportedException(string.Format("Unsupported pressure unit '{0}'", unitName));
            }
        }

        public static MassUnits ParseBodyWeightUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case KilogramString:
                    conversionFactor = 1.0;
                    break;

                case GramString:
                    conversionFactor = ConversionFactors.One2Kilo;
                    break;

                case MilligramString:
                    conversionFactor = ConversionFactors.Milli2Kilo;
                    break;

                case MicrogramString:
                    conversionFactor = ConversionFactors.Micro2Kilo;
                    break;

                default:
                    throw new NotSupportedException(string.Format("Unsupported mass unit '{0}'", unitName));
            }

            return MassUnits.Kilogram;
        }

        public static FractionUnits ParseFractionUnit(string unitName)
        {
            const string PercentageString = "%";
            const string FractionString = "fraction";

            switch (unitName)
            {
                case FractionString:
                    return FractionUnits.Fraction;

                case PercentageString:
                    return FractionUnits.Percentage;

                default:
                    throw new NotSupportedException(string.Format("Unsupported fraction unit '{0}'", unitName));
            }
        }

        public static MassUnits ParseProductAmountUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case KilogramString:
                    conversionFactor = ConversionFactors.Kilo2One;
                    return MassUnits.Gram;

                case GramString:
                    conversionFactor = 1.0;
                    return MassUnits.Gram;

                case MilligramString:
                    conversionFactor = 1.0;
                    return MassUnits.Milligram;

                case MicrogramString:
                    conversionFactor = ConversionFactors.Micro2Milli;
                    return MassUnits.Milligram;

                default:
                    throw new NotSupportedException(string.Format("Unsupported product amount unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseExposureDurationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = ConversionFactors.MinutesPerSecond;
                    return DurationUnits.Minute;

                case MinuteString:
                    conversionFactor = 1.0;
                    return DurationUnits.Minute;

                case HourString:
                    conversionFactor = 1.0;
                    return DurationUnits.Hour;

                case DayString:
                    conversionFactor = 1.0;
                    return DurationUnits.Day;

                case WeekString:
                    conversionFactor = ConversionFactors.DaysPerWeek;
                    return DurationUnits.Day;

                case MonthString:
                    conversionFactor = ConversionFactors.DaysPerMonth;
                    return DurationUnits.Day;

                case YearString:
                    conversionFactor = ConversionFactors.DaysPerYear;
                    return DurationUnits.Day;

                default:
                    throw new NotSupportedException(string.Format("Unsupported exposure duration unit '{0}'", unitName));
            }
        }

        public static VolumeUnits ParseRoomVolumeUnit(string unitName)
        {
            switch (unitName)
            {
                case DecimeterCubedString:
                case LiterString:
                    return VolumeUnits.Litre;

                case MeterCubedString:
                    return VolumeUnits.CubicMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported room volume unit '{0}'", unitName));
            }
        }

        public static VolumeRateUnits ParseVolumeRateUnit(string unitName, out double conversionFactor)
        {
            const string Cm3PerSecondString = "cm3/sec";
            const string Cm3PerMinuteString = "cm3/min";
            const string LiterPerMinuteString = "liter/min";
            const string LiterPerHourString = "liter/h";
            const string M3PerSecondString = "m3/sec";
            const string M3PerMinuteString = "m3/min";
            const string M3PerHourString = "m3/hour";
            const string M3PerDayString = "m3/day";
            const string LiterPerDayString = "liter/day";

            switch (unitName)
            {
                case Cm3PerSecondString:
                    conversionFactor = ConversionFactors.LitresPerCubicCentimetre / ConversionFactors.MinutesPerSecond;
                    return VolumeRateUnits.LiterPerMinute;

                case Cm3PerMinuteString:
                    conversionFactor = ConversionFactors.LitresPerCubicCentimetre;
                    return VolumeRateUnits.LiterPerMinute;

                case LiterPerMinuteString:
                    conversionFactor = 1.0;
                    return VolumeRateUnits.LiterPerMinute;

                case LiterPerHourString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerHour;
                    return VolumeRateUnits.LiterPerMinute;

                case M3PerSecondString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerSecond;
                    return VolumeRateUnits.CubicMetrePerHour;

                case M3PerMinuteString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerMinute;
                    return VolumeRateUnits.CubicMetrePerHour;

                case M3PerHourString:
                    conversionFactor = 1.0;
                    return VolumeRateUnits.CubicMetrePerHour;

                case M3PerDayString:
                    conversionFactor = 1.0;
                    return VolumeRateUnits.CubicMetrePerDay;

                case LiterPerDayString:
                    conversionFactor = ConversionFactors.CubicMetresPerLitre;
                    return VolumeRateUnits.CubicMetrePerDay;

                default:
                    throw new NotSupportedException(string.Format("Unsupported volume rate unit '{0}'", unitName));
            }
        }

        public static RateUnits ParseRateUnit(string unitName)
        {
            const string PerHourString = "1/hr";

            unitName = unitName.Trim();

            switch (unitName)
            {
                case PerHourString:
                    return RateUnits.TimesPerHour;

                default:
                    throw new NotSupportedException(string.Format("Unsupported ConsExpo 4 rate unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseSprayDurationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = 1.0;
                    return DurationUnits.Second;

                case MinuteString:
                    conversionFactor = 1.0;
                    return DurationUnits.Minute;

                case HourString:
                    conversionFactor = ConversionFactors.MinutesPerHour;
                    return DurationUnits.Minute;

                case DayString:
                    conversionFactor = ConversionFactors.MinutesPerDay;
                    return DurationUnits.Minute;

                case WeekString:
                    conversionFactor = ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerWeek;
                    return DurationUnits.Minute;

                case MonthString:
                    conversionFactor = ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerMonth;
                    return DurationUnits.Minute;

                case YearString:
                    conversionFactor = ConversionFactors.MinutesPerDay * ConversionFactors.DaysPerYear;
                    return DurationUnits.Minute;

                default:
                    throw new NotSupportedException(string.Format("Unsupported spray duration unit '{0}'", unitName));
            }
        }

        public static DensityUnits ParseDensitySolidUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MicrogramPerCm3String:
                    conversionFactor = ConversionFactors.Micro2Milli;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerM3String:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicMetre;

                case GramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicCentimetre;

                case GramPerM3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicMetre;

                case MilligramPerLiterString:
                    conversionFactor = ConversionFactors.LitresPerCubicCentimetre;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case GramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerLitre;

                case KilogramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.KilogramPerLitre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported density unit '{0}'", unitName));
            }
        }

        public static DensityUnits ParseSubstanceConcentrationPackagingUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MicrogramPerCm3String:
                    conversionFactor = ConversionFactors.Micro2Milli;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerM3String:
                    conversionFactor = 1 / Math.Pow(ConversionFactors.One2Centi, 3);
                    return DensityUnits.MilligramPerCubicCentimetre;

                case GramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicCentimetre;

                case GramPerM3String:
                    conversionFactor = ConversionFactors.One2Milli / Math.Pow(ConversionFactors.One2Centi, 3);
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerLiterString:
                    conversionFactor = ConversionFactors.Milli2One;
                    return DensityUnits.GramPerLitre;

                case GramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerLitre;

                case KilogramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.KilogramPerLitre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported density unit '{0}'", unitName));
            }
        }

        public static SurfaceRateUnits ParseDiffusionCoefficientUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case Cm2PerSecString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return SurfaceRateUnits.SquareCentiMetrePerMinute;

                case Cm2PerMinuteString:
                    conversionFactor = 1.0;
                    return SurfaceRateUnits.SquareCentiMetrePerMinute;

                case Cm2PerHourString:
                    conversionFactor = 1.0;
                    return SurfaceRateUnits.SquareCentiMetrePerHour;

                case M2PerMinuteString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerMinute;
                    return SurfaceRateUnits.SquareMetrePerHour;

                case M2PerHourString:
                    conversionFactor = 1.0;
                    return SurfaceRateUnits.SquareMetrePerHour;

                default:
                    throw new NotSupportedException(string.Format("Unsupported diffusion coefficient unit '{0}'", unitName));
            }
        }

        public static MassRateUnits ParseMassGenerationRateUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilliGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return MassRateUnits.MilligramPerMinute;

                case MilliGramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.MilligramPerMinute;

                case MicroGramPerMinString:
                    conversionFactor = ConversionFactors.Micro2Milli;
                    return MassRateUnits.MilligramPerMinute;

                case MicroGramPerSecString:
                    conversionFactor = ConversionFactors.Micro2Milli / ConversionFactors.MinutesPerSecond;
                    return MassRateUnits.MilligramPerMinute;

                case GramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerMinute;

                case GramPerSecString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerSecond;

                case GramPerDayString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerDay;
                    return MassRateUnits.GramPerMinute;

                default:
                    throw new NotSupportedException(string.Format("Unsupported mass generation rate unit '{0}'", unitName));
            }
        }

        public static AreaUnits ParseReleaseAreaUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MillimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Milli2Centi, 2);
                    return AreaUnits.SquareCentimetre;

                case CentimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareCentimetre;

                case DecimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Deci2One, 2);
                    return AreaUnits.SquareMetre;

                case MeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported release area unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseApplicationDurationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = ConversionFactors.MinutesPerSecond;
                    return DurationUnits.Minute;

                case MinuteString:
                    conversionFactor = 1.0;
                    return DurationUnits.Minute;

                case HourString:
                    conversionFactor = 1.0;
                    return DurationUnits.Hour;

                case DayString:
                    conversionFactor = 1.0;
                    return DurationUnits.Day;

                case WeekString:
                    conversionFactor = ConversionFactors.DaysPerWeek;
                    return DurationUnits.Day;

                case MonthString:
                    conversionFactor = ConversionFactors.DaysPerMonth;
                    return DurationUnits.Day;

                case YearString:
                    conversionFactor = ConversionFactors.DaysPerYear;
                    return DurationUnits.Day;

                default:
                    throw new NotSupportedException(string.Format("Unsupported application duration unit '{0}'", unitName));
            }
        }

        public static VelocityUnits ParseMassTransferCoefficientUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MeterPerSecond:
                    conversionFactor = 1.0;
                    return VelocityUnits.MetrePerSecond;

                case CentimeterPerSecond:
                    conversionFactor = ConversionFactors.Centi2One;
                    return VelocityUnits.MetrePerSecond;

                case MeterPerMinute:
                    conversionFactor = 1.0;
                    return VelocityUnits.MetrePerMinute;

                case CentimeterPerMinute:
                    conversionFactor = ConversionFactors.Centi2One / ConversionFactors.HoursPerMinute;
                    return VelocityUnits.MetrePerHour;

                case CentimeterPerHour:
                    conversionFactor = ConversionFactors.Centi2One;
                    return VelocityUnits.MetrePerHour;

                default:
                    throw new NotSupportedException(string.Format("Unsupported mass transfer coefficient unit '{0}'", unitName));
            }
        }

        public static LengthUnits ParseHeightUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case CentimeterString:
                    conversionFactor = ConversionFactors.Centi2One;
                    return LengthUnits.Metre;

                case MicrometerString:
                    conversionFactor = ConversionFactors.Micro2One;
                    return LengthUnits.Metre;

                case MillimeterString:
                    conversionFactor = ConversionFactors.Milli2One;
                    return LengthUnits.Metre;

                case DecimeterString:
                    conversionFactor = ConversionFactors.Deci2One;
                    return LengthUnits.Metre;

                case MeterString:
                    conversionFactor = 1.0;
                    return LengthUnits.Metre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported height unit '{0}'", unitName));
            }
        }

        public static LengthUnits ParseDiameterUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MicrometerString:
                    conversionFactor = 1.0;
                    return LengthUnits.Micrometre;

                case MillimeterString:
                    conversionFactor = ConversionFactors.Milli2Micro;
                    return LengthUnits.Micrometre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported diameter unit '{0}'", unitName));
            }
        }

        public static DensityUnits ParseDensityNonVolatileUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilligramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case KilogramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.KilogramPerLitre;

                case GramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicCentimetre;

                case MicrogramPerCm3String:
                    conversionFactor = ConversionFactors.Micro2Milli;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case GramPerM3String:
                    conversionFactor = 1.0 / ConversionFactors.LitresPerCubicMetre;
                    return DensityUnits.GramPerLitre;

                case MilligramPerM3String:
                    conversionFactor = 1.0 / Math.Pow(ConversionFactors.One2Centi, 3);
                    return DensityUnits.MilligramPerCubicCentimetre;

                case GramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerLitre;

                case MilligramPerLiterString:
                    conversionFactor = ConversionFactors.Milli2One;
                    return DensityUnits.GramPerLitre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported density non volatile unit '{0}'", unitName));
            }
        }

        public static VolumeUnits ParseCloudVolumeUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MillimeterCubedString:
                    conversionFactor = Math.Pow(ConversionFactors.Milli2One, 3);
                    return VolumeUnits.CubicMetre;

                case CentimeterCubedString:
                    conversionFactor = Math.Pow(ConversionFactors.Centi2One, 3);
                    return VolumeUnits.CubicMetre;

                case DecimeterCubedString:
                case LiterString:
                    conversionFactor = Math.Pow(ConversionFactors.Deci2One, 3);
                    return VolumeUnits.CubicMetre;

                case MeterCubedString:
                    conversionFactor = 1.0;
                    return VolumeUnits.CubicMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported cloud volume unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseReleaseDurationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = 1.0;
                    return DurationUnits.Second;

                case MinuteString:
                    conversionFactor = 1.0;
                    return DurationUnits.Minute;

                case HourString:
                    conversionFactor = 1.0;
                    return DurationUnits.Hour;

                case DayString:
                    conversionFactor = 1.0;
                    return DurationUnits.Day;

                case WeekString:
                    conversionFactor = ConversionFactors.DaysPerWeek;
                    return DurationUnits.Day;

                case MonthString:
                    conversionFactor = ConversionFactors.DaysPerMonth;
                    return DurationUnits.Day;

                case YearString:
                    conversionFactor = ConversionFactors.DaysPerYear;
                    return DurationUnits.Day;

                default:
                    throw new NotSupportedException(string.Format("Unsupported release duration unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseEmissionDurationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = ConversionFactors.MinutesPerSecond;
                    return DurationUnits.Minute;

                case MinuteString:
                    conversionFactor = 1.0;
                    return DurationUnits.Minute;

                case HourString:
                    conversionFactor = 1.0;
                    return DurationUnits.Hour;

                case DayString:
                    conversionFactor = 1.0;
                    return DurationUnits.Day;

                case WeekString:
                    conversionFactor = 1.0;
                    return DurationUnits.Week;

                case MonthString:
                    conversionFactor = 1.0;
                    return DurationUnits.Month;

                case YearString:
                    conversionFactor = 1.0;
                    return DurationUnits.Year;

                default:
                    throw new NotSupportedException(string.Format("Unsupported emission duration unit '{0}'", unitName));
            }
        }

        public static AreaUnits ParseExposedAreaUnit(string unitName)
        {
            switch (unitName)
            {
                case MillimeterSquaredString:
                    return AreaUnits.SquareMillimetre;

                case CentimeterSquaredString:
                    return AreaUnits.SquareCentimetre;

                case DecimeterSquaredString:
                    return AreaUnits.SquareDecimetre;

                case MeterSquaredString:
                    return AreaUnits.SquareMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported exposed area unit '{0}'", unitName));
            }
        }

        public static DensityUnits ParseSubstanceConcentrationUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilligramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case KilogramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.KilogramPerLitre;

                case GramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicCentimetre;

                case MicrogramPerCm3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicMetre;

                case GramPerM3String:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicMetre;

                case MilligramPerM3String:
                    conversionFactor = ConversionFactors.Milli2One;
                    return DensityUnits.GramPerCubicMetre;

                case GramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.MilligramPerCubicCentimetre;

                case MilligramPerLiterString:
                    conversionFactor = 1.0;
                    return DensityUnits.GramPerCubicMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported substance concentration unit '{0}'", unitName));
            }
        }

        public static AreaUnits ParseRubbingContactAreaUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MillimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Milli2Centi, 2);
                    return AreaUnits.SquareCentimetre;

                case CentimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareCentimetre;

                case DecimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Deci2One, 2);
                    return AreaUnits.SquareMetre;

                case MeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported rubbing contact area unit '{0}'", unitName));
            }
        }

        public static SurfaceRateUnits ParseTransferCoefficientUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case Cm2PerSecString:
                    conversionFactor = Math.Pow(ConversionFactors.Centi2One, 2) / ConversionFactors.HoursPerSecond;
                    return SurfaceRateUnits.SquareMetrePerHour;

                case Cm2PerMinuteString:
                    conversionFactor = Math.Pow(ConversionFactors.Centi2One, 2) / ConversionFactors.HoursPerMinute;
                    return SurfaceRateUnits.SquareMetrePerHour;

                case Cm2PerHourString:
                    conversionFactor = Math.Pow(ConversionFactors.Centi2One, 2);
                    return SurfaceRateUnits.SquareMetrePerHour;

                case M2PerMinuteString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerMinute;
                    return SurfaceRateUnits.SquareMetrePerHour;

                case M2PerHourString:
                    conversionFactor = 1.0;
                    return SurfaceRateUnits.SquareMetrePerHour;

                default:
                    throw new NotSupportedException(string.Format("Unsupported transfer coefficient unit '{0}'", unitName));
            }
        }

        public static LengthUnits ParseThicknessUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case CentimeterString:
                    conversionFactor = 1.0;
                    return LengthUnits.Centimetre;

                case MicrometerString:
                    conversionFactor = 1.0;
                    return LengthUnits.Micrometre;

                case MillimeterString:
                    conversionFactor = 1.0;
                    return LengthUnits.Millimetre;

                case DecimeterString:
                    conversionFactor = ConversionFactors.Deci2Centi;
                    return LengthUnits.Centimetre;

                case MeterString:
                    conversionFactor = ConversionFactors.One2Centi;
                    return LengthUnits.Centimetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported thickness unit '{0}'", unitName));
            }
        }

        public static AreaDensityUnits ParseAreaDensityUnit(string unitName, out double conversionFactor)
        {
            const string GramPerCm2String = "g/cm2";
            const string MilligramPerCm2String = "mg/cm2";
            const string GramPerM2String = "g/m2";
            const string MilligramPerM2String = "mg/m2";

            switch (unitName)
            {
                case GramPerCm2String:
                    conversionFactor = 1.0 / Math.Pow(ConversionFactors.One2Centi, 2);
                    return AreaDensityUnits.GramPerSquareMetre;

                case MilligramPerCm2String:
                    conversionFactor = 1.0 / Math.Pow(ConversionFactors.One2Centi, 2);
                    return AreaDensityUnits.MilligramPerSquareMetre;

                case GramPerM2String:
                    conversionFactor = 1.0;
                    return AreaDensityUnits.GramPerSquareMetre;

                case MilligramPerM2String:
                    conversionFactor = 1.0;
                    return AreaDensityUnits.MilligramPerSquareMetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported are density unit '{0}'", unitName));
            }
        }

        public static AreaUnits ParseContactAreaMouthingUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MillimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Milli2Centi, 2);
                    return AreaUnits.SquareCentimetre;

                case CentimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareCentimetre;

                case DecimeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.Deci2Centi, 2);
                    return AreaUnits.SquareCentimetre;

                case MeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.One2Centi, 2);
                    return AreaUnits.SquareCentimetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported mouthing contact area unit '{0}'", unitName));
            }
        }

        public static MassRateUnits ParseIngestionRateUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilliGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return MassRateUnits.MilligramPerMinute;

                case MilliGramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.MilligramPerMinute;

                case MicroGramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.MicrogramPerMinute;

                case MicroGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return MassRateUnits.MicrogramPerMinute;

                case GramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerMinute;

                case GramPerSecString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerSecond;

                case GramPerDayString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerDay;

                default:
                    throw new NotSupportedException(string.Format("Unsupported ingestion rate unit '{0}'", unitName));
            }
        }

        public static MigrationRateUnits ParseMigrationRateUnit(string unitName)
        {
            const string GramPerCm2PerSecondString = "g/cm2/sec";
            const string GramPerCm2PerMinuteString = "g/cm2/min";

            switch (unitName)
            {
                case GramPerCm2PerSecondString:
                    return MigrationRateUnits.GramPerSquareCentimetrePerSecond;

                case GramPerCm2PerMinuteString:
                    return MigrationRateUnits.GramPerSquareCentimetrePerMinute;

                default:
                    throw new NotSupportedException(string.Format("Unsupported migration rate unit '{0}'", unitName));
            }
        }

        public static VelocityUnits ParseSkinPermeabilityUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MeterPerSecond:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return VelocityUnits.MetrePerMinute;

                case CentimeterPerSecond:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return VelocityUnits.CentimetrePerMinute;

                case MeterPerMinute:
                    conversionFactor = 1.0;
                    return VelocityUnits.MetrePerMinute;

                case CentimeterPerMinute:
                    conversionFactor = 1.0;
                    return VelocityUnits.CentimetrePerMinute;

                case CentimeterPerHour:
                    conversionFactor = 1.0;
                    return VelocityUnits.CentimetrePerHour;

                default:
                    throw new NotSupportedException(string.Format("Unsupported skin permeability unit '{0}'", unitName));
            }
        }

        public static MassRateUnits ParseContactRateUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilliGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.MinutesPerSecond;
                    return MassRateUnits.MilligramPerMinute;

                case MilliGramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.MilligramPerMinute;

                case MicroGramPerMinString:
                    conversionFactor = 1.0;
                    return MassRateUnits.MicrogramPerMinute;

                case MicroGramPerSecString:
                    conversionFactor = ConversionFactors.Micro2One / ConversionFactors.DaysPerSecond;
                    return MassRateUnits.GramPerDay;

                case GramPerMinString:
                    conversionFactor = 1.0 / ConversionFactors.DaysPerMinute;
                    return MassRateUnits.GramPerDay;

                case GramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.DaysPerSecond;
                    return MassRateUnits.GramPerDay;

                case GramPerDayString:
                    conversionFactor = 1.0;
                    return MassRateUnits.GramPerDay;

                default:
                    throw new NotSupportedException(string.Format("Unsupported contact rate unit '{0}'", unitName));
            }
        }

        public static DurationUnits ParseStorageTimeUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case SecondString:
                    conversionFactor = ConversionFactors.SecondsPerHour;
                    return DurationUnits.Hour;

                case MinuteString:
                    conversionFactor = ConversionFactors.MinutesPerHour;
                    return DurationUnits.Hour;

                case HourString:
                    conversionFactor = 1;
                    return DurationUnits.Hour;

                case DayString:
                    conversionFactor = 1;
                    return DurationUnits.Day;

                case WeekString:
                    conversionFactor = ConversionFactors.DaysPerWeek;
                    return DurationUnits.Day;

                case MonthString:
                    conversionFactor = ConversionFactors.DaysPerMonth;
                    return DurationUnits.Day;

                case YearString:
                    conversionFactor = ConversionFactors.DaysPerYear;
                    return DurationUnits.Day;

                default:
                    throw new NotSupportedException(string.Format("Unsupported storage time unit '{0}'", unitName));
            }
        }

        public static MassRateUnits ParseMigrationRatePackagingUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MilliGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerSecond;
                    return MassRateUnits.MilligramPerHour;

                case MilliGramPerMinString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerMinute;
                    return MassRateUnits.MilligramPerHour;

                case MicroGramPerMinString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerMinute;
                    return MassRateUnits.MicrogramPerHour;

                case MicroGramPerSecString:
                    conversionFactor = 1.0 / ConversionFactors.HoursPerSecond;
                    return MassRateUnits.MicrogramPerHour;

                case GramPerMinString:
                    conversionFactor = ConversionFactors.One2Milli / ConversionFactors.HoursPerMinute;
                    return MassRateUnits.MilligramPerHour;

                case GramPerSecString:
                    conversionFactor = ConversionFactors.One2Milli / ConversionFactors.HoursPerSecond;
                    return MassRateUnits.MilligramPerHour;

                case GramPerDayString:
                    conversionFactor = ConversionFactors.One2Milli;
                    return MassRateUnits.MilligramPerDay;

                default:
                    throw new NotSupportedException(string.Format("Unsupported contact rate unit '{0}'", unitName));
            }
        }

        public static AreaUnits ParseContactAreaPackagingUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case MillimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareMillimetre;

                case CentimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareCentimetre;

                case DecimeterSquaredString:
                    conversionFactor = 1.0;
                    return AreaUnits.SquareDecimetre;

                case MeterSquaredString:
                    conversionFactor = Math.Pow(ConversionFactors.One2Deci, 2);
                    return AreaUnits.SquareDecimetre;

                default:
                    throw new NotSupportedException(string.Format("Unsupported packaging contact area unit '{0}'", unitName));
            }
        }

        public static MassUnits ParsePackagingProductAmountUnit(string unitName, out double conversionFactor)
        {
            switch (unitName)
            {
                case KilogramString:
                    conversionFactor = 1.0;
                    return MassUnits.Kilogram;

                case GramString:
                    conversionFactor = 1.0;
                    return MassUnits.Gram;

                case MilligramString:
                    conversionFactor = 1.0;
                    return MassUnits.Milligram;

                case MicrogramString:
                    conversionFactor = 1.0;
                    return MassUnits.Microgram;

                default:
                    throw new NotSupportedException(string.Format("Unsupported packaged product amount unit '{0}'", unitName));
            }
        }
    }
}