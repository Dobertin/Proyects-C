﻿using Facturacion.Data;
using Facturacion.Models;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Facturacion.Repositories
{
    public class RolRepository
    {
        private readonly IMongoCollection<Rol> _roles;

        public RolRepository(MongoDBContext context)
        {
            _roles = context.Database.GetCollection<Rol>("Roles");
        }

        public async Task<List<Rol>> GetAllAsync()
        {
            return await _roles.Find(rol => true).ToListAsync();
        }

        public async Task<Rol> GetByIdAsync(string id)
        {
            return await _roles.Find<Rol>(rol => rol.Id == id).FirstOrDefaultAsync();
        }

        public async Task AddAsync(Rol rol)
        {
            await _roles.InsertOneAsync(rol);
        }

        public async Task UpdateAsync(string id, Rol rolIn)
        {
            await _roles.ReplaceOneAsync(rol => rol.Id == id, rolIn);
        }

        public async Task DeleteAsync(string id)
        {
            await _roles.DeleteOneAsync(rol => rol.Id == id);
        }
    }
}