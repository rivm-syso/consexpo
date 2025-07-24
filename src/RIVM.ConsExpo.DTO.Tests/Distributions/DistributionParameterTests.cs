using Microsoft.VisualStudio.TestTools.UnitTesting;
using RIVM.ConsExpo.DTO.Distributions;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace RIVM.ConsExpo.DTO.Tests.Distributions
{
    /// <summary>
    /// Tests for parametrization of the distribution class.
    /// </summary>
    [TestClass]
    public class DistributionParameterTests
    {
        [TestMethod]
        public void UniformParameterTestUniformEqualBounds()
        {
            var uniform = new Distribution
            {
                DistributionType = DistributionTypes.Uniform,
                LowerBound = 1,
                UpperBound = 1
            };

            Validate(uniform, 1);
        }

        [TestMethod]
        public void UniformParameterTestUniformReverseBounds()
        {
            var uniform = new Distribution
            {
                DistributionType = DistributionTypes.Uniform,
                LowerBound = 1,
                UpperBound = 0
            };

            Validate(uniform, 1);
        }

        [TestMethod]
        public void UniformParameterTestUniformCorrectBounds()
        {
            var uniform = new Distribution
            {
                DistributionType = DistributionTypes.Uniform,
                LowerBound = 0,
                UpperBound = 1
            };

            Validate(uniform, 0);
        }

        [TestMethod]
        public void UniformParameterTestTriangularReverseBounds()
        {
            var triangular = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 2,
                Shape = 1,
                Scale = 0
            };

            Validate(triangular, 3);
        }

        [TestMethod]
        public void UniformParameterTestTriangularEqualLocationAndShape()
        {
            var triangular = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 1,
                Shape = 1,
                Scale = 2
            };

            Validate(triangular, 0);
        }

        [TestMethod]
        public void UniformParameterTestTriangularEqualScaleAndShape()
        {
            var triangular = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 1,
                Shape = 2,
                Scale = 2
            };

            Validate(triangular, 0);
        }

        [TestMethod]
        public void UniformParameterTestTriangularEqualLocationAndScale()
        {
            var triangular = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 1,
                Shape = 1,
                Scale = 1
            };

            //Only the fact that Location and Scale are equal should generate a validation error.
            Validate(triangular, 1);
        }

        [TestMethod]
        public void UniformParameterTestTriangularCorrectBounds()
        {
            var triangular = new Distribution
            {
                DistributionType = DistributionTypes.Triangular,
                Location = 0,
                Shape = 1,
                Scale = 2
            };

            Validate(triangular, 0);
        }

        [TestMethod]
        public void UniformParameterTestBetaCorrectBounds()
        {
            var beta = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = 1,
                Beta = 1
            };

            Validate(beta, 0);
        }

        [TestMethod]
        public void UniformParameterTestBetaMissing()
        {
            var beta = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = null,
                Beta = null
            };

            Validate(beta, 2);
        }

        //[TestMethod]
#warning ToDo: Why does this test not detect the Range violations on alpha and beta?
        public void UniformParameterTestBetaIncorrectBounds()
        {
            var beta = new Distribution
            {
                DistributionType = DistributionTypes.Beta,
                Alpha = -1,
                Beta = 0
            };

            Validate(beta, 2);
        }

        /// <summary>
        /// Test the validation of the specified distribution.
        /// </summary>
        /// <param name="distribution">The instance of a distribution to use.</param>
        /// <param name="expectedNumberOfValidationResults">The expected number of validation results (errors).</param>
        private static void Validate(Distribution distribution, int expectedNumberOfValidationResults)
        {
            var validationContext = new ValidationContext(distribution);
            var actualNumberOfValidationResults = distribution.Validate(validationContext).Count();
            Assert.IsTrue(actualNumberOfValidationResults == expectedNumberOfValidationResults, $"Validation should fail with {expectedNumberOfValidationResults} errors, but found {actualNumberOfValidationResults} for distribution {TestHelpers.DumpObject(distribution)}");
        }
    }
}