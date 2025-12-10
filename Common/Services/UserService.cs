using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Entities;
using Microsoft.AspNetCore.Http;

namespace Common.Services
{
    public class UserService : BaseService<User>
    {
        public User CreateUser(User user, IFormFile photo)
        {
            PhotoService service = new PhotoService();

            string path = service.SavePhoto(photo);

            user.Profile_Photo = path;

            Save(user);

            return user;
        }
    }
}
