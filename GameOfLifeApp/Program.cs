using GameOfLifeApp.Interfaces;
using GameOfLifeApp.Services;
using GameOfLifeApp;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
        services.AddHostedService<Worker>()
            .AddScoped<IGame, Game>()
            .AddScoped<ICellArena, CellArena>()
            .AddScoped<ISaver, GameToXMLSaver>()
            .AddScoped<IGameConsole, GameConsole>()
            .AddScoped<IGameFileManager, GameFileManager>()
            .AddScoped<IVisualize, VisualizeSymbols>(x =>
                new VisualizeSymbols(x.GetRequiredService<IGameConsole>())
                {
                    CharacterForDeadCell = "◌",
                    CharacterForLiveCell = "●",
                    ClearConsoleEachPrint = true
                })
    ).Build();

host.Run();