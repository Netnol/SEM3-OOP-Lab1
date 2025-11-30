using CourseManagementSystem.Models;

namespace CourseManagementSystem.Services
{
    public class CourseManager
    {
        private static CourseManager? _instance;
        private static readonly object _lock = new object();

        private readonly List<Course> _courses;
        private readonly List<Teacher> _teachers;
        private readonly List<Student> _students;

        private CourseManager()
        {
            _courses = new List<Course>();
            _teachers = new List<Teacher>();
            _students = new List<Student>();
        }

        public static CourseManager GetInstance()
        {
            if (_instance == null)
            {
                lock (Lock)
                {
                    _instance ??= new CourseManager();
                }
            }
            return _instance;
        }

#if DEBUG
        public static void ResetForTesting()
        {
            _instance = null;
        }
#endif

        public void AddCourse(Course? course)
        {
            if (course != null && !_courses.Contains(course))
            {
                _courses.Add(course);
            }
        }

        public void RemoveCourse(Course course)
        {
            if (!_courses.Contains(course)) return;
            var studentsToUnenroll = course.Students.ToList();
            foreach (var student in studentsToUnenroll)
            {
                course.UnEnrollStudent(student);
            }

            if (course.Teacher != null)
            {
                course.Teacher.UnassignCourse(course);
            }

            _courses.Remove(course);
        }

        public List<Course> GetAllCourses()
        {
            return [.._courses];
        }

        public void AssignTeacherToCourse(Teacher teacher, Course course)
        {
            if (_teachers.Contains(teacher) && _courses.Contains(course))
            {
                teacher.AssignCourse(course);
            }
        }

        public void EnrollStudentInCourse(Student student, Course course)
        {
            if (_students.Contains(student) && _courses.Contains(course))
            {
                course.EnrollStudent(student);
            }
        }

        public List<Course> GetCoursesByTeacher(Teacher teacher)
        {
            if (_teachers.Contains(teacher))
            {
                return [..teacher.Courses];
            }
            return [];
        }

        public void AddTeacher(Teacher? teacher)
        {
            if (teacher != null && !_teachers.Contains(teacher))
            {
                _teachers.Add(teacher);
            }
        }

        public void RemoveTeacher(Teacher teacher)
        {
            if (_teachers.Contains(teacher))
            {
                var coursesToUnassign = teacher.Courses.ToList();
                foreach (var course in coursesToUnassign)
                {
                    teacher.UnassignCourse(course);
                }
                _teachers.Remove(teacher);
            }
        }

        public void AddStudent(Student? student)
        {
            if (student != null && !_students.Contains(student))
            {
                _students.Add(student);
            }
        }

        public void RemoveStudent(Student student)
        {
            if (_students.Contains(student))
            {
                var coursesToUnenroll = student.EnrolledCourses.ToList();
                foreach (var course in coursesToUnenroll)
                {
                    course.UnEnrollStudent(student);
                }
                _students.Remove(student);
            }
        }

        public List<Teacher> GetAllTeachers()
        {
            return [.._teachers];
        }

        public List<Student> GetAllStudents()
        {
            return [.._students];
        }
    }
}