"use client";
import { trpc } from "@/app/_trpc/client";

const TodoList = ({ initialTodos }: { initialTodos?: number[] }) => {
  const { data: todoList } = trpc.getTodos.useQuery(undefined, {
    initialData: initialTodos,
    refetchOnMount: false,
    refetchOnReconnect: false,
  });

  return <div>{JSON.stringify(todoList)}</div>;
};

TodoList.displayName = "TodoList";
export default TodoList;
