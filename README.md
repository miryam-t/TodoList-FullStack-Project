## ToDo List Fullstack App
פרויקט ניהול משימות מקצה לקצה (Fullstack) המבוסס על Minimal API בצד השרת, React בצד הלקוח, ו-MySQL כמסד נתונים.

תיאור הפרויקט
הפרויקט נבנה כחלק מתהליך למידה של ארכיטקטורת Microservices ושירותים רזים. האפליקציה מאפשרת למשתמשים להירשם, להתחבר ולנהל רשימת משימות אישית בצורה מאובטחת.

טכנולוגיות וכלים
Backend: .NET 8.0 Minimal API

Frontend: React (Axios)

Database: MySQL & Entity Framework Core

Security: JWT Authentication

IDE: Visual Studio Code

CLI: Dotnet CLI

צד השרת - #Minimal API
למה Minimal API?
הבחירה ב-Minimal API נועדה לספק מענה לשירותים קטנים ותכליתיים ללא העומס של קוד מסביב (Boilerplate). כל הלוגיקה מרוכזת בקובץ Program.cs, מה שמאפשר פיתוח מהיר וקל.

צעדים ראשונים (Dotnet CLI)
הפרויקט נוצר ונוהל באמצעות שורת הפקודה כדי לאפשר עבודה חוצת-פלטפורמות-
 בחרתי בדרך זו כדי להכיר לעומק את פקודות התשתית של .NET ולעבוד בצורה קלה ומהירה יותר דרך הטרמינל של VS Code, ללא תלות בממשק הכבד של Visual Studio הרגיל.

יצירת הפרויקט: dotnet new web -o TodoApi

הרצה במצב מעקב: dotnet watch run

התחברות למסד הנתונים (DB First)
עבודה מול MySQL Workbench ושימוש ב-Entity Framework Core:

התקנת חבילות: EntityFrameworkCore, Pomelo.EntityFrameworkCore.MySql, Design, Tools.

פקודת Scaffold ליצירת ה-Models וה-DbContext באופן אוטומטי מהטבלאות הקיימות.

מיפוי נתיבים (Routes Mapping)
מימוש פעולות CRUD בסיסיות ישירות מול ה-Context:

GET /items - שליפת משימות של המשתמש המחובר.

POST /items - הוספת משימה חדשה.

PUT /items/{id} - עדכון סטטוס משימה.

DELETE /items/{id} - מחיקת משימה.

צד הלקוח - React & Axios
הקליינט מתקשר עם ה-API באמצעות ספריית Axios.

CORS: הוגדרה פוליסת הרשאה בשרת המאפשרת לקליינט לבצע קריאות.

Interceptors:

מימוש Interceptor לתפיסת שגיאות ב-Response ורישום ללוג.

מימוש Interceptor לזיהוי שגיאת 401 (Unauthorized) והפניה אוטומטית לדף ההתחברות.

Defaults: הגדרת baseURL כברירת מחדל לכל הקריאות.

אבטחה והזדהות - JWT
בפרויקט הוטמע מנגנון אבטחה מתקדם:

טבלת Users: הוספת ישות משתמש עם מזהה, שם וסיסמה.

JWT Authentication:

הוספת שירותי אימות ב-API.

הנפקת Token מאובטח בנתיב ה-login.

הגנה על נתיבי המשימות באמצעות RequireAuthorization.

הפרדת נתונים: עדכון ה-Routes כך שכל משתמש יוכל לראות ולנהל רק את המשימות המשויכות ל-ID שלו.

דרישות קדם והרצה
התקנת MySQL ו-Workbench.

התקנת .NET SDK.

הגדרת Connection String בקובץ appsettings.json.

הרצת השרת: dotnet run.

הרצת הקליינט: npm i ולאחר מכן npm start.

פותח כחלק מפרויקט לימודי ב-Net.
פותח כחלק מקורס פרקטיקוד-פרויקט 3.
