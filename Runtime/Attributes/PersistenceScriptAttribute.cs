using System;

namespace MFramework.DataPersistence
{
    [AttributeUsage(AttributeTargets.Class)]
    public class PersistenceScriptAttribute : Attribute
    {
        public string persistenceFileName { get; }

        public PersistenceScriptAttribute()
        {
        }

        public PersistenceScriptAttribute(Type persistenceScriptType)
        {
            this.persistenceFileName = persistenceScriptType.Name;
        }

        public PersistenceScriptAttribute(string persistenceFileName)
        {
            this.persistenceFileName = persistenceFileName;
        }
    }
}