using Microsoft.AspNetCore.Mvc;
using System.Linq;
using Newtonsoft.Json;
using Tutoring_Platform.Models;

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

        [Route("SearchTutors")]
        [HttpPost]
        public string SearchTutors([FromBody]LookForTutorParam tutorParams)
        {
            Console.WriteLine("****************************************************************************");            
            Console.WriteLine(tutorParams.CourseName);
            string jsonResults = "";
            string[,] allTutors = new string[1,1];
            if (tutorParams.CourseName != null && tutorParams.Days != null)
            {
                Console.WriteLine("****************************************************************************");
                Console.WriteLine(tutorParams.Days[0].ToString());
                allTutors = searchTutors( tutorParams.CourseName, tutorParams.Days);
                Console.WriteLine("I am out of model");
                List<SearchTutorsReturn> results = new List<SearchTutorsReturn>();
                for(int i = 0; i < allTutors.GetLength(0); i++)
                {
                    results.Add(new SearchTutorsReturn
                    {
                        FirstName = allTutors[i, 0],
                        LastName = allTutors[i, 1],
                        School=allTutors[i, 2],
                        Program=allTutors[i, 3],
                        Status=allTutors[i, 4],
                        Wage=allTutors[i, 5]
                    });
                }
                jsonResults = JsonConvert.SerializeObject(results);
                Console.WriteLine(jsonResults);
                return jsonResults;
            }
            return jsonResults;
        }

        public string[,] searchTutors(string course, int[] days)

        {
            Console.WriteLine("****************************************************************************");
            
            IEnumerable<TutorCourse> tutorCourses = (from t in db.TutorCourses
                               where t.Course.Contains(course)
                               select t).ToList<TutorCourse>();
            IEnumerable<TutorInfo> tutor = (from t in db.TutorInfos select t).ToList<TutorInfo>();
            IEnumerable<StudTutorInfo> stud_Tutor_Info = (from sti in db.StudTutorInfos select sti).ToList<StudTutorInfo>();
            IEnumerable<User> user_ = (from u in db.Users select u).ToList<User>();
            Console.WriteLine(tutorCourses.ToArray().Length);
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

            Console.WriteLine(tutorInfo.ToArray().Length);
            
            IEnumerable<StudTutorInfo> studTutorInfo = (from sti in stud_Tutor_Info
                                                          join ti in tutorInfo on sti.Id equals ti.UserId
                                                          select sti).ToList<StudTutorInfo>();
            Console.WriteLine(studTutorInfo.ToArray().Length);
            
            IEnumerable<User> user = from u in user_
                                    join sti in studTutorInfo on u.Id equals sti.UserId
                                    select u;
            Console.WriteLine(user.ToArray().Length);
            string[,] returnValue = new String[user.Count(), 6];
            int i = 0;
            foreach (var u in user)
            {
                Console.WriteLine(u.FirstName);
                returnValue[i, 0] = u.FirstName.ToString();
                returnValue[i, 1] = u.LastName.ToString();
                i += 1;
            }
            i = 0;
            foreach (var s in studTutorInfo)
            {
                Console.WriteLine(s.School);
                returnValue[i, 2] = s.School.ToString();
                returnValue[i, 3] = s.Program.ToString();
                i += 1;
            }
            i = 0;
            
            foreach (var t in tutorInfo)
            {
                Console.WriteLine(t.Status);
                returnValue[i, 4] = t.Status.ToString();
                returnValue[i, 5] = t.Wage.ToString();
                i += 1;
            }
            
            return returnValue;
        }
    }
}
