// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MS-PL license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;

namespace SnkFramework.IoC
{
    public static class SnkConventionAttributeExtensions
    {
        /// <summary>
        /// A type is conventional if and only it is:
        /// - not marked with an unconventional attribute
        /// - all marked conditional conventions return true
        /// </summary>
        /// <param name="candidateType"></param>
        /// <returns></returns>
        public static bool IsConventional(this Type candidateType)
        {
            var unconventionalAttributes = candidateType.GetCustomAttributes(typeof(SnkUnconventionalAttribute),
                                                                             true);
            if (unconventionalAttributes.Length > 0)
                return false;

            return candidateType.SatisfiesConditionalConventions();
        }

        /// <summary>
        /// A propertyInfo is conventional if and only it is:
        /// - not marked with an unconventional attribute
        /// - all marked conditional conventions return true
        /// </summary>
        /// <param name="propertyInfo"></param>
        /// <returns></returns>
        public static bool IsConventional(this PropertyInfo propertyInfo)
        {
            var unconventionalAttributes = propertyInfo.GetCustomAttributes(typeof(SnkUnconventionalAttribute),
                                                                             true);
            if (unconventionalAttributes.Any())
                return false;

            return propertyInfo.SatisfiesConditionalConventions();
        }

        public static bool SatisfiesConditionalConventions(this Type candidateType)
        {
            var conditionalAttributes =
                candidateType.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            foreach (SnkConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConditionSatisfied;
                if (!result)
                    return false;
            }
            return true;
        }

        public static bool SatisfiesConditionalConventions(this PropertyInfo propertyInfo)
        {
            var conditionalAttributes =
                propertyInfo.GetCustomAttributes(typeof(SnkConditionalConventionalAttribute), true);

            foreach (SnkConditionalConventionalAttribute conditional in conditionalAttributes)
            {
                var result = conditional.IsConditionSatisfied;
                if (!result)
                    return false;
            }
            return true;
        }
    }
}
