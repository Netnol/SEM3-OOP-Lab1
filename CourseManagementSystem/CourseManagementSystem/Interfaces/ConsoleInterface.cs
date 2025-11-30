using CourseManagementSystem.Services;

namespace CourseManagementSystem.Interfaces
{
    public class ConsoleInterface
    {
        private readonly CourseService _courseService;
        private readonly Dictionary<int, Action> _menuActions;

        public ConsoleInterface()
        {
            var manager = CourseManager.GetInstance();
            _courseService = new CourseService(manager);
            
            _menuActions = new Dictionary<int, Action>
            {
                { 1, ListAllCourses },
                { 2, ListCoursesByTeacher },
                { 3, AddTeacher },
                { 4, AddStudent },
                { 5, AddOnlineCourse },
                { 6, AddOfflineCourse },
                { 7, AssignTeacherToCourse },
                { 8, EnrollStudentInCourse },
                { 9, RemoveCourse },
                { 0, Exit },
            };
        }

        public void Start()
        {
            Console.WriteLine("Добро пожаловать в систему управления курсами и преподавателями!");
            Run();
        }

        private void Run()
        {
            while (true)
            {
                PrintMenu();
                if (int.TryParse(Console.ReadLine(), out var choice))
                {
                    if (choice == 0)
                    {
                        Exit();
                        break;
                    }

                    if (_menuActions.TryGetValue(choice, out var action))
                    {
                        try
                        {
                            action();
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Произошла ошибка: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Неверный выбор. Пожалуйста, выберите действие.");
                    }
                }
                else
                {
                    Console.WriteLine("Пожалуйста, введите число.");
                }
                Console.WriteLine("\nНажмите любую клавишу для продолжения...");
                Console.ReadKey();
                Console.Clear();
            }
        }

        private void PrintMenu()
        {
            Console.WriteLine("\n--- Меню ---");
            Console.WriteLine("1. Показать все курсы");
            Console.WriteLine("2. Показать курсы преподавателя");
            Console.WriteLine("3. Добавить преподавателя");
            Console.WriteLine("4. Добавить студента");
            Console.WriteLine("5. Добавить онлайн-курс");
            Console.WriteLine("6. Добавить оффлайн-курс");
            Console.WriteLine("7. Назначить преподавателя на курс");
            Console.WriteLine("8. Записать студента на курс");
            Console.WriteLine("9. Удалить курс");
            Console.WriteLine("0. Выйти из программы");
            Console.Write("Пожалуйста, выберите действие: ");
        }

        private void ListAllCourses()
        {
            var courses = _courseService.GetAllCourses();
            if (courses.Count == 0)
            {
                Console.WriteLine("Нет доступных курсов.");
                return;
            }
            Console.WriteLine("Все курсы:");
            for (int i = 0; i < courses.Count; i++)
            {
                var course = courses[i];
                Console.WriteLine($"{i + 1}. Название: {course.Name}, Преподаватель: {course.Teacher?.Name ?? "Нет"}, {course.GetCourseTypeDetails()}");
                Console.WriteLine($"   Студенты: {string.Join(", ", course.Students.Select(s => s.Name))}");
            }
        }

        private void ListCoursesByTeacher()
        {
            var teachers = _courseService.GetAllTeachers();
            if (!teachers.Any())
            {
                Console.WriteLine("Нет доступных преподавателей.");
                return;
            }
            Console.WriteLine("Доступные преподаватели:");
            for (int i = 0; i < teachers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {teachers[i].Name}");
            }
            Console.Write("Выберите номер преподавателя: ");
            if (int.TryParse(Console.ReadLine(), out int teacherIndex) && teacherIndex > 0 && teacherIndex <= teachers.Count)
            {
                var selectedTeacher = teachers[teacherIndex - 1];
                var courses = _courseService.GetCoursesByTeacher(selectedTeacher);
                if (!courses.Any())
                {
                    Console.WriteLine($"У преподавателя {selectedTeacher.Name} нет назначенных курсов.");
                    return;
                }
                Console.WriteLine($"Курсы преподавателя {selectedTeacher.Name}:");
                foreach (var course in courses)
                {
                    Console.WriteLine($"- {course.Name}, {course.GetCourseTypeDetails()}");
                    Console.WriteLine($"  Студенты: {string.Join(", ", course.Students.Select(s => s.Name))}");
                }
            }
            else
            {
                Console.WriteLine("Неверный номер преподавателя.");
            }
        }

        private void AddTeacher()
        {
            Console.Write("Введите имя преподавателя: ");
            string? name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя не может быть пустым.");
                return;
            }
            
            var result = _courseService.AddTeacher(name);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void AddStudent()
        {
            Console.Write("Введите имя студента: ");
            string? name = Console.ReadLine();
            
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Имя не может быть пустым.");
                return;
            }
            
            var result = _courseService.AddStudent(name);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void AddOnlineCourse()
        {
            Console.Write("Введите название онлайн-курса: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }
            
            Console.Write("Введите URL платформы: ");
            string? url = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(url))
            {
                Console.WriteLine("URL не может быть пустым.");
                return;
            }

            var result = _courseService.CreateOnlineCourse(name, url);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void AddOfflineCourse()
        {
            Console.Write("Введите название оффлайн-курса: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }
            
            Console.Write("Введите адрес аудитории: ");
            string? location = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(location))
            {
                Console.WriteLine("Адрес не может быть пустым.");
                return;
            }

            var result = _courseService.CreateOfflineCourse(name, location);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void AssignTeacherToCourse()
        {
            var teachers = _courseService.GetAllTeachers();
            var courses = _courseService.GetAllCourses();

            if (!teachers.Any() || !courses.Any())
            {
                Console.WriteLine("Недостаточно данных: добавьте преподавателей и/или курсы.");
                return;
            }

            Console.WriteLine("Доступные преподаватели:");
            for (int i = 0; i < teachers.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {teachers[i].Name}");
            }
            Console.Write("Выберите номер преподавателя: ");
            if (!int.TryParse(Console.ReadLine(), out int teacherIndex) || teacherIndex <= 0 || teacherIndex > teachers.Count)
            {
                Console.WriteLine("Неверный номер преподавателя.");
                return;
            }

            Console.WriteLine("Доступные курсы:");
            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name} (Преподаватель: {courses[i].Teacher?.Name ?? "Нет"})");
            }
            Console.Write("Выберите номер курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseIndex) || courseIndex <= 0 || courseIndex > courses.Count)
            {
                Console.WriteLine("Неверный номер курса.");
                return;
            }

            var result = _courseService.AssignTeacherToCourse(teacherIndex - 1, courseIndex - 1);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void EnrollStudentInCourse()
        {
            var students = _courseService.GetAllStudents();
            var courses = _courseService.GetAllCourses();

            if (!students.Any() || !courses.Any())
            {
                Console.WriteLine("Недостаточно данных: добавьте студентов и/или курсы.");
                return;
            }

            Console.WriteLine("Доступные студенты:");
            for (int i = 0; i < students.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {students[i].Name}");
            }
            Console.Write("Выберите номер студента: ");
            if (!int.TryParse(Console.ReadLine(), out int studentIndex) || studentIndex <= 0 || studentIndex > students.Count)
            {
                Console.WriteLine("Неверный номер студента.");
                return;
            }

            Console.WriteLine("Доступные курсы:");
            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name}");
            }
            Console.Write("Выберите номер курса: ");
            if (!int.TryParse(Console.ReadLine(), out int courseIndex) || courseIndex <= 0 || courseIndex > courses.Count)
            {
                Console.WriteLine("Неверный номер курса.");
                return;
            }

            var result = _courseService.EnrollStudentInCourse(studentIndex - 1, courseIndex - 1);
            Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
        }

        private void RemoveCourse()
        {
            var courses = _courseService.GetAllCourses();
            if (!courses.Any())
            {
                Console.WriteLine("Нет доступных курсов для удаления.");
                return;
            }
            Console.WriteLine("Доступные курсы:");
            for (int i = 0; i < courses.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {courses[i].Name} (Преподаватель: {courses[i].Teacher?.Name ?? "Нет"})");
            }
            Console.Write("Выберите номер курса для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int courseIndex) && courseIndex > 0 && courseIndex <= courses.Count)
            {
                var result = _courseService.RemoveCourse(courseIndex - 1);
                Console.WriteLine(result.IsSuccess ? result.Message : $"Ошибка: {result.Message}");
            }
            else
            {
                Console.WriteLine("Неверный номер курса.");
            }
        }

        private void Exit()
        {
            Console.WriteLine("Выход из программы.");
        }
    }
}