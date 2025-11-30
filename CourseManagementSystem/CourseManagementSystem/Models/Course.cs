namespace CourseManagementSystem.Models
{
    public abstract class Course(string name)
    {
        public string Name { get; protected set; } = name;
        public Teacher? Teacher { get; set; }
        public List<Student> Students { get; } = new();

        public void EnrollStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
                student.EnrollInCourse(this);
            }
        }

        public void UnEnrollStudent(Student student)
        {
            if (Students.Contains(student))
            {
                Students.Remove(student);
                student.UnEnrollFromCourse(this);
            }
        }

        public abstract string GetCourseTypeDetails();

        public abstract class CourseBuilder
        {
            protected string _name;

            public CourseBuilder SetName(string name)
            {
                _name = name;
                return this;
            }

            public abstract Course Build();
        }
    }
}