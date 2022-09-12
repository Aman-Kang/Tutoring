using System;
using System.Collections.Generic;
using System.Linq;
namespace Tutoring_Platform.Models
{
    public class StudentModel
    {
        tutoringContext db = new tutoringContext();
        public String[,] searchTutors(String course, List<bool> days)

        {
            var tutorCourses = from t in db.TutorCourses
                                          where t.Course.Contains(course)
                                          select new { tId = t.TutorId,tCourse = t.Course };
            var daysAvailable = from d in db.DaysAvailables
                                             where d.Sunday == days[0] && d.Monday == days[1] && d.Tuesday == days[2] && d.Wednesday == days[3]
                                             && d.Thursday == days[4] && d.Friday == days[5] && d.Saturday == days[6]
                                             select new { d.UserId };
            var tutorInfo = db.TutorInfos.Where(ti => tutorCourses.Any(tc => ti.Id == tc.tId) && daysAvailable.Any(da => ti.Id == da.UserId)).Select(ti => new { ti.Id,ti.Status,ti.Wage});
            var studTutorInfo = db.StudTutorInfos.Where(sti => tutorInfo.Any(ti => sti.Id == ti.Id)).Select(sti => new { sti.Id , sti.School,sti.Program});
            var user = db.Users.Where(u => studTutorInfo.Any(sti => u.Id == sti.Id));
            String[,] returnValue = new String[user.Count(),6];
            int i = 0;
            foreach(var u in user)
            {
                returnValue[i, 0] = u.FirstName.ToString();
                returnValue[i, 1] = u.LastName.ToString();
                i += 1;
            }
            i = 0;
            foreach (var s in studTutorInfo)
            {
                returnValue[i, 2] = s.School.ToString();
                returnValue[i, 3] = s.Program.ToString();
                i += 1;
            }
            i = 0;
            foreach (var t in tutorInfo)
            {
                returnValue[i, 4] = t.Status.ToString();
                returnValue[i, 5] = t.Wage.ToString();
                i += 1;
            }

            return returnValue;
        }

    }
}
