using System;
using System.Web.Http;
using WindowsService.Helpers;
using WindowsService.Models;
using WindowsService.Checks;
using WebApplication.Models;
using System.Net;

namespace WebApplication.Controllers
{
    /// <summary>
    /// Controller for managing students.
    /// </summary>
    [RoutePrefix("api/students")]
    public class StudentsController : ApiController
    {

        #region Proxies

        private readonly CheckID _checkIDProxy = new CheckID();
        private readonly CheckEmail _checkEmailProxy = new CheckEmail();
        private readonly RetrieveTable _retrieveTableProxy = new RetrieveTable();
        private readonly CreationHandler _creationHandlerProxy = new CreationHandler();
        private readonly DeletionHandler _deletionHandlerProxy = new DeletionHandler();
        private readonly EditHandler _editHandlerProxy = new EditHandler();
        private readonly FindByID _findByIDProxy = new FindByID();

        #endregion


        #region Methods

        /// <summary>
        /// Retrieves all students.
        /// </summary>
        /// <returns>A list of students.</returns>
        [HttpGet]
        [Route("")]
        public IHttpActionResult GetStudents()
        {
            try
            {
                var students = _retrieveTableProxy.RetrieveTableContents<StudentDataModel>();
                if (students.Count != 0)
                {
                    Logger.MonitoringLogger.Info("Request successful for endpoint GET api/students");
                    return Ok(students);
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
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Retrieves a student by ID.
        /// </summary>
        /// <param name="ID">The ID of the student.</param>
        /// <returns>The student data.</returns>
        [HttpGet]
        [Route("{ID}")]
        public IHttpActionResult Get(string ID)
        {
            try
            {
                StudentDataModel student = _findByIDProxy.Find<StudentDataModel>(ID);
                if (student.ID != null && student.ID != "")
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/students/search/{ID}.");
                    return Ok(student);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint GET api/students/search/{ID}. No student found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No student found with the requested ID {ID}.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Creates a new student.
        /// </summary>
        /// <param name="createStudentModelBody">The student data model.</param>
        /// <param name="ID">The ID of the student.</param>
        /// <param name="FirstName">The first name of the student.</param>
        /// <param name="LastName">The last name of the student.</param>
        /// <returns>The created student.</returns>
        [HttpPost]
        [Route("{ID}/{FirstName}/{LastName}")]
        public IHttpActionResult Post([FromBody] CreateStudentModel createStudentModelBody, string ID, string FirstName, string LastName)
        {
            try
            {
                if (createStudentModelBody == null)
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
                if (!_checkIDProxy.ValidFormat<StudentDataModel>(ID))
                {
                    Logger.MonitoringLogger.Error("Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The ID was not in the right format. The ID needs to be a string of 8 numbers.");
                    return Content(System.Net.HttpStatusCode.BadRequest, "The ID was not in the right format. The ID needs to be a string of 8 numbers.");
                }

                if (_checkIDProxy.CanCreate<StudentDataModel>(ID))
                {

                    if (_checkEmailProxy.Check(createStudentModelBody.SchoolEmail))
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

                        bool success = _creationHandlerProxy.Create<StudentDataModel>(student);
                        if (!success)
                        {
                            Logger.MonitoringLogger.Error($"Request failed for endpoint POST api/students/{ID}/{FirstName}/{LastName}. The student could not be created.");
                            return Content(System.Net.HttpStatusCode.InternalServerError, "The student could not be created.");
                        }

                        student = _findByIDProxy.Find<StudentDataModel>(ID);
                        Logger.MonitoringLogger.Info($"Request successful for endpoint GET api/students/{ID}/{FirstName}/{LastName}.");
                        return Created($"api/students/{ID}/{FirstName}/{LastName}", student);
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

        /// <summary>
        /// Updates an existing student.
        /// </summary>
        /// <param name="studentModelBody">The student data model.</param>
        /// <param name="ID">The ID of the student.</param>
        /// <returns>The updated student.</returns>
        [HttpPut]
        [Route("{ID}")]
        public IHttpActionResult Put([FromBody] EditStudentModel studentModelBody, string ID)
        {
            try
            {
                // Check if the table Students contains a student with the requested ID
                var student = _findByIDProxy.Find<StudentDataModel>(ID);
                if (student.ID == null || student.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint PUT api/students/{ID}. No student found with the requested ID {ID}.");
                    return Content(System.Net.HttpStatusCode.NotFound, $"No student found with the requested ID {ID}.");
                }

                StudentDataModel studentDataModel = new StudentDataModel()
                {
                    ID = ID,
                    FirstName = studentModelBody.FirstName,
                    LastName = studentModelBody.LastName,
                    Program = studentModelBody.Program,
                    SchoolEmail = studentModelBody.SchoolEmail,
                    YearOfAdmission = studentModelBody.YearOfAdmission,
                    Classes = studentModelBody.Classes,
                    Graduated = studentModelBody.Graduated
                };

                var success = _editHandlerProxy.Edit<StudentDataModel>(ID, studentDataModel);
                if (success)
                {
                    Logger.MonitoringLogger.Info($"Request successful for endpoint PUT api/students/{ID}");
                    return Created($"api/students/{ID}", studentDataModel);
                }
                else
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint PUT api/students/{ID}");
                    return Content(System.Net.HttpStatusCode.InternalServerError, "The student could not be updated.");
                }
            }
            catch (Exception e)
            {
                Logger.MonitoringLogger.Error(e.Message);
                return Content(System.Net.HttpStatusCode.InternalServerError, e);
            }
        }

        /// <summary>
        /// Deletes a student by ID.
        /// </summary>
        /// <param name="ID">The ID of the student to delete.</param>
        /// <returns>A confirmation message.</returns>
        [HttpDelete]
        [Route("{ID}")]
        public IHttpActionResult Delete(string ID)
        {
            try
            {
                // Check if the table Students contains a student with the requested ID
                var student = _findByIDProxy.Find<StudentDataModel>(ID);
                if (student.ID == null || student.ID == "")
                {
                    Logger.MonitoringLogger.Error($"Request failed for endpoint DELETE api/students/{ID}. No student found with the requested ID {ID}.");
                    return Content(HttpStatusCode.NotFound, $"No student found with the requested ID {ID}.");
                }

                var success = _deletionHandlerProxy.Delete<StudentDataModel>(ID);
                if (success)
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

        #endregion
    }
}