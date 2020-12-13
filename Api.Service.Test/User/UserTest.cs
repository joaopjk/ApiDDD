using System;
using Api.Domain.Dtos.User;
using System.Collections.Generic;

namespace Api.Service.Test.User
{
    public class UserTest
    {
        public static string NomeUser { get; set; }
        public static string EmailUser { get; set; }
        public static string NomeUserAlterado { get; set; }
        public static string EmailUserAlterado { get; set; }
        public static Guid IdUser { get; set; }
        public List<UserDto> listaUserDto = new List<UserDto>();
        public UserDto userDto;
        public UserDtoCreate userDtoCreate;
        public UserDtoCreateResult userDtoCreateResult;
        public UserDtoUpdate userDtoUpdate;
        public UserDtoUpdateResult userDtoUpdateResult;

        public UserTest()
        {
            IdUser = Guid.NewGuid();
            NomeUser = Faker.Name.FullName();
            EmailUser = Faker.Internet.Email();
            NomeUserAlterado = Faker.Name.FullName();
            EmailUserAlterado = Faker.Internet.Email();

            for (var i = 0; i < 10; i++)
            {
                var dto = new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Nome = Faker.Name.FullName(),
                    Email = Faker.Internet.Email()
                };
                listaUserDto.Add(dto);


            }
            userDto = new UserDto()
            {
                Id = IdUser,
                Nome = NomeUser,
                Email = EmailUser
            };
            userDtoCreate = new UserDtoCreate()
            {
                Nome = NomeUser,
                Email = EmailUser
            };
            userDtoCreateResult = new UserDtoCreateResult()
            {
                Id = IdUser,
                Nome = NomeUser,
                Email = EmailUser,
                CreateAt = DateTime.UtcNow
            };
             userDtoUpdate = new UserDtoUpdate()
            {
                Nome = NomeUser,
                Email = EmailUser
            };
            userDtoUpdateResult = new UserDtoUpdateResult()
            {
                Nome = NomeUser,
                Email = EmailUser,
                UpdateAt = DateTime.UtcNow
            };
        }
    }
}