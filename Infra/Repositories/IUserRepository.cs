﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Infra.Data;
using Infra.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Infra.Repositories
{
    public interface IUserRepository
    {
        Task<ApplicationUser> FindByEmailAsync(string email);
        Task UpdateSignalR(string id, string connectionId);
        Task<string> FindConnectionIdAsync(string id);
    }


    public class UserRepository : IUserRepository
    {


        private readonly IConfiguration _configuration;


        public UserRepository(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ApplicationUser> FindByEmailAsync(string email)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT * FROM AspNetUsers WHERE Email=@Email";
                return await db.QueryFirstOrDefaultAsync<ApplicationUser>(sql, new { @Email = email });
            }
        }

        public async Task UpdateSignalR(string id, string connectionId)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "UPDATE AspNetUsers SET ConnectionId=@ConnectionId WHERE Id = @Id ";
                await db.ExecuteAsync(sql);
            }
        }

        public async Task<string> FindConnectionIdAsync(string id)
        {
            using (IDbConnection db = new SqlConnection(_configuration.GetConnectionString("DefaultConnection")))
            {
                var sql = "SELECT ConnectionId FROM AspNetUsers WHERE Id = @Id ";
                var a =  db.QueryFirstOrDefaultAsync(sql, new { @Id = id }).ToString();
                return a;
            }
        }
    }
}
