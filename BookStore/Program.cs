using BookStore.DB;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "Book Store API",
        Description = "Best BookStore in SA",
        Version = "v1"
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Book Store API v1"));
}

// Get all bookstores
app.MapGet("/bookstores", () => BookStoreDB.GetBookStores());

// Get a bookstore by ID
app.MapGet("/bookstores/{id}", (int id) =>
{
    var store = BookStoreDB.GetBookStoreById(id);
    return store is not null ? Results.Ok(store) : Results.NotFound();
});

// Create a new bookstore
app.MapPost("/bookstores", (BookStoreModel bookStore) =>
{
    var created = BookStoreDB.CreateBookStore(bookStore);
    return Results.Created($"/bookstores/{created.Id}", created);
});

// Update an existing bookstore
app.MapPut("/bookstores/{id}", (int id, BookStoreModel bookStore) =>
{
    bookStore.Id = id;
    var updated = BookStoreDB.UpdateBookStore(bookStore);
    return Results.Ok(updated);
});
// Delete a bookstore by ID
app.MapDelete("/bookstores/{id}", (int id) =>
{
    var store = BookStoreDB.GetBookStoreById(id);
    if (store is null)
        return Results.NotFound($"Bookstore with ID {id} not found.");

    BookStoreDB.RemoveBookStore(id);
    return Results.Ok($"Bookstore with ID {id} deleted.");
});
// PATCH: Partial update (e.g., just update the name)
app.MapPatch("/bookstores/{id}", (int id, string name) =>
{
    var store = BookStoreDB.GetBookStoreById(id);
    if (store is null)
        return Results.NotFound($"Bookstore with ID {id} not found.");

    var updated = store with { Name = name };
    BookStoreDB.UpdateBookStore(updated);
    return Results.Ok(updated);
});

// HEAD: Check if a bookstore exists (no body)
app.MapMethods("/bookstores/{id}", new[] { "HEAD" }, (int id) =>
{
    var exists = BookStoreDB.GetBookStoreById(id) is not null;
    return exists ? Results.Ok() : Results.NotFound();
});

// OPTIONS: List allowed HTTP methods
app.MapMethods("/bookstores", new[] { "OPTIONS" }, () =>
{
    const string allowed = "GET, POST, PUT, PATCH, DELETE, OPTIONS, HEAD";
    return Results.Text(allowed, "text/plain");
});


app.Run();
