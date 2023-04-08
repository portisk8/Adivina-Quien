import { axiosRequest } from "../utils/requestsApi";
import CONFIG from "../configs/Config";

const { API_URL } = CONFIG;

export async function getCategoriesServiceAsync() {
  return axiosRequest(`${API_URL}/api/adivinaQuien/category/list`, {
    method: "GET",
    headers: {
      "Content-Type": "application/json",
    },
  });
}

export async function getCharactersServiceAsync(filter) {
  return axiosRequest(`${API_URL}/api/adivinaQuien/personajes/listar`, {
    method: "POST",
    headers: {
      "Content-Type": "application/json",
    },
    body: filter,
  });
}
