using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Mongo2Go;
using MongoDB.Driver;
using We.CrudService.Api.Repository;
using We.CrudService.Tests.TestControllers;
using We.CrudService.Tests.TestEntities;
using Xunit;

namespace We.CrudService.Tests
{
    public class EntityControllerTest
    {
        private MongoDbRunner _runner;
        private Repository<TestEntity> _repository;
        private TestController _controller;
        private IMongoCollection<TestEntity> _collection;
        private MongoClient _client;
        private MongoDatabaseBase _database;
        private List<TestEntity> _entities;

        public EntityControllerTest()
        {
            _runner = MongoDbRunner.Start();
        
            _client = new MongoClient(_runner.ConnectionString);
            _database = (MongoDatabaseBase)_client.GetDatabase("Integration");
            _collection = _database.GetCollection<TestEntity>("TestEntity");


            _repository = new Repository<TestEntity>(_database);
            _controller = new TestController(_repository);
        }


        [Fact]
        public async Task CanGetAll()
        {
            BuildCollection();
            var result = await _controller.Get();

            result.Should().BeEquivalentTo(_entities);
        }

        [Fact]
        public async Task CanGetById()
        {
            BuildCollection();
            var entity = _entities[0];

            var result = ((OkObjectResult)await _controller.Get(entity.Id)).Value;

            result.Should().BeEquivalentTo(entity);
        }

        [Fact]
        public async Task CanPostAnEntity()
        {
            var entity = new TestEntity
            {
                Name = "Jolyon Wharton",
                Age = 15
            };

            await _controller.Post(entity);

            _collection.Find(x => true).First().Should().BeEquivalentTo(entity);
        }


        [Fact]
        public async Task CanPutAnEntity()
        {
            BuildCollection();
            var entity = _entities[1];
            entity.Name = "Brian Blessed";
            entity.Age = 21;


        }

        private void BuildCollection()
        {
            _entities = new List<TestEntity>
            {
                new TestEntity
                {
                    Name = "Jolyon Wharton",
                    Age = 15
                },
                new TestEntity
                {
                    Name = "Kieran Iles",
                    Age = 12
                },
                new TestEntity
                {
                    Name = "Peter Srank",
                    Age = 14
                }
            };

            _collection.InsertMany(_entities);
        }
    }
}