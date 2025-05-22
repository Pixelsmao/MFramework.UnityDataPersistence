using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using Debug = UnityEngine.Debug;

namespace MFramework.DataPersistence
{
    public class PersistenceFile
    {
        public string fileFullName => file.FullName;
        public string fileName => file.Name;
        public string readme { get; private set; } = string.Empty;
        private readonly FileInfo file;
        private readonly List<PersistenceScript> scripts = new List<PersistenceScript>();
        public PersistenceFile(string saveDirectory, string fileName, string fileExtension = "ini")
        {
            var directory = new DirectoryInfo(saveDirectory);
            if (!directory.Exists) directory.Create();
            file = new FileInfo(Path.Combine(saveDirectory, $"{fileName}.{fileExtension}"));
        }

        public void AddPersistenceScript(PersistenceScript persistenceScript)
        {
            if (ContainsScript(persistenceScript)) return;
            scripts.Add(persistenceScript);
        }


        public void SaveOrRead()
        {
            if (file.Exists) Read();
            else Save();
        }

        public bool ContainsScript(object script)
        {
            return scripts.Any(persistenceScript => ReferenceEquals(persistenceScript, script));
        }

        public void Save()
        {
            File.WriteAllText(file.FullName, ToString());
            OutputSaveLogInfo();
        }

        public void OpenFileInWindows()
        {
            Process.Start(fileFullName);
        }

        public void OpenFileLocationInWindows()
        {
            if (file.Directory != null) Process.Start(file.Directory.FullName);
        }

        private static bool IsHeaderLine(string line)
        {
            return !line.Contains("<") && !line.Contains(">") && line.Contains("[") && line.Contains("]");
        }

        public void Read()
        {
            var contentLines = File.ReadAllLines(file.FullName);
            var scriptSectionIndexes = contentLines.Select((t, i) => i)
                .Where(index => IsHeaderLine(contentLines[index])).ToList();
            scriptSectionIndexes.Add(contentLines.Length - 1);
            List<Tuple<int, int>> pairs = new List<Tuple<int, int>>();
            for (int i = 0; i < scriptSectionIndexes.Count - 1; i++)
            {
                pairs.Add(Tuple.Create(scriptSectionIndexes[i], scriptSectionIndexes[i + 1]));
            }
            if (pairs.Count < scripts.Count) Debug.Log($"本地配置文件【{fileName}】中的持久化配置数目少于已添加的持久化脚本数。");
            
            for (var i = 0; i < pairs.Count; i++)
            {
                var tuple = pairs[i];
                var sectionLines = new List<string>();
                for (var j = tuple.Item1; j < tuple.Item2 + 1; j++)
                {
                    sectionLines.Add(contentLines[j]);
                }

                if (i > scripts.Count - 1)
                {
                    StringBuilder builder = new StringBuilder();
                    builder.AppendLine($"本地配置文件【{fileName}】中的持久化配置数目多于已添加的持久化脚本数，多余部分将被丢弃。");
                    builder.AppendLine("多余部分:");
                    foreach (var sectionLine in sectionLines)
                    {
                        builder.AppendLine(sectionLine);
                    }

                    Debug.LogWarning(builder.ToString());
                    continue;
                }

                scripts[i].TryApply(sectionLines);
            }
            Debug.Log($"已加载持久化文件【{fileName}】");
        }

        private void OutputSaveLogInfo()
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine($"已保存持久化文件【{file.Name}】");
            builder.AppendLine($"文件路径{file.FullName}");
            builder.AppendLine($"文件内容：");
            builder.Append(ToString());
            Debug.Log(builder.ToString());
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            builder.Append(readme);
            foreach (var script in scripts)
            {
                builder.Append(script);
                builder.AppendLine();
            }

            return builder.ToString();
        }
    }
}