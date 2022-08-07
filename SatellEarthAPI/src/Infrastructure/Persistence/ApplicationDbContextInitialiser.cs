using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Npgsql;
using SatellEarthAPI.Domain.Entities;
using SatellEarthAPI.Infrastructure.Common;
using SatellEarthAPI.Infrastructure.Identity;

namespace SatellEarthAPI.Infrastructure.Persistence;

public class ApplicationDbContextInitialiser
{
    private readonly ILogger<ApplicationDbContextInitialiser> _logger;
    private readonly ApplicationDbContext _context;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public ApplicationDbContextInitialiser(ILogger<ApplicationDbContextInitialiser> logger, ApplicationDbContext context, UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task InitialiseAsync()
    {
        try
        {
            //if (_context.Database.IsSqlServer())
            if (_context.Database.IsNpgsql())
            {
                await _context.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initialising the database.");
            throw;
        }
    }

    public async Task SeedAsync()
    {
        try
        {
            await TrySeedAsync();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while seeding the database.");
            throw;
        }
    }

    public async Task TrySeedAsync()
    {

        _context.Database.OpenConnection();
        //Reload types after Postgis extension creation
        ((NpgsqlConnection)_context.Database.GetDbConnection()).ReloadTypes();

        // Default roles
        var administratorRole = new IdentityRole("Administrator");

        if (_roleManager.Roles.All(r => r.Name != administratorRole.Name))
        {
            await _roleManager.CreateAsync(administratorRole);
        }

        // Default users
        var administrator = new ApplicationUser { UserName = "administrator@localhost", Email = "administrator@localhost" };

        if (_userManager.Users.All(u => u.UserName != administrator.UserName))
        {
            await _userManager.CreateAsync(administrator, "Administrator1!");
            await _userManager.AddToRolesAsync(administrator, new[] { administratorRole.Name });
        }

        // Default data
        // Seed, if necessary
        if (!_context.TodoLists.Any())
        {
            _context.TodoLists.Add(new TodoList
            {
                Title = "Todo List",
                Items =
            {
                new TodoItem { Title = "Make a todo list 📃" },
                new TodoItem { Title = "Check off the first item ✅" },
                new TodoItem { Title = "Realise you've already done two things on the list! 🤯"},
                new TodoItem { Title = "Reward yourself with a nice, long nap 🏆" },
                new TodoItem { Title = "Ok let's go 🏆" },
            }
            }); 

            await _context.SaveChangesAsync();
        }

        if (!_context.Aleas.Any())
        {
            _context.Aleas.Add(new Alea { Title = "Foudre", Legend = "Foudre" });
            _context.Aleas.Add(new Alea
            {
                Title = "Feu",
                Legend = "Feux de forêts",
                Disasters =
            {
                new Disaster
                {
                    PremierReleve = DateTime.Now.AddDays(-10).SetKindUtc(),
                    DernierReleve = DateTime.Now.AddDays(-5).SetKindUtc(),
                    LienSource = "https://gdacs.com",
                    Visible = true,
                    NbRessenti = 1,
                    Point = new NetTopologySuite.Geometries.Point(-5,5)
                },
                new Disaster
                {
                    PremierReleve = DateTime.Now.AddDays(-27).SetKindUtc(),
                    DernierReleve = DateTime.Now.SetKindUtc(),
                    LienSource = "https://usgs.com",
                    Visible = false,
                    NbRessenti = 2500,
                    Point = new NetTopologySuite.Geometries.Point(0,10)
                }
            }
            });
            _context.Aleas.Add(new Alea { Title = "Tsunami", Legend = "Tsunami et submersion" });
            _context.Aleas.Add(new Alea
            {
                Title = "Seisme",
                Legend = "Seisme",
                Disasters = { new Disaster
            {
                PremierReleve = DateTime.Now.AddDays(-30).SetKindUtc(),
                DernierReleve = DateTime.Now.SetKindUtc(),
                LienSource = "https://satellearth.com",
                Visible = true,
                NbRessenti = 10,
                Point = new NetTopologySuite.Geometries.Point(10,10)
            } }
            });

            await _context.SaveChangesAsync();
        }

        _context.Database.CloseConnection();

    }
}