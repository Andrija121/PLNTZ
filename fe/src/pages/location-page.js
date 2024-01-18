import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect, useState } from "react";
import { useNavigate } from "react-router-dom";
import { PageLayout } from "../components/page-layout";
import { getProtectedResource } from "../services/message.service";
import { getUserWithAuthZId } from "../services/user.service";
import {
  getUsersByCity,
  getUsersByCountry,
  SendUserIdsForFriendship,
} from "../services/location.service";
import { SendFriendshipRequest } from "../services/friendship.service";

const countries = [
  "USA",
  "The Netherlands",
  "Belgium",
  "England",
  "Germany",
  "Spain",
  "France",
  "Serbia",
];

const citiesByCountry = {
  USA: ["New York", "Los Angeles", "Chicago"],
  "The Netherlands": ["Amsterdam", "Rotterdam", "Utrecht", "Eindhoven"],
  Belgium: ["Brussels", "Antwerp", "Ghent"],
  England: ["London", "Manchester", "Liverpool"],
  Germany: ["Berlin", "Munich", "Hamburg"],
  Spain: ["Madrid", "Barcelona", "Seville"],
  France: ["Paris", "Marseille", "Lyon"],
  Serbia: ["Belgrade", "Novi Sad", "Nis"],
};

export const LocationPage = () => {
  const [loading, setLoading] = useState(true);
  const { user, getAccessTokenSilently } = useAuth0();
  const navigate = useNavigate();
  const [selectedLocation, setSelectedLocation] = useState("");
  const [users, setUsers] = useState([]);
  const [error, setError] = useState(null);
  const [friendshipStatus, setFriendshipStatus] = useState({});

  const handleLocationChange = async (location) => {
    setSelectedLocation(location);
    if (!location) {
      setUsers([]);
      setError(null); // Clear any previous errors
      return;
    }

    try {
      const accessToken = await getAccessTokenSilently();
      const [country, city] = location.split(",");

      let result;
      if (city) {
        result = await getUsersByCity(accessToken, city.trim());
      } else {
        result = await getUsersByCountry(accessToken, country.trim());
      }

      if (result.error) {
        setError(result.error);
      } else {
        setUsers(result.data);
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const handleAddFriend = async (friendId) => {
    try {
      const accessToken = await getAccessTokenSilently();

      // Send user IDs for friendship
      await SendUserIdsForFriendship(
        accessToken,
        user.sub.replace(/^auth0\|/, ""),
        friendId
      );
      await new Promise((resolve) => setTimeout(resolve, 5000));

      await SendFriendshipRequest(accessToken);
      setFriendshipStatus((prevStatus) => ({
        ...prevStatus,
        [friendId]: true,
      }));

      // Send friendship request

      // You can optionally handle the success or show a message to the user
      console.log("Friendship request sent successfully");
    } catch (error) {
      // Handle errors
      console.error("Error adding friend:", error);
    }
  };

  useEffect(() => {
    let isMounted = true;

    const getLocationAndMessage = async () => {
      try {
        setLoading(true);

        const accessToken = await getAccessTokenSilently();

        // Fetch user data
        const { data: userData, error: userError } = await getUserWithAuthZId(
          accessToken,
          user.sub.replace(/^auth0\|/, "")
        );

        // Check if location is not set, redirect to profile page
        if (userData.city == null || userData.country == null) {
          console.error("Error fetching user data:", userError);
          alert("Please add your location");
          setTimeout(() => {
            navigate("/profile"); // Redirect after a short delay
          }, 3500); // Adjust the delay as needed
          return;
        }

        // Fetch protected resource (message)
        const { error } = await getProtectedResource(accessToken);

        if (!isMounted) {
          return;
        }

        if (error) {
          // Display a message to set the location on the profile page
          setLoading(false);
        }
      } catch {
        console.log("error");
      }
    };

    getLocationAndMessage();

    return () => {
      isMounted = false;
    };
  }, [getAccessTokenSilently, navigate, user.sub]);

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          Location page
        </h1>
        <div className="content__body">
          <p id="page-description">
            {loading ? (
              "Loading..."
            ) : (
              <div className="content-layout">
                {/* Add a dropdown or input field for selecting location */}
                <select
                  style={{
                    margin: "1rem 0",
                    padding: "0.5rem",
                    fontSize: "1.2rem",
                  }}
                  value={selectedLocation}
                  onChange={(e) => handleLocationChange(e.target.value)}
                >
                  <option value="">Select Location</option>
                  {countries.map((country) => [
                    <option key={country} value={country}>
                      {country}
                    </option>,
                    <optgroup label={country} key={`optgroup-${country}`}>
                      {citiesByCountry[country].map((city) => (
                        <option
                          key={`${country}-${city}`}
                          value={`${country},${city}`}
                        >
                          {city}
                        </option>
                      ))}
                    </optgroup>,
                  ])}
                </select>

                {/* Display the list of users */}
                <div>
                  <h3 style={{ color: "white" }}>Available People</h3>
                </div>
                <div style={{ alignContent: "center" }}>
                  {users
                    .filter(
                      (u) => u.authzId !== user.sub.replace(/^auth0\|/, "")
                    )
                    .map((user) => (
                      <div
                        key={user.authzId}
                        style={{
                          display: "flex",
                          justifyContent: "space-between",
                          alignItems: "center",
                          marginBottom: "8px",
                        }}
                      >
                        <p style={{ marginRight: "10px", marginBottom: "0" }}>
                          <b>Name:</b> {user.firstName}, {user.lastName},
                          <br />
                          <b>Email:</b> {user.email},<br /> <b>Place:</b>{" "}
                          {user.city}, {user.country}
                        </p>

                        <button
                          style={{
                            padding: "4px 8px",
                            fontSize: "1.5rem",
                            cursor: "pointer",
                          }}
                          onClick={() => handleAddFriend(user.authzId)}
                          disabled={friendshipStatus[user.authzId]}
                        >
                          {friendshipStatus[user.authzId]
                            ? "Friend Request Sent"
                            : "Add Friend +"}
                        </button>
                      </div>
                    ))}
                </div>
                {error &&
                  selectedLocation &&
                  (() => alert("Please select a location"))}
              </div>
            )}
          </p>
        </div>
      </div>
    </PageLayout>
  );
};
