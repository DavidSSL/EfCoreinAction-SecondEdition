// Copyright (c) 2020 Jon P Smith, GitHub: JonPSmith, web: http://www.thereformedprogrammer.net/
// Licensed under MIT license. See License.txt in the project root for license information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MyFirstEfCoreApp
{
    public class AppDbContext : DbContext
    {
        private readonly string connectionString ; //#A

        public AppDbContext()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.Development.json")
                .AddUserSecrets<Program>()
                .Build();

            connectionString = config.GetConnectionString("MyFirstDb");
        }

        public DbSet<Book> Books { get; set; }

        protected override void OnConfiguring(
            DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString); //#B
        }
    }

    /********************************************************
    #A The connection string is used by the SQL Server database provider to find the database
    #B Using the SQL Server database provider’s UseSqlServer command sets up the options ready for creating the applications’s DBContext
     ********************************************************/
}
