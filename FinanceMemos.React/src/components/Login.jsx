import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../styles/Login.css";

const Login = () => {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");
    const [error, setError] = useState("");
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
        setError("");

        try {
            const response = await axios.post("https://localhost:7035/api/Auth/login", {
                username,
                password,
            });

            // Save the token to localStorage
            localStorage.setItem("token", response.data.token);

            // Redirect to the dashboard
            navigate("/dashboard");
        } catch (err) {
            console.error("API Error:", err);
            if (err.response) {
                // Handle errors returned from the backend
                if (err.response.status === 401) {
                    setError(err.response.data.message || "Invalid username or password.");
                } else {
                    setError("An error occurred. Please try again.");
                }
            } else if (err.request) {
                // Handle network errors (e.g., no response from the server)
                console.log("Request Error:", err.request);
                setError("No response from the server. Please check your connection.");
            } else {
                // Handle other errors (e.g., issues with the request setup)
                console.log("Error:", err.message);
                setError("An error occurred. Please try again.");
            }
        }
    };

    return (
        <div className="login-container">
            <h2 className="login-title">Login</h2>
            {error && <p className="login-error">{error}</p>}
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
                <button type="submit" className="login-button">
                    Login
                </button>
            </form>
            <p style={{ textAlign: "center", marginTop: "10px" }}>
                Don't have an account? <a href="/register">Register here</a>.
            </p>
        </div>
    );
};

export default Login;