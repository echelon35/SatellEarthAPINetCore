using System.Text.Json;
using GeoJSON.Net.Geometry;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Npgsql;
using SatellEarthAPI.Domain.Entities;
using SatellEarthAPI.Domain.ValueObjects;
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

        }

        var gf = NetTopologySuite.NtsGeometryServices.Instance.CreateGeometryFactory(4326);

        if (!_context.Aleas.Any())
        {

            string geojson1 = @"{
                ""type"": ""Point"",
                ""coordinates"": [
                    4.57031,
                    47.3983
                ]
            }";

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
                    Point = JsonConvert.DeserializeObject<Point>(geojson1)
        },
                new Disaster
                {
                    PremierReleve = DateTime.Now.AddDays(-27).SetKindUtc(),
                    DernierReleve = DateTime.Now.SetKindUtc(),
                    LienSource = "https://usgs.com",
                    Visible = false,
                    NbRessenti = 2500,
                    Point = JsonConvert.DeserializeObject<Point>(geojson1)
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
                Point = JsonConvert.DeserializeObject<Point>(geojson1)
            } }
            });

        }

        if (!_context.Pays.Any())
        {
            string geojson = "{\"type\":\"Polygon\",\"coordinates\":[[[-63.001220703,18.221777344],[-63.160009766,18.171386719],[-63.153320313,18.200292969],[-63.026025391,18.269726562],[-62.979589844,18.264794922],[-63.001220703,18.221777344]]]}";
            _context.Pays.Add(new Pays
            {
                Surface = JsonConvert.DeserializeObject<MultiPolygon>(geojson),
                NameFr = "Anguilla",
                NameUs = "Anguilla",
                Trigramme = "AIA"
            });

            string geojson2 = @"{
                ""type"": ""Polygon"",
                ""coordinates"": [
                    [
                    [
                        0.87890625,
                        50.680797145321655
                    ],
                    [
                        -4.5703125,
                        47.87214396888731
                    ],
                    [
                        -0.791015625,
                        43.004647127794435
                    ],
                    [
                        4.306640625,
                        42.09822241118974
                    ],
                    [
                        7.734374999999999,
                        43.45291889355465
                    ],
                    [
                        6.591796875,
                        46.86019101567027
                    ],
                    [
                        8.4375,
                        49.439556958940855
                    ],
                    [
                        0.87890625,
                        50.680797145321655
                    ]
                    ]
                ]
            }";

            _context.Pays.Add(new Pays
            {
                Surface = JsonConvert.DeserializeObject<MultiPolygon>(geojson2),
                NameFr = "France",
                NameUs = "France",
                Trigramme = "FRA"
            });
        }

        await _context.SaveChangesAsync();

        _context.Database.CloseConnection();

    }
}