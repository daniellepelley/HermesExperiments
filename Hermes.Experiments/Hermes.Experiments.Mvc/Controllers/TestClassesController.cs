using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.ModelBinding;
using System.Web.Http.OData;
using System.Web.Http.OData.Query;
using System.Web.Http.OData.Routing;
using Hermes.Data.MongoDb;
using Hermes.Data.Repositories.Interfaces;
using Hermes.Experiments.Mvc.Models;
using Microsoft.Data.OData;
using MongoDB.Driver;

namespace Hermes.Experiments.Mvc.Controllers
{
    /*
    The WebApiConfig class may require additional changes to add a route for this controller. Merge these statements into the Register method of the WebApiConfig class as applicable. Note that OData URLs are case sensitive.

    using System.Web.Http.OData.Builder;
    using System.Web.Http.OData.Extensions;
    using Hermes.Experiments.Mvc.Models;
    ODataConventionModelBuilder builder = new ODataConventionModelBuilder();
    builder.EntitySet<TestClass>("TestClasses");
    config.Routes.MapODataServiceRoute("odata", "odata", builder.GetEdmModel());
    */
    public class TestClassesController : ODataController
    {
        private IRepository<TestClass> _repository; 

        public TestClassesController()
        {
            var connectionString = "mongodb://user:password@ds035750.mongolab.com:35750/test";
            var client = new MongoClient(connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase("test");

            var factory = new MongoDbRepositoryFactory(new MongoDbDataContext(database));

            _repository = factory.Create<TestClass>();

            _repository.Insert(new TestClass
            {
                _id = Guid.NewGuid().ToString(),
                Number = 1,
                Title = "Test"
            }
                
                
                );

        }

        private static ODataValidationSettings _validationSettings = new ODataValidationSettings();

        // GET: odata/TestClasses
        public async Task<IHttpActionResult> GetTestClasses(ODataQueryOptions<TestClass> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var result = queryOptions.ApplyTo(_repository.Items);

            return Ok((IEnumerable<TestClass>)result);
            //return StatusCode(HttpStatusCode.NotImplemented);
        }

        // GET: odata/TestClasses(5)
        public async Task<IHttpActionResult> GetTestClass([FromODataUri] string key, ODataQueryOptions<TestClass> queryOptions)
        {
            // validate the query.
            try
            {
                queryOptions.Validate(_validationSettings);
            }
            catch (ODataException ex)
            {
                return BadRequest(ex.Message);
            }

            var testClass = _repository.Items.FirstOrDefault(x => x._id == key);

            return Ok<TestClass>(testClass);
        }

        // PUT: odata/TestClasses(5)
        public async Task<IHttpActionResult> Put([FromODataUri] string key, Delta<TestClass> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.
            var testClass = _repository.Items.FirstOrDefault(x => x._id == key);

            delta.Put(testClass);

            // TODO: Save the patched entity.
            _repository.Insert(testClass);

            return Updated(testClass);
        }

        // POST: odata/TestClasses
        public async Task<IHttpActionResult> Post(TestClass testClass)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Add create logic here.
            _repository.Insert(testClass);

            return Created(testClass);
        }

        // PATCH: odata/TestClasses(5)
        [AcceptVerbs("PATCH", "MERGE")]
        public async Task<IHttpActionResult> Patch([FromODataUri] string key, Delta<TestClass> delta)
        {
            Validate(delta.GetEntity());

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // TODO: Get the entity here.
            var testClass = _repository.Items.FirstOrDefault(x => x._id == key);

            delta.Put(testClass);
            // TODO: Save the patched entity.
            _repository.Insert(testClass);


            // return Updated(testClass);
            return StatusCode(HttpStatusCode.NotImplemented);
        }

        // DELETE: odata/TestClasses(5)
        public async Task<IHttpActionResult> Delete([FromODataUri] string key)
        {
            var testClass = _repository.Items.FirstOrDefault(x => x._id == key);
            // TODO: Add delete logic here.
            _repository.Delete(testClass);
            return StatusCode(HttpStatusCode.NoContent);
        }
    }
}
