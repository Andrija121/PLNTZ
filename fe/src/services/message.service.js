import { callExternalApi } from "./external-api.service";

const apiServerUrl = process.env.REACT_APP_API_SERVER_URL;

export const getPublicResource = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/User`,
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

export const getProtectedResource = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/User/2`,
    method: "GET",
    headers: {
      "content-type": "application/json",
      Authorization: `Bearer ${accessToken}`,
    },
  };

  const { data, error, statusCode } = await callExternalApi({ config });

  if (statusCode === 401) {
    console.log("unAUTHORIZED");
  }

  return {
    data: data || null,
    error,
  };
};

export const getAdminResource = async (accessToken) => {
  const config = {
    url: `${apiServerUrl}/api/User/1`,
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
