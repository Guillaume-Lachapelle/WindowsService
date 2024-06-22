using System;
using System.Web.Http;
using System.Threading.Tasks;
using WindowsService.Helpers;
using WindowsService.Checks;
using WindowsService.Models;
using WebApplication.Models;
using System.Net;

namespace WebApplication.Controllers
{
    [RoutePrefix("api/teachers")]
    public class TeachersController : ApiController
    {
        #region Proxies

        CheckID checkID = new CheckID();
        CheckEmail checkEmail = new CheckEmail();
        RetrieveTable retrieveTableProxy = new RetrieveTable();
        CreateTeacher createTeacherProxy = new CreateTeacher();
        DeleteTeacher deleteTeacherProxy = new DeleteTeacher();
        EditTeacher editTeacherProxy = new EditTeacher();
        FindByID findByIDProxy = new FindByID();

        #endregion


        #region Methods

        /// <summary>
        /// Retrieves all teachers.
        /// </summary>
        /// <returns>A list of teachers.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetTeachers()
        {
            try
            {
                var inst = retrieveTableProxy.RetrieveTableContents<TeacherDataModel>();
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
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Retrieves a teacher by ID.
        /// </summary>
        /// <param name="ID">The ID of the teacher.</param>
        /// <returns>The teacher data.</returns>
        [HttpGet]
        [Route("{ID}")]
        public IHttpActionResult Get(string ID)
        {
            try
            {
                TeacherDataModel teacher = findByIDProxy.Find<TeacherDataModel>(ID);
                if (teacher.ID != null && teacher.ID != "")
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/teachers/search/{ID}.");
                    return Ok(teacher);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint GET api/teachers/search/{ID}. No teacher found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No teacher found with the requested ID {ID}.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Creates a new teacher.
        /// </summary>
        /// <param name="createTeacherModelBody">The teacher data model.</param>
        /// <param name="ID">The ID of the teacher.</param>
        /// <param name="FirstName">The first name of the teacher.</param>
        /// <param name="LastName">The last name of the teacher.</param>
        /// <returns>The created teacher.</returns>
        [HttpPost]
        [Route("{ID}/{FirstName}/{LastName}")]
        public IHttpActionResult Post([FromBody] CreateTeacherModel createTeacherModelBody, string ID, string FirstName, string LastName)
        {
            try
            {
                if (createTeacherModelBody == null)
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
                        return Created($"api/teachers/{ID}/{FirstName}/{LastName}", teacher);
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

        /// <summary>
        /// Updates an existing teacher.
        /// </summary>
        /// <param name="teacherModelBody">The teacher data model.</param>
        /// <param name="ID">The ID of the teacher.</param>
        /// <returns>The updated teacher.</returns>
        [HttpPut]
        [Route("{ID}")]
        public IHttpActionResult Put([FromBody] EditTeacherModel teacherModelBody, string ID)
        {
            try
            {
                // Check if the table teachers contains a teacher with the requested ID
                var teacher = findByIDProxy.Find<TeacherDataModel>(ID);
                if (teacher.ID == null || teacher.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint PUT api/teachers/{ID}. No teacher found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No teacher found with the requested ID {ID}.");
                }

                TeacherDataModel teacherDataModel = new TeacherDataModel()
                {
                    ID = ID,
                    FirstName = teacherModelBody.FirstName,
                    LastName = teacherModelBody.LastName,
                    Department = teacherModelBody.Department,
                    Salary = teacherModelBody.Salary,
                    HiringDate = teacherModelBody.HiringDate,
                    SchoolEmail = teacherModelBody.SchoolEmail,
                    Classes = teacherModelBody.Classes
                };

                var inst = editTeacherProxy.Edit(ID, teacherDataModel);
                if (inst)
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint PUT api/teachers/{ID}");
                    return Created($"api/teachers/{ID}", teacherDataModel);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint PUT api/teachers/{ID}");
                    return Content(System.Net.HttpStatusCode.InternalServerError, "The teacher could not be updated.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Deletes a teacher by ID.
        /// </summary>
        /// <param name="ID">The ID of the teacher to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete]
        [Route("{ID}/delete")]
        public IHttpActionResult Delete(string ID)
        {
            try
            {
                // Check if the table Teachers contains a teacher with the requested ID
                FindByID find = new FindByID();
                var teacher = find.Find<TeacherDataModel>(ID);
                if (teacher.ID == null || teacher.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/teachers/{ID}. No teacher found with the requested ID {ID}.");
                    return Content(HttpStatusCode.NotFound, $"No teacher found with the requested ID {ID}.");
                }

                var inst = deleteTeacherProxy.Delete(ID);
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

        #endregion
    }
}