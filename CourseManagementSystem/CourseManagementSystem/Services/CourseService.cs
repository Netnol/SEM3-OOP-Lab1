using CourseManagementSystem.Models;
using CourseManagementSystem.Models.Results;

namespace CourseManagementSystem.Services
{
    public class CourseService(CourseManager manager)
    {
        public OperationResult CreateOnlineCourse(string name, string platformUrl)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Failure("Название курса обязательно");
            
            if (string.IsNullOrWhiteSpace(platformUrl))
                return OperationResult.Failure("URL платформы обязателен");

            try
            {
                var builder = new OnlineCourse.OnlineCourseBuilder();
                builder.SetName(name.Trim());
                builder.SetPlatformUrl(platformUrl.Trim());
                var course = builder.Build();

                manager.AddCourse(course);
                return OperationResult.Success($"Онлайн-курс '{name}' создан");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка создания курса: {ex.Message}");
            }
        }

        public OperationResult CreateOfflineCourse(string name, string location)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Failure("Название курса обязательно");
            
            if (string.IsNullOrWhiteSpace(location))
                return OperationResult.Failure("Адрес обязателен");

            try
            {
                var builder = new OfflineCourse.OfflineCourseBuilder();
                builder.SetName(name.Trim());
                builder.SetLocation(location.Trim());
                var course = builder.Build();

                manager.AddCourse(course);
                return OperationResult.Success($"Оффлайн-курс '{name}' создан");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка создания курса: {ex.Message}");
            }
        }

        public OperationResult AddTeacher(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Failure("Имя преподавателя обязательно");

            try
            {
                var teacher = new Teacher(name.Trim());
                manager.AddTeacher(teacher);
                return OperationResult.Success($"Преподаватель '{name}' добавлен");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка добавления преподавателя: {ex.Message}");
            }
        }

        public OperationResult AddStudent(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return OperationResult.Failure("Имя студента обязательно");

            try
            {
                var student = new Student(name.Trim());
                manager.AddStudent(student);
                return OperationResult.Success($"Студент '{name}' добавлен");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка добавления студента: {ex.Message}");
            }
        }

        public OperationResult AssignTeacherToCourse(int teacherIndex, int courseIndex)
        {
            var teachers = manager.GetAllTeachers();
            var courses = manager.GetAllCourses();

            if (teacherIndex < 0 || teacherIndex >= teachers.Count)
                return OperationResult.Failure("Неверный номер преподавателя");

            if (courseIndex < 0 || courseIndex >= courses.Count)
                return OperationResult.Failure("Неверный номер курса");

            try
            {
                var teacher = teachers[teacherIndex];
                var course = courses[courseIndex];
                
                manager.AssignTeacherToCourse(teacher, course);
                return OperationResult.Success($"Преподаватель '{teacher.Name}' назначен на курс '{course.Name}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка назначения преподавателя: {ex.Message}");
            }
        }

        public OperationResult EnrollStudentInCourse(int studentIndex, int courseIndex)
        {
            var students = manager.GetAllStudents();
            var courses = manager.GetAllCourses();

            if (studentIndex < 0 || studentIndex >= students.Count)
                return OperationResult.Failure("Неверный номер студента");

            if (courseIndex < 0 || courseIndex >= courses.Count)
                return OperationResult.Failure("Неверный номер курса");

            try
            {
                var student = students[studentIndex];
                var course = courses[courseIndex];
                
                manager.EnrollStudentInCourse(student, course);
                return OperationResult.Success($"Студент '{student.Name}' записан на курс '{course.Name}'");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка записи студента: {ex.Message}");
            }
        }

        public OperationResult RemoveCourse(int courseIndex)
        {
            var courses = manager.GetAllCourses();

            if (courseIndex < 0 || courseIndex >= courses.Count)
                return OperationResult.Failure("Неверный номер курса");

            try
            {
                var course = courses[courseIndex];
                manager.RemoveCourse(course);
                return OperationResult.Success($"Курс '{course.Name}' удален");
            }
            catch (Exception ex)
            {
                return OperationResult.Failure($"Ошибка удаления курса: {ex.Message}");
            }
        }

        public List<Course> GetAllCourses() => manager.GetAllCourses();
        public List<Teacher> GetAllTeachers() => manager.GetAllTeachers();
        public List<Student> GetAllStudents() => manager.GetAllStudents();
        public List<Course> GetCoursesByTeacher(Teacher teacher) => manager.GetCoursesByTeacher(teacher);
    }
}