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
    const [isNoteModalOpen, setIsNoteModalOpen] = useState(false);
    const [isExpenseModalOpen, setIsExpenseModalOpen] = useState(false);
    const [newNoteTitle, setNewNoteTitle] = useState("");
    const [newNoteDescription, setNewNoteDescription] = useState("");
    const [newNoteType, setNewNoteType] = useState("Memo");
    const [newExpenseAmount, setNewExpenseAmount] = useState("");
    const [newExpenseCategory, setNewExpenseCategory] = useState("");
    const [newExpenseDescription, setNewExpenseDescription] = useState("");
    const [newExpenseDate, setNewExpenseDate] = useState("");
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

    const formatDescription = (description) => {
        return description.split("\\n").map((line, index, array) => (
            <React.Fragment key={index}>
                {line}
                {index < array.length - 1 && <br />}
            </React.Fragment>
        ));
    };

    const handleCreateNote = async (e) => {
        e.preventDefault();
        setError([]); // Clear previous errors

        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const payload = {
                title: newNoteTitle,
                description: newNoteDescription,
                type: newNoteType,
                eventId: id,
            };

            const response = await axios.post("https://localhost:7035/api/notes", payload, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            // Close the modal and refresh the notes list
            setIsNoteModalOpen(false);
            await fetchNotesByEventId();

            // Clear the form fields
            setNewNoteTitle("");
            setNewNoteDescription("");
            setNewNoteType("Memo");
        } catch (err) {
            console.error("Error creating note:", err);
            if (err.response) {
                if (err.response.headers["content-type"]?.includes("application/problem+json")) {
                    const problemDetails = err.response.data;
                    if (problemDetails.errors) {
                        const errorMessages = Object.entries(problemDetails.errors)
                            .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                            .flat();
                        setError(errorMessages);
                    } else {
                        setError([problemDetails.title || "Failed to create note. Please try again."]);
                    }
                } else {
                    setError([err.response.data.message || "Failed to create note. Please try again."]);
                }
            } else if (err.request) {
                setError(["No response from the server. Please check your connection."]);
            } else {
                setError(["An error occurred. Please try again."]);
            }
        }
    };

    const handleCreateExpense = async (e) => {
        e.preventDefault();
        setError([]); // Clear previous errors

        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const payload = {
                amount: parseFloat(newExpenseAmount),
                category: newExpenseCategory,
                description: newExpenseDescription,
                date: newExpenseDate,
                eventId: id,
            };

            const response = await axios.post("https://localhost:7035/api/expenses", payload, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            // Close the modal and refresh the expenses list
            setIsExpenseModalOpen(false);
            await fetchExpensesByEventId();

            // Clear the form fields
            setNewExpenseAmount("");
            setNewExpenseCategory("");
            setNewExpenseDescription("");
            setNewExpenseDate("");
        } catch (err) {
            console.error("Error creating expense:", err);
            if (err.response) {
                if (err.response.headers["content-type"]?.includes("application/problem+json")) {
                    const problemDetails = err.response.data;
                    if (problemDetails.errors) {
                        const errorMessages = Object.entries(problemDetails.errors)
                            .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                            .flat();
                        setError(errorMessages);
                    } else {
                        setError([problemDetails.title || "Failed to create expense. Please try again."]);
                    }
                } else {
                    setError([err.response.data.message || "Failed to create expense. Please try again."]);
                }
            } else if (err.request) {
                setError(["No response from the server. Please check your connection."]);
            } else {
                setError(["An error occurred. Please try again."]);
            }
        }
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
                <button onClick={() => setIsNoteModalOpen(true)} className="create-button">
                    Create Note
                </button>
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
                <button onClick={() => setIsExpenseModalOpen(true)} className="create-button">
                    Create Expense
                </button>
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

            {/* Modal for creating a new note */}
            {isNoteModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h2>Create a New Note</h2>
                        <form onSubmit={handleCreateNote} className="create-form">
                            <div className="form-group">
                                <label htmlFor="noteTitle">Title:</label>
                                <input
                                    type="text"
                                    id="noteTitle"
                                    value={newNoteTitle}
                                    onChange={(e) => setNewNoteTitle(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="noteDescription">Description:</label>
                                <textarea
                                    id="noteDescription"
                                    value={newNoteDescription}
                                    onChange={(e) => setNewNoteDescription(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="noteType">Type:</label>
                                <select
                                    id="noteType"
                                    value={newNoteType}
                                    onChange={(e) => setNewNoteType(e.target.value)}
                                    required
                                >
                                    <option value="Memo">Memo</option>
                                    <option value="To-Do">To-Do</option>
                                    <option value="Shopping List">Shopping List</option>
                                    <option value="Reminder">Reminder</option>
                                </select>
                            </div>
                            <button type="submit" className="create-button">
                                Create Note
                            </button>
                            <button
                                type="button"
                                onClick={() => setIsNoteModalOpen(false)}
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

            {/* Modal for creating a new expense */}
            {isExpenseModalOpen && (
                <div className="modal-overlay">
                    <div className="modal">
                        <h2>Create a New Expense</h2>
                        <form onSubmit={handleCreateExpense} className="create-form">
                            <div className="form-group">
                                <label htmlFor="expenseAmount">Amount:</label>
                                <input
                                    type="number"
                                    id="expenseAmount"
                                    value={newExpenseAmount}
                                    onChange={(e) => setNewExpenseAmount(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="expenseCategory">Category:</label>
                                <input
                                    type="text"
                                    id="expenseCategory"
                                    value={newExpenseCategory}
                                    onChange={(e) => setNewExpenseCategory(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="expenseDescription">Description:</label>
                                <textarea
                                    id="expenseDescription"
                                    value={newExpenseDescription}
                                    onChange={(e) => setNewExpenseDescription(e.target.value)}
                                    required
                                />
                            </div>
                            <div className="form-group">
                                <label htmlFor="expenseDate">Date:</label>
                                <input
                                    type="date"
                                    id="expenseDate"
                                    value={newExpenseDate}
                                    onChange={(e) => setNewExpenseDate(e.target.value)}
                                    required
                                />
                            </div>
                            <button type="submit" className="create-button">
                                Create Expense
                            </button>
                            <button
                                type="button"
                                onClick={() => setIsExpenseModalOpen(false)}
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

export default EventDetails;