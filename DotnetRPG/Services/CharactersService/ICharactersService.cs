using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using DotnetRpg.Dto;

namespace DotnetRpg.Services.CharactersService
{
    public interface ICharactersService
    {
        Task<List<GetCharacterDto>?> GetAll();
        Task<GetCharacterDto?> GetById(string id);
        Task<List<GetCharacterDto>?> Add(CharacterDto characterDto);
        Task<GetCharacterDto?> Edit(string characterId,CharacterDto characterDto);
        Task<ServiceResponse<bool>> Delete(string characterId);
    }
}