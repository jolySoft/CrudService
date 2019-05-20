using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using We.CrudService.Api.Entities;

namespace We.CrudService.Api.Repository
{
    public class Repository<TEntity> where TEntity : Entity
    {
        private readonly MongoDatabaseBase _database;
        private IMongoCollection<TEntity> _entities;

        public Repository(MongoDatabaseBase database)
        {
            _database = database;
            _entities = _database.GetCollection<TEntity>(typeof(TEntity).Name);
        }

        public Task<List<TEntity>> Get()
        {
            return _entities.Find(x => true).ToListAsync();
        }

        public async Task<TEntity> Get(Guid id)
        {
            var query = Builders<TEntity>.Filter.Eq(e => e.Id, id);
            var result = await _entities.FindAsync(query);
            return await result.FirstOrDefaultAsync();
        }

        public Task Insert(TEntity entity)
        {
            return _entities.InsertOneAsync(entity);
        }

        public async Task<bool> Update(TEntity entity)
        {
            var filter = Builders<TEntity>.Filter.Eq(e => e.Id, entity.Id);
            var updateResult = await _entities.ReplaceOneAsync(filter, entity, new UpdateOptions {IsUpsert = false});
            return updateResult.ModifiedCount == 1;
        }

        public Task<bool> Delete(TEntity entity)
        {
            return Delete(entity.Id);
        }

        public async Task<bool> Delete(Guid id)
        {
            var query = Builders<TEntity>.Filter.Eq("Id", id);
            var deleteResult = await _entities.DeleteOneAsync(query);
            return deleteResult.IsAcknowledged;
        }
    }
}