using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotnetRpg.Core;
using DotnetRpg.Dto;
using DotnetRpg.Services.Settings;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Serilog;
using System.Data;
using System.Security.Claims;

namespace DotnetRpg.Services.CharactersService
{
    public class CharactersService : ICharactersService
    {

        private readonly ConnectionStrings _connectionStrings;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CharactersService(ISettingsSingletonService settingsSingletonService, IHttpContextAccessor httpContextAccessor)
        {

            _connectionStrings = settingsSingletonService.ConnectionStrings;
            _httpContextAccessor = httpContextAccessor;
        }
        private static List<GetCharacterDto> characters = new()
        {
            new GetCharacterDto() {Id=0,Name="Pond1",Class = Core.JobClass.Knight},
            new GetCharacterDto() {Id=1,Name="Pond2",Class = Core.JobClass.Archer},
            new GetCharacterDto() {Id=2,Name="Pond3",Class = Core.JobClass.Assassin},
            new GetCharacterDto() {Id=3,Name="Pond4",Class = Core.JobClass.Mage},
        };

        public IOptions<JwtSettings> JwtSettings { get; }

        public async Task<List<GetCharacterDto>?> Add(CharacterDto characterDto)
        {
            await Task.Delay(5);
            int nextId = characters.Max(c => c.Id) + 1;
            GetCharacterDto newCharacter = new()
            {
                Id = nextId,
                Name = characterDto.Name,
                Class = characterDto.Class,
                Blood = characterDto.Blood,
                Mana = characterDto.Mana,
                Hit = characterDto.Hit,
                Strength = characterDto.Strength

            };
            characters.Add(newCharacter);
            return characters;
        }

        public async Task<ServiceResponse<bool>> Delete(string characterId)
        {
            ServiceResponse<bool> result = new();

            await Task.Delay(5);
            try
            {
                var character = characters.SingleOrDefault(c => c.Id == int.Parse(characterId));
                if (character != null)
                {
                    characters.Remove(character);
                    result.Data = true;
                }
                else
                {
                    result.Success = false;
                    result.Code = ErrorCode.NoDataFound;
                    result.Message = "No Data Found";
                }
            }
            catch (System.Exception)
            {
                throw;
            }

            return result;


        }

        public async Task<GetCharacterDto?> Edit(string characterId, CharacterDto characterDto)
        {
            await Task.Delay(5);
            var character = characters.SingleOrDefault(c => c.Id == int.Parse(characterId));
            if (character != null)
            {

                character.Name = characterDto.Name;
                character.Class = characterDto.Class;
                character.Blood = characterDto.Blood;
                character.Mana = characterDto.Mana;
                character.Hit = characterDto.Hit;
                character.Strength = characterDto.Strength;
            };
            return character;
        }

        public async Task<List<GetCharacterDto>?> GetAll()
        {

            List<GetCharacterDto>? result = null;

            int? userId = GetUserId();
            Log.Debug("User Id is {userId}", userId);
            using (SqlConnection conn = new(_connectionStrings.DotNetRpgDatabase))
            {
                await conn.OpenAsync();
                try
                {
                    using (SqlCommand command = conn.CreateCommand())
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.CommandText = "[dot].[GET_ALL_CHARACTERS]";
                        SqlDataReader reader = await command.ExecuteReaderAsync();
                        result = new();
                        while (await reader.ReadAsync())
                        {
                            result.Add(new GetCharacterDto()
                            {
                                Id = reader.GetFieldValue<int>("Id"),
                                Name = reader.GetFieldValue<string>("Name"),
                                Blood = reader.GetFieldValue<int>("Blood"),
                                Mana = reader.GetFieldValue<int>("Mana"),
                                Hit = reader.GetFieldValue<int>("Hit"),
                                Strength = reader.GetFieldValue<int>("Strength"),
                                Class = (JobClass)reader.GetFieldValue<int>("Class"),
                            });
                        }
                        await reader.CloseAsync();
                    }
                }
                finally
                {
                    await conn.CloseAsync();
                }

            }
            return result;
        }

        public async Task<GetCharacterDto?> GetById(string id)
        {
            await Task.Delay(5);
            var character = characters.SingleOrDefault(c => c.Id == int.Parse(id));
            return character;
        }

        private int? GetUserId()
        {
            if (_httpContextAccessor.HttpContext != null)
            {
                return int.Parse(_httpContextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier));
            }
            else
            {
                return null;
            }
        }
    }
}