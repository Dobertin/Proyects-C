const express = require('express');
const { protect } = require('../middleware/authMiddleware');
const { getTareas, createTarea } = require('../controllers/tareaController');

const router = express.Router();

router.route('/')
    .get(protect, getTareas)
    .post(protect, createTarea);

module.exports = router;
