using System;
using System.Collections.Generic;
using System.Reflection;
using Erazer.Framework.DTO;
using Erazer.Infrastructure.MongoDb;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MongoDbDtoBuilder
    {
        public List<Assembly> Assemblies { get; private set; }
        
        // ReSharper disable once MemberCanBeMadeStatic.Global
        public void Dto<T>(Action<MongoDbDtoConfig<T>> configure) where T : class, IDto
        {
            var dto = new MongoDbDtoConfig<T>();
            configure(dto);
        }

        public void AddAssembly(params Assembly[] assemblies)
        {
            Assemblies = new List<Assembly>(assemblies);
        }
    }
    
    public class MongoDbDtoConfig<T>
    {
        public void SetCollectionName(string collectionName)
        {
            CollectionNameMapping.AddCollectionName<T>(collectionName);
        }
    }
}