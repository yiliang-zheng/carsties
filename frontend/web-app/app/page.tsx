import { serverClient } from "@/app/_trpc/serverClient";

import TodoList from "@/app/_components/TodoList/TodoList";
export default async function Home() {
  const result = await serverClient.getTodos();
  return (
    <div className="w-full items-center justify-between font-mono text-sm lg:flex">
      <TodoList initialTodos={result} />
    </div>
  );
}
