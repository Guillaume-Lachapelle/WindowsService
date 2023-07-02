using System;
using System.Web.Http;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/student/search")]
    public class StudentSearchController : ApiController
    {
        [HttpGet]
        [Route("{ID}")]
        public IHttpActionResult Get(string ID)
        {
            try
            {
                RetrieveStudentsTable info = new RetrieveStudentsTable();
                StudentDataModel student = info.FindStudent(ID);
                if (student.ID != null)
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/student/search/{ID}.");
                    return Ok(student);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint GET api/student/search/{ID}. No student found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No student found with the requested ID {ID}.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.ServiceUnavailable, e);
            }
        }
    }
}