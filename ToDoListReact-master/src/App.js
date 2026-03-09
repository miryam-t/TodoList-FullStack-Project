import React from 'react';
import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import Login from './Login';
import TodoList from './TodoList';
import Register from './Register';

function App() {
  return (
    <Router>
      <Routes>
        {/* דף ההתחברות */}
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        {/* דף המשימות שלך (העברנו אותו לתוך TodoList) */}
        <Route path="/items" element={<TodoList />} />

        {/* אם המשתמש נכנס לכתובת לא ידועה, נשלח אותו ללוגין */}
        <Route path="*" element={<Navigate to="/login" />} />
      </Routes>
    </Router>
  );
}

export default App;