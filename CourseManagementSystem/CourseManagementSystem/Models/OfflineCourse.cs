namespace CourseManagementSystem.Models
{
    public class OfflineCourse : Course
    {
        private string Location { get; set; }

        private OfflineCourse(string name, string location) : base(name)
        {
            Location = location;
        }

        public override string GetCourseTypeDetails()
        {
            return $"Тип: Оффлайн, Адрес: {Location}";
        }

        public class OfflineCourseBuilder : CourseBuilder
        {
            private string _location;

            public OfflineCourseBuilder SetLocation(string location)
            {
                _location = location;
                return this;
            }

            public override Course Build()
            {
                if (string.IsNullOrWhiteSpace(_name) || string.IsNullOrWhiteSpace(_location))
                {
                    throw new InvalidOperationException("Имя курса и адрес обязательны для оффлайн-курса.");
                }
                return new OfflineCourse(_name, _location);
            }
        }
    }
}