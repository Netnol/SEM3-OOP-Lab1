using CourseManagementSystem.Services;

namespace CourseManagementSystem.Tests
{
    public class CourseServiceTests
    {
        private readonly CourseService _service;
        private readonly CourseManager _manager;

        public CourseServiceTests()
        {
            _manager = CourseManager.GetInstance();
            _service = new CourseService(_manager);
            
            ClearTestData();
        }

        private void ClearTestData()
        {
            var courses = _manager.GetAllCourses().ToList();
            foreach (var course in courses)
            {
                _manager.RemoveCourse(course);
            }

            var teachers = _manager.GetAllTeachers().ToList();
            foreach (var teacher in teachers)
            {
                _manager.RemoveTeacher(teacher);
            }

            var students = _manager.GetAllStudents().ToList();
            foreach (var student in students)
            {
                _manager.RemoveStudent(student);
            }
        }

        [Fact]
        public void CreateOnlineCourse_ValidData_ReturnsSuccess()
        {
            var result = _service.CreateOnlineCourse("C# Basics", "https://platform.com");
            
            Assert.True(result.IsSuccess);
            Assert.Contains("C# Basics", result.Message);
            Assert.Single(_service.GetAllCourses());
        }

        [Fact]
        public void CreateOnlineCourse_EmptyName_ReturnsFailure()
        {
            var result = _service.CreateOnlineCourse("", "https://platform.com");
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Название курса обязательно", result.Message);
            Assert.Empty(_service.GetAllCourses());
        }

        [Fact]
        public void CreateOnlineCourse_EmptyPlatformUrl_ReturnsFailure()
        {
            var result = _service.CreateOnlineCourse("C# Basics", "");
            
            Assert.False(result.IsSuccess);
            Assert.Contains("URL платформы обязателен", result.Message);
            Assert.Empty(_service.GetAllCourses());
        }

        [Fact]
        public void CreateOfflineCourse_ValidData_ReturnsSuccess()
        {
            var result = _service.CreateOfflineCourse("Java Programming", "Room 101");
            
            Assert.True(result.IsSuccess);
            Assert.Contains("Java Programming", result.Message);
            Assert.Single(_service.GetAllCourses());
        }

        [Fact]
        public void CreateOfflineCourse_EmptyLocation_ReturnsFailure()
        {
            var result = _service.CreateOfflineCourse("Java Programming", "");
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Адрес обязателен", result.Message);
            Assert.Empty(_service.GetAllCourses());
        }

        [Fact]
        public void AddTeacher_ValidName_ReturnsSuccess()
        {
            var result = _service.AddTeacher("John Smith");
            
            Assert.True(result.IsSuccess);
            Assert.Contains("John Smith", result.Message);
            Assert.Single(_service.GetAllTeachers());
        }

        [Fact]
        public void AddTeacher_EmptyName_ReturnsFailure()
        {
            var result = _service.AddTeacher("");
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Имя преподавателя обязательно", result.Message);
            Assert.Empty(_service.GetAllTeachers());
        }

        [Fact]
        public void AddStudent_ValidName_ReturnsSuccess()
        {
            var result = _service.AddStudent("Alice Johnson");
            
            Assert.True(result.IsSuccess);
            Assert.Contains("Alice Johnson", result.Message);
            Assert.Single(_service.GetAllStudents());
        }

        [Fact]
        public void AddStudent_EmptyName_ReturnsFailure()
        {
            var result = _service.AddStudent("");
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Имя студента обязательно", result.Message);
            Assert.Empty(_service.GetAllStudents());
        }

        [Fact]
        public void AssignTeacherToCourse_ValidIndices_ReturnsSuccess()
        {
            _service.AddTeacher("Dr. Brown");
            _service.CreateOnlineCourse("Math", "https://math.com");
            
            var result = _service.AssignTeacherToCourse(0, 0);
            
            Assert.True(result.IsSuccess);
            Assert.Contains("Dr. Brown", result.Message);
            Assert.Contains("Math", result.Message);
            
            var course = _service.GetAllCourses()[0];
            var teacher = _service.GetAllTeachers()[0];
            Assert.Equal(teacher, course.Teacher);
            Assert.Contains(course, teacher.Courses);
        }

        [Fact]
        public void AssignTeacherToCourse_InvalidTeacherIndex_ReturnsFailure()
        {
            _service.CreateOnlineCourse("Math", "https://math.com");
            
            var result = _service.AssignTeacherToCourse(999, 0);
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Неверный номер преподавателя", result.Message);
        }

        [Fact]
        public void AssignTeacherToCourse_InvalidCourseIndex_ReturnsFailure()
        {
            _service.AddTeacher("Dr. Brown");
            
            var result = _service.AssignTeacherToCourse(0, 999);
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Неверный номер курса", result.Message);
        }

        [Fact]
        public void EnrollStudentInCourse_ValidIndices_ReturnsSuccess()
        {
            _service.AddStudent("Bob Wilson");
            _service.CreateOnlineCourse("Physics", "https://physics.com");
            
            var result = _service.EnrollStudentInCourse(0, 0);
            
            Assert.True(result.IsSuccess);
            Assert.Contains("Bob Wilson", result.Message);
            Assert.Contains("Physics", result.Message);
            
            var course = _service.GetAllCourses()[0];
            var student = _service.GetAllStudents()[0];
            Assert.Contains(student, course.Students);
            Assert.Contains(course, student.EnrolledCourses);
        }

        [Fact]
        public void EnrollStudentInCourse_InvalidStudentIndex_ReturnsFailure()
        {
            _service.CreateOnlineCourse("Physics", "https://physics.com");
            
            var result = _service.EnrollStudentInCourse(999, 0);
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Неверный номер студента", result.Message);
        }

        [Fact]
        public void RemoveCourse_ValidIndex_ReturnsSuccess()
        {
            _service.CreateOnlineCourse("Chemistry", "https://chem.com");
            Assert.Single(_service.GetAllCourses());
            
            var result = _service.RemoveCourse(0);
            
            Assert.True(result.IsSuccess);
            Assert.Contains("Chemistry", result.Message);
            Assert.Empty(_service.GetAllCourses());
        }

        [Fact]
        public void RemoveCourse_InvalidIndex_ReturnsFailure()
        {
            var result = _service.RemoveCourse(999);
            
            Assert.False(result.IsSuccess);
            Assert.Contains("Неверный номер курса", result.Message);
        }

        [Fact]
        public void GetAllCourses_AfterAddingCourses_ReturnsCorrectCount()
        {
            _service.CreateOnlineCourse("Course 1", "https://1.com");
            _service.CreateOfflineCourse("Course 2", "Room 1");
            _service.CreateOnlineCourse("Course 3", "https://3.com");
            
            var courses = _service.GetAllCourses();
            
            Assert.Equal(3, courses.Count);
        }

        [Fact]
        public void GetAllTeachers_AfterAddingTeachers_ReturnsCorrectCount()
        {
            _service.AddTeacher("Teacher 1");
            _service.AddTeacher("Teacher 2");
            
            var teachers = _service.GetAllTeachers();
            
            Assert.Equal(2, teachers.Count);
        }

        [Fact]
        public void GetAllStudents_AfterAddingStudents_ReturnsCorrectCount()
        {
            _service.AddStudent("Student 1");
            _service.AddStudent("Student 2");
            _service.AddStudent("Student 3");
            
            var students = _service.GetAllStudents();
            
            Assert.Equal(3, students.Count);
        }

        [Fact]
        public void GetCoursesByTeacher_WithAssignedCourses_ReturnsCorrectCourses()
        {
            _service.AddTeacher("Professor X");
            _service.CreateOnlineCourse("Course A", "https://a.com");
            _service.CreateOfflineCourse("Course B", "Room B");
            
            _service.AssignTeacherToCourse(0, 0);
            _service.AssignTeacherToCourse(0, 1);
            
            var teacher = _service.GetAllTeachers()[0];
            
            var courses = _service.GetCoursesByTeacher(teacher);
            
            Assert.Equal(2, courses.Count);
            Assert.All(courses, course => Assert.Equal(teacher, course.Teacher));
        }

        [Fact]
        public void MultipleOperations_IntegrationTest_WorksCorrectly()
        {
            _service.AddTeacher("Dr. Watson");
            _service.AddStudent("Sherlock Holmes");
            _service.CreateOnlineCourse("Detective Course", "https://detective.com");
            
            _service.AssignTeacherToCourse(0, 0);
            _service.EnrollStudentInCourse(0, 0);
            
            Assert.Single(_service.GetAllTeachers());
            Assert.Single(_service.GetAllStudents());
            Assert.Single(_service.GetAllCourses());
            
            var course = _service.GetAllCourses()[0];
            var teacher = _service.GetAllTeachers()[0];
            var student = _service.GetAllStudents()[0];
            
            Assert.Equal(teacher, course.Teacher);
            Assert.Contains(student, course.Students);
            Assert.Contains(course, student.EnrolledCourses);
            Assert.Contains(course, teacher.Courses);
        }
    }
}