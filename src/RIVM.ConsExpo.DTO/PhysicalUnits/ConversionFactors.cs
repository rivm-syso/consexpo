#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.DTO.PhysicalUnits
{
    /// <summary>
    /// Conversion factors for transforming parameters to another unit.
    /// </summary>
    public static class ConversionFactors
    {
        #region Time

        public const int SecondsPerMinute = 60;
        public const int MinutesPerHour = 60;
        public const double HoursPerMinute = 1.0 / MinutesPerHour;
        public const int SecondsPerHour = SecondsPerMinute * MinutesPerHour;
        public const int HoursPerDay = 24;
        public const double DaysPerHour = 1.0 / HoursPerDay;
        public const int MinutesPerDay = MinutesPerHour * HoursPerDay;
        public const int MinutesPerYear = MinutesPerDay * DaysPerYear;
        public const double DaysPerMinute = 1.0 / MinutesPerDay;
        public const double YearsPerMinute = 1.0 / MinutesPerYear;
        public const int SecondsPerDay = SecondsPerMinute * MinutesPerDay;
        public const int SecondsPerWeek = SecondsPerDay * DaysPerWeek;
        public const int DaysPerWeek = 7;
        public const int MonthsPerYear = 12;
        public const int HoursPerWeek = DaysPerWeek * HoursPerDay;
        public const int DaysPerYear = 365; //Ignore leap years, as this is also done in the defaults database.
        public const int HoursPerYear = DaysPerYear * HoursPerDay;
        public const double HoursPerMonth = (double)HoursPerYear / MonthsPerYear;
        public const double SecondsPerMonth = (double)SecondsPerYear / MonthsPerYear;
        public const double DaysPerMonth = (double)DaysPerYear / MonthsPerYear;
        public const double WeeksPerYear = (double)DaysPerYear / DaysPerWeek;

        public const int SecondsPerYear = SecondsPerMinute * MinutesPerDay * DaysPerYear;

        public const double MinutesPerSecond = 1.0 / SecondsPerMinute;

        /// <summary>
        /// Multiply by this factor to convert from base unit to kilo unit.
        /// </summary>
        public const double HoursPerSecond = 1.0 / SecondsPerHour;

        /// <summary>
        /// Multiply by this factor to convert from base unit to kilo unit.
        /// </summary>
        public const double DaysPerSecond = 1.0 / SecondsPerDay;

        #endregion Time

        #region Unit prefixes, used in conversion.

        //These cannot be used directly, to prevent mistakes. Multiply by <prefix>2One in stead.
        private const double Kilo = 1000;

        private const double Deci = 1.0E-1;
        private const double Centi = 1.0E-2;
        private const double Milli = 1.0E-3;
        private const double Micro = 1.0E-6;
        private const double Nano = 1.0E-9;

        #endregion Unit prefixes, used in conversion.

        #region Unit prefix conversions

        /// <summary>
        /// Multiply by this factor to convert from base unit to kilo unit.
        /// </summary>
        public const double One2Kilo = 1 / Kilo;

        /// <summary>
        /// Multiply by this factor to convert from kilo unit to base unit.
        /// </summary>
        public const double Kilo2One = Kilo;

        /// <summary>
        /// Multiply by this factor to convert from base unit to centi unit.
        /// </summary>
        public static double One2Centi = 1 / Centi;

        /// <summary>
        /// Multiply by this factor to convert from centi unit to base unit.
        /// </summary>
        public static double Centi2One = Centi;

        /// <summary>
        /// Multiply by this factor to convert from base unit to milli unit.
        /// </summary>
        public const double One2Milli = 1 / Milli;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to base unit.
        /// </summary>
        public const double Milli2One = Milli;

        /// <summary>
        /// Multiply by this factor to convert from base unit to micro unit.
        /// </summary>
        public const double One2Micro = 1 / Micro;

        /// <summary>
        /// Multiply by this factor to convert from micro unit to base unit.
        /// </summary>
        public const double Micro2One = Micro;

        /// <summary>
        /// Multiply by this factor to convert from micro unit to centi unit.
        /// </summary>
        public const double Micro2Centi = Micro / Centi;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to micro unit.
        /// </summary>
        public const double Milli2Micro = Milli / Micro;

        /// <summary>
        /// Multiply by this factor to convert from micro unit to milli unit.
        /// </summary>
        public const double Micro2Milli = Micro / Milli;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to kilo unit.
        /// </summary>
        public const double Milli2Kilo = Milli / Kilo;

        /// <summary>
        /// Multiply by this factor to convert from kilo unit to milli unit.
        /// </summary>
        public const double Kilo2Milli = Kilo / Milli;

        /// <summary>
        /// Multiply by this factor to convert from micro unit to kilo unit.
        /// </summary>
        public const double Micro2Kilo = Micro / Kilo;

        /// <summary>
        /// Multiply by this factor to convert from kilo unit to micro unit.
        /// </summary>
        public const double Kilo2Micro = Kilo / Micro;

        /// <summary>
        /// Multiply by this factor to convert from centi unit to milli unit.
        /// </summary>
        public const double Centi2Milli = Centi / Milli;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to centi unit.
        /// </summary>
        public const double Milli2Centi = Milli / Centi;

        /// <summary>
        /// Multiply by this factor to convert from deci unit to milli unit.
        /// </summary>
        public const double Deci2Centi = Deci / Centi;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to deci unit.
        /// </summary>
        public const double Centi2Deci = Centi / Deci;

        /// <summary>
        /// Multiply by this factor to convert from base unit to milli unit.
        /// </summary>
        public const double One2Deci = 1 / Deci;

        /// <summary>
        /// Multiply by this factor to convert from milli unit to base unit.
        /// </summary>
        public const double Deci2One = Deci;

        #endregion Unit prefix conversions

        #region Temperature

        /// <summary>
        /// The temperature of 0 Celsius expressed in Kelvin.
        /// </summary>
        public const double CelsiusOffset = 273.15;

        /// <summary>
        /// The temperature of 0 Kelvin expressed in Celsius.
        /// </summary>
        public const double KelvinOffset = -1 * CelsiusOffset;

        #endregion Temperature

        #region Pressure

        /// <summary>
        ///
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Atmosphere_(unit)"/>
        public static double Atmosphere2Pascal = 101325.0;

        /// <summary>
        ///
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Bar_(unit)"/>
        public static double Bar2Pascal = 100000.0;

        public static double Millibar2Pascal = Milli * Bar2Pascal;

        /// <summary>
        ///
        /// </summary>
        /// <see href="https://en.wikipedia.org/wiki/Millimeter_of_mercury"/>
        public static double MmHg2Pascal = 133.322387415;

        #endregion Pressure

        #region Volume

        /// <summary>
        /// Multiply by this factor to convert from l unit to m3 unit.
        /// </summary>
        public const double LitresPerCubicMetre = 1000;

        /// <summary>
        /// Multiply by this factor to convert from m3 unit to l unit.
        /// </summary>
        public const double CubicMetresPerLitre = 1 / LitresPerCubicMetre;

        /// <summary>
        /// Multiply by this factor to convert from l unit to cm3 unit.
        /// </summary>
        public const double CubicCentimetresPerLitre = 1000;

        /// <summary>
        /// Multiply by this factor to convert from cm3 unit to l unit.
        /// </summary>
        public const double LitresPerCubicCentimetre = 1 / CubicCentimetresPerLitre;

        #endregion Volume

        #region Dimensionless

        public const double FractionToPercentage = 100;
        public const double PercentageToFraction = 1.0 / FractionToPercentage;

        #endregion Dimensionless
    }
}