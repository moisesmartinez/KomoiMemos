import { BrowserRouter as Router, Routes, Route, Navigate } from "react-router-dom";
import Login from "./components/Login";
import Register from "./components/Register";
import Dashboard from "./components/Dashboard";
import EventDetails from "./components/EventDetails";
import "./styles/index.css";
import backgroundImage from "./assets/finance memos.jpeg";

const App = () => {
    const isAuthenticated = !!localStorage.getItem("token"); // Check if the user is authenticated

    return (
        <Router>
            <Routes>
                {/* Route for the Landing Page */}
                <Route
                    path="/"
                    element={
                        <div className="landing-container">
                            <img
                                src={backgroundImage}
                                alt="FinanceMemos"
                                className="landing-image"
                            />
                            <h1>Welcome to FinanceMemos!</h1>
                            <p>Your personal finance and memo management app.</p>
                            <div className="landing-buttons">
                                {isAuthenticated ? (
                                    <a href="/dashboard" className="landing-button">
                                        Go to Dashboard
                                    </a>
                                ) : (
                                    <>
                                        <a href="/login" className="landing-button">
                                            Login
                                        </a>
                                        <a href="/register" className="landing-button">
                                            Register
                                        </a>
                                    </>
                                )}
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
                        isAuthenticated ? (
                            <Dashboard />
                        ) : (
                            <Navigate to="/login" replace />
                        )
                    }
                />

                {/* Route for Event Details */}
                <Route
                    path="/event/:id"
                    element={
                        isAuthenticated ? (
                            <EventDetails />
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