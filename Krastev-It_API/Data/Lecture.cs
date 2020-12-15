namespace Krastev_It_API.Data
{
    public class Lecture
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string VideoUrl { get; set; }

        public int CourseId { get; set; }

        public Course Course { get; set; }
    }
}
