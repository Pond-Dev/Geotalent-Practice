using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using DotnetRpg.Dto;
using DotnetRpg.Services.CharactersService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Serilog;

namespace DotnetRpg.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CharactersController : ControllerBase
    {

        private readonly ICharactersService _charactersService;

        public CharactersController(ICharactersService charactersService)
        {

            _charactersService = charactersService;
        }

        [Authorize]
        [HttpGet]
        public async Task<ActionResult<List<GetCharacterDto>>> GetAllCharacters()
        {
            var characters = await _charactersService.GetAll();
            return Ok(characters);
        }

        [HttpGet("{charactersId}")]
        public async Task<ActionResult> GetById(string charactersId)
        {
            Log.Information($"Get by Id called with {charactersId}");
            var character = await _charactersService.GetById(charactersId);

            return Ok(character);
        }

        [HttpPost]
        public async Task<ActionResult<List<GetCharacterDto>>> CreateCharacter([FromBody] CharacterDto characterDto)
        {

            var characters = await _charactersService.Add(characterDto);
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState.Values);
            }
            return Ok(characters);
        }

        [HttpPut("{characterId}")]
        public async Task<ActionResult> UpdateCharacter(string characterId, [FromBody] CharacterDto characterDto)
        {
            var updateCharacter = await _charactersService.Edit(characterId, characterDto);
            // GetCharacterDto updateCharacter = new()
            // {
            //     Id = int.Parse(characterId),
            //     Name = characterDto.Name,
            //     Class = characterDto.Class,
            //     Blood = characterDto.Blood,
            //     Mana = characterDto.Mana,
            //     Hit = characterDto.Hit,
            //     Strength = characterDto.Strength

            // };
            return Ok(updateCharacter);

        }


        [HttpDelete("{characterId}")]
        public async Task<ActionResult<ServiceResponse<bool>>> DeleteCharacter(string characterId)
        {
            var result = await _charactersService.Delete(characterId);

            if (result.Code == ErrorCode.NoDataFound)
            {
                return NotFound(result.Message);
            }
            return Ok(result.Data);
        }
    }
}