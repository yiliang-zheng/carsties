const baseUrl = process.env.BACKEND_URL;
export function getErrorMessage(error: unknown) {
  if (error instanceof Error) return error.message;
  return String(error);
}

function constructHeaders(accessToken?: string): Record<string, string> {
  let headers: Record<string, string> = {
    "Content-Type": "application/json",
  };
  if (!!accessToken) {
    headers["Authorization"] = `Bearer ${accessToken}`;
  }
  return headers;
}

async function handleResponse<T>(response: Response): Promise<T> {
  try {
    if (!response.ok) {
      throw new Error(response.statusText);
    }

    const data = (await response.json()) as T;
    return data;
  } catch (error) {
    throw new Error(getErrorMessage(error));
  }
}

async function get<T>(url: string, accessToken?: string): Promise<T> {
  const fetchOptions: RequestInit = {
    method: "GET",
  };

  const response = await fetch(`${baseUrl}${url}`, fetchOptions);
  const data = await handleResponse<T>(response);
  return data;
}

async function post<TOutput, TPayload>(
  url: string,
  payload?: TPayload,
  accessToken?: string
): Promise<TOutput> {
  const fetchOptions: RequestInit = {
    method: "POST",
    headers: constructHeaders(accessToken),
    body: JSON.stringify(payload),
  };

  const response = await fetch(`${baseUrl}${url}`, fetchOptions);
  const data = await handleResponse<TOutput>(response);
  return data;
}

async function put<TOutput, TPayload>(
  url: string,
  payload?: TPayload,
  accessToken?: string
): Promise<TOutput> {
  const fetchOptions: RequestInit = {
    method: "PUT",
    headers: constructHeaders(),
    body: JSON.stringify(payload),
  };

  const response = await fetch(`${baseUrl}${url}`, fetchOptions);
  const data = await handleResponse<TOutput>(response);
  return data;
}

async function del<TOutput>(
  url: string,
  accessToken?: string
): Promise<TOutput> {
  const fetchOptions: RequestInit = {
    method: "DELETE",
    headers: constructHeaders(accessToken),
  };

  const response = await fetch(`${baseUrl}${url}`, fetchOptions);
  const data = await handleResponse<TOutput>(response);
  return data;
}
export const fetchWrapper = {
  get,
  post,
  put,
  del,
};
