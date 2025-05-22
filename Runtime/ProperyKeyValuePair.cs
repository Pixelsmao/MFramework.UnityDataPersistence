using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MFramework.DataPersistence
{
    public class PropertyKeyValuePair : PersistenceKeyValuePair
    {
        public PropertyInfo property { get; }
        protected override string keyValuePairName => property.Name;
        protected sealed override string keyValuePairValue { get; set; }
        public override string ToString() => $"{GetFieldTooltip(property)}{keyValuePairName}={keyValuePairValue}";

        public PropertyKeyValuePair(object script, PropertyInfo property, string fieldValue) : base(script)
        {
            this.property = property;
            keyValuePairValue = fieldValue;
        }

        public override bool ParseKeyValuePair(string memberKeyValuePair)
        {
            if (!memberKeyValuePair.Contains("=")) return false;
            var clearKeyValuePair = Regex.Replace(memberKeyValuePair, @"\s+", "");
            var keyValuePair = clearKeyValuePair.Split("=");
            var key = keyValuePair[0];
            var value = keyValuePair[1];
            if (!string.Equals(key, keyValuePairName)) return false;
            keyValuePairValue = value;
            if (property.PropertyType.IsEnum) ApplyEnumMember(property);
            else if (property.PropertyType == typeof(Vector2)) ApplyVector2Member(property);
            else if (property.PropertyType == typeof(Vector3)) ApplyVector3Member(property);
            else if (property.PropertyType == typeof(Vector4)) ApplyVector4Member(property);
            else if (property.PropertyType == typeof(Vector4)) ApplyQuaternionMember(property);
            else if (property.PropertyType == typeof(Color)) ApplyColorMember(property);
            else
            {
                ApplyValueTypeMember(property);
            }

            return true;
        }

        private string GetFieldTooltip(PropertyInfo propertyInfo)
        {
            var attributes = propertyInfo.GetCustomAttributes(typeof(TooltipAttribute), false);
            var tooltip = attributes.Length > 0 ? ((TooltipAttribute)attributes[0]).tooltip : string.Empty;
            return string.IsNullOrEmpty(tooltip) ? string.Empty : $"<{tooltip}>";
        }
    }
}