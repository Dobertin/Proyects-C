const express = require('express');
const dotenv = require('dotenv');
const connectDB = require('./config/db');

dotenv.config();
connectDB();

const app = express();

app.use(express.json());

// Rutas de autenticación y tareas
app.use('/api/auth', require('./routes/authRoutes'));
app.use('/api/tareas', require('./routes/tareaRoutes'));

const PORT = process.env.PORT || 5000;

app.listen(PORT, () => {
    console.log(`Server running on port ${PORT}`);
});
