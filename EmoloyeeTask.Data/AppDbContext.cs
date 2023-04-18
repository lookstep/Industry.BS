﻿using Microsoft.EntityFrameworkCore;

namespace EmoloyeeTask.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {

        }
        public DbSet<Division> Divisions { get; set; }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Issue> Assignments { get; set; }
        public DbSet<LaborCost> LaborCosts { get; set; }
    }
}