using AutoMapper;
using CourseLibrary.API.Entities;
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

        [HttpGet("{authorId}", Name = "GetAuthor")]
        public ActionResult<AuthorDto> GetAuthor(Guid authorId)
        {
            var authorFromRepo = _courseLibraryRepository.GetAuthor(authorId);

            if (authorFromRepo == null)
            {
                return NotFound();
            }

            return Ok(_mapper.Map<AuthorDto>(authorFromRepo));
        }

        [HttpPost]
        /* authorForCreationDto is a complex type
         APIController attribute will automatically deserialize it from the request body
         APIController attribute automatically check empty or invalid serialized data,
         which will result in null and return 400 Bad Request */
        public ActionResult<AuthorDto> CreateAuthor(AuthorForCreationDto author)
        {
            var authorEntity = _mapper.Map<Author>(author);
            _courseLibraryRepository.AddAuthor(authorEntity);
            _courseLibraryRepository.Save();

            var authorToReturn = _mapper.Map<AuthorDto>(authorEntity);

            /* route name (refer to public ActionResult<AuthorDto> GetAuthor(Guid authorId) Name property),
               route value (new { function parameter = courseToReturn's property }, response body) */

            // to get 201 Created response with the newly created author in the response body
            return CreatedAtRoute("GetAuthor",
                new { authorId = authorToReturn.Id},
                authorToReturn);
        }

        [HttpOptions]
        public IActionResult GetAuthorsOptions()
        {
            Response.Headers.Add("Allow", "GET, OPTION, POST");
            return Ok();
        }
    }
}
