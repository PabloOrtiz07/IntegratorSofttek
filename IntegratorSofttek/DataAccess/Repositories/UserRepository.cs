﻿using IntegratorSofttek.DataAccess.Repositories.Interfaces;
using IntegratorSofttek.DTOs;
using IntegratorSofttek.Entities;
using Microsoft.EntityFrameworkCore;
using System.Xml.Linq;
using System.Linq;
using IntegratorSofttek.Helper;
using System.Collections.Generic;

namespace IntegratorSofttek.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(ContextDB contextDB) : base(contextDB)
        {

        }

        public  async Task<bool> UpdateUser(User user,int id)
        {
            try
            {
                var userFinding = await GetById(id);
                if (userFinding != null) {
                    _contextDB.Update(user);
                    return true;

                }
                return false;
            }
            catch (Exception)
            {
                
                return false;
            }
        }


        public virtual async Task<List<User>> GetAllUsers(int parameter)
        {
            try
            {
                if (parameter == 0)
                {
                    List<User> users = await _contextDB.Users
                        .Include(user => user.Role)
                        .Where(user => user.IsDeleted != true)
                        .ToListAsync();

                    return users;
                }
                else if (parameter == 1)
                {
                    List<User> users = await _contextDB.Users.Include(user => user.Role).ToListAsync();
                    return users;
                }

                return null;
            }
            catch (Exception)
            {
                return null;
            }
        }


        public async Task<User> GetUserById(int id, int parameter)
        {
            try
            {
                User user = await _contextDB.Users
                         .Include(u => u.Role)
                         .Where(u => u.Id == id)
                         .FirstOrDefaultAsync();

                if (user.IsDeleted != true && parameter == 0)
                {
                    return user;
                }
                if (parameter == 1)
                {
                    return user;
                }
                return null;

            }
            catch (Exception)
            {
                return null;
            }
        }


        public  async Task<bool> DeleteUserById(int id,int parameter)
        {

            try
            {
                User user = await GetById(id);
                if (user != null && parameter == 0)
                {
                    user.IsDeleted = true;
                    user.DeletedTimeUtc = DateTime.UtcNow;

                    return true;
                }
                if (user != null && parameter == 1)
                {
                    _contextDB.Users.Remove(user);
                    return true;
                }

                return false;
            }
            catch(Exception) {
                return false;
            }

        }


        public async Task<User?> AuthenticateCredentials(AuthenticateDto dto)
        {

            try
            {
                return await _contextDB.Users.Include(user => user.Role).SingleOrDefaultAsync
                              (user => user.Email == dto.Email && user.Password == PasswordEncryptHelper.EncryptPassword(dto.Password, dto.Email));
            }
            catch (Exception) {
                return null;
            }
           
        }
        public async Task<bool> UserExists(string email)
        {
            return await _contextDB.Users.AnyAsync(x => x.Email == email);
        }

    }
}