namespace RIVM.ConsExpo.DTO
{
    /// <summary>
    /// Global constants.
    /// </summary>
    public class Constants
    {
#warning To Do: set tolerance to a good value.

        /// <summary>
        /// The target numerical accuracy to be used in numerical computations.
        /// </summary>
        public const double RelativeTolerance = 0.01;

        /// <summary>
        /// The scale threshold percentage to be used in numerical computations.
        /// </summary>
        public const double ScaleThresholdPercentage = 0.01;

        public const double MedianOfBeta2_3 = 0.6142724318676105;

        public const double MedianOfBeta3_2 = 1 - MedianOfBeta2_3;

        public const string AssessmentCurrentSchemaVersion = "1.4";

        public const string AssessmentCurrentSchemaNamespace = @"http://www.consexpo.com/assessment/" + AssessmentCurrentSchemaVersion;

        /// <summary>
        /// The url of the latest version of the schema.
        /// </summary>
        public const string AssessmentCurrentSchemaLocation = "~/schemas/" + AssessmentCurrentSchemaVersion + "/assessment.xsd";

        /// <summary>Old version of the ConsExpo file definition. Uploading of old versions is still supported.</summary>
        public const string AssessmentSchemaNamespace_1_0 = @"http://www.consexpo.com/assessment";

        /// <summary>
        /// Old version 1.1 of the ConsExpo file definition. Uploading of old versions is still supported.
        /// </summary>
        public const string AssessmentSchemaNamespace_1_1 = @"http://www.consexpo.com/assessment/1.1";

        /// <summary>
        /// Current version of the ConsExpo file definition.</summary>
        /// </summary>
        public const string AssessmentSchemaNamespace_1_2 = @"http://www.consexpo.com/assessment/1.2";

        /// <summary>
        /// Current version of the ConsExpo file definition.</summary>
        /// </summary>
        public const string AssessmentSchemaNamespace_1_3 = @"http://www.consexpo.com/assessment/1.3";

        /// <summary>
        /// Current version of the ConsExpo file definition.</summary>
        /// </summary>
        public const string AssessmentSchemaNamespace_1_4 = @"http://www.consexpo.com/assessment/1.4";

        /// <summary>
        /// The maximum value for the parameters of the beta distribution (alpha and beta).
        /// </summary>
        public const double BetaDistributionMaxParameterValue = 500;

        /// <see href="https://en.wikipedia.org/wiki/Byte_order_mark">Byte order mark</see>
        public static readonly byte[] Utf8Bom = new byte[] { 0xEF, 0xBB, 0xBF };

        /// <summary>
        /// The maximum value a fraction can have.
        /// </summary>
        public static readonly double MaxFraction = 1.0;
    }
}