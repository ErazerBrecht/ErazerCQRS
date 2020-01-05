using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Erazer.Infrastructure.EventStore.Subscription;
using Erazer.Read.Data.Ticket;
using Erazer.Read.Data.Ticket.Events;

namespace Erazer.Infrastructure.MongoDb
{
    public class CollectionNameDictionary : Dictionary<Type, string>
    {
    }
    
    internal static class CollectionNameMapping
    {
        private static readonly CollectionNameDictionary CollectionNamesInternal = new CollectionNameDictionary();

        internal static void AddCollectionName<T>(string name)
        {
            CollectionNamesInternal.Add(typeof(T), name);
        }
        
        internal static string RetrieveCollectionName(Type type)
        {
            if (CachedCollectionNames.ContainsKey(type))
                return CachedCollectionNames[type];

            var collectionName = RetrieveCollectionNameHelper(type);
            CachedCollectionNames.TryAdd(type, collectionName);
            return collectionName;
        }

        #region Helpers
        private static readonly ConcurrentDictionary<Type,string> CachedCollectionNames = new ConcurrentDictionary<Type, string>();

        private static string RetrieveCollectionNameHelper(Type type)
        {
            if (CollectionNamesInternal.ContainsKey(type))
                return CollectionNamesInternal[type];

            var allParentTypes = GetParentTypes(type);
            var result = CollectionNamesInternal
                .Where(x => allParentTypes.Contains(x.Key))
                .ToList();

            return result.Count switch
            {
                1 => result.Single().Value,
                0 => type.Name,
                _ => throw new Exception("TODO")
            };
        }
        
        
        private static IEnumerable<Type> GetParentTypes(this Type type)
        {
            // Is there any base type?
            if (type == null)
            {
                yield break;
            }

            // return all implemented or inherited interfaces
            foreach (var i in type.GetInterfaces())
            {
                yield return i;
            }

            // return all inherited types
            var currentBaseType = type.BaseType;
            while (currentBaseType != null)
            {
                yield return currentBaseType;
                currentBaseType = currentBaseType.BaseType;
            }
        }
        #endregion
    }
    
    
}