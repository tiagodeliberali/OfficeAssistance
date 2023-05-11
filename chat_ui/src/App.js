import React from "react";
import { Layout } from "antd";
import Chat from "./Chat";
import Input from "./Input";

const { Header, Content } = Layout;

const App = () => {
  return (
    <Layout>
      <Header>
        <h1>Chat App</h1>
      </Header>
      <Content>
        <Chat />
        <Input />
      </Content>
    </Layout>
  );
};

export default App;
