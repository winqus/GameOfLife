using GameOfLifeApp.Interfaces;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameOfLifeApp
{
    public class Worker : BackgroundService
    {
        private readonly IGame _game;

        public Worker(IGame game)
        {
            _game = game;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _game.StartGame();
            _game.RunGame();
            await Task.Yield();
        }
    }
}
