import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";
import "../styles/EventDetails.css";

const EventDetails = () => {
    const { id } = useParams();
    const [event, setEvent] = useState(null);
    const [notes, setNotes] = useState([]);
    const [expenses, setExpenses] = useState([]);
    const [error, setError] = useState("");
    const navigate = useNavigate();

    useEffect(() => {
        fetchEventDetails();
        fetchNotesByEventId();
        fetchExpensesByEventId();
    }, [id]);

    const fetchEventDetails = async () => {
        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const response = await axios.get(`https://localhost:7035/api/events/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            setEvent(response.data);
        } catch (err) {
            console.error("Error fetching event details:", err);
            setError("Failed to fetch event details. Please try again.");
        }
    };

    const fetchNotesByEventId = async () => {
        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const response = await axios.get(`https://localhost:7035/api/notes/event/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            setNotes(response.data);
        } catch (err) {
            console.error("Error fetching notes:", err);
            setError("Failed to fetch notes. Please try again.");
        }
    };

    const fetchExpensesByEventId = async () => {
        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const response = await axios.get(`https://localhost:7035/api/expenses/event/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            setExpenses(response.data);
        } catch (err) {
            console.error("Error fetching expenses:", err);
            setError("Failed to fetch expenses. Please try again.");
        }
    };

    const totalExpenses = expenses.reduce((sum, expense) => sum + expense.amount, 0);

    // Function to replace \n with <br />
    const formatDescription = (description) => {
    if (!description) return null;

    // Split description by newline characters and render each line
    return description.split("\\n").map((line, index, array) => (
        <React.Fragment key={index}>
            {line}
            {index < array.length - 1 && <br />}
        </React.Fragment>
    ));
};


    if (error) {
        return <div className="error-message">{error}</div>;
    }

    if (!event) {
        return <div>Loading...</div>;
    }

    return (
        <div className="event-details-container">
            <h1>{event.name}</h1>
            <p>{event.description}</p>
            <small>Created on: {new Date(event.createdAt).toLocaleDateString()}</small>

            <div className="notes-section">
                <h2>Notes</h2>
                {notes.length === 0 ? (
                    <p>No notes found for this event.</p>
                ) : (
                    <ul>
                        {notes.map((note) => (
                            <li key={note.id} className="note-item">
                                <h3>{note.title}</h3>
                                <p>{formatDescription(note.description)}</p>
                                <small>Type: {note.type}</small>
                                <small>Created on: {new Date(note.createdAt).toLocaleDateString()}</small>
                            </li>
                        ))}
                    </ul>
                )}
            </div>

            <div className="expenses-section">
                <h2>Expenses</h2>
                <p className="total-expenses">Total Expenses: ${totalExpenses.toFixed(2)}</p>
                {expenses.length === 0 ? (
                    <p>No expenses found for this event.</p>
                ) : (
                    <table className="expenses-table">
                        <thead>
                            <tr>
                                <th>Amount</th>
                                <th>Category</th>
                                <th>Description</th>
                                <th>Date</th>
                            </tr>
                        </thead>
                        <tbody>
                            {expenses.map((expense) => (
                                <tr key={expense.id}>
                                    <td>${expense.amount.toFixed(2)}</td>
                                    <td>{expense.category}</td>
                                    <td>{expense.description}</td>
                                    <td>{new Date(expense.date).toLocaleDateString()}</td>
                                </tr>
                            ))}
                        </tbody>
                    </table>
                )}
            </div>

            <button onClick={() => navigate("/dashboard")} className="back-button">
                Back to Dashboard
            </button>
        </div>
    );
};

export default EventDetails;