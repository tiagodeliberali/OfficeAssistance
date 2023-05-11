import React from "react";
import { List } from "antd";

// A presentational component that renders the list of messages in the chat history
function ChatHistory({ messages }) {
  return (
    <List
      id="chat-history"
      style={{ height: "400px", overflowY: "auto" }}
      dataSource={messages}
      renderItem={(item) => (
        <List.Item
          style={{ textAlign: item.sender === "user" ? "right" : "left" }}
        >
          <List.Item.Meta
            title={item.sender}
            // Check if the payload is an object and render the botResponse property, otherwise render the payload as it is
            description={
              typeof item.payload === "object"
                ? item.payload.botResponse
                : item.payload
            }
            style={{ color: item.sender === "user" ? "blue" : "green" }}
          />
        </List.Item>
      )}
    />
  );
}

export default ChatHistory;
