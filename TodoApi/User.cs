using System.ComponentModel.DataAnnotations.Schema; // הוסיפי את ה-using הזה
namespace TodoApi;

[Table("Users")] // זה מבטיח שהקוד יפנה בדיוק לטבלה עם ה-U הגדולה וה-s
    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; } // בפרקטיקה אמיתית מצפינים סיסמאות
    }
