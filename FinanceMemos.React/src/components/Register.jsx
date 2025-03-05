import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css";

const Register = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [error, setError] = useState([]);
    const navigate = useNavigate();

    // Redirect to dashboard if already signed in
    useEffect(() => {
        const token = localStorage.getItem("token");
        if (token) {
            navigate("/dashboard");
        }
    }, [navigate]);

    const handleSubmit = async (e) => {
        e.preventDefault();
        setError([]);

        // Check if passwords match
        if (password !== confirmPassword) {
            setError(["Passwords do not match."]);
            return;
        }

        try {
            const response = await axios.post("https://localhost:7035/api/Auth/register", {
                Username: username,
                Password: password,
                ConfirmPassword: confirmPassword,
            });

            // Save the token to localStorage
            localStorage.setItem("token", response.data.token);

            // Redirect to the dashboard
            navigate("/dashboard");
        } catch (err) {
            console.error("API Error:", err);
            if (err.response) {
                if (err.response.data.errors) {
                    const errorMessages = Object.values(err.response.data.errors).flat();
                    setError(errorMessages);
                } else {
                    setError([err.response.data.title || "Registration failed. Please try again."]);
                }
            } else if (err.request) {
                setError(["No response from the server. Please check your connection."]);
            } else {
                setError(["An error occurred. Please try again."]);
            }
        }
    };

    return (
        <div className="login-container">
            <h2 className="login-title">Register</h2>
            {error.length > 0 && (
                <div className="login-error">
                    <ul>
                        {error.map((message, index) => (
                            <li key={index}>{message}</li>
                        ))}
                    </ul>
                </div>
            )}
            <form onSubmit={handleSubmit} className="login-form">
                <div className="login-form-group">
                    <label htmlFor="username">Username:</label>
                    <input
                        type="text"
                        id="username"
                        value={username}
                        onChange={(e) => setUsername(e.target.value)}
                        required
                        className="login-input"
                    />
                </div>
                <div className="login-form-group">
                    <label htmlFor="password">Password:</label>
                    <input
                        type="password"
                        id="password"
                        value={password}
                        onChange={(e) => setPassword(e.target.value)}
                        required
                        className="login-input"
                    />
                </div>
                <div className="login-form-group">
                    <label htmlFor="confirmPassword">Confirm Password:</label>
                    <input
                        type="password"
                        id="confirmPassword"
                        value={confirmPassword}
                        onChange={(e) => setConfirmPassword(e.target.value)}
                        required
                        className="login-input"
                    />
                </div>
                <button type="submit" className="login-button">
                    Register
                </button>
            </form>
            <p style={{ textAlign: "center", marginTop: "10px" }}>
                Already have an account? <a href="/login">Login here</a>.
            </p>
        </div>
    );
};

export default Register;