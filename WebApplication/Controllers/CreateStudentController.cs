using System;
using System.Web.Http;
using WebApplication.Models;
using WindowsService.Checks;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/students")]
    public class CreateStudentController : ApiController
    {

        #region Proxies

        CheckID checkID = new CheckID();
        CheckEmail checkEmail = new CheckEmail();
        CreateStudent createStudentProxy = new CreateStudent();
        FindByID findByIDProxy = new FindByID();

        #endregion


        #region Methods

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

                // If the ID doesn't have the right format, return bad request.
                if (!checkID.ValidFormat<StudentDataModel>(ID))
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The ID was not in the right format. The ID needs to be a string of 8 numbers.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "The ID was not in the right format. The ID needs to be a string of 8 numbers.");
                }

                if (checkID.CanCreate<StudentDataModel>(ID))
                {

                    if (checkEmail.Check(createStudentModelBody.SchoolEmail))
                    {
                        bool Graduated = false;
                        StudentDataModel student = new StudentDataModel()
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
                        
                        bool success = createStudentProxy.Create(student);
                        if (!success)
                        {
                            Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The student could not be created.");
                            return Content(System.Net.HttpStatusCode.InternalServerError, "The student could not be created.");
                        }
                        
                        student = findByIDProxy.Find<StudentDataModel>(ID);
                        Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/student/{ID}/{FirstName}/{LastName}.");
                        return Ok(student);
                    }
                    else
                    {
                        Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The requested email {createStudentModelBody.SchoolEmail} already exists or does not have a valid format @university.com.");
                        return Content(System.Net.HttpStatusCode.NotFound, $"The requested email {createStudentModelBody.SchoolEmail} already exists or does not have a valid format @university.com.");
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

        #endregion
    }
}