﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Threading.Tasks;
using Engine.Model;
using Microsoft.AspNetCore.Components.Forms;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.GridFS;
using ForeignKeyAttribute = Engine.CustomAttribute.ForeignKeyAttribute;
using static Engine.Handler.Handler;

namespace Engine.Database
{
    public class BaseContext : IDisposable
    {
        private readonly IMongoDatabase _mongoDatabase;
        private MongoClient MongoClient { get; }
        private string DatabaseName { get; }

        public BaseContext(string connectionString, string database)
        {
            DatabaseName = database;
            MongoClient = new MongoClient(connectionString);
            _mongoDatabase = MongoClient.GetDatabase(database);
        }

        private IMongoCollection<T> Collection<T>() where T : IEntity => _mongoDatabase.GetCollection<T>(typeof(T).Name);

        public void Dispose()
        {
        }

        public async Task<Fichier> UploadFileAsync(IBrowserFile file)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions
            {
                ChunkSizeBytes = 261120,
                WriteConcern = WriteConcern.WMajority,
            });

            await using var stream = file.OpenReadStream(99999999999);
            await using var ms = new MemoryStream();
            await stream.CopyToAsync(ms);
            var arrayByte = ms.ToArray();

            var id = await bucket.UploadFromBytesAsync(file.Name, arrayByte);

            var fichier = new Fichier
            {
                Id = id.ToString(),
                Name = file.Name,
                UploadDate = DateTime.Now,
                Size = file.Size,
                DataBytes = arrayByte,
                FileType = GetTypeMime(file.Name),
                DataString = "data:" + GetTypeMime(file.Name) + ";base64," + Convert.ToBase64String(arrayByte)
            };

            stream.Close();
            return fichier;
        }

        public async Task<Fichier> DownloadFileAsync(string id)
        {
            var bucket = new GridFSBucket(_mongoDatabase, new GridFSBucketOptions
            {
                ChunkSizeBytes = 261120,
                ReadConcern = ReadConcern.Majority,
            });

            var objectId = ObjectId.Parse(id);

            var arrayByteTask = bucket.DownloadAsBytesAsync(objectId);
            using var streamTask = bucket.OpenDownloadStreamAsync(objectId);

            var arrayByte = await arrayByteTask;
            var stream = await streamTask;

            var fichier = new Fichier
            {
                Id = stream.FileInfo.Id.ToString(),
                Name = stream.FileInfo.Filename,
                UploadDate = stream.FileInfo.UploadDateTime,
                Size = stream.FileInfo.Length,
                DataBytes = arrayByte,
                FileType = GetTypeMime(stream.FileInfo.Filename),
                DataString = "data:" + GetTypeMime(stream.FileInfo.Filename) + ";base64," + Convert.ToBase64String(arrayByte)
            };

            stream.Close();
            return fichier;
        }

        public void DropDatabase() => MongoClient.DropDatabase(DatabaseName);

        public void DropCollection<T>() where T : IEntity => _mongoDatabase.DropCollection(typeof(T).Name);

        /// <summary>
        /// Get all entities of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public IQueryable<T> QueryCollection<T>() where T : IEntity => Collection<T>().AsQueryable();

        /// <summary>
        /// Get a collection filtered
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public IQueryable<T> QueryCollection<T>(Expression<Func<T, bool>> predicate) where T : IEntity => Collection<T>().AsQueryable().Where(predicate);

        /// <summary>
        /// Get a specific entity of a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id"></param>
        /// <returns></returns>
        public T GetEntity<T>(string id) where T : IEntity
        {
            return Collection<T>().Find(v => v.Id == id).FirstOrDefault();
        }

        /// <summary>
        /// Insert an entity in a collection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void Insert<T>(T entity) where T : IEntity
        {
            Collection<T>().InsertOne(entity);
        }

        /// <summary>
        /// Insert all entities of the parameter
        /// Parameter must be an IEnumerable of the same type
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entities"></param>
        public void InsertAll<T>(IEnumerable<T> entities) where T : IEntity
        {
            if (!entities.Any()) return;
            Collection<T>().InsertMany(entities);
        }

        /// <summary>
        /// Delete one entity selected by a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void RemoveOne<T>(Expression<Func<T, bool>> predicate) where T : IEntity
        {
            Collection<T>().DeleteOne(predicate);
        }

        /// <summary>
        /// Delete multiple entities of the same type
        /// selected by a predicate
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="predicate"></param>
        public void RemoveAll<T>(Expression<Func<T, bool>> predicate) where T : IEntity
        {
            Collection<T>().DeleteMany(predicate);
        }

        /// <summary>
        /// Update a single property of an entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <param name="propertyName"></param>
        /// <param name="newValue"></param>
        public void UpdateProperty<T>(T entity, string propertyName, object newValue) where T : IEntity
        {
            var update = Builders<T>.Update.Set(propertyName, newValue);
            Collection<T>().UpdateOne(v => v.Id == entity.Id, update);
        }

        /// <summary>
        /// Update all the entity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        public void UpdateEntity<T>(T entity) where T : IEntity
        {
            Collection<T>().ReplaceOne(v => v.Id == entity.Id, entity);
        }

        /// <summary>
        /// If entity contain ForeignKey
        /// Get the entity linked to the ForeignKey
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<T> GetEntityWithForeignKey<T>(IEntity entity) where T : IEntity
        {
            var type = entity.GetType();
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties)
            {
                var attributes = propertyInfo.GetCustomAttributes(true);
                foreach (var attribute in attributes)
                {
                    if (attribute is not ForeignKeyAttribute fkAttribute) continue;
                    var typeOfForeignKey = fkAttribute.TheType;
                    var valueOfForeignKey = propertyInfo.GetValue(entity);

                    var entitiesResultQuery = new object();
                    if (typeOfForeignKey == typeof(Fichier))
                    {
                        entitiesResultQuery = await DownloadFileAsync(valueOfForeignKey.ToString());
                    }
                    else
                    {
                        var methodInfo = this.GetType().GetMethod(nameof(GetEntity));
                        var genericMethod = methodInfo?.MakeGenericMethod(typeOfForeignKey);
                        entitiesResultQuery = genericMethod?.Invoke(this, new[] { valueOfForeignKey });
                        if (entitiesResultQuery == null) continue;
                    }

                    PropertyInfo propToUpdate = null;
                    foreach (var property in properties)
                    {
                        var propertyType = property.PropertyType;
                        if (propertyType == typeOfForeignKey)
                        {
                            propToUpdate = property;
                        }
                    }
                    propToUpdate?.SetValue(entity, entitiesResultQuery);
                }
            }
            return (T)entity;
        }

        /// <summary>
        /// Get a collection of linked IEntity
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        /// <returns></returns>
        public T GetCollectionEntity<T>(IEntity entity) where T : IEntity
        {
            var type = entity.GetType();
            var properties = type.GetProperties();
            foreach (var propertyInfo in properties.Where(v => v.PropertyType.IsGenericType))
            {
                var typeOfIEnumerable = propertyInfo.PropertyType.GenericTypeArguments;
                var methodInfo = this.GetType().GetMethods().First(v => v.Name == (nameof(QueryCollection)) && v.GetParameters().Length == 0);
                var genericMethod = methodInfo.MakeGenericMethod(typeOfIEnumerable);
                var queryResult = genericMethod?.Invoke(this, Array.Empty<object>());
                if (queryResult == null) continue;

                var result = Activator.CreateInstance(typeof(List<>).MakeGenericType(typeOfIEnumerable));

                foreach (var entityResult in (IEnumerable<IEntity>)queryResult)
                {
                    var propertiesResult = entityResult.GetType().GetProperties();
                    foreach (var property in propertiesResult)
                    {
                        var attributes = property.GetCustomAttributes(true);
                        foreach (var attribute in attributes)
                        {
                            if (attribute is not ForeignKeyAttribute) continue;
                            if (property.GetValue(entityResult)?.ToString() != entity.Id || property.GetValue(entityResult) == null) continue;

                            result?.GetType().GetMethod("Add")?.Invoke(result, new object[] { entityResult });
                        }
                    }
                }
                propertyInfo.SetValue(entity, result, null);
            }
            return (T)entity;
        }
    }
}
