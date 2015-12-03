using EmptyWebAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Http.Routing;

namespace EmptyWebAPI.Controllers
{
    public class StudentController : ApiController
    {
        // Data source
        List<Student> db = new List<Student>();
        public StudentController()
        {
            db = new List<Student>
            {
                new Student { Id = 1, Name = "Abhimanyu K Vatsa", City = "Bokaro" },
                new Student { Id = 2, Name = "Deepak Kumar Gupta", City = "Bokaro" },
                new Student { Id = 3, Name = "Manish Kumar", City = "Muzaffarpur" },
                new Student { Id = 4, Name = "Rohit Ranjan", City = "Patna" },
                new Student { Id = 5, Name = "Shiv", City = "Motihari" },
                new Student { Id = 6, Name = "Rajesh Singh", City = "Dhanbad" },
                new Student { Id = 7, Name = "Staya", City = "Bokaro" },
                new Student { Id = 8, Name = "Samiran", City = "Chas" },
                new Student { Id = 9, Name = "Rajnish", City = "Bokaro" },
                new Student { Id = 10, Name = "Mahtab", City = "Dhanbad" }
            };
        }

        [Route("api/student", Name = "StudentList")]
        public IHttpActionResult Get(string sort = "id", int page = 1, int pageSize = 5)
        {
            // Get data
            var data = db.AsQueryable();

            // Apply sorting
            data = data.ApplySort(sort);

            // Limit max page size in response to 4
            if(pageSize > 4)
            {
                pageSize = 4;
            }

            // Calculate totalCount and totalPage may have in database
            var totalCount = data.Count();
            var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);

            // Generate previous page and next page Urls
            var urlHelper = new UrlHelper(Request);
            var previousPageLink = page > 1 ? urlHelper.Link("StudentList",
                new {
                    page = page - 1,
                    pageSize = pageSize,
                    sort = sort
                    // pass other query string vriables if any
                }) : "";
            var nextPageLink = page < totalPages ? urlHelper.Link("StudentList", 
                new {
                    page = page + 1,
                    pageSize = pageSize,
                    sort = sort
                    // pass other query string vriables if any
                }) : "";

            // Create response header
            var pagingResponseHeader = new
            {
                currentPage = page,
                pageSize = pageSize,
                totalCount = totalCount,
                totalPages = totalPages,
                previousPageLink = previousPageLink,
                nextPageLink = nextPageLink
            };

            // Add paging stuff in HTTP response header
            HttpContext.Current.Response.Headers.Add("Paging", Newtonsoft.Json.JsonConvert.SerializeObject(pagingResponseHeader));

            return Ok(data.Skip(pageSize * (page -1 )).Take(pageSize));
        }

    }
}
