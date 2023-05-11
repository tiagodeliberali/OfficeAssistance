import axios from "axios";
import { createAsyncThunk } from "@reduxjs/toolkit";
import { addMessage } from "./chatSlice";

export const sendMessage = createAsyncThunk(
  "messages/sendMessage",
  async (message, thunkAPI) => {
    try {
      const response = await axios.post("/api/messages", { message });
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response.data);
    }
  }
);

export const fetchMessages = createAsyncThunk(
  "messages/fetchMessages",
  async (_, thunkAPI) => {
    try {
      const response = await axios.get("/api/messages");
      return response.data;
    } catch (error) {
      return thunkAPI.rejectWithValue(error.response.data);
    }
  }
);

export const setupWSConnection = (dispatch) => {
  const ws = new WebSocket("ws://localhost:3001");

  ws.onopen = () => {
    console.log("WebSocket connection established");
  };

  ws.onmessage = (event) => {
    const data = JSON.parse(event.data);
    dispatch(addMessage(data.message));
  };

  ws.onclose = () => {
    console.log("WebSocket connection closed");
  };

  ws.onerror = (error) => {
    console.error("WebSocket error:", error);
  };
};
