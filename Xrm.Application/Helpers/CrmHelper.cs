using Microsoft.Xrm.Sdk;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Xrm.Application.Helpers
{
    public static class CrmHelper
    {
        /// <summary>
        /// Get's the current value of the attribute combining data from the PreImage and Target.
        /// </summary>
        /// <returns></returns>
        public static T TrueValue<T>(Entity target, Entity preImage, string attributeName)
        {
            if(target == null || preImage == null || attributeName == null)
            {
                return default;
            }
            
            attributeName = attributeName.ToLower();

            T targetValue = target.GetAttributeValue<T>(attributeName);
            T preValue = preImage.GetAttributeValue<T>(attributeName);

            if (preValue == null || targetValue != null)
            {
                return targetValue;
            }
            else
            {
                if (target.HasAttribute(attributeName))
                {
                    return default;
                }
                else
                {
                    return preValue;
                }
            }
        }

        /// <summary>
        /// Checks if the attribute value did change (from preImage to Target)
        /// </summary>
        public static bool AttributeValueChanged(Entity target, Entity preImage, string attributeName)
        {
            attributeName = (attributeName ?? "").ToLowerInvariant();

            if (!target.Contains(attributeName))
            {
                return false;
            }
            
            if(target.Contains(attributeName) && target[attributeName] != null && !preImage.Contains(attributeName))
            {
                return true;
            }

            if (target[attributeName] == null && (!preImage.Contains(attributeName) || preImage[attributeName] == null)) { return false; }

            return !Equals(target[attributeName],preImage[attributeName]);
        }

        /// <summary>
        /// Does the entity have the attribute filled (equal to [entity].Attributes.ContainKey)
        /// </summary>
        public static bool HasAttribute(this Entity entity, string attributeName)
        {
            if(entity == null || attributeName == null)
            {
                return false;
            }

            return entity.Attributes.ContainsKey(attributeName.ToLowerInvariant());
        }

        /// <summary>
        /// Does the entity have any of attributes filled (equal to [entity].Attributes.ContainKey)
        /// </summary>
        public static bool HasAnyAttribute(this Entity entity, params string[] attributeNames)
        {
            if (attributeNames == null || attributeNames.Length == 0) { return false; }

            return attributeNames.Any(a => HasAttribute(entity, a));
        }

        public static T LatestValue<T>(Entity target, Entity preImage, string attributeName)
        {
            if (target == null || preImage == null || attributeName == null)
            {
                return default;
            }

            attributeName = attributeName.ToLower();

            if (AttributeValueChanged(target, preImage, attributeName))
                return target.GetAttributeValue<T>(attributeName);
            else
                return preImage.GetAttributeValue<T>(attributeName);
        }
    }
}
