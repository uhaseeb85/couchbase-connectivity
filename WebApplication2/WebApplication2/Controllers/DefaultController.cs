using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Couchbase;
using Couchbase.Core;
using Couchbase.N1QL;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    public class DefaultController : ControllerBase
    {
        private IBucket _bucket;

        [HttpGet]
        [Route("api/getall")]
        public IActionResult GetAll()
        {
            //define a configuration object
            var config = new Couchbase.Configuration.Client.ClientConfiguration
            {
                Servers = new List<Uri> {
                new Uri("couchbase://localhost")
                    }
            };

            //create the cluster and pass in the RBAC user
            var cluster = new Cluster(config);
            var credentials = new Couchbase.Authentication.PasswordAuthenticator("myuser", "password");
            cluster.Authenticate(credentials);

            //open the new bucket
            ClusterHelper.Initialize(config, credentials);
            _bucket = ClusterHelper.GetBucket("demo");
            var n1q1 = "SELECT g.*,META(g).id FROM demo g USE KEYS [\"a6a3cd46-e7e6-4c08-bbe7-769105c84315\"];";
            var query = QueryRequest.Create(n1q1);
            query.ScanConsistency(ScanConsistency.RequestPlus);
            var result = _bucket.Query<WishListItem>(query);
            ClusterHelper.Close();
            return Ok(result.Rows);
        }
    }
}
