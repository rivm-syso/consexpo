using System;
using System.ComponentModel.DataAnnotations;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Validator for CAS-numbers.
    /// </summary>
    public class CasNumberChecksum : ValidationAttribute
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <see href="http://www.cas.org/content/chemical-substances/checkdig"/>
        public override bool IsValid(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
            {
                return true;
            }

            try
            {
                var casFragments = value.ToString().Split('-');

                int checksum = int.Parse(casFragments[2]);
                string significantDigits = casFragments[0] + casFragments[1];

                int sum = 0;

                for (int i = 0; i < significantDigits.Length; i++)
                {
                    sum += (significantDigits.Length - i) * int.Parse(significantDigits[i].ToString());
                }

                return ((sum % 10) == checksum);
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}