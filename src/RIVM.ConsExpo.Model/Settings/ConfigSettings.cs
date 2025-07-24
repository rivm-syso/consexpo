using System;
using System.Configuration;

#pragma warning disable 1591 //Suppress "Missing XML comment for publicly visible type or member", as most are self-explanatory.

namespace RIVM.ConsExpo.Model.Settings
{
    /// <summary>
    /// Class that reads settings from configuration.
    /// </summary>
    /// <remarks>There is also a config settings class in the web project, but that cannot be used here. Also, it is not easy to pass the settings to here from the web project, as the submodel itself knows which settings it needs.</remarks>
    public class ConfigSettings
    {
        public static double RelTolDermalExposureDiffusion
        {
            get { return GetRelativeTolerance("RelTolDermalExposureDiffusion"); }
        }

        public static double RelTolDermalDiffusionThroughSkinForDiffusion
        {
            get { return GetRelativeTolerance("RelTolDermalDiffusionThroughSkinForDiffusion"); }
        }

        public static double RelTolInhalationExposureEmissionFromSolidMaterials
        {
            get { return GetRelativeTolerance("RelTolInhalationExposureEmissionFromSolidMaterials"); }
        }

        public static double RelTolInhalationExposureVapourEvaporation
        {
            get { return GetRelativeTolerance("RelTolInhalationExposureVapourEvaporation"); }
        }

        private static double GetRelativeTolerance(string keyName)
        {
            const double DefaultRelativeTolerance = 0.0001;

            double relativeTolerance;

            if (double.TryParse(GetSetting(keyName, false), out relativeTolerance))
            {
                return relativeTolerance;
            }
            else
            {
                return DefaultRelativeTolerance;
            }
        }

        /// <summary>
        /// Read a setting. If it is not available, an error is thrown.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected static string GetSetting(string key)
        {
            return GetSetting(key, true);
        }

        /// <summary>
        /// Read a setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="throwException">If the setting is not available, specify if an error must be thrown.</param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        protected static string GetSetting(string key, bool throwException)
        {
            string ConfigKey = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(ConfigKey))
            {
                if (throwException)
                {
                    throw new NullReferenceException(string.Format("Er werd geen waarde geretourneerd bij het opvragen van een configuratiewaarde. Controleer of de key: '{0}' gevuld is onder AppSettings in het configuratiebestand.", key));
                }
            }

            return ConfigKey;
        }

        /// <summary>
        /// Read a setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="defaultValue">The default value.</param>
        /// <returns></returns>
        protected static string GetSetting(string key, string defaultValue)
        {
            string ConfigKey = ConfigurationManager.AppSettings[key];

            if (string.IsNullOrEmpty(ConfigKey))
            {
                ConfigKey = defaultValue;
            }

            return ConfigKey;
        }
    }
}