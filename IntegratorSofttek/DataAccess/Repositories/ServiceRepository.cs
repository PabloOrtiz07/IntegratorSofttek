﻿using IntegratorSofttek.DataAccess.Repositories.Interfaces;
using IntegratorSofttek.DataAccess.Repositories;
using IntegratorSofttek.DataAccess;
using IntegratorSofttek.Entities;

namespace IntegratorSofttek.DataAccess.Repositories
{
    public class ServiceRepository : Repository<Service>, IServiceRepository
    {
        public ServiceRepository(ContextDB contextDB) : base(contextDB)
        {

        }
        public virtual async Task<bool> Update(Service updatedService)
        {
            try
            {
                var serviceFinding = await GetById(updatedService.Id);

                if (serviceFinding != null)
                {
                    _contextDB.Update(updatedService); 
                    return true;
                }

                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public override async Task<bool> DeleteSoftById(int id)
        {
            Service service = await GetById(id);
            if (service != null)
            {
                service.IsDeleted = true;
                service.DeletedTimeUtc = DateTime.UtcNow;

                return true;
            }

            return false;
        }

    }
}
