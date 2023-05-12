import axios from "axios";

// Create an axios instance with the base URL of the backend
const api = axios.create({
  baseURL: "https://localhost:7124/",
});

// Define a function for sending the conversation to the backend and returning the response
export const sendMessage = async (conversation) => {
  try {
    // Map the conversation array to convert the bot payload to a string
    const formattedConversation = conversation.map((message) => {
      // Check if the sender is the bot
      if (message.sender === "bot") {
        // Return a new message object with the payload as a string
        return {
          ...message,
          payload: message.payload.error
            ? `Error: ${message.payload.error}`
            : message.payload.botResponse
            ? message.payload.botResponse
            : "Unknown message",
        };
      } else {
        // Return the original message object
        return message;
      }
    });
    // Send a post request with the formatted conversation as the data
    const response = await api.post("/Assistance", {
      conversation: formattedConversation,
    });
    // Return the response data
    return response.data;
  } catch (error) {
    // Handle the error
    console.error(error);
    // Return a default error message
    return { error: "Something went wrong" };
  }
};
