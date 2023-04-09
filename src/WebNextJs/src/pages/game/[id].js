"use client";
import { SignalRContext } from "@/components/SignalR/SignalRContext";
import {
  getCategoriesServiceAsync,
  getCharactersServiceAsync,
} from "@/services/adivinaQuienService";
import { useRouter } from "next/router";
import React, { useContext, useEffect, useState } from "react";
import { useSelector } from "react-redux";

function Game() {
  const [categoryList, setCategoryList] = useState([]);
  const [categoryId, setCategoryId] = useState();
  const [characterList, setCharacterList] = useState([]);
  const [characterSelectedId, setCharacterSelectedId] = useState();
  const [inputChat, setInputChat] = useState();
  const chatList = useSelector((state) => state.hub.chat);

  const router = useRouter();
  const connection = useContext(SignalRContext);
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
  };

  useEffect(() => {
    init();
  }, []);

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
              await connection.invoke("SendMessage", {
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
            <div>
              {chat.player.name} dice: {chat.text}
            </div>
          ))}
        </div>
      </div>
    </div>
  );
}

export default Game;
