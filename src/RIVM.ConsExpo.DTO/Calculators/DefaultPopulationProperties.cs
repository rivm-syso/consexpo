namespace RIVM.ConsExpo.DTO.Calculators
{
    /// <summary>
    /// This class acts as an enumeration for options to retrieve population defaults from the database.
    /// The list of available populations depends on the purpose, currently two purposes are available:
    /// 1) defaults for body weight
    /// 2) defaults for inhalation rate.
    /// </summary>
    public class DefaultPopulationProperties
    {
        private DefaultPopulationProperties(string code)
        {
            this.code = code;
        }

        private readonly string code;

        public string Code => code;

        public static readonly DefaultPopulationProperties BodyWeight = new DefaultPopulationProperties("BW");

        public static readonly DefaultPopulationProperties InhalationRate = new DefaultPopulationProperties("IR");
    }
}