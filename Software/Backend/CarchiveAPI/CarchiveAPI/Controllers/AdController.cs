using CarchiveAPI.Dto;
using CarchiveAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Claims;
using System.Xml;
using Formatting = Newtonsoft.Json.Formatting;

namespace CarchiveAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdController : Controller
    {
        private readonly AdServices _adServices;
        public AdController(AdServices adServices)
        {
            this._adServices = adServices;
        }

        [HttpGet]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(ICollection<AdDto>))]
        public IActionResult GetAds()
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var ads = _adServices.GetAds(email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok(ads);

        }

        [HttpGet("index/{companyId}")]
         [Produces("application/xml")]
        [ProducesResponseType(200, Type = typeof(ICollection<IndexAdDto>))]
        public IActionResult GetIndexAds(int companyId)
        {
            var ads = _adServices.GetIndexAds(companyId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var jsonString = JsonConvert.SerializeObject(new { ad_item = ads });
            Console.WriteLine(JsonConvert.SerializeObject(ads, Formatting.Indented));
            XmlDocument doc = JsonConvert.DeserializeXmlNode(jsonString, "ad-list");
            return Ok(doc);
        }



        [HttpPost]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult AddAd([FromBody] AdDto newAdDto, [FromQuery] int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _adServices.AddAd(newAdDto, email, id);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!result)
            {
                return BadRequest("Failed to add ad.");
            }
            return Ok("Ad added!");
        }

        [HttpPut]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult UpdateAd([FromBody] AdDto adDto)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _adServices.UpdateAd(adDto, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!result)
            {
                return BadRequest("Failed to update ad.");
            }
            return Ok("Ad updated!");
        }

        [HttpDelete]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(bool))]
        public IActionResult DeleteAd([FromQuery] int id)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var result = _adServices.DeleteAd(id, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (!result)
            {
                return BadRequest("Failed to delete ad.");
            }
            return Ok("Ad deleted!");
        }

        [HttpGet("{adId}")]
        [Authorize(Roles = "Admin, User")]
        [ProducesResponseType(200, Type = typeof(AdDto))]
        public IActionResult GetAd(int adId)
        {
            var email = User.FindFirst(ClaimTypes.Name)?.Value;
            var ad = _adServices.GetAd(adId, email);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (ad == null)
            {
                return BadRequest("Ad not found.");
            }
            return Ok(ad);
        }
    }
}
