import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Login from "./components/Login";
import Register from "./components/Register";
import Dashboard from "./components/Dashboard";
import "./styles/index.css";

const App = () => {
    return (
        <Router>
            <Routes>
                {/* Route for the Landing Page */}
                <Route
                    path="/"
                    element={
                        <div className="landing-container">
                            <img
                                src="https://images.pexels.com/photos/5716032/pexels-photo-5716032.jpeg"
                                alt="FinanceMemos"
                                className="landing-image"
                            />
                            <h1>Welcome to FinanceMemos!</h1>
                            <p>Your personal finance and memo management app.</p>
                            <div className="landing-buttons">
                                <a href="/login" className="landing-button">
                                    Login
                                </a>
                                <a href="/register" className="landing-button">
                                    Register
                                </a>
                            </div>
                        </div>
                    }
                />

                {/* Route for the Login Page */}
                <Route path="/login" element={<Login />} />

                {/* Route for the Register Page */}
                <Route path="/register" element={<Register />} />

                {/* Route for the Dashboard */}
                <Route
                    path="/dashboard"
                    element={
                        localStorage.getItem("token") ? (
                            <Dashboard />
                        ) : (
                            <Navigate to="/login" replace />
                        )
                    }
                />
            </Routes>
        </Router>
    );
};

export default App;