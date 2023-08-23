using System;
using System.Net;
using System.Web.Http;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/teachers")]
    public class DeleteTeacherController: ApiController
    {
        [HttpDelete]
        [Route("{ID}/delete")]
        public IHttpActionResult Delete(string ID)
        {
            try
            {
                DeleteTeacher info = new DeleteTeacher();

                // Check if the table Teachers contains a teacher with the requested ID
                FindByID find = new FindByID();
                var teacher = find.Find<TeacherDataModel>(ID);
                if (teacher.ID == null || teacher.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/teachers/{ID}. No teacher found with the requested ID {ID}.");
                    return Content(HttpStatusCode.NotFound, $"No teacher found with the requested ID {ID}.");
                }

                var inst = info.Delete(ID);
                if (inst)
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint DELETE api/teachers/{ID}");
                    return Ok($"Request successful. The teacher with ID {ID} has been deleted");
                }
                else
                {
                    // Error message
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/teachers/{ID}");
                    return Content(HttpStatusCode.InternalServerError, $"Request failed for endpoint DELETE api/teachers/{ID}");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(HttpStatusCode.InternalServerError, e);
            }
        }
    }
}