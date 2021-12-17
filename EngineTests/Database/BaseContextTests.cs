using System;
using System.Collections.Generic;
using System.Linq;
using EngineTests.Base;
using EngineTests.Base.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace EngineTests.Database
{
    [TestClass()]
    public class BaseContextTests
    {
        [TestInitialize]
        public void Init()
        {
            using var context = ContextFactoryTest.MakeContext();
            context.DropDatabase();
        }


        [TestMethod()]
        public void InsertQueryTest()
        {
            using var context = ContextFactoryTest.MakeContext();
            var collection = context.QueryCollection<MyEntity>();
            Assert.AreEqual(0, collection.Count());

            var list = new List<MyEntity>();

            for (var i = 0; i < 100; i++)
            {
                var entityToInsert = new MyEntity
                {
                    Name = i.ToString(),
                    Numeric = i,
                    DateTime = DateTime.Now,
                };

                if (i == 0)
                {
                    Assert.AreEqual(0, context.QueryCollection<MyEntity>().Count());
                    // Insert<T>(T entity) TEST
                    context.Insert(entityToInsert);
                    Assert.AreEqual(1, context.QueryCollection<MyEntity>().Count());
                }
                else
                {
                    list.Add(entityToInsert);
                }
            }

            // InsertAll<T>(IEnumerable<T> entities) TEST
            context.InsertAll(list);

            // QueryCollection<T>() TEST
            Assert.AreEqual(100, context.QueryCollection<MyEntity>().Count());

            // QueryCollection<T>(Expression<Func<T, bool>> predicate) Filtered TEST
            Assert.AreEqual(50, context.QueryCollection<MyEntity>(v => (v.Numeric % 2) == 0).Count());

            var datas = context.QueryCollection<MyEntity>();
            var data = datas.FirstOrDefault(v => v.Numeric == 0);

            // GetEntity(string id) TEST
            Assert.AreEqual(data?.Numeric, context.GetEntity<MyEntity>(data?.Id).Numeric);

            Assert.AreEqual(100, context.QueryCollection<MyEntity>().Count());

            // RemoveOne<T>(Expression<Func<T, bool>> predicate) TEST
            context.RemoveOne<MyEntity>(v => v.Numeric == 0);
            datas = context.QueryCollection<MyEntity>();
            Assert.AreEqual(99, datas.Count());
            foreach (var myEntity in datas)
            {
                Assert.AreNotEqual(0, myEntity.Numeric);
            }

            // RemoveAll<T>(Expression<Func<T, bool>> predicate) TEST
            context.RemoveAll<MyEntity>(v => v.Numeric % 2 == 0);
            Assert.AreEqual(50, context.QueryCollection<MyEntity>().Count());
            foreach (var myEntity in datas)
            {
                Assert.AreNotEqual(0, myEntity.Numeric % 2);
            }

        }


        [TestMethod()]
        public void UpdatePropertyTest()
        {
            using var context = ContextFactoryTest.MakeContext();

            foreach (var it in new[]
            {
                new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
            })
            {
                var entity = new MyEntity {Numeric = it.origin};
                
                context.Insert(entity);
                Assert.AreEqual(it.origin, context.GetEntity<MyEntity>(entity.Id).Numeric);

                context.UpdateProperty(entity, nameof(MyEntity.Numeric), it.modif);
                Assert.AreEqual(it.modif, context.GetEntity<MyEntity>(entity.Id).Numeric);
            }
        }

        [TestMethod()]
        public void UpdateEntityTest()
        {
            using var context = ContextFactoryTest.MakeContext();

            foreach (var it in new[]
                     {
                         new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                         new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                         new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                         new{origin = new Random().Next(0,10), modif = new Random().Next(0,10)},
                     })
            {
                var entity = new MyEntity { Numeric = it.origin };

                context.Insert(entity);
                Assert.AreEqual(it.origin, context.GetEntity<MyEntity>(entity.Id).Numeric);

                entity.Numeric = it.modif; 
                context.UpdateEntity(entity);
                Assert.AreEqual(it.modif, context.GetEntity<MyEntity>(entity.Id).Numeric);
            }
        }

        [TestMethod()]
        public void GetEntityWithForeignKeyTest()
        {
            using var context = ContextFactoryTest.MakeContext();
            var myEntity = new MyEntity
            {
                Numeric = 5,
                DateTime = DateTime.Now.ToLocalTime(),
                Name = "MyEntity"
            };
            context.Insert(myEntity);

            var entityContainForeignKey = new EntityContainForeignKey
            {
                MyEntityId = myEntity.Id,
                Data = "EntityContainForeignKey"
            };

            context.GetEntityWithForeignKey<EntityContainForeignKey>(entityContainForeignKey);
            
            Assert.AreEqual(myEntity.DateTime.ToString(), entityContainForeignKey.MyEntity?.DateTime.ToLocalTime().ToString());
            Assert.AreEqual(myEntity.Name, entityContainForeignKey.MyEntity?.Name);
            Assert.AreEqual(myEntity.Id, entityContainForeignKey.MyEntity?.Id);
        }

        [TestMethod()]
        public void GetCollectionEntityTest()
        {
            using var context = ContextFactoryTest.MakeContext(); 

            var entityContainManyEntity = new EntityContainManyEntity
            {
                Data = "Parent",
            };
            context.Insert(entityContainManyEntity);

            var listEntityContainForeignKey = new List<EntityContainForeignKey>(); 
            for (var i = 0; i < 10; i++)
            {
                var entityContainForeignKey = new EntityContainForeignKey
                {
                    EntityContainManyEntityId = entityContainManyEntity.Id, 
                    Data = i.ToString()
                };
                listEntityContainForeignKey.Add(entityContainForeignKey);
            }
            context.InsertAll(listEntityContainForeignKey);

            context.GetCollectionEntity<EntityContainManyEntity>(entityContainManyEntity); 

            Assert.AreEqual(10, entityContainManyEntity.CollectionOfEntityContainForeignKeys!.Count);


        }
    }
}