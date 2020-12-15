using System.Collections.Generic;

namespace Krastev_It_API.Data
{
    public class Course
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string ImgUrl { get; set; }

        public ICollection<Lecture> Lectures { get; set; }
    }
}
