import axios from 'axios';

// 1. הגדרת כתובת ה-API כברירת מחדל
// axios.defaults.baseURL = process.env.REACT_APP_API_URL;
// יצירת מופע ספציפי עם הכתובת מה-env
const apiClient = axios.create({
    baseURL: process.env.REACT_APP_API_URL,
});

// 2. Interceptor לבקשות (Request): הוספת הטוקן לכל בקשה שיוצאת
apiClient.interceptors.request.use(config => {
    const token = localStorage.getItem('token');
    if (token) {
        config.headers.Authorization = 'Bearer ' + token;
    }
    return config;
}, error => {
    return Promise.reject(error);
});

// 3. Interceptor לתגובות (Response): תפיסת שגיאה 401 והעברה ללוגין
apiClient.interceptors.response.use(
    response => response,
    error => {
        if (error.response && error.response.status === 401) {
            console.warn('Unauthorized! Redirecting to login...');
            localStorage.removeItem('token'); // מחיקת טוקן פג תוקף
            window.location.href = '/login';   // העברה לדף לוגין
        }
        console.error('API Error:', error.response ? error.response.data : error.message);
        return Promise.reject(error);
    }
);

const service = {

    // --- פונקציות חדשות לאימות ---
    login: async (Username, password) => {
        const result = await apiClient.post('/login', { Username, password });
        // שמירת הטוקן שחזר מהשרת
        localStorage.setItem('token', result.data.token);
        return result.data;
    },

    register: async (Username, password) => {
        const result = await apiClient.post('/register', { Username, password });
        return result.data;
    },
    // שליפת כל המשימות
    getTasks: async () => {
        const result = await apiClient.get('/items');

        return result.data;
    },

    // הוספת משימה חדשה
    addTask: async (taskName) => {
        console.log('addTask', taskName);
        const result = await apiClient.post('/items', {
            name: taskName,
            isComplete: false
        });
        return result.data;
    },

    // עדכון משימה (כולל שליחת ה-ID והשם כדי שהנתונים לא יימחקו ב-Database)
    setCompleted: async (id, taskName, isComplete) => {
        console.log('setCompleted', { id, taskName, isComplete });
        const result = await apiClient.put('/items/' + id, {
            id: id,
            name: taskName,
            isComplete: isComplete
        });
        return result.data;
    },

    // מחיקת משימה לפי ID
    deleteTask: async (id) => {
        console.log('deleteTask', id);
        await apiClient.delete('/items/' + id);
        return { success: true };
    }
};

export default service;