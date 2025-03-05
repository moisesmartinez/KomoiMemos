import React, { useState, useEffect } from "react";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "../styles/Dashboard.css";

const Dashboard = () => {
    const [events, setEvents] = useState([]);
    const [isModalOpen, setIsModalOpen] = useState(false);
    const [newEventName, setNewEventName] = useState("");
    const [newEventDescription, setNewEventDescription] = useState("");
    const [error, setError] = useState([]);
    const navigate = useNavigate();

    // Fetch events when the component mounts
    useEffect(() => {
        fetchEvents();
    }, []);

    const fetchEvents = async () => {
        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login"); // Redirect to login if no token is found
                return;
            }

            const response = await axios.get("https://localhost:7035/api/events/user-events", {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            setEvents(response.data);
        } catch (err) {
            console.error("Error fetching events:", err);
            setError("Failed to fetch events. Please try again.");
        }
    };

    const handleCreateEvent = async (e) => {
        e.preventDefault();
        setError([]);

        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login"); // Redirect to login if no token is found
                return;
            }

            const payload = {
                name: newEventName,
                description: newEventDescription,
            };

            console.log("Sending request to:", "https://localhost:7035/api/events");
            console.log("Request payload:", payload);

            const response = await axios.post(
                "https://localhost:7035/api/events",
                payload,
                {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                }
            );

            console.log("Response received:", response.data);

            // Close the modal and refresh the events list
            setIsModalOpen(false);
            await fetchEvents();

            // Clear the form fields
            setNewEventName("");
            setNewEventDescription("");
        } catch (err) {
            console.error("Error creating event:", err);
            if (err.response) {
                if (err.response.headers["content-type"]?.includes("application/problem+json")) {
                    const problemDetails = err.response.data;
                    if (problemDetails.errors) {
                        const errorMessages = Object.entries(problemDetails.errors)
                            .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                            .flat();
                        setError(errorMessages);
                    } else {
                        setError([problemDetails.title || "Failed to create event. Please try again."]);
                    }
                } else {
                    setError([err.response.data.message || "Failed to create event. Please try again."]);
                }
            } else if (err.request) {
                setError(["No response from the server. Please check your connection."]);
            } else {
                setError(["An error occurred. Please try again."]);
            }
        }
    };

    return (
        <div className="dashboard-container">
            <header className="dashboard-header">
                <h1>Welcome to Your Dashboard</h1>
                <button onClick={() => setIsModalOpen(true)} className="create-event-button">
                    Create Event
                </button>
                <button onClick={() => { localStorage.removeItem("token"); navigate("/login"); }} className="logout-button">
                    Logout
                </button>
            </header>

            <div className="dashboard-content">
                <section className="event-list">
                    <h2>Your Events</h2>
                    {events.length === 0 ? (
                        <p>You have no events. Create one!</p>
                    ) : (
                        <ul>
                            {events.map((event) => (
                                <li key={event.id} className="event-item">
                                    <h3>{event.name}</h3>
                                    <p>{event.description}</p>
                                    <small>Created on: {new Date(event.createdAt).toLocaleDateString()}</small>
                                </li>
                            ))}
                        </ul>
                    )}
                </section>
            </div>

            {/* Modal for creating a new event */}
            {isModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h2>Create a New Event</h2>
                        <form onSubmit={handleCreateEvent} className="create-event-form">
                            <div className="form-group">
                                <label htmlFor="eventName">Event Name:</label>
                                <input
                                    type="text"
                                    id="eventName"
                                    value={newEventName}
                                    onChange={(e) => setNewEventName(e.target.value)}
                                    required
                                    className="form-input"
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="eventDescription">Event Description:</label>
                                <textarea
                                    id="eventDescription"
                                    value={newEventDescription}
                                    onChange={(e) => setNewEventDescription(e.target.value)}
                                    required
                                    className="form-input"
                                />
                            </div>
                            <button type="submit" className="create-button">
                                Create Event
                            </button>
                            <button
                                type="button"
                                onClick={() => setIsModalOpen(false)}
                                className="cancel-button"
                            >
                                Cancel
                            </button>
                        </form>
                        {error.length > 0 && (
                            <div className="error-message">
                                <ul>
                                    {error.map((message, index) => (
                                        <li key={index}>{message}</li>
                                    ))}
                                </ul>
                            </div>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default Dashboard;