using System;

namespace Api.Domain.Dtos.User
{
    public class UserDtoUpdateResult
    {
         public Guid id {get;set;}
        public string Nome {get;set;}
        public string Email {get;set;}
        public DateTime UpdateAt {get;set;}
    }
}