using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace Copious.Utilities {
    /// <summary>
    /// Map entity type values using reflection.
    /// <author>Abdul Kader Jeelani.A.</author>
    /// </summary>
    /// <remarks></remarks>
    public static class EntityMapper {
        public static bool CanMap<TTarget, TSource> (Type targetType, Type sourceType) => (targetType == typeof (TTarget) && sourceType == typeof (TSource)) ||
            (targetType == typeof (TSource) && sourceType == typeof (TTarget));

        public static bool CanMap<TTarget, TSource> () => CanMap<TTarget, TSource> (typeof (TTarget), typeof (TSource));

        public static TTarget Map<TTarget, TSource> (TSource source)
        where TTarget : class, new ()
        where TSource : class, new () {
            if (!CanMap<TTarget, TSource> ())
                throw new EntityMapperException ("Mapping not possible");

            return FillObject<TTarget, TSource> (source);
        }

        public static IEnumerable<TTarget> Map<TTarget, TSource> (IEnumerable<TSource> tableSource)
        where TTarget : class, new ()
        where TSource : class, new () {
            if (!CanMap<TTarget, TSource> ())
                throw new EntityMapperException ("Mapping not possible");

            foreach (TSource sourceentity in tableSource)
                yield return FillObject<TTarget, TSource> (sourceentity);
        }

        /// <summary>
        /// Return an object of Target type with the properties specified in value.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="referenceValue"></param>
        /// <returns></returns>
        static object FillObject (object value, Type targetType, object referenceValue) {
            var sourceProperties = value.GetType ().GetTypeInfo ().GetProperties ().Where (pi => pi.CanWrite);
            var targetProperties = targetType.GetTypeInfo ().GetProperties ().Where (pi => pi.CanWrite);

            var target = targetType.GetTypeInfo ().Assembly.CreateInstance (targetType.FullName);

            PropertyInfo targetProperty;

            foreach (var sourceProperty in sourceProperties) {
                if (targetProperties.Any (tp => tp.Name.Equals (sourceProperty.Name, StringComparison.OrdinalIgnoreCase))) {
                    targetProperty = targetProperties.Single (bp => bp.Name.Equals (sourceProperty.Name, StringComparison.OrdinalIgnoreCase));

                    if (targetProperty.PropertyType.GetTypeInfo ().IsGenericType)
                        MapGenericType (value, target, targetProperty, sourceProperty);
                    else
                        MapNonGenericType (value, targetType, referenceValue, target, targetProperty, sourceProperty);
                }
            }

            return target;
        }

        static void MapNonGenericType (object value, Type targetType, object referenceValue, object target, PropertyInfo targetProperty, PropertyInfo sourceProperty) {
            if (targetProperty.PropertyType.GetTypeInfo ().IsClass && targetProperty.PropertyType.FullName != "System.String") {
                if (referenceValue != null && targetProperty.DeclaringType == referenceValue.GetType ())
                    targetProperty.SetValue (target, referenceValue, null);
                else
                    targetProperty.SetValue (target, FillObject (value, targetType, null), null);
            } else {
                switch (targetProperty.PropertyType.FullName) {
                    case nameof (System.Guid):
                        {
                            targetProperty.SetValue (target, new Guid (sourceProperty.GetValue (value, null).ToString ()), null);
                            break;
                        }
                    default:
                        {
                            try {
                                targetProperty.SetValue (target, Convert.ChangeType (sourceProperty.GetValue (value, null), targetProperty.PropertyType, System.Globalization.CultureInfo.CurrentCulture), null);
                            } catch (Exception) {
                                targetProperty.SetValue (target, null);
                            }

                            break;
                        }
                }
            }
        }

        static void MapGenericType (object value, object target, PropertyInfo targetProperty, PropertyInfo sourceProperty) {
            var genericType = targetProperty.PropertyType.GetGenericTypeDefinition ();
            var genericTypeParameter1 = targetProperty.PropertyType.GetTypeInfo ().GetGenericArguments () [0];
            if (genericType == typeof (Nullable<>)) {
                if (sourceProperty.GetValue (value, null) != null)
                    targetProperty.SetValue (target, Convert.ChangeType (sourceProperty.GetValue (value, null), genericTypeParameter1, System.Globalization.CultureInfo.CurrentCulture), null);
                else
                    targetProperty.SetValue (target, null, null);
            } else if (genericType == typeof (IEnumerable<>)) {
                var sourceEntities = (IEnumerable) sourceProperty.GetValue (value, null);
                var targetEntitySet = (IList) targetProperty.PropertyType.GetTypeInfo ().Assembly.CreateInstance (targetProperty.PropertyType.FullName);

                foreach (var entity in sourceEntities) {
                    targetEntitySet.Add (Convert.ChangeType (typeof (Caster)
                        .GetTypeInfo ()
                        .GetMethod ("Cast", BindingFlags.Static | BindingFlags.Public)
                        .MakeGenericMethod (new Type[] { genericTypeParameter1 })
                        .Invoke (null, new object[] { FillObject (entity, genericTypeParameter1, target) }), genericTypeParameter1));
                }

                targetProperty.SetValue (target, targetEntitySet, null);
            }
        }

        public static T GetDefaultValue<T> () {
            // We want an Func<T> which returns the default.
            // Create that expression here.
            var e = Expression.Lambda<Func<T>> (
                // The default value, always get what the *code* tells us.
                Expression.Default (typeof (T))
            );

            // Compile and return the value.
            return e.Compile () ();
        }

        public static object GetDefaultValue (this Type type) {
            // Validate parameters.
            if (type == null) throw new ArgumentNullException (nameof (type));

            // We want an Func<object> which returns the default.
            // Create that expression here.
            var e = Expression.Lambda<Func<object>> (
                // Have to convert to object.
                Expression.Convert (
                    // The default value, always get what the *code* tells us.
                    Expression.Default (type), typeof (object)
                )
            );

            // Compile and return the value.
            return e.Compile () ();
        }

        static TTarget FillObject<TTarget, TSource> (TSource value)
        where TTarget : class, new ()
        where TSource : class, new () => (TTarget) FillObject (value, typeof (TTarget), null);
    }

    internal static class Caster {
        public static T Cast<T> (object o) => (T) o;
    }
}