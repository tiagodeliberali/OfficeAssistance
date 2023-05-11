import { createSlice } from "@reduxjs/toolkit";

const chatSlice = createSlice({
  name: "messages",
  initialState: [],
  reducers: {
    addMessage: (state, action) => {
      state.push(action.payload);
    },
    setMessages: (state, action) => {
      return action.payload;
    },
  },
});

export const { addMessage, setMessages } = chatSlice.actions;

export default chatSlice.reducer;
