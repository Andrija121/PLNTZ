import { callExternalApi } from "./external-api.service";

const apiServerUrl = process.env.REACT_APP_API_SERVER_URL;

export const getAllUsers = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/User`,
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
    url: `${apiServerUrl}/api/User/${id}`,
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
    url: `${apiServerUrl}/api/User`,
    method: "POST",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
    data: userData,
  };

  try {
    const { data, error } = await callExternalApi({ config });

    return {
      data: data || null,
      error,
    };
  } catch (error) {
    // Check if the error is a response from the server
    if (error.response) {
      // Display the error message on the frontend
      alert(error.response.data);
      return {
        data: null,
        error: error.response.data,
      };
    } else {
      // Handle other types of errors (e.g., network errors)
      console.error("Error creating user:", error);
      return {
        data: null,
        error: "An error occurred while creating the user.",
      };
    }
  }
};

export const updateUser = async (accessToken, id, updatedUserData) => {
  const config = {
    url: `${apiServerUrl}/api/User/${id}`,
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
    url: `${apiServerUrl}/api/User/${id}`,
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
