using System;
using RIVM.ConsExpo.DTO.PhysicalQuantities;
using System.Linq;
using System.Reflection;

namespace RIVM.ConsExpo.DTO.Helpers
{
    internal static class SampleHelper
    {
        /// <summary>
        /// Calls all 'Sample' functions on properties of the given source object
        /// </summary>
        /// <seealso href="https://stackoverflow.com/a/4963179/456456">How to determine if a type implements an interface with C# reflection</seealso>
        public static void SampleAll<T>(T source)
        {
            string functionName = nameof(IDistributablePhysicalQuantityBase.Sample);

            var properties = typeof(T).GetProperties().Where(p => typeof(IDistributablePhysicalQuantityBase).IsAssignableFrom(p.PropertyType));

            foreach (PropertyInfo property in properties)
            {
                // Get Sample(), the overload of Sample without any parameters. We know it is implemented, as we filtered for properties implementing IDistributablePhysicalQuantityBase.
                MethodInfo method = property.PropertyType.GetMethod(functionName, Type.EmptyTypes);
                var value = property.GetValue(source, null);
                if (value != null)
                {
                    // This parameter is instantiated, so sample it. (Many parameters are used in only some submodels.)
                    method.Invoke(value, null);
                }
            }
        }
    }
}