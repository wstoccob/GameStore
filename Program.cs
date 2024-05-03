using GameStore.Dtos;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

List<GameDto> games = [
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

// GET /games
app.MapGet("games", () => games);

// GET /games/1
app.MapGet("games/{id}", (int id) => games.Find(game => game.Id == id));

app.Run();