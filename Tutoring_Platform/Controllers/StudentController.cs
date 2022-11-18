using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;

namespace Tutoring_Platform.Controllers
{
    /// <summary>
    /// Any API Calls made on student side are handled in the student controller. Any common API
    /// calls made on student and tutor side are also addressed on the student controller.
    /// </summary>
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private tutoringContext db;
        public StudentController(tutoringContext dbModel)
        {
            db = dbModel;
        }

        /// <summary>
        /// Find the role of the user logged into the application
        /// </summary>
        /// <param name="email">Email of the user logged in</param>
        /// <returns>The role (student or tutor)</returns>
        [Route("FindRole")]
        [HttpPost]
        public string FindRole([FromBody] string email)
        {
            string role = "";
            try
            {
                IEnumerable<string> userRole = from u in db.Users
                                               where u.Email == email
                                               select u.Role;

                if (userRole != null)
                {
                    role = userRole.First();
                }
                return role;
            }
            catch
            {
                return role;
            }
            
        }

        /// <summary>
        /// Checks in the database if the logged in user's record has been created in the StudTutorInfo table
        /// </summary>
        /// <param name="userId">The id of the user logged in</param>
        /// <returns>Returns 0 or 1 where 0 means the record is not created and 1 means that record is created</returns>
        [Route("UserCreated")]
        [HttpPost]
        public int UserCreated([FromBody] string userId)
        {
            
            int showDialog = 0;
            try
            {
                int id = Convert.ToInt32(userId);
                IEnumerable<int> getId = (from sti in db.StudTutorInfos
                                          where sti.UserId == id
                                          select sti.Id);
                System.Diagnostics.Debug.WriteLine(getId);
                if (getId.Count() < 1)
                {
                    showDialog = 1;
                }
                return showDialog;
            }
            catch
            {
                return showDialog;
            }
            
        }

        /// <summary>
        /// Creates a student and tutor record in the StudTutorInfo and TutorInfo tables
        /// </summary>
        /// <param name="studentParams">The data submitted by user related to different info fields of StudTutorInfo and TutorInfo tables</param>
        /// <returns>Success or Failure message</returns>
        [Route("CreateStudent")]
        [HttpPost]
        public string CreateStudent([FromBody] ProfileData studentParams)
        {
            string response = "";
            try
            {
                if (studentParams.Name != null && studentParams.Address != null && studentParams.City != null && studentParams.Postal != null &&
                studentParams.Province != null && studentParams.School != null && studentParams.Field != null && studentParams.Program != null &&
                studentParams.Semester != null && studentParams.UserId != null && studentParams.Role != null)
                {
                    IEnumerable<User> getUser = from u in db.Users
                                                where u.Id == Convert.ToInt32(studentParams.UserId)
                                                select u;
                    foreach (User user in getUser)
                    {
                        user.Name = studentParams.Name;
                    }
                    try
                    {
                        db.SaveChanges();
                    }
                    catch { }

                    StudTutorInfo studentTutor = new StudTutorInfo
                    {
                        UserId = Convert.ToInt32(studentParams.UserId),
                        Role = studentParams.Role,
                        Address = studentParams.Address,
                        City = studentParams.City,
                        Province = studentParams.Province,
                        PostalCode = studentParams.Postal,
                        School = studentParams.School,
                        StudyField = studentParams.Field,
                        Program = studentParams.Program,
                        Semester = Convert.ToInt32(studentParams.Semester)
                    };
                    db.StudTutorInfos.Add(studentTutor);
                    db.SaveChanges();

                    if (studentParams.Role == "tutor")
                    {
                        int getStudId = (from sti in db.StudTutorInfos
                                            where sti.UserId == Convert.ToInt32(studentParams.UserId)
                                            select sti.Id).First();
                        string status = "";
                        if (Convert.ToInt32(studentParams.Semester) == 0)
                        {
                            status = "Graduate";
                        }
                        else
                        {
                            status = "Semester " + studentParams.Semester + " student";
                        }
                        TutorInfo tutor = new TutorInfo
                        {
                            UserId = getStudId,
                            Status = status
                        };
                        db.TutorInfos.Add(tutor);
                        db.SaveChanges();


                        int tutorId = (from ti in db.TutorInfos
                                         where ti.UserId == getStudId
                                         select ti.Id).First();
                        
                        DaysAvailable days = new DaysAvailable
                        {
                            UserId = tutorId,
                            Sunday = 0,
                            Monday = 0,
                            Tuesday = 0,
                            Wednesday = 0,
                            Thursday = 0,
                            Friday = 0,
                            Saturday = 0
                        };
                        db.DaysAvailables.Add(days);
                        db.SaveChanges();

                        TutorCourse course1 = new TutorCourse
                        {
                            TutorId = tutorId,
                            Course = ""
                        };
                        db.TutorCourses.Add(course1);
                        db.SaveChanges();

                        TutorCourse course2 = new TutorCourse
                        {
                            TutorId = tutorId,
                            Course = ""
                        };
                        db.TutorCourses.Add(course2);
                        db.SaveChanges();

                        TutorCourse course3 = new TutorCourse
                        {
                            TutorId = tutorId,
                            Course = ""
                        };
                        db.TutorCourses.Add(course3);
                        db.SaveChanges();
                    }
                    response = "Profile Created";
                    
                }
                return response;
            }
            catch
            {
                response = "Could not create profile";
                return response;
            }
        }


        /// <summary>
        /// When student submits the form to search for tutors, then this method sends the matching tutors back to frontend
        /// by making a call another method (searchTutors)
        /// </summary>
        /// <param name="tutorParams">The course name and available days data submitted by student</param>
        /// <returns>JSON object list of all the tutors that match with the student conditions</returns>
        [Route("SearchTutors")]
        [HttpPost]
        public string SearchTutors([FromBody] LookForTutorParam tutorParams)
        {
            string jsonResults = "";
            try
            {
                string[,] allTutors = new string[1, 1];
                if (tutorParams.CourseName != null && tutorParams.Days != null)
                {
                    allTutors = searchTutors(tutorParams.CourseName, tutorParams.Days, tutorParams.UserId);
                    List<SearchTutorsReturn> results = new List<SearchTutorsReturn>();
                    for (int i = 0; i < allTutors.GetLength(0); i++)
                    {
                        results.Add(new SearchTutorsReturn
                        {
                            studId = allTutors[i, 1],
                            Name = allTutors[i, 0],
                            School = allTutors[i, 2],
                            Program = allTutors[i, 3],
                            Status = allTutors[i, 4],
                            Wage = allTutors[i, 5],
                            tutorId = allTutors[i, 6],
                            CourseName = tutorParams.CourseName,
                            Days = tutorParams.Days
                        });
                    }
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch{
                jsonResults = "Invalid values entered!";
                return jsonResults;
            }
        }

        /// <summary>
        /// Searches for the tutors in the database by checking the courses that they teach in TutorCourse table and
        /// the days that they are available in DaysAvailable table
        /// </summary>
        /// <param name="course">Course name submitted by student</param>
        /// <param name="days">Days on which the student is available</param>
        /// <param name="userId">The user id of the student logged in</param>
        /// <returns>An array of the matching tutors data</returns>
        private string[,] searchTutors(string course, int[] days, int userId)
        {
            IEnumerable<TutorCourse> tutorCourses = (from t in db.TutorCourses
                                                     where t.Course.Contains(course)
                                                     select t).ToList();
            IEnumerable<TutorInfo> tutor = (from t in db.TutorInfos select t).ToList();
            IEnumerable<StudTutorInfo> stud_Tutor_Info = (from sti in db.StudTutorInfos select sti).ToList<StudTutorInfo>();
            IEnumerable<User> user_ = (from u in db.Users select u).ToList();
            List<DaysAvailable> daysAvailable = new List<DaysAvailable>();
            int studentDays = 0;
            foreach (int day in days)
            {
                if (day == 1)
                {
                    studentDays++;
                }
            }
            foreach (DaysAvailable d in db.DaysAvailables)
            {
                int count = studentDays;
                if (days[0] == 1 && d.Sunday == 1)
                {
                    count--;
                }
                if (days[1] == 1 && d.Monday == 1)
                {
                    count--;
                }
                if (days[2] == 1 && d.Tuesday == 1)
                {
                    count--;
                }
                if (days[3] == 1 && d.Wednesday == 1)
                {
                    count--;
                }
                if (days[4] == 1 && d.Thursday == 1)
                {
                    count--;
                }
                if (days[5] == 1 && d.Friday == 1)
                {
                    count--;
                }
                if (days[6] == 1 && d.Saturday == 1)
                {
                    count--;
                }
                if (count == 0)
                {
                    daysAvailable.Add(d);
                }
            }

            IEnumerable<TutorInfo> tutorInfoList = (from ti in tutor
                                                    join tc in tutorCourses on ti.Id equals tc.TutorId
                                                    select ti).ToList();
            IEnumerable<TutorInfo> tutorInfo = (from ti in tutorInfoList
                                                join da in daysAvailable on ti.Id equals da.UserId
                                                select ti).ToList();

            IEnumerable<StudTutorInfo> studTutorInfo = (from sti in stud_Tutor_Info
                                                        join ti in tutorInfo on sti.Id equals ti.UserId
                                                        select sti).ToList();

            IEnumerable<User> user = from u in user_
                                     join sti in studTutorInfo on u.Id equals sti.UserId
                                     select u;

            IEnumerable<StudTutorInfo> studTutorInfo2 = (from sti in stud_Tutor_Info
                                                         where sti.UserId == userId
                                                         select sti).ToList();
            string[,] returnValue = new string[user.Count(), 7];

            int i = 0;
            foreach (var u in user)
            {
                if (u.Name != null)
                {
                    returnValue[i, 0] = u.Name.ToString();
                }
                i += 1;
            }
            i = 0;
            foreach (var s in studTutorInfo)
            {
                returnValue[i, 1] = studTutorInfo2.First().Id.ToString();
                if (s.School != null && s.Program != null)
                {
                    returnValue[i, 2] = s.School.ToString();
                    returnValue[i, 3] = s.Program.ToString();
                }
                i += 1;
            }
            i = 0;

            foreach (var t in tutorInfo)
            {
                if (t.Status != null && t.Wage != null)
                {
                    returnValue[i, 4] = t.Status.ToString();
                    returnValue[i, 5] = t.Wage.ToString();
                }
                returnValue[i, 6] = t.Id.ToString();
                i += 1;
            }

            return returnValue;
        }

        /// <summary>
        /// When the student clicks the buttons to send tutoring request to a tutors then this method is called
        /// It creates a record about the request in AppointRequest table
        /// </summary>
        /// <param name="requestParam">The parameters required to create a new row in AppointRequest table</param>
        /// <returns>Success or failure message</returns>
        [Route("SendTutorRequest")]
        [HttpPost]
        public string sendTutorRequest([FromBody] sendTutorRequestParam requestParam)
        {
            string jsonResults = "";
            if (requestParam.CourseName != null && requestParam.Days != null && requestParam.studId != null && requestParam.tutorId != null)
            {
                AppointRequest aRequest = new AppointRequest
                {
                    TutorId = Convert.ToInt32(requestParam.tutorId),
                    StudId = Convert.ToInt32(requestParam.studId),
                    Course = requestParam.CourseName,
                    Sunday = requestParam.Days[0],
                    Monday = requestParam.Days[1],
                    Tuesday = requestParam.Days[2],
                    Wednesday = requestParam.Days[3],
                    Thursday = requestParam.Days[4],
                    Friday = requestParam.Days[5],
                    Saturday = requestParam.Days[6],
                    Message = requestParam.Message
                };

                db.AppointRequests.Add(aRequest);
                try
                {
                    db.SaveChanges();
                    jsonResults = "Request sent";
                }
                catch
                {
                    jsonResults = "Could not send Request";
                }
            }
            return jsonResults;
        }

        /// <summary>
        /// Any requests that has been accepted by the tutor and for which the tutor has submitted time slots
        /// are retreived from the AppointSlots table
        /// </summary>
        /// <param name="userId">The id of the user logged in</param>
        /// <returns>The JSON Object list of the accepted requests with time slots</returns>
        [Route("DisplayRequests")]
        [HttpPost]
        public string DisplayRequests([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
                IEnumerable<int> studId = from sti in db.StudTutorInfos
                                          where sti.UserId == user
                                          select sti.Id;
                var requestsMade = (from ar in db.AppointRequests
                                    where ar.StudId == studId.First()
                                    select new { ar.Id, ar.Course, ar.TutorId }).ToArray();
                List<DisplayStudRequestsReturn> results = new List<DisplayStudRequestsReturn>();
                for (int i = 0; i < requestsMade.Count(); i++)
                {
                    IEnumerable<string> getRequestTutorName = from ti in db.TutorInfos
                                                              where ti.Id == requestsMade[i].TutorId
                                                              select ti.User.User.Name;
                    string courseName = requestsMade[i].Course;
                    var slots = (from asl in db.AppointSlots
                                 where asl.RequestId == requestsMade[i].Id
                                 select new { asl.Id, asl.Slot,asl.Message}).ToArray();
                    if(slots.Length > 4)
                    {
                        var slot1 = slots[0].Slot;
                        var slot2 = slots[1].Slot;
                        var slot3 = slots[2].Slot;
                        var slot4 = slots[3].Slot;
                        var slot5 = slots[4].Slot;
                        results.Add(new DisplayStudRequestsReturn
                        {
                            Name = getRequestTutorName.First(),
                            CourseName = courseName,
                            Slot1 = slot1,
                            Slot2 = slot2,
                            Slot3 = slot3,
                            Slot4 = slot4,
                            Slot5 = slot5,
                            Id1 = slots[0].Id,
                            Id2 = slots[1].Id,
                            Id3 = slots[2].Id,
                            Id4 = slots[3].Id,
                            Id5 = slots[4].Id,
                            Message = slots[0].Message
                        });
                    }
                }
                jsonResults = JsonConvert.SerializeObject(results);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }

        /// <summary>
        /// When student picks one slot out of the all the slots sent by tutor, the this method is called. It updates
        /// that one selected slot row in the database by setting the selected field to true
        /// </summary>
        /// <param name="slot">The selected slot's id</param>
        /// <returns>Success or failure message that tells the user if the slot was successfully selected.</returns>
        [Route("SendConfirmedSlot")]
        [HttpPost]
        public string SendConfirmedSlot([FromBody] string slot)
        {
            try
            {
                int slotId = Convert.ToInt32(slot);
                if (slotId > 0)
                {
                    IEnumerable<AppointSlot> appointSlots = from asl in db.AppointSlots
                                                            where asl.Id == slotId
                                                            select asl;
                    foreach (AppointSlot appointSlot in appointSlots)
                    {
                        appointSlot.Selected = true;
                    }
                    db.SaveChanges();
                    return "Slot Selected";
                }
                return "Slot Selected";
            }
            catch
            {
                return "Slot could not be Selected";
            }
        }

        /// <summary>
        /// Gets the confirmed appointments from the database table AppointConfirms for which the tutor has sent paypal link and meeting link
        /// </summary>
        /// <param name="userId">The id of the studnet logged in</param>
        /// <returns>JSON Object list of the confirmed appointments data</returns>
        [Route("GetAppointments")]
        [HttpPost]
        public string GetAppointments([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
                List<GetAppointments> results = new List<GetAppointments>();
                IEnumerable<int> studId = from sti in db.StudTutorInfos
                                          where sti.UserId == user
                                          select sti.Id;
                var requestId = (from ar in db.AppointRequests
                                 where ar.StudId == studId.First()
                                 select new { ar.Id, ar.Course, ar.Tutor.User.User.Name }).ToArray();
                for (int i = 0; i < requestId.Count(); i++)
                    {
                    var slotId = from asl in db.AppointSlots
                                    where asl.RequestId == requestId[i].Id && asl.Selected == true
                                    select new { asl.Id, asl.Slot };
                    foreach(var id in slotId)
                    {
                        var confirmAppoint = from ac in db.AppointConfirms
                                                where ac.SlotId == id.Id
                                                select new { ac.Id, ac.PaypalLink, ac.MeetingLink };
                        foreach(var confirmAppointment in confirmAppoint)
                        {
                            GetAppointments getAppointments = new GetAppointments
                            {
                                ConfirmId = confirmAppointment.Id,
                                Date = id.Slot,
                                Course = requestId[i].Course,
                                TutorStud = requestId[i].Name,
                                Paypal = confirmAppointment.PaypalLink,
                                Zoom = confirmAppointment.MeetingLink
                            };
                            results.Add(getAppointments);
                        }
                    }
                }
                jsonResults = JsonConvert.SerializeObject(results);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }

        /// <summary>
        /// Gets the Profile Info of the student from database
        /// </summary>
        /// <param name="userId">The id of the student logged in</param>
        /// <returns>JSON Object of the profile data retreived</returns>
        [Route("GetInfo")]
        [HttpPost]
        public string GetInfo([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                int user = Convert.ToInt32(userId);
                List<ProfileData> results = new List<ProfileData>();
                var stud = (from sti in db.StudTutorInfos
                            where sti.UserId == user
                            select new { sti, sti.User.Name, sti.User.Email }).ToArray();
                if (stud.Length > 0)
                {
                    ProfileData student = new ProfileData
                    {
                        Name = stud[0].Name,
                        Address = stud[0].sti.Address,
                        City = stud[0].sti.City,
                        Postal = stud[0].sti.PostalCode,
                        Province = stud[0].sti.Province,
                        School = stud[0].sti.School,
                        Field = stud[0].sti.StudyField,
                        Program = stud[0].sti.Program,
                        Semester = stud[0].sti.Semester.ToString()
                    };
                    results.Add(student);
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }

        /// <summary>
        /// Updates the profile data of the student by using the newly submitted data by the student
        /// </summary>
        /// <param name="student">The new profile data submitted by the student</param>
        /// <returns>Success or failure message</returns>
        [Route("UpdateInfo")]
        [HttpPost]
        public string UpdateInfo([FromBody] ProfileData student)
        {
            if (student.UserId != null &&  student.Address != null && student.City != null && student.Postal != null &&
                student.Province != null && student.School != null && student.Field != null && student.Program != null &&
                student.Semester != null)
            {
                IEnumerable<StudTutorInfo> getUser = from u in db.StudTutorInfos
                            where u.UserId == Convert.ToInt32(student.UserId)
                            select u;
                foreach (StudTutorInfo user in getUser)
                {
                    user.Address = student.Address;
                    user.City = student.City;
                    user.PostalCode = student.Postal;
                    user.Program = student.Program;
                    user.Province = student.Province;
                    user.School = student.School;
                    user.StudyField = student.Field;
                    user.Semester = Convert.ToInt32(student.Semester);
                }
                try
                {
                    db.SaveChanges();
                    return "Profile Updated";
                }
                catch { }
            }
            return "Profile could not be updated!";
        }

        /// <summary>
        /// This method is shared by both tutor and student side and it creates a record in the HelpQueries table
        /// 
        /// </summary>
        /// <param name="query">The query message submitted by student or tutor</param>
        /// <returns>Success or failure message</returns>
        [Route("AskQuery")]
        [HttpPost]
        public string AskQuery([FromBody] AskQuery query)
        {
            try
            {
                if (query.UserId != null && query.Query != null)
                {
                    int user = (from sti in db.StudTutorInfos
                                where sti.UserId == Convert.ToInt32(query.UserId)
                                select sti.Id).First();

                    HelpQuery helpQuery = new HelpQuery
                    {
                        UserId = user,
                        Query = query.Query
                    };
                    db.HelpQueries.Add(helpQuery);
                    db.SaveChanges();
                    return "Query Sent!";
                }
                return "Query Sent!";
            }
            catch
            {
                return "Could not send query!";
            } 
            
        }

        /// <summary>
        /// This method creates a new record in ReportAccounts table when any user
        /// reports another user
        /// </summary>
        /// <param name="reportRequest">The data related to the report request made by user</param>
        /// <returns>Success or failure message</returns>
        [Route("ReportUser")]
        [HttpPost]
        public string ReportUser([FromBody] ReportAccountRequest reportRequest)
        {
            try
            {
                if (reportRequest.AccountId != null && reportRequest.UserId != null)
                {
                    int user = (from sti in db.StudTutorInfos
                                where sti.UserId == Convert.ToInt32(reportRequest.UserId)
                                select sti.Id).First();
                    int tutor = (from ti in db.TutorInfos
                                 where ti.Id == Convert.ToInt32(reportRequest.AccountId)
                                 select ti.User.User.Id).First();
                    ReportAccount reportAccount = new ReportAccount
                    {
                        UserId = user,
                        AccountId = tutor
                    };
                    db.ReportAccounts.Add(reportAccount);
                    db.SaveChanges();
                    return "Report request sent!";
                }
                return "Report request sent!";
            }
            catch
            {
                return "Could not send the report request!";
            }
            
            
        }

        /// <summary>
        /// When the data and time of an apointment has passed, then either student or tutor can decide to mark the
        /// appointment as done which removes all appointment related data from database
        /// </summary>
        /// <param name="confirmId">The id of the appointment in ApointConfirm table</param>
        /// <returns>Success or failure message which tells the user if the appointment has been marked as done</returns>
        [Route("MarkAsDone")]
        [HttpPost]
        public string MarkAsDone([FromBody] string confirmId)
        {
            try
            {
                int confirmedId = Convert.ToInt32(confirmId);
                IEnumerable<AppointConfirm> confirmed = from ac in db.AppointConfirms
                                where ac.Id == confirmedId
                                select ac;
                IEnumerable<AppointConfirm> allConfirmed = from ac in db.AppointConfirms
                                                        where ac.SlotId == confirmed.First().SlotId
                                                        select ac;
                IEnumerable<AppointSlot> slots = from asl in db.AppointSlots
                                                    where asl.Id == confirmed.First().SlotId
                                                    select asl;
                IEnumerable<AppointSlot> allSlots = from asl in db.AppointSlots
                                                    where asl.RequestId == slots.First().RequestId
                                                    select asl;
                IEnumerable < AppointRequest > requests = from ar in db.AppointRequests
                                                            where ar.Id == slots.First().RequestId
                                                            select ar;
                foreach(AppointConfirm appointConfirm in allConfirmed)
                {
                    db.AppointConfirms.Remove(appointConfirm);
                }
                foreach (AppointSlot appointSlot in allSlots)
                {
                    db.AppointSlots.Remove(appointSlot);
                }
                foreach (AppointRequest appointReq in requests)
                {
                    db.AppointRequests.Remove(appointReq);
                }
                db.SaveChanges();
                return "The appointment has been marked as Done!";
            }
            catch(Exception)
            {
                return "The appointment could not be marked as Done!";
            }
        }

        /// <summary>
        /// If the admin has replied to any user queries then that data gets stored in AdminReplies table
        /// This method retreives the reply to the queries that logged in user had made
        /// </summary>
        /// <param name="userId">The id of logged in user</param>
        /// <returns>JSON Object list of the admin replies</returns>
        [Route("GetReplies")]
        [HttpPost]
        public string GetReplies([FromBody] string userId)
        {
            string jsonResults = "";
            try
            {
                var replies = from r in db.AdminReplies
                              where r.Query.User.User.Id == Convert.ToInt32(userId)
                              select new {r.Id,r.Query.Query,r.Message};
                List<GetReplies> results = new List<GetReplies>();
                foreach (var ar in replies)
                {
                    GetReplies getReplies = new GetReplies
                    {
                        question = ar.Query,
                        answer = ar.Message
                    };
                    results.Add(getReplies);
                }
                jsonResults = JsonConvert.SerializeObject(results);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
        }
    }
}
