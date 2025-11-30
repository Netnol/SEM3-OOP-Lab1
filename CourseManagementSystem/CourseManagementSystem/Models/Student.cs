namespace CourseManagementSystem.Models
{
    public class Student(string name)
    {
        public string Name { get; set; } = name;
        public List<Course> EnrolledCourses { get; set; } = new();

        public void EnrollInCourse(Course course)
        {
            if (!EnrolledCourses.Contains(course))
            {
                EnrolledCourses.Add(course);
            }
        }

        public void UnEnrollFromCourse(Course course)
        {
            if (EnrolledCourses.Contains(course))
            {
                EnrolledCourses.Remove(course);
            }
        }
    }
}