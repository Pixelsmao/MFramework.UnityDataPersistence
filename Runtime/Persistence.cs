using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;
using Object = UnityEngine.Object;

namespace MFramework.DataPersistence
{
    public static class Persistence
    {
        private static PersistenceFile defaultPersistenceFile;
        private static readonly List<PersistenceFile> persistenceFiles = new List<PersistenceFile>();
        private static string applicationDirectory => Path.GetDirectoryName(Application.dataPath);

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.AfterSceneLoad)]
        private static void InitDefaultPersistenceFile()
        {
            defaultPersistenceFile = new PersistenceFile(applicationDirectory, "Application");
            var scripts = Object.FindObjectsByType<MonoBehaviour>(FindObjectsSortMode.None);
            foreach (var script in scripts)
            {
                var type = script.GetType();
                var attribute = type.GetCustomAttribute<PersistenceScriptAttribute>();
                if (attribute == null) continue;
                if (string.IsNullOrEmpty(attribute.persistenceFileName))
                {
                    defaultPersistenceFile.AddPersistenceScript(new PersistenceScript(script));
                }
                else
                {
                    var persistenceFile =
                        persistenceFiles.FirstOrDefault(persistenceFile => persistenceFile.fileName == type.Name);
                    if (persistenceFile != null)
                    {
                        persistenceFile.AddPersistenceScript(new PersistenceScript(script));
                        continue;
                    }

                    persistenceFile = new PersistenceFile(applicationDirectory, type.Name);
                    persistenceFile.AddPersistenceScript(new PersistenceScript(script));
                    persistenceFiles.Add(persistenceFile);
                }
            }

            SaveOrReadPersistenceFile();
        }


        public static void Save(string fileName, object obj)
        {
        }

        public static void SaveOrReadPersistenceFile()
        {
            defaultPersistenceFile.SaveOrRead();
            foreach (var persistenceFile in persistenceFiles) persistenceFile.SaveOrRead();
        }

        public static void SavePersistenceFile()
        {
            defaultPersistenceFile.Save();
            foreach (var persistenceFile in persistenceFiles) persistenceFile.Save();
        }

        public static void ReadPersistenceFile()
        {
            defaultPersistenceFile.Read();
            foreach (var persistenceFile in persistenceFiles) persistenceFile.Read();
        }
    }
}