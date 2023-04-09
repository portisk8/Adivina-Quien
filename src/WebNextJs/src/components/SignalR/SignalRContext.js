import CONFIG from "@/configs/Config";
import { setChat } from "@/store/slices/hubSlice";
import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { createContext, useEffect, useState } from "react";
import { useDispatch } from "react-redux";

export const SignalRContext = createContext();

export const SignalRProvider = ({ children }) => {
  const [connection, setConnection] = useState(null);
  const dispatch = useDispatch();

  const connect = async () => {};

  useEffect(() => {
    // Crear la conexión de SignalR
    let newConnection = new HubConnectionBuilder()
      .withUrl(`${CONFIG.API_URL}/signalr/adivinaquien`)
      .configureLogging(LogLevel.Information)
      .build();

    newConnection.on("recibirMensaje", (notificacion) => {
      console.log("recibirMensaje >> ", notificacion);
      dispatch(setChat(notificacion));
    });

    newConnection.on("actualizacion", (notificacion) => {
      try {
        console.log("actualizacion", notificacion);
      } catch (error) {
        console.error(error);
      }
    });

    newConnection
      .start()
      .then(() => setConnection(newConnection))
      .catch(console.error);

    // Terminar la conexión al desmontar el componente
    return () => {
      newConnection.stop();
    };
  }, []);

  return (
    <SignalRContext.Provider value={connection}>
      {children}
    </SignalRContext.Provider>
  );
};
