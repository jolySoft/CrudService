using System;
using We.CrudService.Api.Entities;

namespace We.CrudService.Tests.TestEntities
{
    public class TestEntity : Entity
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}