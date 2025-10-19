using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using StudentMobileApp.Models;


// Contains all the methods for ineracting with classes
namespace StudentMobileApp.Data
{
    public class AppData
    {
        public static List<Term> Terms = [];
        private static int nextTermId = 1;

        public static List<Course> Courses = [];
        private static int nextCourseId = 1;

        public static List<Assessment> Assessments = [];
        private static int nextAssessmentId = 1;

        //Add a term
        public static void AddTerm(Term newTerm)
        {
            newTerm.Id = nextTermId;
            nextTermId++;
            Terms.Add(newTerm);
        }

        //Update Term
        public static void UpdateTerm(Term updatedTerm)
        {
            foreach (var term in Terms)
            {
                if (updatedTerm.Id == term.Id)
                {
                    term.TermTitle = updatedTerm.TermTitle;
                    term.StartDate = updatedTerm.StartDate;
                    term.EndDate = updatedTerm.EndDate;
                    break;
                }
            }
        }

        //Delete term
        public static void DeleteTerm(int termId)
        {
            // Search for termId
            var termToRemove = Terms.FirstOrDefault(t => t.Id == termId);
            if (termToRemove != null)
            {
                Terms.Remove(termToRemove);
            }
        }

        //Get term
        public static Term GetTerm(int TermId)
        {
            foreach (Term term in Terms)
            {
                if (TermId == term.Id)
                {
                    return term;
                }
            }
            return null;
        }

        //Add a course
        public static void AddCourse(Course newCourse)
        {
            newCourse.Id = nextCourseId;
            nextCourseId++;
            Courses.Add(newCourse);
        }

        //Get courses
        public static List<Course> GetCoursesForTerm(int TermId)
        {
            List<Course> Result = [];

            foreach (var course in Courses)
            {
                if (course.TermId == TermId)
                {
                    Result.Add(course);
                }
            }
            return Result;
        }

        //Find a course
        public static Course GetCourseById(int courseId)
        {
            return Courses.FirstOrDefault(c => c.Id == courseId);
        }

        //Update courses
        public static void UpdateCourse(Course updatedCourse)
        {
            for (int i = 0; i < Courses.Count; i++)
            {
                if (Courses[i].Id == updatedCourse.Id)
                {
                    Courses[i].CourseTitle = updatedCourse.CourseTitle;
                    Courses[i].StartDate = updatedCourse.StartDate;
                    Courses[i].EndDate = updatedCourse.EndDate;
                    Courses[i].Status = updatedCourse.Status;
                    Courses[i].InstructorName = updatedCourse.InstructorName;
                    Courses[i].InstructorPhone = updatedCourse.InstructorPhone;
                    Courses[i].InstructorEmail = updatedCourse.InstructorEmail;
                    break;
                }
            }
        }

        //Delete course
        public static void DeleteCourse(int courseId)
        {
            var courseToRemove = Courses.FirstOrDefault(c => c.Id == courseId);
            if (courseToRemove != null)
            {
                Courses.Remove(courseToRemove);
            }
        }

        //Add assessment
        public static void AddAssessment(Assessment newAssessment)
        {
            newAssessment.Id = nextAssessmentId;
            nextAssessmentId++;
            Assessments.Add(newAssessment);
        }

        //find assessments
        public static List<Assessment> GetAssessmentsForCourse(int courseId)
        {
            return Assessments.Where(a => a.CourseId == courseId).ToList();
        }

        public static Assessment GetAssessmentById(int assessmentId)
        {
            return Assessments.FirstOrDefault(a => a.Id == assessmentId);
        }

        //update assessments
        public static void UpdateAssessment(Assessment updatedAssessment)
        {
            var existing = Assessments.FirstOrDefault(a => a.Id == updatedAssessment.Id);
            if (existing != null)
            {
                existing.Title = updatedAssessment.Title;
                existing.Type = updatedAssessment.Type;
                existing.StartDate = updatedAssessment.StartDate;
                existing.EndDate = updatedAssessment.EndDate;
            }
        }

        //delete assessments
        public static void DeleteAssessment(int assessmentId)
        {
            var exists = Assessments.FirstOrDefault(a => a.Id == assessmentId);
            if (exists != null)
            {
                Assessments.Remove(exists);
            }
        }
    }
}
