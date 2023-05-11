import { configureStore } from "@reduxjs/toolkit";
import chatReducer from "./chatSlice";
import { setupWSConnection } from "./api";

const store = configureStore({
  reducer: {
    messages: chatReducer,
  },
});

setupWSConnection(store.dispatch);

export default store;
