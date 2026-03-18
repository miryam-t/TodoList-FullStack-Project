using Microsoft.EntityFrameworkCore;
using TodoApi;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// --- 1. הגדרות שירותים (Services) ---
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ToDoDbContext>();

// הגדרת CORS - מאפשר ל-React לתקשר עם השרת
builder.Services.AddCors(options =>
{
options.AddPolicy("AllowAll", b => 
b.AllowAnyOrigin()
.AllowAnyMethod()
.AllowAnyHeader());
});

// הגדרת אימות (JWT Authentication)
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
.AddJwtBearer(options =>
{
options.TokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidateAudience = true,
ValidateLifetime = true,
ValidateIssuerSigningKey = true,
ValidIssuer = builder.Configuration["Jwt:Issuer"],
ValidAudience = builder.Configuration["Jwt:Audience"],
IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
};
});

builder.Services.AddAuthorization(); // הוספת שירותי הרשאות
 // --- 2. בניית האפליקציה (Build) ---
var app = builder.Build();
// --- 3. הגדרות התנהגות (Middleware) ---
app.UseCors("AllowAll");
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication(); // הזדהות - מי אתה?
app.UseAuthorization();  // הרשאות - מה מותר לך לעשות?
// פונקציית עזר לחילוץ ה-ID של המשתמש מהטוקן
int GetUserId(ClaimsPrincipal user) => int.Parse(user.FindFirst(ClaimTypes.NameIdentifier)?.Value);

// --- 4. נתיבים (Endpoints) ---
// --- משימות (Todo Items) ---
app.MapGet("/items", async (ToDoDbContext db, ClaimsPrincipal user) =>
{
var userId = GetUserId(user); // חילוץ ה-ID
 // שליפת משימות - מסונן לפי המשתמש המחובר
var myTasks = await db.Items.Where(t => t.UserId == userId).ToListAsync();
return Results.Ok(myTasks);
}).RequireAuthorization();

// הוספת משימה חדשה והצמדתה למשתמש
app.MapPost("/items", async (ToDoDbContext db, Item newItem, ClaimsPrincipal user) =>
{
newItem.UserId = GetUserId(user); // הצמדת המשימה למשתמש המחובר
db.Items.Add(newItem);
await db.SaveChangesAsync();
return Results.Created($"/items/{newItem.Id}", newItem);
}).RequireAuthorization();

 // עדכון משימה קיימת (רק אם היא שייכת למשתמש)
app.MapPut("/items/{id}", async (ToDoDbContext db, int id, Item inputItem, ClaimsPrincipal user) =>
{
var userId = GetUserId(user);
var item = await db.Items.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
if (item is null) return Results.NotFound();
    item.Name = inputItem.Name;
    item.IsComplete = inputItem.IsComplete;
    await db.SaveChangesAsync();
    return Results.NoContent();
}).RequireAuthorization();

// מחיקת משימה (רק אם היא שייכת למשתמש)
app.MapDelete("/items/{id}", async (ToDoDbContext db, int id, ClaimsPrincipal user) =>
{
var userId = GetUserId(user);
var item = await db.Items.FirstOrDefaultAsync(t => t.Id == id && t.UserId == userId);
if (item is null) return Results.NotFound();
    db.Items.Remove(item);
    await db.SaveChangesAsync();
    return Results.Ok(item);
}).RequireAuthorization();

// --- נתיבי משתמשים (Authentication) ---
// התחברות והנפקת טוקן JWT
app.MapPost("/login", async (ToDoDbContext db, User loginUser) =>
{
    // 1. חיפוש המשתמש במסד הנתונים לפי שם וסיסמה
    var user = await db.Users.FirstOrDefaultAsync(u => 
        u.Username == loginUser.Username && u.Password == loginUser.Password);

    // אם לא נמצא משתמש - מחזירים שגיאת אימות
    if (user is null) return Results.Unauthorized();

    // 2. הגדרת המידע שייכנס לתוך הטוקן (Claims)
    var claims = new[]
    {
        new Claim(ClaimTypes.Name, user.Username),
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
    };

    // 3. יצירת המפתח לחתימה (לפי מה שהגדרת ב-appsettings.json)
    var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]));
    var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

    // 4. יצירת אובייקט הטוקן
    var token = new JwtSecurityToken(
        issuer: builder.Configuration["Jwt:Issuer"],
        audience: builder.Configuration["Jwt:Audience"],
        claims: claims,
        expires: DateTime.Now.AddDays(1), // תוקף ליום אחד
        signingCredentials: creds
    );
    // 5. שליחת הטוקן חזרה ללקוח כטקסט
    return Results.Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
});
// הוספת משתמש חדש
app.MapPost("/register", async (ToDoDbContext db, User newUser) => {
    db.Users.Add(newUser);
    await db.SaveChangesAsync();
    return Results.Created($"/users/{newUser.Id}", newUser);
});
app.Run();