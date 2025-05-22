using System;

namespace MFramework.DataPersistence
{
    [AttributeUsage(AttributeTargets.Field | AttributeTargets.Property )]
    public class PersistenceFieldAttribute : Attribute
    {
    }
}