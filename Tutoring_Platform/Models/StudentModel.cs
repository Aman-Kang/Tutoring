using System.Collections.Generic;
using System.Linq;
namespace Tutoring_Platform.Models
{
    public class StudentModel
    {
        tutoringContext db = new tutoringContext();
        public void searchTutors(String course, List<bool> days)
        {
            List<TutorCourse> tutorCourses = db.TutorCourses.Where(t => t.Course.Equals(course)).ToList();
            List<int> tutorIds = new List<int>();
            for(int i = 0; i < tutorCourses.Count; i++)
            {
                tutorIds.Add(tutorCourses[i].TutorId);
            }

            List<int> tutorIds2 = new List<int>();
            List<DaysAvailable> daysAvailable = db.DaysAvailables.Where(d => d.Sunday == days[0] && d.Monday == days[1] && d.Tuesday == days[2] && d.Wednesday == days[3] 
            && d.Thursday == days[4] && d.Friday == days[5] && d.Saturday == days[6]).ToList();
            for (int i = 0; i < daysAvailable.Count; i++)
            {
                tutorIds2.Add(daysAvailable[i].UserId);
            }
            List<int> tutorIds3 = new List<int>();
            for (int i = 0; i < tutorIds.Count; i++)
            {
                for (int j = 0; i < tutorIds2.Count; i++)
                {
                   if(tutorIds[i] == tutorIds2[j])
                    {
                        tutorIds3.Add(tutorIds2[j]);
                    }
                }

            }

            List<TutorInfo> tutorInfo = new List<TutorInfo>();
            for (int i = 0; i < tutorIds3.Count; i++)
            {
                tutorInfo.Add(db.TutorInfos.Where(t => t.Id == tutorIds3[i]).FirstOrDefault());
            }
            List<StudTutorInfo> studTutorInfo = new List<StudTutorInfo>();
            for (int i = 0; i < tutorInfo.Count; i++)
            {
                for (int j = 0; i < db.StudTutorInfos.Count(); i++)
                {
                    try
                    {
                        if (tutorInfo[i].Id == db.StudTutorInfos.Find(j).UserId)
                        {
                            studTutorInfo.Add(db.StudTutorInfos.Find(j));
                        }
                    }
                    catch
                    {

                    }
                    
                }
            }
        }

    }
}
