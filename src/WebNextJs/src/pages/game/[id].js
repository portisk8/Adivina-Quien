"use client";
import { useSignalR } from "@/components/SignalR/useSignalR";
import {
  getCategoriesServiceAsync,
  getCharactersServiceAsync,
} from "@/services/adivinaQuienService";
import { useRouter } from "next/router";
import React, { useEffect, useState } from "react";

function Game() {
  const [categoryList, setCategoryList] = useState([]);
  const [categoryId, setCategoryId] = useState();
  const [characterList, setCharacterList] = useState([]);
  const [characterSelectedId, setCharacterSelectedId] = useState();
  const [chatList, setChatList] = useState([]);
  const [inputChat, setInputChat] = useState();

  const router = useRouter();
  const signalR = useSignalR();
  const { id } = router.query;

  const getCategories = async () => {
    const response = await getCategoriesServiceAsync();
    if (response?.data) {
      setCategoryList(response.data);
    }
  };
  const getCharacters = async (categoryId) => {
    const response = await getCharactersServiceAsync({ categoryId });
    if (response?.data) {
      setCharacterList(response.data);
    }
  };

  const init = async () => {
    getCategories();
    await signalR.connect({ onReceiveMessage: handleChatList });
    // await signalR.start(JSON.parse(localStorage.getItem("player")));
  };

  useEffect(() => {
    init();
  }, []);

  const handleChatList = async (message) => {
    console.log("handleChatList > ", message);
    chatList.push(message);
    setChatList([...chatList]);
  };

  const handleSetCategory = async (item) => {
    setCategoryId(item.categoryId);
    await getCharacters(item.categoryId);
  };

  const handleRemoveCharacter = async (item) => {};

  return (
    <div className="h-full grid gap-2 grid-cols-2">
      <div>
        {!categoryId && categoryList.length > 0 && (
          <div className="grid gap-2 grid-cols-2 lg:grid-cols-3 mx-3 lg:px-1 xl:pl-20 xl:gap-6 ">
            {categoryList.map((item, ix) => (
              <div
                key={`categoryId-${item.categoryId}`}
                className="flex flex-col h-full border-2 rounded-2xl border-red-500 hover:cursor-pointer"
                onClick={() => handleSetCategory(item)}
              >
                {item.description}
              </div>
            ))}
          </div>
        )}
        {characterList.length > 0 && (
          <div>
            {!characterSelectedId && <div>Selecciona un Personaje</div>}
            <div className="grid gap-2 grid-cols-2 lg:grid-cols-3 mx-3 lg:px-1 xl:pl-20 xl:gap-6 ">
              {characterList.map((item, ix) => (
                <div
                  key={`characterId-${item.characterId}`}
                  className="flex flex-col h-full border-2 rounded-2xl border-red-500 hover:cursor-pointer"
                  onClick={() =>
                    !characterSelectedId
                      ? setCharacterSelectedId(item)
                      : handleRemoveCharacter(item)
                  }
                >
                  {item.description}
                </div>
              ))}
            </div>
          </div>
        )}
      </div>
      <div>
        <div>CHAT</div>
        <div className="flex gap-2">
          <input onChange={(e) => setInputChat(e.target.value)} />
          <button
            className="p-2"
            onClick={async () => {
              await signalR.send("SendMessage", {
                player: JSON.parse(localStorage.getItem("player")),
                text: inputChat,
              });
            }}
          >
            Enviar
          </button>
        </div>

        <div>
          {chatList.map((chat, ix) => (
            <div>Mensaje: {JSON.stringify(chat)}</div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default Game;
