using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.CE4;
using RIVM.ConsExpo.DTO.PhysicalUnits;

namespace RIVM.ConsExpo.DTO.Tests.CE4
{
    [TestClass]
    public class UnitsUnitTests
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

        [TestMethod]
        public void ParseFrequencyUnitTest()
        {
            double conversionFactor;
            const string PerSecondString = "1/sec";
            const string PerMinuteString = "1/min";
            const string PerHourString = "1/hr";

            FrequencyUnits frequencyUnit;

            frequencyUnit = Units.ParseFrequencyUnit(PerSecondString, out conversionFactor);
            Assert.AreEqual<FrequencyUnits>(FrequencyUnits.Daily, frequencyUnit);
            TestHelpers.AreEqualDoubles(60 * 60 * 24, conversionFactor);

            frequencyUnit = Units.ParseFrequencyUnit(PerMinuteString, out conversionFactor);
            Assert.AreEqual<FrequencyUnits>(FrequencyUnits.Daily, frequencyUnit);
            TestHelpers.AreEqualDoubles(60 * 24, conversionFactor);

            frequencyUnit = Units.ParseFrequencyUnit(PerHourString, out conversionFactor);
            Assert.AreEqual<FrequencyUnits>(FrequencyUnits.Daily, frequencyUnit);
            TestHelpers.AreEqualDoubles(24, conversionFactor);
        }

        [TestMethod]
        public void ParseBodyWeightUnitTest()
        {
            double conversionFactor;

            MassUnits massUnit;

            massUnit = Units.ParseBodyWeightUnit(GramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Kilogram, massUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            massUnit = Units.ParseBodyWeightUnit(MilligramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Kilogram, massUnit);
            TestHelpers.AreEqualDoubles(1E-6, conversionFactor);

            massUnit = Units.ParseBodyWeightUnit(MicrogramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Kilogram, massUnit);
            TestHelpers.AreEqualDoubles(1E-9, conversionFactor);
        }

        [TestMethod]
        public void ParseProductAmountUnitTest()
        {
            double conversionFactor;

            MassUnits massUnit;

            massUnit = Units.ParseProductAmountUnit(KilogramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Gram, massUnit);
            TestHelpers.AreEqualDoubles(1E3, conversionFactor);

            massUnit = Units.ParseProductAmountUnit(MicrogramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Milligram, massUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);
        }

        [TestMethod]
        public void ParseExposureDurationUnitTest()
        {
            double conversionFactor;

            DurationUnits durationUnit;

            durationUnit = Units.ParseExposureDurationUnit(SecondString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(1.0 / 60, conversionFactor);

            durationUnit = Units.ParseExposureDurationUnit(WeekString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(7, conversionFactor);

            durationUnit = Units.ParseExposureDurationUnit(MonthString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365.0 / 12, conversionFactor);

            durationUnit = Units.ParseExposureDurationUnit(YearString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365, conversionFactor);
        }

        [TestMethod]
        public void ParseVolumeRateUnitTest()
        {
            double conversionFactor;

            const string Cm3PerSecondString = "cm3/sec";
            const string Cm3PerMinuteString = "cm3/min";
            const string LiterPerHourString = "liter/h";
            const string M3PerSecondString = "m3/sec";
            const string M3PerMinuteString = "m3/min";
            const string LiterPerDayString = "liter/day";

            VolumeRateUnits volumeRateUnit;

            volumeRateUnit = Units.ParseVolumeRateUnit(Cm3PerSecondString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.LiterPerMinute, volumeRateUnit);
            TestHelpers.AreEqualDoubles(1E-3 * 60, conversionFactor);

            volumeRateUnit = Units.ParseVolumeRateUnit(Cm3PerMinuteString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.LiterPerMinute, volumeRateUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            volumeRateUnit = Units.ParseVolumeRateUnit(LiterPerHourString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.LiterPerMinute, volumeRateUnit);
            TestHelpers.AreEqualDoubles(1.0 / 60, conversionFactor);

            volumeRateUnit = Units.ParseVolumeRateUnit(M3PerSecondString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.CubicMetrePerHour, volumeRateUnit);
            TestHelpers.AreEqualDoubles(60 * 60, conversionFactor);

            volumeRateUnit = Units.ParseVolumeRateUnit(M3PerMinuteString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.CubicMetrePerHour, volumeRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            volumeRateUnit = Units.ParseVolumeRateUnit(LiterPerDayString, out conversionFactor);
            Assert.AreEqual<VolumeRateUnits>(VolumeRateUnits.CubicMetrePerDay, volumeRateUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);
        }

        [TestMethod]
        public void ParseSprayDurationUnitTest()
        {
            double conversionFactor;

            DurationUnits durationUnit;

            durationUnit = Units.ParseSprayDurationUnit(HourString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            durationUnit = Units.ParseSprayDurationUnit(DayString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(60 * 24, conversionFactor);

            durationUnit = Units.ParseSprayDurationUnit(WeekString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(60 * 24 * 7, conversionFactor);

            durationUnit = Units.ParseSprayDurationUnit(MonthString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(60 * 24 * 365.0 / 12, conversionFactor);

            durationUnit = Units.ParseSprayDurationUnit(YearString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(60 * 24 * 365, conversionFactor);
        }

        [TestMethod]
        public void ParseDensitySolidUnitTest()
        {
            double conversionFactor;

            DensityUnits densityUnit;

            densityUnit = Units.ParseDensitySolidUnit(MicrogramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensitySolidUnit(MilligramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);
        }

        [TestMethod]
        public void ParseDiffusionCoefficientUnitTest()
        {
            double conversionFactor;

            SurfaceRateUnits surfaceRateUnit;

            surfaceRateUnit = Units.ParseDiffusionCoefficientUnit(Cm2PerSecString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareCentiMetrePerMinute, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            surfaceRateUnit = Units.ParseDiffusionCoefficientUnit(M2PerMinuteString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareMetrePerHour, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);
        }

        [TestMethod]
        public void ParseMassGenerationRateUnitTest()
        {
            double conversionFactor;
            MassRateUnits massRateUnit;

            massRateUnit = Units.ParseMassGenerationRateUnit(MilliGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            massRateUnit = Units.ParseMassGenerationRateUnit(MicroGramPerMinString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            massRateUnit = Units.ParseMassGenerationRateUnit(MicroGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(1E-3 * 60, conversionFactor);

            massRateUnit = Units.ParseMassGenerationRateUnit(GramPerDayString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.GramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(1.0 / (60 * 24), conversionFactor);
        }

        [TestMethod]
        public void ParseReleaseAreaUnitTest()
        {
            double conversionFactor;
            AreaUnits areaUnit;

            areaUnit = Units.ParseReleaseAreaUnit(MillimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);

            areaUnit = Units.ParseReleaseAreaUnit(DecimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareMetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);
        }

        [TestMethod]
        public void ParseApplicationDurationUnitTest()
        {
            double conversionFactor;

            DurationUnits durationUnit;

            durationUnit = Units.ParseApplicationDurationUnit(SecondString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(1.0 / 60, conversionFactor);

            durationUnit = Units.ParseApplicationDurationUnit(WeekString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(7, conversionFactor);

            durationUnit = Units.ParseApplicationDurationUnit(MonthString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365.0 / 12, conversionFactor);

            durationUnit = Units.ParseApplicationDurationUnit(YearString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365, conversionFactor);
        }

        [TestMethod]
        public void ParseMassTransferCoefficientUnitTest()
        {
            double conversionFactor;
            VelocityUnits velocityUnit;

            velocityUnit = Units.ParseMassTransferCoefficientUnit(CentimeterPerSecond, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.MetrePerSecond, velocityUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);

            velocityUnit = Units.ParseMassTransferCoefficientUnit(MeterPerMinute, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.MetrePerMinute, velocityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            velocityUnit = Units.ParseMassTransferCoefficientUnit(CentimeterPerMinute, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.MetrePerHour, velocityUnit);
            TestHelpers.AreEqualDoubles(1E-2 * 60, conversionFactor);

            velocityUnit = Units.ParseMassTransferCoefficientUnit(CentimeterPerHour, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.MetrePerHour, velocityUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);
        }

        [TestMethod]
        public void ParseHeightUnitTest()
        {
            double conversionFactor;
            LengthUnits lengthUnit;

            lengthUnit = Units.ParseHeightUnit(CentimeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Metre, lengthUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);

            lengthUnit = Units.ParseHeightUnit(MicrometerString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Metre, lengthUnit);
            TestHelpers.AreEqualDoubles(1E-6, conversionFactor);

            lengthUnit = Units.ParseHeightUnit(MillimeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Metre, lengthUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            lengthUnit = Units.ParseHeightUnit(DecimeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Metre, lengthUnit);
            TestHelpers.AreEqualDoubles(1E-1, conversionFactor);
        }

        [TestMethod]
        public void ParseDiameterUnitTest()
        {
            double conversionFactor;
            LengthUnits lengthUnit;

            lengthUnit = Units.ParseDiameterUnit(MillimeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Micrometre, lengthUnit);
            TestHelpers.AreEqualDoubles(1E3, conversionFactor);
        }

        [TestMethod]
        public void ParseDensityNonVolatileUnitTest()
        {
            double conversionFactor;

            DensityUnits densityUnit;

            densityUnit = Units.ParseDensityNonVolatileUnit(MicrogramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(GramPerM3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(MilligramPerM3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-6, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(GramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(MilligramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);
        }

        [TestMethod]
        public void ParseCloudVolumeUnitTest()
        {
            double conversionFactor;

            VolumeUnits volumeUnit;

            volumeUnit = Units.ParseCloudVolumeUnit(MillimeterCubedString, out conversionFactor);
            Assert.AreEqual<VolumeUnits>(VolumeUnits.CubicMetre, volumeUnit);
            TestHelpers.AreEqualDoubles(1E-9, conversionFactor);

            volumeUnit = Units.ParseCloudVolumeUnit(CentimeterCubedString, out conversionFactor);
            Assert.AreEqual<VolumeUnits>(VolumeUnits.CubicMetre, volumeUnit);
            TestHelpers.AreEqualDoubles(1E-6, conversionFactor);

            volumeUnit = Units.ParseCloudVolumeUnit(DecimeterCubedString, out conversionFactor);
            Assert.AreEqual<VolumeUnits>(VolumeUnits.CubicMetre, volumeUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            volumeUnit = Units.ParseCloudVolumeUnit(LiterString, out conversionFactor);
            Assert.AreEqual<VolumeUnits>(VolumeUnits.CubicMetre, volumeUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);
        }

        [TestMethod]
        public void ParseReleaseDurationUnitTest()
        {
            double conversionFactor;

            DurationUnits durationUnit;

            durationUnit = Units.ParseReleaseDurationUnit(WeekString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(7, conversionFactor);

            durationUnit = Units.ParseReleaseDurationUnit(MonthString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365.0 / 12, conversionFactor);

            durationUnit = Units.ParseReleaseDurationUnit(YearString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, durationUnit);
            TestHelpers.AreEqualDoubles(365, conversionFactor);
        }

        [TestMethod]
        public void ParseEmissionDurationUnitTest()
        {
            double conversionFactor;

            DurationUnits durationUnit = Units.ParseEmissionDurationUnit(SecondString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Minute, durationUnit);
            TestHelpers.AreEqualDoubles(1.0 / 60, conversionFactor);
        }

        [TestMethod]
        public void ParseSubstanceConcentrationUnitTest()
        {
            double conversionFactor;

            DensityUnits densityUnit;

            densityUnit = Units.ParseSubstanceConcentrationUnit(MicrogramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerCubicMetre, densityUnit);
            TestHelpers.AreEqualDoubles(1.0, conversionFactor);

            densityUnit = Units.ParseSubstanceConcentrationUnit(MilligramPerM3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerCubicMetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseSubstanceConcentrationUnit(GramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1.0, conversionFactor);

            densityUnit = Units.ParseSubstanceConcentrationUnit(MilligramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerCubicMetre, densityUnit);
            TestHelpers.AreEqualDoubles(1.0, conversionFactor);
        }

        [TestMethod]
        public void ParseRubbingContactAreaUnitTest()
        {
            double conversionFactor;

            AreaUnits areaUnit;

            areaUnit = Units.ParseRubbingContactAreaUnit(MillimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);

            areaUnit = Units.ParseRubbingContactAreaUnit(DecimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareMetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);
        }

        [TestMethod]
        public void ParseTransferCoefficientUnitTest()
        {
            double conversionFactor;

            SurfaceRateUnits surfaceRateUnit;

            surfaceRateUnit = Units.ParseTransferCoefficientUnit(Cm2PerSecString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareMetrePerHour, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(3600 * 1E-4, conversionFactor);

            surfaceRateUnit = Units.ParseTransferCoefficientUnit(Cm2PerMinuteString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareMetrePerHour, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(60 * 1E-4, conversionFactor);

            surfaceRateUnit = Units.ParseTransferCoefficientUnit(Cm2PerHourString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareMetrePerHour, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(1E-4, conversionFactor);

            surfaceRateUnit = Units.ParseTransferCoefficientUnit(M2PerMinuteString, out conversionFactor);
            Assert.AreEqual<SurfaceRateUnits>(SurfaceRateUnits.SquareMetrePerHour, surfaceRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);
        }

        [TestMethod]
        public void ParseThicknessUnitTest()
        {
            double conversionFactor;

            LengthUnits lengthUnit;

            lengthUnit = Units.ParseThicknessUnit(DecimeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Centimetre, lengthUnit);
            TestHelpers.AreEqualDoubles(10, conversionFactor);

            lengthUnit = Units.ParseThicknessUnit(MeterString, out conversionFactor);
            Assert.AreEqual<LengthUnits>(LengthUnits.Centimetre, lengthUnit);
            TestHelpers.AreEqualDoubles(100, conversionFactor);
        }

        [TestMethod]
        public void ParseAreaDensityUnitTest()
        {
            const string GramPerCm2String = "g/cm2";
            const string MilligramPerCm2String = "mg/cm2";
            const string GramPerM2String = "g/m2";
            const string MilligramPerM2String = "mg/m2";

            double conversionFactor;

            AreaDensityUnits areaDensityUnit;

            areaDensityUnit = Units.ParseAreaDensityUnit(GramPerCm2String, out conversionFactor);
            Assert.AreEqual<AreaDensityUnits>(AreaDensityUnits.GramPerSquareMetre, areaDensityUnit);
            TestHelpers.AreEqualDoubles(1E-4, conversionFactor);

            areaDensityUnit = Units.ParseAreaDensityUnit(MilligramPerCm2String, out conversionFactor);
            Assert.AreEqual<AreaDensityUnits>(AreaDensityUnits.MilligramPerSquareMetre, areaDensityUnit);
            TestHelpers.AreEqualDoubles(1E-4, conversionFactor);

            areaDensityUnit = Units.ParseAreaDensityUnit(GramPerM2String, out conversionFactor);
            Assert.AreEqual<AreaDensityUnits>(AreaDensityUnits.GramPerSquareMetre, areaDensityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            areaDensityUnit = Units.ParseAreaDensityUnit(MilligramPerM2String, out conversionFactor);
            Assert.AreEqual<AreaDensityUnits>(AreaDensityUnits.MilligramPerSquareMetre, areaDensityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);
        }

        [TestMethod]
        public void ParseContactAreaMouthingUnitTest()
        {
            double conversionFactor;

            AreaUnits areaUnit;

            areaUnit = Units.ParseContactAreaMouthingUnit(MillimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E-2, conversionFactor);

            areaUnit = Units.ParseContactAreaMouthingUnit(DecimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E2, conversionFactor);

            areaUnit = Units.ParseContactAreaMouthingUnit(MeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E4, conversionFactor);
        }

        [TestMethod]
        public void ParseIngestionRateUnitTest()
        {
            double conversionFactor;

            MassRateUnits massRateUnit;

            massRateUnit = Units.ParseIngestionRateUnit(MilliGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            massRateUnit = Units.ParseIngestionRateUnit(MicroGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MicrogramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);
        }

        [TestMethod]
        public void ParseSkinPermeabilityUnitTest()
        {
            double conversionFactor;
            VelocityUnits velocityUnit;

            velocityUnit = Units.ParseSkinPermeabilityUnit(MeterPerSecond, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.MetrePerMinute, velocityUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            velocityUnit = Units.ParseSkinPermeabilityUnit(CentimeterPerSecond, out conversionFactor);
            Assert.AreEqual<VelocityUnits>(VelocityUnits.CentimetrePerMinute, velocityUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);
        }

        [TestMethod]
        public void ParseContactRateUnitTest()
        {
            double conversionFactor;

            MassRateUnits massRateUnit;

            massRateUnit = Units.ParseContactRateUnit(MilliGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerMinute, massRateUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            massRateUnit = Units.ParseContactRateUnit(MicroGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.GramPerDay, massRateUnit);
            TestHelpers.AreEqualDoubles(1E-6 * 60 * 60 * 24, conversionFactor);

            massRateUnit = Units.ParseContactRateUnit(GramPerMinString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.GramPerDay, massRateUnit);
            TestHelpers.AreEqualDoubles(60 * 24, conversionFactor);

            massRateUnit = Units.ParseContactRateUnit(GramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.GramPerDay, massRateUnit);
            TestHelpers.AreEqualDoubles(60 * 60 * 24, conversionFactor);
        }

        [TestMethod]
        public void ParseSubstanceConcentrationPackagingUnitTest()
        {
            double conversionFactor;

            DensityUnits densityUnit;

            densityUnit = Units.ParseDensityNonVolatileUnit(MicrogramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(MilligramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(MilligramPerM3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.MilligramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-6, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(GramPerCm3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerCubicCentimetre, densityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(GramPerM3String, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(MilligramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1E-3, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(GramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.GramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            densityUnit = Units.ParseDensityNonVolatileUnit(KilogramPerLiterString, out conversionFactor);
            Assert.AreEqual<DensityUnits>(DensityUnits.KilogramPerLitre, densityUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);
        }

        [TestMethod]
        public void ParseStorageTimeUnitTest()
        {
            double conversionFactor;

            DurationUnits storageTimeUnit;

            storageTimeUnit = Units.ParseStorageTimeUnit(SecondString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Hour, storageTimeUnit);
            TestHelpers.AreEqualDoubles(3600, conversionFactor);

            storageTimeUnit = Units.ParseStorageTimeUnit(MinuteString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Hour, storageTimeUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            storageTimeUnit = Units.ParseStorageTimeUnit(HourString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Hour, storageTimeUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            storageTimeUnit = Units.ParseStorageTimeUnit(DayString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, storageTimeUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            storageTimeUnit = Units.ParseStorageTimeUnit(WeekString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, storageTimeUnit);
            TestHelpers.AreEqualDoubles(7, conversionFactor);

            storageTimeUnit = Units.ParseStorageTimeUnit(MonthString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, storageTimeUnit);
            TestHelpers.AreEqualDoubles(30, conversionFactor, 1);

            storageTimeUnit = Units.ParseStorageTimeUnit(YearString, out conversionFactor);
            Assert.AreEqual<DurationUnits>(DurationUnits.Day, storageTimeUnit);
            TestHelpers.AreEqualDoubles(365, conversionFactor, 1);
        }

        [TestMethod]
        public void ParseMigrationRatePackagingUnitTest()
        {
            double conversionFactor;

            MassRateUnits migrationRatePackagingUnit;

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(MilliGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(3600, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(MilliGramPerMinString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(MicroGramPerMinString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MicrogramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(60, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(MicroGramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MicrogramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(3600, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(GramPerMinString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(6E4, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(GramPerSecString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerHour, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(3.6E6, conversionFactor);

            migrationRatePackagingUnit = Units.ParseMigrationRatePackagingUnit(GramPerDayString, out conversionFactor);
            Assert.AreEqual<MassRateUnits>(MassRateUnits.MilligramPerDay, migrationRatePackagingUnit);
            TestHelpers.AreEqualDoubles(1000, conversionFactor);
        }

        [TestMethod]
        public void ParseContactAreaPackagingUnitTest()
        {
            double conversionFactor;

            AreaUnits areaUnit;

            areaUnit = Units.ParseContactAreaPackagingUnit(MillimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareMillimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            areaUnit = Units.ParseContactAreaPackagingUnit(CentimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareCentimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            areaUnit = Units.ParseContactAreaPackagingUnit(DecimeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareDecimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            areaUnit = Units.ParseContactAreaPackagingUnit(MeterSquaredString, out conversionFactor);
            Assert.AreEqual<AreaUnits>(AreaUnits.SquareDecimetre, areaUnit);
            TestHelpers.AreEqualDoubles(1E2, conversionFactor);
        }

        [TestMethod]
        public void ParsePackagingProductAmountUnitTest()
        {
            double conversionFactor;

            MassUnits massUnit;

            massUnit = Units.ParsePackagingProductAmountUnit(KilogramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Kilogram, massUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            massUnit = Units.ParsePackagingProductAmountUnit(GramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Gram, massUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            massUnit = Units.ParsePackagingProductAmountUnit(MilligramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Milligram, massUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);

            massUnit = Units.ParsePackagingProductAmountUnit(MicrogramString, out conversionFactor);
            Assert.AreEqual<MassUnits>(MassUnits.Microgram, massUnit);
            TestHelpers.AreEqualDoubles(1, conversionFactor);
        }
    }
}