import { configureStore } from "@reduxjs/toolkit";
import hubSlice from "./slices/hubSlice";

export const store = configureStore({
  reducer: {
    hub: hubSlice,
  },
});
