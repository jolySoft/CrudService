using We.CrudService.Api.Controllers;
using We.CrudService.Api.Repository;
using We.CrudService.Tests.TestEntities;

namespace We.CrudService.Tests.TestControllers
{
    public class TestController : EntityController<TestEntity>
    {
        public TestController(Repository<TestEntity> repository) : base(repository)
        {
        }
    }
}