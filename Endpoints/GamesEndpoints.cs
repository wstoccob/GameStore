using System.Reflection.Metadata.Ecma335;
using GameStore.Data;
using GameStore.Dtos;
using GameStore.Entities;
using GameStore.Mapping;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameSummaryDto> games = [
        new GameSummaryDto(
            1,
            "Street Fighter II",
            "Fighting",
            21.99m,
            new DateOnly(1992, 7, 15)),
        new GameSummaryDto(
            2,
            "Final Fantasy XIV",
            "Roleplaying",
            59.99m,
            new DateOnly(2010, 9, 30)),
        new GameSummaryDto(
            3,
            "Third game",
            "SampleGenre",
            111.89m,
            new DateOnly(2020, 2, 9))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games")
            .WithParameterValidation();

        // GET /games
        group.MapGet("/", (GameStoreContext dbContext) => 
            dbContext.Games.Any() ? Results.Ok(dbContext.Games) : Results.NoContent());

        // GET /games/1
        group.MapGet("/{id}", (int id, GameStoreContext dbContext) =>
            {
                var game = dbContext.Games.Find(id);
                
                // GameSummaryDto gameSummaryDto = 

                return game is null ? 
                    Results.NotFound() : Results.Ok(game.ToGameDetailsDto());

            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame, GameStoreContext dbContext) =>
        {
            Game game = newGame.ToEntity();

            dbContext.Games.Add(game);
            dbContext.SaveChanges();

            return Results.CreatedAtRoute(GetGameEndpointName, 
                new { id = game.Id }, 
                game.ToGameDetailsDto());
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame, GameStoreContext dbContext) =>
        {
            var existingGame = dbContext.Games.Find(id);

            if (existingGame is null)
            {
                return Results.NotFound();
            }
            
            dbContext.Entry(existingGame)
                .CurrentValues.SetValues(updatedGame.ToEntity(id));
            dbContext.SaveChanges();
            
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id, GameStoreContext dbContext) =>
        {
            Game? gameToRemove = dbContext.Games.Find(id);
            dbContext.Games.Remove(gameToRemove);
            dbContext.SaveChanges();
            return Results.NoContent();
        });

        return group;
    }
}