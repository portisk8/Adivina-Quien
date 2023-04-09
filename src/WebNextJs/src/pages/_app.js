import { SignalRProvider } from "@/components/SignalR/SignalRContext";
import { store } from "@/store/store";
import "@/styles/globals.css";
import { useEffect } from "react";
import { Provider } from "react-redux";

export default function App({ Component, pageProps }) {
  return (
    <Provider store={store}>
      <SignalRProvider>
        <Component {...pageProps} />
      </SignalRProvider>
    </Provider>
  );
}
