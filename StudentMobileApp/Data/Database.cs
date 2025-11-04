using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SQLite;
using StudentMobileApp.Models;

/// <summary>
/// The Database class handles all data persistence using SQLite.
/// This structure supports scalability by isolating data access from the UI.
/// Future versions can switch to a cloud database without modifying views.
/// </summary>

namespace StudentMobileApp.Data
{
    public static class Database
    {
        private static SQLiteAsyncConnection _database;
        private static readonly string _dbPath =
            Path.Combine(FileSystem.AppDataDirectory, "StudentApp.db3");

        public static async Task Init()
        {
            if (_database != null)
                return;

            _database = new SQLiteAsyncConnection(_dbPath);
            await _database.CreateTableAsync<Term>();
            await _database.CreateTableAsync<Course>();
            await _database.CreateTableAsync<Assessment>();
        }

        public static async Task CleanupOrphanedAssessmentsAsync()
        {
            var allAssessments = await _database.Table<Assessment>().ToListAsync();
            var allCourses = await _database.Table<Course>().ToListAsync();
            var validCourseIds = allCourses.Select(c => c.Id).ToHashSet();

            int deletedCount = 0;

            foreach (var a in allAssessments)
            {
                if (!validCourseIds.Contains(a.CourseId))
                {
                    await _database.DeleteAsync(a);
                    deletedCount++;
                }
            }

            System.Diagnostics.Debug.WriteLine($"[Cleanup] Removed {deletedCount} orphaned assessments.");
        }


        //Term CRUD 
        public static async Task<List<Term>> GetTermsAsync()
        {
            await Init();
            return await _database.Table<Term>().ToListAsync();
        }

        public static async Task<int> AddTermAsync(Term term)
        {
            await Init();
            return await _database.InsertAsync(term);
        }

        public static async Task<int> UpdateTermAsync(Term term)
        {
            await Init();
            return await _database.UpdateAsync(term);
        }

        public static async Task DeleteTermAsync(Term term)
        {
            // Find all courses in this term
            var courses = await _database.Table<Course>()
                .Where(c => c.TermId == term.Id)
                .ToListAsync();

            // Delete related assessments for each course
            foreach (var course in courses)
            {
                var assessments = await _database.Table<Assessment>()
                    .Where(a => a.CourseId == course.Id)
                    .ToListAsync();

                foreach (var a in assessments)
                    await _database.DeleteAsync(a);

                await _database.DeleteAsync(course);
            }

            // Finally, delete the term itself
            await _database.DeleteAsync(term);
        }


        //Course CRUD
        public static async Task<List<Course>> GetCoursesAsync()
        {
            await Init();
            return await _database.Table<Course>().ToListAsync();
        }

        public static async Task<List<Course>> GetCoursesByTermAsync(int termId)
        {
            await Init();
            return await _database.Table<Course>()
                .Where(c => c.TermId == termId)
                .ToListAsync();
        }

        public static async Task<int> AddCourseAsync(Course course)
        {
            await Init();
            return await _database.InsertAsync(course);
        }

        public static async Task<int> UpdateCourseAsync(Course course)
        {
            await Init();
            return await _database.UpdateAsync(course);
        }

        public static async Task DeleteCourseAsync(Course course)
        {
            // Delete all assessments that belong to this course
            var relatedAssessments = await _database.Table<Assessment>()
                .Where(a => a.CourseId == course.Id)
                .ToListAsync();

            foreach (var a in relatedAssessments)
            {
                await _database.DeleteAsync(a);
            }

            // Then delete the course itself
            await _database.DeleteAsync(course);
        }


        //Aassessment CRUD
        public static async Task<List<Assessment>> GetAssessmentsAsync()
        {
            await Init();
            return await _database.Table<Assessment>().ToListAsync();
        }

        public static async Task<List<Assessment>> GetAssessmentsByCourseAsync(int courseId)
        {
            await Init();
            return await _database.Table<Assessment>()
                .Where(a => a.CourseId == courseId)
                .ToListAsync();
        }

        public static async Task<int> AddAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.InsertAsync(assessment);
        }

        public static async Task<int> UpdateAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.UpdateAsync(assessment);
        }

        public static async Task<int> DeleteAssessmentAsync(Assessment assessment)
        {
            await Init();
            return await _database.DeleteAsync(assessment);
        }


        public static SQLiteAsyncConnection GetConnection()
        {
            return _database;
        }

    }
}
