using System;
using System.Net;
using System.Web.Http;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/students")]
    public class DeleteStudentController: ApiController
    {
        [HttpDelete]
        [Route("{ID}/delete")]
        public IHttpActionResult DeleteStudent(string ID)
        {
            try
            {
                DeleteStudent info = new DeleteStudent();

                // Check if the table Students contains a student with the requested ID
                FindByID find = new FindByID();
                var student = find.Find<StudentDataModel>(ID);
                if (student.ID == null || student.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/students/{ID}. No student found with the requested ID {ID}.");
                    return Content(HttpStatusCode.NotFound, $"No student found with the requested ID {ID}.");
                }

                var inst = info.Delete(ID);
                if (inst)
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint DELETE api/students/{ID}");
                    return Ok($"Request successful. The student with ID {ID} has been deleted");
                }
                else
                {
                    // Error message
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/students/{ID}");
                    return Content(HttpStatusCode.InternalServerError, $"Request failed for endpoint DELETE api/students/{ID}");
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