using System;
using System.Data;
using System.Web.Http;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/teachers")]
    public class TeacherSearchController : ApiController
    {
        [HttpGet]
        [Route("{ID}")]
        public IHttpActionResult Get(string ID)
        {
            try
            {
                FindByID info = new FindByID();
                TeacherDataModel teacher = info.Find<TeacherDataModel>(ID);
                if (teacher.ID != null && teacher.ID != "")
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/teacher/search/{ID}.");
                    return Ok(teacher);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint GET api/teacher/search/{ID}. No teacher found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No teacher found with the requested ID {ID}.");
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