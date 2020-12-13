using System;

namespace Api.Domain.Dtos.Models
{
    public class UserModel
    {
        private Guid _id;
         private DateTime _creatAt;
        private DateTime _updateAt;
         private string _name;
         private string _email;
        public Guid Id
        {
            get { return _id; }
            set { _id = value; }
        }
        
        public string Nome
        {
            get { return _name; }
            set { _name = value; }
        }
        
        public string Email
        {
            get { return _email; }
            set { _email = value; }
        }
       
        public DateTime CreatAt
        {
            get { return _creatAt; }
            set
            {
                _creatAt = value == null ? DateTime.UtcNow : value;
            }
        }

        public DateTime UpdateAt
        {
            get { return _updateAt; }
            set { _updateAt = value; }
        }        
    }
}