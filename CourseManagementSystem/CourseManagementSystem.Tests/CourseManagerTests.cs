using CourseManagementSystem.Models;
using CourseManagementSystem.Services;
using Xunit;

namespace CourseManagementSystem.Tests
{
    public class CourseManagerTests
    {
        private readonly CourseManager _manager;

        public CourseManagerTests()
        {
            CourseManager.ResetForTesting();
            _manager = CourseManager.GetInstance();
        }

        [Fact]
        public void AddCourse_AddsCourseToList()
        {
            var course = new OnlineCourse("Тест", "https://example.com");
            _manager.AddCourse(course);
            var courses = _manager.GetAllCourses();
            Assert.Contains(course, courses);
        }

        [Fact]
        public void RemoveCourse_RemovesCourseFromList()
        {
            var course = new OnlineCourse("Тест", "https://example.com");
            _manager.AddCourse(course);
            _manager.RemoveCourse(course);
            var courses = _manager.GetAllCourses();
            Assert.DoesNotContain(course, courses);
        }

        [Fact]
        public void AssignTeacherToCourse_SetsTeacherOnCourse()
        {
            var teacher = new Teacher("Иванов И.И.");
            var course = new OnlineCourse("Введение в C#", "https://myplatform.edu");

            _manager.AddTeacher(teacher);
            _manager.AddCourse(course);

            _manager.AssignTeacherToCourse(teacher, course);

            Assert.Equal(teacher, course.Teacher);
            Assert.Contains(course, teacher.Courses);
        }

        [Fact]
        public void EnrollStudentInCourse_AddsStudentToCourse()
        {
            var student = new Student("Сидоров С.С.");
            var course = new OnlineCourse("Введение в C#", "https://myplatform.edu");

            _manager.AddStudent(student);
            _manager.AddCourse(course);

            _manager.EnrollStudentInCourse(student, course);

            Assert.Contains(student, course.Students);
            Assert.Contains(course, student.EnrolledCourses);
        }

        [Fact]
        public void GetCoursesByTeacher_ReturnsCorrectCourses()
        {
            var teacher = new Teacher("Иванов И.И.");
            var course = new OnlineCourse("Введение в C#", "https://myplatform.edu");

            _manager.AddTeacher(teacher);
            _manager.AddCourse(course);
            _manager.AssignTeacherToCourse(teacher, course);

            var teacherCourses = _manager.GetCoursesByTeacher(teacher);

            Assert.Single(teacherCourses);
            Assert.Contains(course, teacherCourses);
        }

        [Fact]
        public void RemoveTeacher_UnassignsCourses()
        {
            var teacher = new Teacher("Иванов И.И.");
            var course = new OnlineCourse("Введение в C#", "https://myplatform.edu");

            _manager.AddTeacher(teacher);
            _manager.AddCourse(course);
            _manager.AssignTeacherToCourse(teacher, course);

            _manager.RemoveTeacher(teacher);

            Assert.Null(course.Teacher);
            Assert.Empty(teacher.Courses);
        }

        [Fact]
        public void RemoveStudent_UnenrollsFromCourses()
        {
            var student = new Student("Сидоров С.С.");
            var course = new OnlineCourse("Введение в C#", "https://myplatform.edu");

            _manager.AddStudent(student);
            _manager.AddCourse(course);
            _manager.EnrollStudentInCourse(student, course);

            _manager.RemoveStudent(student);

            Assert.DoesNotContain(student, course.Students);
            Assert.Empty(student.EnrolledCourses);
        }

        [Fact]
        public void GetAllCourses_ReturnsCopyOfList()
        {
            var course1 = new OnlineCourse("Курс 1", "https://1.com");
            var course2 = new OnlineCourse("Курс 2", "https://2.com");
            _manager.AddCourse(course1);
            _manager.AddCourse(course2);

            var courses = _manager.GetAllCourses();
            courses.Clear();

            Assert.Equal(2, _manager.GetAllCourses().Count);
        }

        [Fact]
        public void AddDuplicateCourse_DoesNotAddDuplicate()
        {
            var course = new OnlineCourse("Дубликат", "https://duplicate.com");
            _manager.AddCourse(course);
            
            // Act
            _manager.AddCourse(course);
            
            Assert.Single(_manager.GetAllCourses());
        }

        [Fact]
        public void RemoveNonExistentCourse_DoesNothing()
        {
            var course = new OnlineCourse("Несуществующий", "https://none.com");
            var initialCount = _manager.GetAllCourses().Count;
            
            _manager.RemoveCourse(course);
            
            Assert.Equal(initialCount, _manager.GetAllCourses().Count);
        }
    }
}