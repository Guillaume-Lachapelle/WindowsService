using System;
using System.Web.Http;
using System.Threading.Tasks;
using WindowsService.Helpers;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetStudents()
        {
            try
            {
                RetrieveStudents info = new RetrieveStudents();
                var inst = info.RetrieveTableContents();
                if (inst.Count != 0)
                {
                    Logger.MonitoringLogger.Info("Request successful for endpoint GET api/students");
                    return Ok(inst);
                }
                else
                {
                    Logger.MonitoringLogger.Info("Request successful for endpoint GET api/students");
                    return Content(System.Net.HttpStatusCode.OK, "The table Students does not contain any elements.");
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