using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("student")]
    public class StudentController : ControllerBase
    {
        private tutoringContext db;
        public StudentController(tutoringContext dbModel)
        {
            db = dbModel;
        }
        //[Authorize("read:messages")]
        [Route("FindRole")]
        [HttpPost]
        public string FindRole([FromBody] string email)
        {
            string role = "";
            IEnumerable<string> userRole = from u in db.Users
                                           where u.Email == email
                                           select u.Role;

            if (userRole != null)
            {
                role = userRole.First();
            }
            return role;
        }
        [Route("UserCreated")]
        [HttpPost]
        public int UserCreated([FromBody] string userId)
        {
            int id = Convert.ToInt32(userId);
            int showDialog = 0;
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

        [Route("CreateStudent")]
        [HttpPost]
        public string CreateStudent([FromBody] StudentParam studentParams)
        {
            string jsonResults = "";
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
                        try
                        {
                            db.SaveChanges();
                        }
                        catch
                        { }
                    }
                    jsonResults = "Profile Created";
                    
                }
                return jsonResults;
            }
            catch
            {
                jsonResults = "Could not create profile";
                return jsonResults;
            }
        }


        //[Authorize("read:messages")]
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
                    Saturday = requestParam.Days[6]
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

        private string[,] searchTutors(string course, int[] days, int userId)
        {
            IEnumerable<TutorCourse> tutorCourses = (from t in db.TutorCourses
                                                     where t.Course.Contains(course)
                                                     select t).ToList<TutorCourse>();
            IEnumerable<TutorInfo> tutor = (from t in db.TutorInfos select t).ToList<TutorInfo>();
            IEnumerable<StudTutorInfo> stud_Tutor_Info = (from sti in db.StudTutorInfos select sti).ToList<StudTutorInfo>();
            IEnumerable<User> user_ = (from u in db.Users select u).ToList<User>();
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
            Console.WriteLine(daysAvailable.ToArray().Length);

            IEnumerable<TutorInfo> tutorInfoList = (from ti in tutor
                                                    join tc in tutorCourses on ti.Id equals tc.TutorId
                                                    select ti).ToList<TutorInfo>();
            IEnumerable<TutorInfo> tutorInfo = (from ti in tutorInfoList
                                                join da in daysAvailable on ti.Id equals da.UserId
                                                select ti).ToList<TutorInfo>();

            IEnumerable<StudTutorInfo> studTutorInfo = (from sti in stud_Tutor_Info
                                                        join ti in tutorInfo on sti.Id equals ti.UserId
                                                        select sti).ToList<StudTutorInfo>();

            IEnumerable<User> user = from u in user_
                                     join sti in studTutorInfo on u.Id equals sti.UserId
                                     select u;

            IEnumerable<StudTutorInfo> studTutorInfo2 = (from sti in stud_Tutor_Info
                                                         where sti.UserId == userId
                                                         select sti).ToList<StudTutorInfo>();
            string[,] returnValue = new String[user.Count(), 7];

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

        [Route("DisplayRequests")]
        [HttpPost]
        public string DisplayRequests([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            try
            {
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
                                 select new { asl.Id, asl.Slot }).ToArray();
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
                    });
                }
                jsonResults = JsonConvert.SerializeObject(results);
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }
        [Route("SendConfirmedSlot")]
        [HttpPost]
        public string SendConfirmedSlot([FromBody] string slot)
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
                try
                {
                    db.SaveChanges();
                    return "Slot Selected";
                }
                catch { }
            }
            return "Slot could not be Selected";
        }

        [Route("GetAppointments")]
        [HttpPost]
        public string GetAppointments([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            try
            {
                List<GetAppointments> results = new List<GetAppointments>();
                IEnumerable<int> studId = from sti in db.StudTutorInfos
                                          where sti.UserId == user
                                          select sti.Id;
                var requestId = (from ar in db.AppointRequests
                                 where ar.StudId == studId.First()
                                 select new { ar.Id, ar.Course, ar.Tutor.User.User.Name }).ToArray();
                if (requestId.Count() > 0)
                {
                    for (int i = 0; i < requestId.Count(); i++)
                    {
                        var slotId = from asl in db.AppointSlots
                                     where asl.RequestId == requestId[i].Id && asl.Selected == true
                                     select new { asl.Id, asl.Slot };
                        var confirmAppoint = from ac in db.AppointConfirms
                                             where ac.SlotId == slotId.First().Id
                                             select new { ac.Id, ac.PaypalLink, ac.MeetingLink };
                        GetAppointments getAppointments = new GetAppointments
                        {
                            Date = slotId.First().Slot,
                            Course = requestId[i].Course,
                            TutorStud = requestId[i].Name,
                            Paypal = confirmAppoint.First().PaypalLink,
                            Zoom = confirmAppoint.First().MeetingLink
                        };
                        results.Add(getAppointments);
                    }
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }

        [Route("GetInfo")]
        [HttpPost]
        public string GetInfo([FromBody] string userId)
        {
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            try
            {
                List<StudentParam> results = new List<StudentParam>();
                var stud = (from sti in db.StudTutorInfos
                            where sti.UserId == user
                            select new { sti, sti.User.Name, sti.User.Email }).ToArray();

                if (stud.Length > 0)
                {
                    StudentParam student = new StudentParam
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

        [Route("UpdateInfo")]
        [HttpPost]
        public string UpdateInfo([FromBody] StudentParam student)
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

        [Route("AskQuery")]
        [HttpPost]
        public string AskQuery([FromBody] AskQuery query)
        {
            if(query.UserId != null && query.Query != null)
            {
                int user = (from sti in db.StudTutorInfos
                           where sti.UserId == Convert.ToInt32(query.UserId)
                           select sti.Id).First();
                try
                {
                    HelpQuery helpQuery = new HelpQuery
                    {
                        UserId = user,
                        Query = query.Query
                    };
                    db.HelpQueries.Add(helpQuery);
                    db.SaveChanges();
                    return "Query Sent!";
                }
                catch { }
            }
            return "Could not send query!";
        }

        [Route("ReportUser")]
        [HttpPost]
        public string ReportUser([FromBody] ReportAccountRequest reportRequest)
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
            return "Could not send the report request!";
        }


        [Route("GetReportedAcc")]
        [HttpPost]
        public string GetReportedAcc([FromBody] string userId)
        {

            string jsonResults = "";
            try
            {
                ReportAccount[] requests = (from ra in db.ReportAccounts
                                            select ra).ToArray();

                List<GetReportAccount> results = new List<GetReportAccount>();
                if (requests.Length > 0)
                {
                    foreach (var request in requests)
                    {
                        var getName = from u in db.Users
                                      where u.Id == request.AccountId
                                      select u.Name;
                        var reportedBy = from sti in db.StudTutorInfos
                                         where sti.Id == request.UserId
                                         select sti.User.Name;
                        GetReportAccount getReportAccount = new GetReportAccount
                        {
                            AccountId = request.AccountId,
                            Name = getName.First(),
                            By = reportedBy.First()
                        };
                        results.Add(getReportAccount);
                    }
                    jsonResults = JsonConvert.SerializeObject(results);
                }
                return jsonResults;
            }
            catch
            {
                return jsonResults;
            }
            
        }

        [Route("DeleteUser")]
        [HttpPost]
        public string DeleteUser([FromBody] GetReportAccount account)
        {
            int user = Convert.ToInt32(account.AccountId);
            try
            {
                var adminReplies = from ar in db.AdminReplies
                                   where ar.Query.User.User.Id == user
                                   select ar;
                if (adminReplies.Count() > 0) Console.WriteLine(adminReplies.First().Id);
                foreach (var a in adminReplies)
                {
                    db.AdminReplies.Remove(a);
                }
                db.SaveChanges();
                var appointConfirm = from ac in db.AppointConfirms
                                     where ac.Slot.Request.Stud.User.Id == user
                                     select ac;
                if (appointConfirm.Count() > 0) Console.WriteLine(appointConfirm.First().Id);
                foreach (var a in appointConfirm)
                {
                    db.AppointConfirms.Remove(a);
                }
                db.SaveChanges();
                var appointConfirm2 = from ac in db.AppointConfirms
                                      where ac.Slot.Request.Tutor.User.User.Id == user
                                      select ac;
                Console.WriteLine(appointConfirm2.First().Id);
                foreach (var a in appointConfirm2)
                {
                    db.AppointConfirms.Remove(a);
                }
                db.SaveChanges();
                var appointSlot = from asl in db.AppointSlots
                                  where asl.Request.Stud.User.Id == user
                                  select asl;
                if (appointSlot.Count() > 0) Console.WriteLine(appointSlot.First().Id);
                foreach (var a in appointSlot)
                {
                    db.AppointSlots.Remove(a);
                }
                db.SaveChanges();
                var appointSlot2 = from asl in db.AppointSlots
                                   where asl.Request.Tutor.User.User.Id == user
                                   select asl;
                if (appointSlot2.Count() > 0) Console.WriteLine(appointSlot2.First().Id);
                foreach (var a in appointSlot2)
                {
                    db.AppointSlots.Remove(a);
                }
                db.SaveChanges();
                var appointRequest = from ar in db.AppointRequests
                                     where ar.Stud.User.Id == user
                                     select ar;
                if (appointRequest.Count() > 0)
                    Console.WriteLine(appointRequest.First().Id);
                foreach (var a in appointRequest)
                {
                    db.AppointRequests.Remove(a);
                }
                db.SaveChanges();
                var helpQueries = from hq in db.HelpQueries
                                  where hq.User.User.Id == user
                                  select hq;
                if (helpQueries.Count() > 0)
                    Console.WriteLine(helpQueries.First().Id);
                foreach (var h in helpQueries)
                {
                    db.HelpQueries.Remove(h);
                }
                db.SaveChanges();
                var reportAccount = from ra in db.ReportAccounts
                                    where ra.User.User.Id == user
                                    select ra;
                if (reportAccount.Count() > 0)
                    Console.WriteLine(reportAccount.First().Id);
                foreach (var r in reportAccount)
                {
                    db.ReportAccounts.Remove(r);
                }
                db.SaveChanges();
                var daysAvail = from da in db.DaysAvailables
                                where da.User.User.User.Id == user
                                select da;
                if (daysAvail.Count() > 0)
                    Console.WriteLine(daysAvail.First().Id);
                foreach (var d in daysAvail)
                {
                    db.DaysAvailables.Remove(d);
                }
                db.SaveChanges();
                var tutorCourse = from tc in db.TutorCourses
                                  where tc.Tutor.User.User.Id == user
                                  select tc;
                if (tutorCourse.Count() > 0)
                    Console.WriteLine(tutorCourse.First().Id);
                foreach (var t in tutorCourse)
                {
                    db.TutorCourses.Remove(t);
                }
                db.SaveChanges();
                var tutorInfo = from ti in db.TutorInfos
                                where ti.User.User.Id == user
                                select ti;
                if (tutorInfo.Count() > 0)
                    Console.WriteLine(tutorInfo.First().Id);
                foreach (var t in tutorInfo)
                {
                    db.TutorInfos.Remove(t);
                }
                db.SaveChanges();
                var studTutor = from sti in db.StudTutorInfos
                                where sti.User.Id == user
                                select sti;
                if (studTutor.Count() > 0)
                    Console.WriteLine(studTutor.First().Id);
                foreach (var s in studTutor)
                {
                    db.StudTutorInfos.Remove(s);
                }
                db.SaveChanges();
                var userAccount =
                            from u in db.Users
                            where u.Id == user
                            select u;
                if (userAccount.Count() > 0)
                    Console.WriteLine(userAccount.First().Id);
                foreach (var u in userAccount)
                {
                    db.Users.Remove(u);
                }
                db.SaveChanges();
                return "Account deleted";
            }
            
            catch
            {
                return "Account could not be deleted";
            }

        }
    }
}
