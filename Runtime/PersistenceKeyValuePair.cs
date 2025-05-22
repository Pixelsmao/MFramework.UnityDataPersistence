using System;
using System.Reflection;
using UnityEngine;

namespace MFramework.DataPersistence
{
    public abstract class PersistenceKeyValuePair
    {
        private object script { get; }

        protected abstract string keyValuePairName { get; }
        protected abstract string keyValuePairValue { get; set; }
        public abstract override string ToString();

        protected PersistenceKeyValuePair(object script)
        {
            this.script = script;
        }

        public abstract bool ParseKeyValuePair(string memberKeyValuePair);

        protected void ApplyValueTypeMember(FieldInfo field)
        {
            var convertedValue = Convert.ChangeType(keyValuePairValue, field.FieldType);
            field.SetValue(script, convertedValue);
        }

        protected void ApplyValueTypeMember(PropertyInfo property)
        {
            var convertedValue = Convert.ChangeType(keyValuePairValue, property.PropertyType);
            if (property.SetMethod != null) property.SetValue(script, convertedValue);
        }

        protected void ApplyEnumMember(FieldInfo field)
        {
            if (!Enum.TryParse(field.FieldType, keyValuePairValue, out var enumValue)) return;
            field.SetValue(script, enumValue);
        }

        protected void ApplyEnumMember(PropertyInfo property)
        {
            if (!Enum.TryParse(property.PropertyType, keyValuePairValue, out var enumValue)) return;
            if (property.SetMethod != null) property.SetValue(script, enumValue);
        }

        protected void ApplyVector2Member(FieldInfo field)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            field.SetValue(script, new Vector2(x, y));
        }

        protected void ApplyVector2Member(PropertyInfo property)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            if (property.SetMethod != null) property.SetValue(script, new Vector2(x, y));
        }

        protected void ApplyVector3Member(FieldInfo field)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            field.SetValue(script, new Vector3(x, y, z));
        }

        protected void ApplyVector3Member(PropertyInfo property)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            if (property.SetMethod != null) property.SetValue(script, new Vector3(x, y, z));
        }

        protected void ApplyVector4Member(FieldInfo field)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            float.TryParse(values[3].Trim(), out var w);
            field.SetValue(script, new Vector4(x, y, z, w));
        }

        protected void ApplyVector4Member(PropertyInfo property)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            float.TryParse(values[3].Trim(), out var w);
            if (property.SetMethod != null) property.SetValue(script, new Vector4(x, y, z, w));
        }

        protected void ApplyQuaternionMember(FieldInfo field)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            float.TryParse(values[3].Trim(), out var w);
            field.SetValue(script, new Vector4(x, y, z, w));
        }

        protected void ApplyQuaternionMember(PropertyInfo property)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var x);
            float.TryParse(values[1].Trim(), out var y);
            float.TryParse(values[2].Trim(), out var z);
            float.TryParse(values[3].Trim(), out var w);
            if (property.SetMethod != null) property.SetValue(script, new Vector4(x, y, z, w));
        }

        protected void ApplyColorMember(FieldInfo field)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var r);
            float.TryParse(values[1].Trim(), out var g);
            float.TryParse(values[2].Trim(), out var b);
            float.TryParse(values[3].Trim(), out var a);
            field.SetValue(script, new Color(r, g, b, a));
        }

        protected void ApplyColorMember(PropertyInfo property)
        {
            var trimmedStr = keyValuePairValue.Trim('(', ')');
            var values = trimmedStr.Split(',');
            float.TryParse(values[0].Trim(), out var r);
            float.TryParse(values[1].Trim(), out var g);
            float.TryParse(values[2].Trim(), out var b);
            float.TryParse(values[3].Trim(), out var a);
            if (property.SetMethod != null) property.SetValue(script, new Color(r, g, b, a));
        }
    }
}