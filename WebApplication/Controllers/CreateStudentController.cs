using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using WebApplication.Models;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/students")]
    public class CreateStudentController : ApiController
    {
        [HttpPost]
        [Route("{ID}/{FirstName}/{LastName}")]
        public IHttpActionResult Post([FromBody] CreateStudentModel createStudentModelBody, string ID, string FirstName, string LastName)
        {
            try
            {
                if(createStudentModelBody == null)
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The request body was empty.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "The request body was empty.");
                }

                // If a non-nullable value is null or empty, return bad request. These fields must be filled out.
                if (string.IsNullOrEmpty(createStudentModelBody.Program) || string.IsNullOrEmpty(createStudentModelBody.SchoolEmail) || 
                    string.IsNullOrEmpty(createStudentModelBody.YearOfAdmission))
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. A field that is non-nullable was not given a value in the API call. For the request to succeed, the Program, SchoolEmail, and YearOfAdmission have to be specified.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "A field that is non-nullable was not given a value in the API call. For the request to succeed, the Program, SchoolEmail, and YearOfAdmission have to be specified.");
                }

                FindByID info = new FindByID();
                StudentDataModel student = info.Find<StudentDataModel>(ID);
                if (string.IsNullOrEmpty(student.ID))
                {

                    FindByEmail emailInfo = new FindByEmail();
                    StudentDataModel studentEmailModel = emailInfo.Find<StudentDataModel>(createStudentModelBody.SchoolEmail);
                    if (string.IsNullOrEmpty(studentEmailModel.SchoolEmail))
                    {
                        bool Graduated = false;
                        student = new StudentDataModel()
                        {
                            ID = ID,
                            FirstName = FirstName,
                            LastName = LastName,
                            Program = createStudentModelBody.Program,
                            SchoolEmail = createStudentModelBody.SchoolEmail,
                            YearOfAdmission = createStudentModelBody.YearOfAdmission,
                            Classes = createStudentModelBody.Classes,
                            Graduated = Graduated
                        };
                        CreateStudent createStudentProxy = new CreateStudent();
                        bool success = createStudentProxy.Create(student);
                        if (!success)
                        {
                            Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The student could not be created.");
                            return Content(System.Net.HttpStatusCode.InternalServerError, "The student could not be created.");
                        }
                        student = info.Find<StudentDataModel>(ID);
                        Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/student/search/{ID}.");
                        return Ok(student);
                    }
                    else
                    {
                        Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. A student already exists with the requested email {createStudentModelBody.SchoolEmail}.");
                        return Content(System.Net.HttpStatusCode.NotFound, $"A student already exists with the requested email {createStudentModelBody.SchoolEmail}.");
                    }
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. A student already exists with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.Conflict, $"A student already exists with the requested ID {ID}.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.Conflict, e);
            }
        }
    }
}