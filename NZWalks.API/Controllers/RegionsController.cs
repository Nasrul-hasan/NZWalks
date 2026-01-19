using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NZWalks.API.Data;
using NZWalks.API.Models.Domain;
using NZWalks.API.Models.DTO;
namespace NZWalks.API.Controllers
{
    //http://localhost:5000/api/regions
    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        private readonly NZWalksDbContext dbContext;

        public RegionsController(NZWalksDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        // GET: api/Regions
        [HttpGet]
        public IActionResult GetAll()
        {
            //var regions = new List<Region>
            //{
            //    new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Wellington",
            //        Code = "WGN",
            //        RegionImageUrl = "https://www.expedia.ca/Wellington.dx6127327"
            //    },
            //     new Region
            //    {
            //        Id = Guid.NewGuid(),
            //        Name = "Auckland Regions",
            //        Code = "WGN",
            //        RegionImageUrl = "https://www.geocurrents.info/blog/2023/11/01/neighborhood-stereotypes-and-recent-voting-patterns-in-auckland-new-zealand/"
            //    }
            //};

            // 1.Get data from Database- Domain Model
            var regionsDomain = dbContext.Regions.ToList();
            // 2.Map Domain Modlesl to DTOs
            var regionsDto = new List<RegionDto>();

            foreach (var region in regionsDomain)
            {
                regionsDto.Add(new RegionDto()
                {
                    Id = region.Id,
                    Code = region.Code,
                    Name = region.Name,
                    RegionImageUrl = region.RegionImageUrl
                });
            }
            // 3.Return DTOs

            return Ok(regionsDto);
        }
        // Get region by id 
        // Get: http://localhost:5000/api/regions/{id}
        [HttpGet]
        [Route("{id:Guid}")]

        public IActionResult GetById([FromRoute] Guid id)
        {
            //  var region = dbContext.Regions.Find(id);
            var regionDomain = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            //1. Get region domain model from database
            // var regionDomain = dbContext.Regions.Find(id);
            //2. Convert/ Map region domain model to DTO
            if (regionDomain == null)
            {
                return NotFound();
            }
            var regionsDto = new List<RegionDto>();
            regionsDto.Add(new RegionDto()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl
            });
            //3. Return DTO
            return Ok(regionsDto);
        }

        // POST To Create a new region
        // POST: http://localhost:5000/api/regions
        [HttpPost]
        // solving undocumented response types warning
        [ProducesResponseType(typeof(RegionDto), StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public IActionResult Create([FromBody] AddRegionRequestDto addRegionRequestDto)
        {
            // mpa or convert DTO to Domain Moldel
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDto.Code,
                Name = addRegionRequestDto.Name,
                RegionImageUrl = addRegionRequestDto.RegionImageUrl
            };

            // Use domain model to create Region
            dbContext.Regions.Add(regionDomainModel);
            dbContext.SaveChanges();
            // Convert back domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            // Return DTO back to client
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);

        }
        // Update a region
        // PUT : http://localhost:5000/api/regions/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        public IActionResult Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDto updateRegionRequestDto)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            // check if region exists
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // update region domain model
            //1. Map DTO to Domain Model
            regionDomainModel.Code = updateRegionRequestDto.Code;
            regionDomainModel.Name = updateRegionRequestDto.Name;
            regionDomainModel.RegionImageUrl = updateRegionRequestDto.RegionImageUrl;
            //2. Save changes to database
            dbContext.SaveChanges();

            // 3. Convert back domain model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };

            return Ok(regionDto);

        }

        //Delete a region
        // DELETE : http://localhost:5000/api/regions/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        public IActionResult Delete([FromRoute] Guid id)
        {
            var regionDomainModel = dbContext.Regions.FirstOrDefault(x => x.Id == id);
            // check if region exists
            if (regionDomainModel == null)
            {
                return NotFound();
            }
            // delete region
            dbContext.Regions.Remove(regionDomainModel);
            dbContext.SaveChanges();
            // return deleted region back
            // map Domain Model to DTO
            var regionDto = new RegionDto
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl
            };
            return Ok(regionDto);
        }
    }
}