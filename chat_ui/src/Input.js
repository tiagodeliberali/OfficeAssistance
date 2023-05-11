import React from "react";
import { useDispatch } from "react-redux";
import { Input, Button } from "antd";
import { sendMessage } from "./api";

const { Search } = Input;

const InputBox = () => {
  const dispatch = useDispatch();
  const [inputValue, setInputValue] = React.useState("");

  const handleInputChange = (event) => {
    setInputValue(event.target.value);
  };

  const handleFormSubmit = (event) => {
    event.preventDefault();
    dispatch(sendMessage(inputValue));
    setInputValue("");
  };

  return (
    <form onSubmit={handleFormSubmit}>
      <Search
        placeholder="Type your message here"
        enterButton={<Button type="primary">Send</Button>}
        size="large"
        value={inputValue}
        onChange={handleInputChange}
      />
    </form>
  );
};

export default InputBox;
