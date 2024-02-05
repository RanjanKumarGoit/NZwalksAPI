using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NZwalksAPI.Data;
using NZwalksAPI.Models.Domain;
using NZWalksAPI.Models.DTO;
using NZWalksAPI.Repositories;

namespace NZWalksAPI.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class RegionsController : ControllerBase
    {
        //Here we have used the contructor to get the access of the data from the database.
        private readonly NZwalksDbContext dbContext;
        private readonly IRegionRepository regionRepository;

        public RegionsController(NZwalksDbContext dbContext, IRegionRepository regionRepository)
        {
            this.dbContext = dbContext;
            this.regionRepository = regionRepository;
        }

        //GET ALL REGIONS 
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //Sending data to user using database
            //var regions = dbContext.Regions.ToList();
            //return Ok(regions);

            //Transfer the data using DTOs
            //Get the data from the database
            //var regionsDomain = await dbContext.Regions.ToListAsync();

            // Call the Region Repository
            var regionsDomain = await regionRepository.GetAllAsync();


            //Map models to DTOs
            var regionsDto = new List<RegionDTO>();

            foreach (var regionDomain in regionsDomain)
            {
                regionsDto.Add(new RegionDTO()
                {
                    Id = regionDomain.Id,
                    Code = regionDomain.Code,
                    Name = regionDomain.Name,
                    RegionImageUrl = regionDomain.RegionImageUrl,

                });
            }

            //return the Region DTO back to Client
            return Ok(regionsDto);
        }

        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetById([FromRoute] Guid id)
        {
            // first method to fetch the single region details with the help of id
            //var region = dbContext.Regions.Find(id);


            //second method to fetch the single region details with the help of id
            //var region = dbContext.Regions.FirstOrDefault(x => x.Id == id); 
            //if (region == null)
            //{
            //    return NotFound();
            //}

            //return Ok(region);

            //var regionDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);

            //using the repository

            var regionDomain = await regionRepository.GetByIdAsync(id);
            if (regionDomain == null)
            {
                return NotFound();
            }

            var regionDto = new List<RegionDTO>();
            regionDto.Add(new RegionDTO()
            {
                Id = regionDomain.Id,
                Code = regionDomain.Code,
                Name = regionDomain.Name,
                RegionImageUrl = regionDomain.RegionImageUrl,
            });

            return Ok(regionDto);
        }


        //POST To Create New Region

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AddRegionRequestDTO addRegionRequestDTO)
        {
            //Map or Convert DTO to Domain Model
            var regionDomainModel = new Region
            {
                Code = addRegionRequestDTO.Code,
                Name = addRegionRequestDTO.Name,
                RegionImageUrl = addRegionRequestDTO.RegionImageUrl,
            };


            //Use Domain Model to Create Region
            //await dbContext.Regions.AddAsync(regionDomainModel);
            //await dbContext.SaveChangesAsync();


            // Using Repository
            regionDomainModel = await regionRepository.CreateAsync(regionDomainModel);

            //Map Domain Model back to DTO
            var regionDto = new Region
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,

            };
            return CreatedAtAction(nameof(GetById), new { id = regionDto.Id }, regionDto);
        }


        [HttpPut]
        [Route("{id:Guid}")]
        public async Task<IActionResult> Update([FromRoute] Guid id, [FromBody] UpdateRegionRequestDTO updateRegionRequestDTO)
        {

            //Using DbContext
            //var regionDomainModel = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);


            var regionDomainModel = new Region
            {
                Code = updateRegionRequestDTO.Code,
                Name = updateRegionRequestDTO.Name,
                RegionImageUrl = updateRegionRequestDTO.RegionImageUrl,
            };

            //Using the Repository
            regionDomainModel = await regionRepository.UpdateAsync(id, regionDomainModel);

            //check if region exists in the database
            if (regionDomainModel == null)
            {
                return NotFound();
            }

            //Map DTO to Domain Model
            //regionDomainModel.Code = updateRegionRequestDTO.Code;
            //regionDomainModel.Name = updateRegionRequestDTO.Name;
            //regionDomainModel.RegionImageUrl = updateRegionRequestDTO.RegionImageUrl;

            //await dbContext.SaveChangesAsync();
            
            //Convert Domain Model to DTO
            var regionDto = new RegionDTO
            {
                Id = regionDomainModel.Id,
                Code = regionDomainModel.Code,
                Name = regionDomainModel.Name,
                RegionImageUrl = regionDomainModel.RegionImageUrl,
            };
            return Ok(regionDto);
        }

        
        
        
        
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id)
        {

            //var regionModelDomain = await dbContext.Regions.FirstOrDefaultAsync(x => x.Id == id);
            var regionModelDomain = await regionRepository.DeleteAsync(id);
            if (regionModelDomain == null)
            {
                return NotFound();
            }

            dbContext.Regions.Remove(regionModelDomain);
            await dbContext.SaveChangesAsync();

            //return the deleted regions back

            //Map Domain Model to DTO first

            var RegionDTO = new RegionDTO
            {
                Id = regionModelDomain.Id,
                Code = regionModelDomain.Code,
                Name = regionModelDomain.Name,
                RegionImageUrl = regionModelDomain.RegionImageUrl,
            };

            return Ok(RegionDTO);
        }
    }
}