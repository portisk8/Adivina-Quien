import { createSlice } from "@reduxjs/toolkit";

const initialState = {
  chat: [],
};

export const hubSlice = createSlice({
  name: "hub",
  initialState,
  reducers: {
    setChat: (state, action) => {
      state.chat.push(action.payload);
    },
    resetChat: (state) => {
      state.chat = [];
    },
  },
});

// Action creators are generated for each case reducer function
export const { setConnection, setChat, resetChat } = hubSlice.actions;

export default hubSlice.reducer;
