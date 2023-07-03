using System;
using System.Web.Http;
using WebApplication.Models;
using WindowsService.Checks;
using WindowsService.Helpers;
using WindowsService.Models;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/teachers")]
    public class CreateTeacherController : ApiController
    {

        #region Proxies

        CheckID checkID = new CheckID();
        CheckEmail checkEmail = new CheckEmail();
        CreateTeacher createTeacherProxy = new CreateTeacher();
        FindByID findByIDProxy = new FindByID();

        #endregion


        #region Methods

        [HttpPost]
        [Route("{ID}/{FirstName}/{LastName}")]
        public IHttpActionResult Post([FromBody] CreateTeacherModel createTeacherModelBody, string ID, string FirstName, string LastName)
        {
            try
            {
                if(createTeacherModelBody == null)
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. The request body was empty.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "The request body was empty.");
                }

                // If a non-nullable value is null or empty, return bad request. These fields must be filled out.
                if (string.IsNullOrEmpty(createTeacherModelBody.Salary) || string.IsNullOrEmpty(createTeacherModelBody.HiringDate) || 
                    string.IsNullOrEmpty(createTeacherModelBody.SchoolEmail))
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. A field that is non-nullable was not given a value in the API call. For the request to succeed, the Salary, HiringDate, and SchoolEmail have to be specified.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "A field that is non-nullable was not given a value in the API call. For the request to succeed, the Salary, HiringDate, and SchoolEmail have to be specified.");
                }

                // If the ID doesn't have the right format, return bad request.
                if (!checkID.ValidFormat<TeacherDataModel>(ID))
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. The ID was not in the right format. The ID needs to be start with 'T' followed by 8 numbers.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "The ID was not in the right format. The ID needs to be start with 'T' followed by 8 numbers.");
                }

                if (checkID.CanCreate<TeacherDataModel>(ID))
                {

                    if (checkEmail.Check(createTeacherModelBody.SchoolEmail))
                    {
                        TeacherDataModel teacher = new TeacherDataModel()
                        {
                            ID = ID,
                            FirstName = FirstName,
                            LastName = LastName,
                            Department = createTeacherModelBody.Department,
                            Salary = createTeacherModelBody.Salary,
                            HiringDate = createTeacherModelBody.HiringDate,
                            SchoolEmail = createTeacherModelBody.SchoolEmail,
                            Classes = createTeacherModelBody.Classes
                        };

                        bool success = createTeacherProxy.Create(teacher);
                        if (!success)
                        {
                            Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. The teacher could not be created.");
                            return Content(System.Net.HttpStatusCode.InternalServerError, "The teacher could not be created.");
                        }

                        teacher = findByIDProxy.Find<TeacherDataModel>(ID);
                        Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/teachers/{ID}/{FirstName}/{LastName}.");
                        return Ok(teacher);
                    }
                    else
                    {
                        Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. The requested email {createTeacherModelBody.SchoolEmail} already exists or does not have a valid format @university.com.");
                        return Content(System.Net.HttpStatusCode.NotFound, $"The requested email {createTeacherModelBody.SchoolEmail} already exists or does not have a valid format @university.com.");
                    }
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/teachers/{ID}/{FirstName}/{LastName}. A teacher already exists with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.Conflict, $"A teacher already exists with the requested ID {ID}.");
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