using System;
using System.Web.Http;
using System.Threading.Tasks;
using WindowsService.Scripts;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/teachers")]
    public class TeachersController : ApiController
    {
        [HttpGet]
        [Route("")]
        public async Task<IHttpActionResult> GetTeachers()
        {
            try
            {
                RetrieveTeachersTable info = new RetrieveTeachersTable();
                var inst = info.RetrieveTableContents();
                if (inst.Count != 0)
                {
                    Logger.MonitoringLogger.Info("Request successful for endpoint GET api/teachers");
                    return Ok(inst);
                }
                else
                {
                    Logger.MonitoringLogger.Info("Request successful for endpoint GET api/teachers");
                    return Content(System.Net.HttpStatusCode.OK, "The table Teachers does not contain any elements.");
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