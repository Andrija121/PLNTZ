import { callExternalApi } from "./external-api.service";

const apiServerUrl = process.env.REACT_APP_API_SERVER_URL;

export const GetFriendshipsForUser = async (accessToken, authzId) => {
  const config = {
    url: `${apiServerUrl}/api/Friendship/${authzId}`,
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
export const GetPendingFriendshipsForUser = async (accessToken, authzId) => {
  const config = {
    url: `${apiServerUrl}/api/Friendship/pending/${authzId}`,
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

export const SendFriendshipRequest = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/Friendship/send-request`,
    method: "Post",
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
export const RespondToFriendshipRequest = async (
  accessToken,
  friendshipId,
  response
) => {
  const config = {
    url: `${apiServerUrl}/api/Friendship/respond-request/${friendshipId}`,
    method: "Post",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
    data: {
      accept: response.accept, // Make sure to access the "accept" property correctly
    }, // Pass the boolean value directly
  };
  const { data, error } = await callExternalApi({ config });

  return {
    data: data || null,
    error,
  };
};
export const deleteFriendshipForUser = async (accessToken, authzId) => {
  const config = {
    url: `${apiServerUrl}/api/Friendship/${authzId}`,
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
