using AutoMapper;
using CourseLibrary.API.Models;
using CourseLibrary.API.ResourceParameters;
using CourseLibrary.API.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CourseLibrary.API.Controller
{
    [ApiController]
    [Route("api/authors")]

    // also see https://docs.microsoft.com/en-us/aspnet/core/web-api/?view=aspnetcore-6.0
    public class AuthorsController : ControllerBase
    {
        private readonly ICourseLibraryRepository _courseLibraryRepository;
        private readonly IMapper _mapper;
        public AuthorsController(ICourseLibraryRepository courseLibraryRepository, IMapper mapper)
        {
            // check if null
            _courseLibraryRepository = courseLibraryRepository ??
                throw new ArgumentNullException(nameof(courseLibraryRepository));

            _mapper = mapper ??
                throw new ArgumentNullException(nameof(mapper));
        }

        [HttpGet()]
        [HttpHead] // similar to GET, but doesn't return the response body
        /* According to the rule of source parameter binding, a complex type (e.g. AuthorsResourceParameter)
             is automatically assumed to be coming from the request body, so AuthorsResourceParameter should be bounded from the query string parameter */
        // if [FromQuery] is omitted, 406 unsupported media type error will occur upon request
        public ActionResult<IEnumerable<AuthorDto>> GetAuthors([FromQuery] AuthorsResourceParameter authorsResourceParameter)
        {
            var authorsFromRepo = _courseLibraryRepository.GetAuthors(authorsResourceParameter);

            // Map<Type expected>(source)
            return Ok(_mapper.Map<IEnumerable<AuthorDto>>(authorsFromRepo));

        }

        [HttpGet("{authorId}")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }
    }
}
