using GalytixAssessment.Data;
using GalytixAssessment.Domain.Entities;
using Microsoft.AspNetCore.Mvc;

namespace GalytixAssessment.Controllers
{
    [ApiController]
    [Route("server/api/gwp")]
    public class CountryGwpController : ControllerBase
    {
        private readonly IGwpRepository _repository;

        public CountryGwpController(IGwpRepository repository)
        {
            _repository = repository;
        }

        /// <summary>
        /// Gets the average gross written premium (GWP) for a country and lines of business over a specified period.
        /// </summary>
        /// <param name="request">Request containing the country and list of lines of business.</param>
        /// <returns>A dictionary with LOBs as keys and their average GWP as values.</returns>
        /// <response code="200">Returns the average GWP data.</response>
        /// <response code="400">If the request is invalid.</response>
        [HttpPost("avg")]
        [ProducesResponseType(typeof(Dictionary<string, double>), 200)]
        [ProducesResponseType(typeof(string), 400)]
        public async Task<IActionResult> GetAverageGwp([FromBody] GwpRequest request)
        {
            if (request == null || string.IsNullOrEmpty(request.Country) || request.Lob == null || !request.Lob.Any())
                return BadRequest("Invalid request");

            var result = await _repository.GetAverageGwpAsync(request.Country.ToLower(), request.Lob);
            return Ok(result);
        }
    }
}
