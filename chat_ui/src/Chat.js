import React from "react";
import { useSelector } from "react-redux";
import { List } from "antd";

const Chat = () => {
  const messages = useSelector((state) => state.messages);

  return (
    <List
      dataSource={messages}
      renderItem={(item) => <List.Item>{item}</List.Item>}
    />
  );
};

export default Chat;
