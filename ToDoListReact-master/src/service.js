import axios from 'axios';

// 1. הגדרת כתובת ה-API כברירת מחדל (דרישה: Config Defaults)
// מעכשיו axios יודע שזו הכתובת הבסיסית ולא צריך לכתוב אותה בכל פעם מחדש
axios.defaults.baseURL = "http://localhost:5251";

// 2. הוספת interceptor שתופס שגיאות ב-response ורושם ללוג (דרישה: Interceptor)
axios.interceptors.response.use(
response => response,
error => {
// רישום השגיאה ללוג בצורה מסודרת
console.error('API Error:', error.response ? error.response.data : error.message);
return Promise.reject(error);
}
);

const service = {
// שליפת כל המשימות
getTasks: async () => {
const result = await axios.get('/items');

return result.data;
},

// הוספת משימה חדשה
addTask: async (taskName) => {
console.log('addTask', taskName);
const result = await axios.post('/items', {
name: taskName,
isComplete: false
});
return result.data;
},

// עדכון משימה (כולל שליחת ה-ID והשם כדי שהנתונים לא יימחקו ב-Database)
setCompleted: async (id, taskName, isComplete) => {
console.log('setCompleted', { id, taskName, isComplete });
const result = await axios.put('/items/' + id, {
id: id,
name: taskName,
isComplete: isComplete
});
return result.data;
},

// מחיקת משימה לפי ID
deleteTask: async (id) => {
console.log('deleteTask', id);
await axios.delete('/items/' + id);
return { success: true };
}
};

export default service;