using System;

namespace CourseLibrary.API.Models
{
    public class AuthorDto
    {        
        // chosen by the server
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int Age { get; set; }

        public string MainCategory { get; set; }
    }
}
