
using System;
using System.Data.Entity;
using System.Data.Entity.Migrations;
using System.Linq;

namespace Workflow.Migrations
{
    internal sealed class Configuration : DbMigrationsConfiguration<WorkflowSampleE6Last.Models.MyContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = true;
        }
    } 
}