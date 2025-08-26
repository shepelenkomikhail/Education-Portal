using EducationPortal.Data.Models;

namespace EducationPortal.App;

class Program
{
    static void Main(string[] args)
    {
        string connStr = @"Data Source=EducationPortal.db";
        PortalDbContext db_ctx = new PortalDbContext(connStr);
    }
}