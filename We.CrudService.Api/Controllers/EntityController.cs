using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using We.CrudService.Api.Entities;
using We.CrudService.Api.Repository;

namespace We.CrudService.Api.Controllers
{
    [ApiController]
    public abstract class EntityController<TEntity> : Controller where TEntity: Entity
    {
        private Repository<TEntity> _repository;

        protected EntityController(Repository<TEntity> repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<List<TEntity>> Get()
        {
            return _repository.Get();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(Guid id)
        {
            var entity = await _repository.Get(id);
            return Ok(entity);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TEntity entity)
        {
            await _repository.Insert(entity);
            return Ok(entity.Id);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(Guid id, [FromBody] TEntity entity)
        {
            entity.Id = id;
            await _repository.Update(entity);
            return Ok();
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Patch(Guid id, JsonPatchDocument<TEntity> patch)
        {
            var entity = await _repository.Get(id);
            patch.ApplyTo(entity);
            await _repository.Update(entity);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _repository.Delete(id);
            return Ok();
        }
    }
}