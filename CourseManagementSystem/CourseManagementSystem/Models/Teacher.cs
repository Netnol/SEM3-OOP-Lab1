namespace CourseManagementSystem.Models
{
    public class Teacher(string name)
    {
        public string Name { get; set; } = name;
        public List<Course> Courses { get; set; } = new();

        public void AssignCourse(Course course)
        {
            if (!Courses.Contains(course))
            {
                Courses.Add(course);
                course.Teacher = this;
            }
        }

        public void UnassignCourse(Course course)
        {
            if (Courses.Contains(course))
            {
                Courses.Remove(course);
                course.Teacher = null;
            }
        }
    }
}