using DatabaseCreation.Database;
using DatabaseCreation.Entities;
using System;

namespace DatabaseCreation
{
    class Program
    {
        static void Main(string[] args)
        {
            var departments = new[]
            {
                "Marketing",
                "Finance",
                "Helpdesk",
                "R&D",
                "HR"
            };

            var providers = new[]
            {
                "Rafiki",
                "Abu",
                "Jack",
                "Chim Chim",
                "Donkey Kong"
            };

            using (var context = new DatabaseContext())
            {
                foreach (var department in departments)
                {
                    var dp = new DepartmentEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = department
                    };

                    context.Departments.Add(dp);
                }

                foreach (var provider in providers)
                {
                    var p = new ProvidersEntity
                    {
                        Id = Guid.NewGuid().ToString(),
                        Name = provider
                    };

                    context.Providers.Add(p);
                }

                context.SaveChanges();
            }
        }
    }
}
