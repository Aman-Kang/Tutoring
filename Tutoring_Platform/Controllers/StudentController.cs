using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;
using Tutoring_Platform.CustomModels;
using Microsoft.AspNetCore.Authorization;

namespace Tutoring_Platform.Controllers
{
    [ApiController]
    [Route("[controller]")]
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
        public string FindRole([FromBody]string email)
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

        [Route("GetTutorRequests")]
        [HttpPost]
        public string GetTutorRequests([FromBody] string userId)
        {
            Console.WriteLine("******************************************************");
            Console.WriteLine(userId.ToString());
            int user = Convert.ToInt32(userId);
            string jsonResults = "";
            IEnumerable<int> studId = from sti in db.StudTutorInfos
                                      where sti.UserId == user
                                      select sti.Id;
            IEnumerable<int> tutorId = from ti in db.TutorInfos
                                       where ti.UserId == studId.First()
                                       select ti.Id;
            AppointRequest[] appointRequests = (from ar in db.AppointRequests
                                                where ar.TutorId == tutorId.First()
                                                select ar).ToArray();
            List<DisplayRequestsReturn> results = new List<DisplayRequestsReturn>();
            for (int i = 0; i < appointRequests.Count(); i++)
            {

                int getRequestStudId = appointRequests[i].StudId;
                Console.WriteLine(getRequestStudId.ToString());
                var getStudData = from sti in db.StudTutorInfos
                                  where sti.Id == getRequestStudId
                                  select new { sti.Semester, sti.Program, sti.School };
                results.Add(new DisplayRequestsReturn
                {
                    Semester = getStudData.First().Semester.ToString(),
                    Program = getStudData.First().Program,
                    School = getStudData.First().School,
                    CourseName = appointRequests[i].Course,
                    Days = new int[] {Convert.ToInt32(appointRequests[i].Sunday), Convert.ToInt32(appointRequests[i].Monday) , Convert.ToInt32(appointRequests[i].Tuesday) , Convert.ToInt32(appointRequests[i].Wednesday) ,
                        Convert.ToInt32(appointRequests[i].Thursday),Convert.ToInt32(appointRequests[i].Friday),Convert.ToInt32(appointRequests[i].Saturday)}
                });
            }
            jsonResults = JsonConvert.SerializeObject(results);

            return jsonResults;
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
            if (studentParams.Name != null && studentParams.Address != null && studentParams.City != null && studentParams.Postal != null &&
                studentParams.Province != null && studentParams.School != null && studentParams.Field != null && studentParams.Program != null &&
                studentParams.Semester != null && studentParams.UserId != null && studentParams.Role != null)
            {
                IEnumerable<User> getUser = from u in db.Users
                                            where u.Id == Convert.ToInt32(studentParams.UserId)
                                            select u;
                foreach(User user in getUser)
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
                try
                {
                    db.SaveChanges();
                    if (studentParams.Role == "tutor")
                    {
                        int getStudId = (from sti in db.StudTutorInfos
                                        where sti.UserId == Convert.ToInt32(studentParams.UserId)
                                        select sti.Id).First();
                        string status = "";
                        if(Convert.ToInt32(studentParams.Semester) == 0)
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
                        {}
                    }
                    jsonResults = "Profile Created";
                }
                catch
                {
                    jsonResults = "Could not create profile";
                }

            }
            return jsonResults;
        }


        //[Authorize("read:messages")]
        [Route("SearchTutors")]
        [HttpPost]
        public string SearchTutors([FromBody]LookForTutorParam tutorParams)
        {          
            string jsonResults = "";
            string[,] allTutors = new string[1,1];
            if (tutorParams.CourseName != null && tutorParams.Days != null)
            {
                allTutors = searchTutors( tutorParams.CourseName, tutorParams.Days);
                List<SearchTutorsReturn> results = new List<SearchTutorsReturn>();
                for(int i = 0; i < allTutors.GetLength(0); i++)
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
                return jsonResults;
            }
            else
            {
                jsonResults = "Invalid values entered!";
            }
            return jsonResults;
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
                
                // Submit the change to the database.
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

        private string[,] searchTutors(string course, int[] days)

        {
            Console.WriteLine("****************************************************************************");
            
            IEnumerable<TutorCourse> tutorCourses = (from t in db.TutorCourses
                               where t.Course.Contains(course)
                               select t).ToList<TutorCourse>();
            IEnumerable<TutorInfo> tutor = (from t in db.TutorInfos select t).ToList<TutorInfo>();
            IEnumerable<StudTutorInfo> stud_Tutor_Info = (from sti in db.StudTutorInfos select sti).ToList<StudTutorInfo>();
            IEnumerable<User> user_ = (from u in db.Users select u).ToList<User>();
            List<DaysAvailable> daysAvailable = new List<DaysAvailable>();
            int studentDays = 0;
            foreach(int day in days)
            {
                if(day == 1)
                {
                    studentDays++;
                }
            }
            foreach(DaysAvailable d in db.DaysAvailables)
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
                if(count == 0)
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
            string[,] returnValue = new String[user.Count(), 7];
            int i = 0;
            foreach (var u in user)
            {
                if(u.Name != null)
                {
                    returnValue[i, 0] = u.Name.ToString();
                }
                i += 1;
            }
            i = 0;
            foreach (var s in studTutorInfo)
            {
                Console.WriteLine(s.School);
                returnValue[i, 1] = s.Id.ToString();
                if(s.School != null && s.Program != null)
                {
                    returnValue[i, 2] = s.School.ToString();
                    returnValue[i, 3] = s.Program.ToString();
                }
                i += 1;
            }
            i = 0;
            
            foreach (var t in tutorInfo)
            {
                Console.WriteLine(t.Status);
                if(t.Status != null && t.Wage != null)
                {
                    returnValue[i, 4] = t.Status.ToString();
                    returnValue[i, 5] = t.Wage.ToString();
                }
                returnValue[i, 6] = t.Id.ToString();
                i += 1;
            }
            
            return returnValue;
        }
    }
}
