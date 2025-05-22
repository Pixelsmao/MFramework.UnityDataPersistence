using System;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEngine;

namespace MFramework.DataPersistence
{
    public class FieldKeyValuePair : PersistenceKeyValuePair
    {
        public FieldInfo field { get; }
        protected override string keyValuePairName => field.Name;
        protected sealed override string keyValuePairValue { get; set; }

        public override string ToString() => $"{GetFieldTooltip(field)}{keyValuePairName}={keyValuePairValue}";

        public FieldKeyValuePair(object script, FieldInfo field, string fieldValue) : base(script)
        {
            this.field = field;
            this.keyValuePairValue = fieldValue;
        }

        public override bool ParseKeyValuePair(string memberKeyValuePair)
        {
            var clearKeyValuePair = Regex.Replace(memberKeyValuePair, @"\s+", "");
            var keyValuePair = clearKeyValuePair.Split("=");
            var key = keyValuePair[0];
            var value = keyValuePair[1];
            if (!string.Equals(key, keyValuePairName)) return false;
            keyValuePairValue = value;
            if (field.FieldType.IsEnum) ApplyEnumMember(field);
            else if (field.FieldType == typeof(Vector2)) ApplyVector2Member(field);
            else if (field.FieldType == typeof(Vector3)) ApplyVector3Member(field);
            else if (field.FieldType == typeof(Vector4)) ApplyVector4Member(field);
            else if (field.FieldType == typeof(Vector4)) ApplyQuaternionMember(field);
            else if (field.FieldType == typeof(Color)) ApplyColorMember(field);
            else
            {
                ApplyValueTypeMember(field);
            }

            return true;
        }

        private string GetFieldTooltip(FieldInfo fieldInfo)
        {
            var attributes = fieldInfo.GetCustomAttributes(typeof(TooltipAttribute), false);
            var tooltip = attributes.Length > 0 ? ((TooltipAttribute)attributes[0]).tooltip : string.Empty;
            return string.IsNullOrEmpty(tooltip) ? string.Empty : $"<{tooltip}>";
        }
    }
}