import React, { useEffect, useState } from "react";
import axios from "axios";
import { useParams, useNavigate } from "react-router-dom";
import "../styles/EventDetails.css";

const EventDetails = () => {
    const { id } = useParams();
    const [event, setEvent] = useState(null);
    const [notes, setNotes] = useState([]);
    const [expenses, setExpenses] = useState([]);
    const [noteErrors, setNoteErrors] = useState([]); // Errors for note creation
    const [expenseErrors, setExpenseErrors] = useState([]); // Errors for expense creation
    const [isNoteModalOpen, setIsNoteModalOpen] = useState(false);
    const [isExpenseModalOpen, setIsExpenseModalOpen] = useState(false);
    const [newNoteTitle, setNewNoteTitle] = useState("");
    const [newNoteDescription, setNewNoteDescription] = useState("");
    const [newNoteType, setNewNoteType] = useState("Memo");
    const [newExpenseAmount, setNewExpenseAmount] = useState("");
    const [newExpenseCategory, setNewExpenseCategory] = useState("");
    const [newExpenseDescription, setNewExpenseDescription] = useState("");
    const [newExpenseDate, setNewExpenseDate] = useState(new Date().toISOString().split("T")[0]);
    const [newNoteImage, setNewNoteImage] = useState(null);
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

            const notesWithImages = response.data.map(note => ({
                ...note,
                imageUrl: note.images?.length > 0 ? note.images[0].imageUrl : null
            }));

            setNotes(response.data);
        } catch (err) {
            console.error("Error fetching notes:", err);
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
        setNoteErrors([]); // Clear previous errors

        try {
            const token = localStorage.getItem("token");
            if (!token) {
                navigate("/login");
                return;
            }

            const formData = new FormData();
            formData.append("title", newNoteTitle);
            formData.append("description", newNoteDescription);
            formData.append("type", newNoteType);
            formData.append("eventId", id);
            if (newNoteImage) {
                formData.append("imageFile", newNoteImage);
            }

            const response = await axios.post("https://localhost:7035/api/Notes/notes", formData, {
                headers: {
                    Authorization: `Bearer ${token}`,
                    "Content-Type": "multipart/form-data",
                },
            });

            // Close the modal and refresh the notes list
            setIsNoteModalOpen(false);
            await fetchNotesByEventId();

            // Clear the form fields
            setNewNoteTitle("");
            setNewNoteDescription("");
            setNewNoteType("Memo");
            setNewNoteImage(null);
        } catch (err) {
            console.error("Error creating note:", err);
            if (err.response) {
                if (err.response.headers["content-type"]?.includes("application/problem+json")) {
                    const problemDetails = err.response.data;
                    if (problemDetails.errors) {
                        const errorMessages = Object.entries(problemDetails.errors)
                            .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                            .flat();
                        setNoteErrors(errorMessages);
                    } else {
                        setNoteErrors([problemDetails.title || "Failed to create note. Please try again."]);
                    }
                } else {
                    setNoteErrors([err.response.data.message || "Failed to create note. Please try again."]);
                }
            } else if (err.request) {
                setNoteErrors(["No response from the server. Please check your connection."]);
            } else {
                setNoteErrors(["An error occurred. Please try again."]);
            }
        }
    };

    const handleCreateExpense = async (e) => {
        e.preventDefault();
        setExpenseErrors([]); // Clear previous errors

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
                date: newExpenseDate, // Already in the correct format (YYYY-MM-DD)
                eventId: id,
            };

            const response = await axios.post("https://localhost:7035/api/Expenses/expenses", payload, {
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
            setNewExpenseDate(new Date().toISOString().split("T")[0]); // Reset to today's date
        } catch (err) {
            console.error("Error creating expense:", err);
            if (err.response) {
                if (err.response.headers["content-type"]?.includes("application/problem+json")) {
                    const problemDetails = err.response.data;
                    if (problemDetails.errors) {
                        const errorMessages = Object.entries(problemDetails.errors)
                            .map(([field, messages]) => `${field}: ${messages.join(", ")}`)
                            .flat();
                        setExpenseErrors(errorMessages);
                    } else {
                        setExpenseErrors([problemDetails.title || "Failed to create expense. Please try again."]);
                    }
                } else {
                    setExpenseErrors([err.response.data.message || "Failed to create expense. Please try again."]);
                }
            } else if (err.request) {
                setExpenseErrors(["No response from the server. Please check your connection."]);
            } else {
                setExpenseErrors(["An error occurred. Please try again."]);
            }
        }
    };

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
                                {note.imageUrl && (
                                    <div className="note-image">
                                        <img src={note.imageUrl} alt="Note attachment" />
                                    </div>
                                )}
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
                            <div className="form-group">
                                <label htmlFor="noteImage">Attach Image (optional):</label>
                                <input
                                    type="file"
                                    id="noteImage"
                                    accept="image/*"
                                    onChange={(e) => setNewNoteImage(e.target.files[0])}
                                />
                            </div>
                            {noteErrors.length > 0 && (
                                <div className="error-message">
                                    <ul>
                                        {noteErrors.map((message, index) => (
                                            <li key={index}>{message}</li>
                                        ))}
                                    </ul>
                                </div>
                            )}
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
                                <select
                                    id="expenseCategory"
                                    value={newExpenseCategory}
                                    onChange={(e) => setNewExpenseCategory(e.target.value)}
                                    required
                                >
                                    <option value="">Select a category</option>
                                    <option value="Food">Food</option>
                                    <option value="Transport">Transport</option>
                                    <option value="Accommodation">Accommodation</option>
                                    <option value="Entertainment">Entertainment</option>
                                    <option value="Shopping">Shopping</option>
                                    <option value="Utilities">Utilities</option>
                                    <option value="Health">Health</option>
                                    <option value="Other">Other</option>
                                </select>
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
                            {expenseErrors.length > 0 && (
                                <div className="error-message">
                                    <ul>
                                        {expenseErrors.map((message, index) => (
                                            <li key={index}>{message}</li>
                                        ))}
                                    </ul>
                                </div>
                            )}
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
                    </div>
                </div>
            )}
        </div>
    );
};

export default EventDetails;