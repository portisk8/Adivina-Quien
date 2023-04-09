"use client";
import Image from "next/image";
import { Inter } from "next/font/google";
import { useContext, useEffect, useState } from "react";
import { useRouter } from "next/router";
import { SignalRContext } from "@/components/SignalR/SignalRContext";

const inter = Inter({ subsets: ["latin"] });

export default function Home() {
  const [name, setName] = useState("");
  const [code, setCode] = useState("");

  const router = useRouter();
  const connection = useContext(SignalRContext);

  useEffect(() => {}, []);

  const handleStart = async () => {
    console.log(name);
    const responseStart = await connection.invoke("addToGroup", {
      name,
      id: null,
      code: code,
    });
    if (responseStart) {
      console.log("handleStart > ", responseStart);
      let player = JSON.parse(responseStart);
      localStorage.setItem("player", responseStart);
      router.push({ pathname: `/game/${player.code}` });
    }
  };

  return (
    <main className="flex min-h-screen flex-col items-center justify-between p-24">
      <div className=" flex justify-center flex-col">
        <div className="text-3xl mb-3">Adivina quien</div>
        <input onChange={(e) => setName(e.target.value)} />
        <input onChange={(e) => setCode(e.target.value)} placeholder="Code" />
        <button
          className="bg-slate-500 rounded-3xl hover:bg-slate-700 text-blue-50 py-2 px-5"
          onClick={() => handleStart()}
        >
          Iniciar
        </button>
      </div>
    </main>
  );
}
