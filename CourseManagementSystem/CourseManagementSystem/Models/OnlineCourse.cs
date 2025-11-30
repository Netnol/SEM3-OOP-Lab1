namespace CourseManagementSystem.Models
{
    public class OnlineCourse(string name, string platformUrl) : Course(name)
    {
        private string PlatformUrl { get; set; } = platformUrl;

        public override string GetCourseTypeDetails()
        {
            return $"Тип: Онлайн, Платформа: {PlatformUrl}";
        }

        public class OnlineCourseBuilder : CourseBuilder
        {
            private string _platformUrl;

            public OnlineCourseBuilder SetPlatformUrl(string platformUrl)
            {
                _platformUrl = platformUrl;
                return this;
            }

            public override Course Build()
            {
                if (string.IsNullOrWhiteSpace(_name) || string.IsNullOrWhiteSpace(_platformUrl))
                {
                    throw new InvalidOperationException("Имя курса и URL платформы обязательны для онлайн-курса.");
                }
                return new OnlineCourse(_name, _platformUrl);
            }
        }
    }
}