import axios from "axios";

// Create an axios instance with the base URL of the backend
const api = axios.create({
  baseURL: "https://localhost:7124/",
});

// Define a function for sending the payload to the backend and returning the response
export const sendMessage = async (payload) => {
  try {
    // Send a post request with the payload as the data
    const response = await api.post("/Assistance", { payload });
    // Return the response data
    return response.data;
  } catch (error) {
    // Handle the error
    console.error(error);
    // Return a default error message
    return { error: "Something went wrong" };
  }
};
