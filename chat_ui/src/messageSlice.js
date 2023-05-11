import { createSlice } from "@reduxjs/toolkit";

// Define the initial state of the message data
const initialState = {
  messages: [],
};

// Create a Redux slice for the message data
const messageSlice = createSlice({
  name: "message",
  initialState,
  reducers: {
    // Define a reducer for sending the payload to the chat history
    sendPayload: (state, action) => {
      // Determine the sender based on the payload type
      const sender = typeof action.payload === "string" ? "user" : "bot";
      // Push a new message object to the state array
      state.messages.push({ sender, payload: action.payload });
    },
  },
});

// Export the action creators and the selector for the message slice
export const { sendPayload } = messageSlice.actions;
export const selectMessages = (state) => state.message.messages;

// Export the message slice reducer
export default messageSlice.reducer;
