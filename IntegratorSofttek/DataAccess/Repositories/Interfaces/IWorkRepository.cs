﻿using IntegratorSofttek.DTOs;
using IntegratorSofttek.Entities;

namespace IntegratorSofttek.DataAccess.Repositories.Interfaces
{
    public interface IWorkRepository : IRepository<Work> // Update interface name and entity type
    {
        public Task<List<WorkDTO>> GetAllWorks(int parameter); // Update method name
        public Task<WorkDTO> GetWorkById(int id, int parameter); // Update method name
        public Task<bool> DeleteWorkById(int id, int parameter); // Update method name
        public Task<bool> UpdateWork(WorkDTO workDTO, int id); // Update method name
        public Task<bool> InsertWork(WorkDTO workDTO);

    }
}
