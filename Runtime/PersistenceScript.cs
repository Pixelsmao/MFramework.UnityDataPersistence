using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;


namespace MFramework.DataPersistence
{
    public class PersistenceScript
    {
        public object script { get; }
        public Type scriptType => script.GetType();
        public string scriptName => scriptType.Name;
        public string scriptFullName => scriptType.FullName;
        public BindingFlags bindingFlags { get; }
        public PersistenceScriptReadme readmeFile { get; } = new PersistenceScriptReadme();
        public string readme => readmeFile.readme;

        public string readmeTitle
        {
            get => readmeFile.readmeTitle;
            set => readmeFile.readmeTitle = value;
        }

        private IEnumerable<FieldInfo> fields => script.GetType().GetFields(bindingFlags);
        private IEnumerable<PropertyInfo> properties => script.GetType().GetProperties(bindingFlags);

        private readonly List<PersistenceKeyValuePair> keyValuePairs = new List<PersistenceKeyValuePair>();

        public PersistenceScript(object script,
            BindingFlags bindingFlags = BindingFlags.Public | BindingFlags.Instance | BindingFlags.DeclaredOnly)
        {
            this.script = script;
            this.bindingFlags = bindingFlags;
            readmeFile.readmeTitle = scriptName;
            foreach (var fieldInfo in fields)
            {
                if (!fieldInfo.FieldType.IsValueType && fieldInfo.FieldType != typeof(string)) continue;
                if (fieldInfo.GetCustomAttribute<NonPersistenceAttribute>() != null) continue;
                TryGetValue(fieldInfo, script, out var fieldValue);
                if (fieldValue == null) continue;
                //如果是枚举类型则转换为int类型存储
                if (fieldInfo.FieldType.IsEnum) fieldValue = (int)fieldValue;
                keyValuePairs.Add(new FieldKeyValuePair(script, fieldInfo, fieldValue.ToString()));
            }

            foreach (var propertyInfo in properties)
            {
                if (!propertyInfo.PropertyType.IsValueType && propertyInfo.PropertyType != typeof(string)) continue;
                if (propertyInfo.GetCustomAttribute<NonPersistenceAttribute>() != null) continue;
                TryGetValue(propertyInfo, script, out var propertyValue);
                if (propertyValue == null) continue;
                //如果是枚举类型则转换为int类型存储
                if (propertyInfo.PropertyType.IsEnum) propertyValue = (int)propertyValue;

                keyValuePairs.Add(new PropertyKeyValuePair(script, propertyInfo, propertyValue.ToString()));
            }
        }

        public void AddReadmeLines(string[] readmeLinesContent) => readmeFile.AddReadmeLines(readmeLinesContent);

        public void AddReadmeLine(string readmeLine) => readmeFile.AddReadmeLine(readmeLine);

        public void TryApply(List<string> sectionLines)
        {
            var header = Regex.Match(sectionLines[0], @"\[(.*?)\]").Groups[1].Value;
            if (header != scriptName) Debug.LogError($"本地持久化配置【{header}】与持久化脚本【{scriptName}】不匹配！");
            //从1开始跳过配置标题
            var validLines = sectionLines.Skip(1).Where(item => item.Contains("=")).ToList();
            for (var i = 0; i < validLines.Count; i++)
            {
                var memberKeyValuePair = validLines[i];
                keyValuePairs[i].ParseKeyValuePair(Regex.Replace(memberKeyValuePair, @"<[^>]*>", ""));
            }
        }

        public static bool TryGetValue(FieldInfo field, object script, out object value)
        {
            var fieldValue = GetDefaultValue(field.FieldType);
            try
            {
                fieldValue = field.GetValue(script);
                value = fieldValue;
                return true;
            }
            catch (Exception)
            {
                value = fieldValue;
                return false;
            }
        }

        public static bool TryGetValue(PropertyInfo property, object script, out object value)
        {
            var fieldValue = GetDefaultValue(property.PropertyType);
            try
            {
                fieldValue = property.GetValue(script);
                value = fieldValue;
                return true;
            }
            catch (Exception)
            {
                value = fieldValue;
                return false;
            }
        }

        public static object GetDefaultValue(Type type)
        {
            var defaultExpression = Expression.Default(type); // 创建默认值表达式
            var lambda = Expression.Lambda<Func<object>>(
                Expression.Convert(defaultExpression, typeof(object))
            );
            return lambda.Compile()(); // 编译并执行
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(readmeFile);
            builder.AppendLine($"[{scriptName}]");
            foreach (var sectionField in keyValuePairs)
            {
                builder.AppendLine(sectionField.ToString());
            }

            return builder.ToString();
        }
    }
}