using CourseManagementSystem.Interfaces;

namespace CourseManagementSystem
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ConsoleInterface consoleInterface = new ConsoleInterface();

            consoleInterface.Start();
        }
    }
}