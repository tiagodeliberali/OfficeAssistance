import React from "react";
import { Form, Input, Button } from "antd";

// A presentational component that renders the input field and the send button for the user
function ChatInput({ value, onChange, onSubmit, loading }) {
  return (
    <Form
      layout="inline"
      style={{ width: "100%", margin: "24px 0" }}
      onFinish={onSubmit}
    >
      <Form.Item name="message" style={{ flex: 1 }}>
        <Input
          placeholder="Type a message"
          value={value}
          onChange={onChange}
          disabled={loading}
        />
      </Form.Item>
      <Form.Item>
        <Button type="primary" htmlType="submit" loading={loading}>
          Send
        </Button>
      </Form.Item>
    </Form>
  );
}

export default ChatInput;
