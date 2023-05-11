import React, { useState, useEffect } from "react";
import { Layout, Row, Col } from "antd";
import ChatInput from "./ChatInput";
import ChatHistory from "./ChatHistory";
import { useSelector, useDispatch } from "react-redux";
import { sendPayload, selectMessages } from "./messageSlice";
import { sendMessage } from "./api";

const { Header, Content } = Layout;

function App() {
  // Use the Redux hooks to access the state and dispatch
  const messages = useSelector(selectMessages);
  const dispatch = useDispatch();

  // Use the React hooks to manage the local state of the input value and the loading status
  const [value, setValue] = useState("");
  const [loading, setLoading] = useState(false);

  // Define a function to handle the change of the input value
  const handleChange = (e) => {
    setValue(e.target.value);
  };

  // Define a function to handle the submit of the input value
  const handleSubmit = async (values) => {
    // Get the input value from the values object
    const value = values.message;
    // Check if the value is not empty
    if (value) {
      // Set the loading status to true
      setLoading(true);
      // Dispatch the sendPayload action with the value as the payload
      dispatch(sendPayload(value));
      // Call the sendMessage function with the value as the argument and await the response
      const response = await sendMessage(value);
      // Dispatch the sendPayload action with the response as the payload
      dispatch(sendPayload(response));
      // Set the loading status to false
      setLoading(false);
      // Reset the input value to empty
      setValue("");
    }
  };

  // Define an effect to scroll to the bottom of the chat history when the messages change
  useEffect(() => {
    const history = document.getElementById("chat-history");
    history.scrollTop = history.scrollHeight;
  }, [messages]);

  return (
    <Layout>
      <Header>
        <h1 style={{ color: "white" }}>Chat App</h1>
      </Header>
      <Content style={{ padding: "24px" }}>
        <Row justify="center">
          <Col span={12}>
            <ChatHistory messages={messages} />
            <ChatInput
              value={value}
              onChange={handleChange}
              onSubmit={handleSubmit}
              loading={loading}
            />
          </Col>
        </Row>
      </Content>
    </Layout>
  );
}

export default App;
