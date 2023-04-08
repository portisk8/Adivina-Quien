import { HubConnectionBuilder } from "@microsoft/signalr";
import { useEffect } from "react";

export function useSignalR() {
  const connect = () => {
    let connection = new HubConnectionBuilder()
      // .withUrl(`${CONFIG.API_URL}/signalr/notificaciones`)
      .withUrl(`https://localhost:44364/signalr/adivinaquien`)
      .build();

    connection.on("notificacion", (notificacion) => {
      console.log(notificacion);
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

    connection
      .start()
      .then(() => {
        //una vez iniciado, nos registramos al canal
        console.log("Conexión con Hub establecida");

        connection
          .invoke("addToGroup", "123-456")
          .catch((err) => console.error(err));

        // const currentUser = JSON.parse(localStorage.getItem("user"));
        // if (currentUser && currentUser.usuario_id) {
        //   connection
        //     .invoke("addToGroup", currentUser.usuario_id.toString())
        //     .catch((err) => console.error(err));
        // }
      })
      .catch((err) => {
        console.log("Error al iniciar la conexión con el hub");
      });
    // dispatch(setHub(connection));
  };

  useEffect(() => {
    console.log("UseSignalR");
    connect();
  }, []); // Empty array ensures that effect is only run on mount

  const send = async () => {
    console.log("SEND");
  };
  const onReceiveMessage = async () => {
    console.log("onReceiveMessage");
  };

  return { send, onReceiveMessage };
}
