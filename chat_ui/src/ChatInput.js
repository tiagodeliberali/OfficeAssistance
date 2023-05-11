import React, { useState, useEffect, useRef } from "react";
import { Form, Input, Button } from "antd";

// A presentational component that renders the input field and the send button for the user
function ChatInput({ value, onChange, onSubmit, loading }) {
  // Use a ref to access the form instance
  const formRef = useRef(null);

  // Use an effect to set the value of the input field to the value prop
  useEffect(() => {
    // Check if the form instance exists
    if (formRef.current) {
      // Set the value of the input field to the value prop
      formRef.current.setFieldsValue({ message: value });
    }
  }, [value]);

  return (
    <Form
      layout="inline"
      style={{ width: "100%", margin: "24px 0" }}
      onFinish={onSubmit}
      // Assign the ref to the form instance
      ref={formRef}
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
