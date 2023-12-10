import { callExternalApi } from "./external-api.service";

const apiServerUrl = process.env.REACT_APP_API_SERVER_URL;

export const getAllUsers = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/v1/User`,
    method: "Get",
    headers: {
      "content-type": "application-json",
      Authorization: `Bearer ${accessToken}`,
    },
  };

  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};

export const getUserWithId = async (accessToken, id) => {
  const config = {
    url: `${apiServerUrl}/api/v1/User/${id}`,
    method: "GET",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
  };

  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};

export const createUser = async (accessToken, userData) => {
  const config = {
    url: `${apiServerUrl}/api/v1/User`,
    method: "POST",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
    data: userData,
  };
  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};

export const updateUser = async (accessToken, id, updatedUserData) => {
  const config = {
    url: `${apiServerUrl}/api/v1/User/${id}`,
    method: "PUT",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
    data: updatedUserData,
  };

  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};

export const deleteUser = async (accessToken, id) => {
  const config = {
    url: `${apiServerUrl}/api/v1/User/${id}`,
    method: "DELETE",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
  };
  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};
