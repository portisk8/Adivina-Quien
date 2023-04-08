import { HubConnectionBuilder, LogLevel } from "@microsoft/signalr";
import { useEffect, useState } from "react";
import CONFIG from "../../configs/Config";

export function useSignalR() {
  const [connection, setConnection] = useState();

  const connect = async ({ onReceiveMessage }) => {
    let connection = new HubConnectionBuilder()
      .withUrl(`${CONFIG.API_URL}/signalr/adivinaquien`)
      .configureLogging(LogLevel.Information)
      .build();

    connection.on("recibirMensaje", (notificacion) => {
      console.log("recibirMensaje >> ", notificacion);
      if (onReceiveMessage) onReceiveMessage(notificacion);

      // handleMesssage(notificacion)
      //   dispatch(incrementTotalNotificationsPending());
      //   notification["info"]({
      //     message: "Notificacion",
      //     description: notificacion,
      //   });
    });

    connection.on("actualizacion", (notificacion) => {
      try {
        console.log(notificacion);
        // var notificationObj = JSON.parse(notificacion);
        // dispatch(incrementTotalNotificationsPending());
        // dispatch(updateNotification(notificationObj));

        // notification["info"]({
        //   message: "TEst",
        //   description: notificacion,
        // });
      } catch (error) {
        console.error(error);
      }
    });

    await connection.start();

    console.log("Conexión con Hub establecida");
    await setConnection(connection);
    // dispatch(setHub(connection));
  };

  useEffect(() => {
    console.log("UseSignalR");
  }, []); // Empty array ensures that effect is only run on mount

  const handleMesssage = () => {};

  const send = async (topic, message) => {
    console.log("SEND", topic, message);

    return connection.invoke(topic, message);
  };
  const start = async (player) => {
    return connection.invoke("addToGroup", player);
  };

  return { start, send, connect };
}
