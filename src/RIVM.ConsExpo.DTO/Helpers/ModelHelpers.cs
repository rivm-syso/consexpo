using RIVM.ConsExpo.DTO.Extensions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace RIVM.ConsExpo.DTO.Helpers
{
    /// <summary>
    /// Extension methods for view models.
    /// </summary>
    public static class ModelHelpers
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        /// <see href="http://stackoverflow.com/questions/5474460/get-displayname-attribute-of-a-property-in-strongly-typed-way/5475513#5475513"/>
        public static string GetDisplayName<TModel>(Expression<Func<TModel, object>> expression)
        {
            Type type = typeof(TModel);

            string propertyName = null;
            string[] properties = null;
            IEnumerable<string> propertyList;
            //unless it's a root property the expression NodeType will always be Convert
            switch (expression.Body.NodeType)
            {
                case ExpressionType.Convert:
                case ExpressionType.ConvertChecked:
                    var ue = expression.Body as UnaryExpression;
                    propertyList = (ue != null ? ue.Operand : null).ToString().Split(".".ToCharArray()).Skip(1); //don't use the root property
                    break;

                default:
                    propertyList = expression.Body.ToString().Split(".".ToCharArray()).Skip(1);
                    break;
            }

            //the property name is what we're after.
            propertyName = propertyList.Last();
            //list of properties - the last property name
            properties = propertyList.Take(propertyList.Count() - 1).ToArray(); //grab all the parent properties

            Expression expr = null;
            foreach (string property in properties)
            {
                PropertyInfo propertyInfo = type.GetProperty(property);
                expr = Expression.Property(expr, type.GetProperty(property));
                type = propertyInfo.PropertyType;
            }

            DisplayAttribute attr;
            attr = (DisplayAttribute)type.GetProperty(propertyName).GetCustomAttributes(typeof(DisplayAttribute), true).SingleOrDefault();

            // Look for [MetadataType] attribute in type hierarchy
            // http://stackoverflow.com/questions/1910532/attribute-isdefined-doesnt-see-attributes-applied-with-metadatatype-class
            if (attr == null)
            {
                MetadataTypeAttribute metadataType = (MetadataTypeAttribute)type.GetCustomAttributes(typeof(MetadataTypeAttribute), true).FirstOrDefault();
                if (metadataType != null)
                {
                    var property = metadataType.MetadataClassType.GetProperty(propertyName);
                    if (property != null)
                    {
                        attr = (DisplayAttribute)property.GetCustomAttributes(typeof(DisplayNameAttribute), true).SingleOrDefault();
                    }
                }
            }
            return (attr != null) ? attr.Name : propertyName.ToFriendly();
        }

        public static T CopyValues<T>(T target, T source, bool copyAll = false)
        {
            Type t = typeof(T);

            var properties = t.GetProperties().Where(prop => prop.CanRead && prop.CanWrite);

            foreach (var prop in properties)
            {
                if (prop.PropertyType.IsClass && !(prop.PropertyType.Namespace?.StartsWith("System") ?? false))
                {
                    dynamic targetProp = prop.GetGetMethod().Invoke(target, null);
                    dynamic sourceProp = prop.GetGetMethod().Invoke(source, null);

                    if (sourceProp != null)
                        prop.SetValue(target, CopyValues(targetProp, sourceProp, copyAll));
                }
                else
                {
                    var value = prop.GetValue(source, null);
                    if (value != null || copyAll)
                        prop.SetValue(target, value, null);
                }
            }
            return target;
        }
    }
}