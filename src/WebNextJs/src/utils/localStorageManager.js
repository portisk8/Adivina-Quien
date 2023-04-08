export function getClientToken() {
  return JSON.parse(localStorage.getItem("clientToken"));
}

export function getUserToken() {
  return localStorage.getItem("token");
}
