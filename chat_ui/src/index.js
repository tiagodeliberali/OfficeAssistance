import React from "react";
import ReactDOM from "react-dom";
import App from "./App";
import { configureStore } from "@reduxjs/toolkit";
import { Provider } from "react-redux";
import messageSlice from "./messageSlice";

// Create the Redux store with the message slice
const store = configureStore({
  reducer: {
    message: messageSlice,
  },
});

// Render the root component and wrap it with the Redux provider
ReactDOM.render(
  <Provider store={store}>
    <App />
  </Provider>,
  document.getElementById("root")
);
