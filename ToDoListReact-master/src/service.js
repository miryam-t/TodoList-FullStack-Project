// import axios from 'axios';

// const apiUrl = "http://localhost:5251"
// // 2. הוספת interceptor שתופס שגיאות ב-response ורושם ללוג
// axios.interceptors.response.use(
// response => response,
// error => {
// // רישום השגיאה ללוג
// console.error('API Error:', error.response ? error.response.data : error.message);
// return Promise.reject(error);
// });

// export default {
//   getTasks: async () => {
//     const result = await axios.get(`${apiUrl}/items`)    
//     return result.data;
//   },

//   // addTask: async(name)=>{
//   //   console.log('addTask', name)
//   //   //TODO
//   //   return {};
//   // },
// // addTask: async (name) => {
// // console.log('addTask', name);
// // // שולחים אובייקט עם שם המשימה וסטטוס התחלתי
// // const result = await axios.post('/items', { name: name, isComplete: false });
// // return result.data;
// // },
// addTask: async (taskName) => {
// console.log('addTask', taskName);
// // שימי לב: הוספתי כאן את הכתובת המלאה עם הפורט 5251
// const result = await axios.post("http://localhost:5251/items", {
// name: taskName,
// isComplete: false
// });
// return result.data;
// },
// setCompleted: async (id, taskName, isComplete) => {
// console.log('setCompleted', { id, isComplete });

// // אנחנו משתמשים בכתובת המלאה עם ה-ID בסוף
// const result = await axios.put("http://localhost:5251/items/" + id, {
//   name: taskName,
// isComplete: isComplete
// });

// return result.data;
// },
//   // setCompleted: async(id, isComplete)=>{
//   //   console.log('setCompleted', {id, isComplete})
//   //   //TODO
//   //   return {};
//   // },

//   // deleteTask:async()=>{
//   //   console.log('deleteTask')
//   // }
//   // };
//   // הוספת משימה חדשה


// // עדכון סטטוס משימה (השלמה/ביטול)
// // setCompleted: async (id, isComplete) => {
// // console.log('setCompleted', { id, isComplete });
// // // שולחים עדכון לפריט ספציפי לפי ה-ID שלו
// // const result = await axios.put(`/items/${id}`, { isComplete: isComplete });
// // return result.data;
// // },

// // מחיקת משימה
// deleteTask: async (id) => {
// console.log('deleteTask', id);
// // קריאת DELETE לפי ה-ID
// await axios.delete("http://localhost:5251/items/"+id);
// return { success: true };
// }
// };

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