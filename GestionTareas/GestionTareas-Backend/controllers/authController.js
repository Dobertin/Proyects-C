const User = require('../models/User');
const jwt = require('jsonwebtoken');

exports.registerUser = async (req, res) => {
    const { nombreUsuario, email, contraseņa } = req.body;

    try {
        const user = await User.create({ nombreUsuario, email, contraseņa });

        const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, {
            expiresIn: '1h'
        });

        res.status(201).json({ token });
    } catch (error) {
        res.status(400).json({ message: error.message });
    }
};

exports.loginUser = async (req, res) => {
    const { email, contraseņa } = req.body;

    try {
        const user = await User.findOne({ email });
        if (!user) {
            return res.status(404).json({ message: 'User not found' });
        }

        const isMatch = await user.matchPassword(contraseņa);
        if (!isMatch) {
            return res.status(400).json({ message: 'Invalid credentials' });
        }

        const token = jwt.sign({ id: user._id }, process.env.JWT_SECRET, {
            expiresIn: '1h'
        });

        res.status(200).json({ token });
    } catch (error) {
        res.status(500).json({ message: error.message });
    }
};
