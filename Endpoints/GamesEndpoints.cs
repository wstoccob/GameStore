using GameStore.Dtos;

namespace GameStore.Endpoints;

public static class GamesEndpoints
{
    const string GetGameEndpointName = "GetGame";

    private static readonly List<GameDto> games = [
        new GameDto(
            1,
            "Street Fighter II",
            "Fighting",
            21.99m,
            new DateOnly(1992, 7, 15)),
        new GameDto(
            2,
            "Final Fantasy XIV",
            "Roleplaying",
            59.99m,
            new DateOnly(2010, 9, 30)),
        new GameDto(
            3,
            "Third game",
            "SampleGenre",
            111.89m,
            new DateOnly(2020, 2, 9))
    ];

    public static RouteGroupBuilder MapGamesEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("games");
        
        // GET /games
        group.MapGet("/", () => games);

        // GET /games/1
        group.MapGet("/{id}", (int id) =>
            {
                var game = games.Find(game => game.Id == id);

                return game is null ? Results.NotFound() : Results.Ok(game);

            })
            .WithName(GetGameEndpointName);

        // POST /games
        group.MapPost("/", (CreateGameDto newGame) =>
        {
            var game = new GameDto(
                games.Count + 1,
                newGame.Name,
                newGame.Genre,
                newGame.Price,
                newGame.ReleaseDate);
            games.Add(game);

            return Results.CreatedAtRoute(GetGameEndpointName, new {id = game.Id}, game);
        });

        // PUT /games
        group.MapPut("/{id}", (int id, UpdateGameDto updatedGame) =>
        {
            var index = games.FindIndex(game => game.Id == id);

            if (index == -1)
            {
                return Results.NotFound();
            }
    
            games[index] = new GameDto(
                id,
                updatedGame.Name,
                updatedGame.Genre,
                updatedGame.Price,
                updatedGame.ReleaseDate
            );
            return Results.NoContent();
        });

        // DELETE /games/1
        group.MapDelete("/{id}", (int id) =>
        {
            games.RemoveAll(game => game.Id == id);

            return Results.NoContent();
        });

        return group;
    }
}