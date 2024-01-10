import { callExternalApi } from "./external-api.service";

const apiServerUrl = process.env.REACT_APP_API_SERVER_URL;

export const getUsersByCountry = async (accessToken, country) => {
  const config = {
    url: `${apiServerUrl}/api/User/country/${country}`,
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

export const getUsersByCity = async (accessToken, city) => {
  const config = {
    url: `${apiServerUrl}/api/User/city/${city}`,
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
