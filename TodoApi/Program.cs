using Microsoft.EntityFrameworkCore;
using TodoApi; // ודאי שזה השם של ה-Namespace שלך

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoDbContext>();
builder.Services.AddCors(options =>
{
options.AddPolicy("AllowAll",
builder =>
{
builder.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader();
});
});
var app = builder.Build();
if (app.Environment.IsDevelopment()){app.UseSwagger();app.UseSwaggerUI();}
app.UseCors("AllowAll");
// 1. שליפת כל המשימות (GET):
app.MapGet("/items", async (ToDoDbContext db) =>
await db.Items.ToListAsync());

// app.MapGet("/", () => "Hello World!");
// 2. הוספת משימה חדשה (POST):
app.MapPost("/items", async (ToDoDbContext db, Item newItem) => {
db.Items.Add(newItem);
await db.SaveChangesAsync();
return Results.Created($"/items/{newItem.Id}", newItem);
});

// 3. עדכון משימה (PUT):
// app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item inputItem) => {
// var item = await db.Items.FindAsync(id);
// if (item is null)

//  return Results.NotFound();

// });
app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item inputItem) => {
var item = await db.Items.FindAsync(id);
if (item is null) return Results.NotFound();
item.Name = inputItem.Name;
item.IsComplete = inputItem.IsComplete;
await db.SaveChangesAsync();
return Results.NoContent();
});
// 4. מחיקת משימה (DELETE):
// app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) => {
// var item = await db.Items.FindAsync(id);
// if (item is null) return Results.NotFound();

// });
app.MapDelete("/items/{id}", async (ToDoDbContext db, int id) => {
// 1. חיפוש המשימה לפי ה-ID שהתקבל בכתובת
var item = await db.Items.FindAsync(id);
if (item is null)
{
return Results.NotFound();
}

db.Items.Remove(item);
await db.SaveChangesAsync();

return Results.Ok(item);
});
app.Run();
