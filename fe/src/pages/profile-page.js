import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect, useState } from "react";
import { PageLayout } from "../components/page-layout";
import {
  createUser,
  getUserWithAuthZId,
  updateUser,
} from "../services/user.service";

// TO BE FIXED HOW STATES CHANGE WHEN TOGGLING BETWEEN SAVED CHANGES AND EDIT USER button

export const Profile = () => {
  const { user, getAccessTokenSilently } = useAuth0();
  const [changeSaved, setChangeSaved] = useState(false);
  const [userData, setUserData] = useState(null);
  const [editMode, setEditMode] = useState(false);
  const [editUserData, setEditUserData] = useState({
    authzId: user.sub.replace(/^auth0\|/, ""),
    email: user.email,
    firstName: "",
    lastName: "",
    birthday: "",
    country: "",
    city: "",
    lastSeen: "",
    isActive: true,
  });
  const handleEditToggle = () => {
    setEditMode(!editMode);
  };
  const handleInputChange = (e) => {
    const { name, value } = e.target;
    setEditUserData({
      ...editUserData,
      [name]: value,
    });
  };

  // Define the list of countries and cities
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

  useEffect(() => {
    const fetchUserData = async () => {
      try {
        const accessToken = await getAccessTokenSilently();

        if (accessToken) {
          const result = await getUserWithAuthZId(
            accessToken,
            user.sub.replace(/^auth0\|/, "")
          );
          const { data, error } = result;

          if (data) {
            // Update the state with the retrieved user data
            setUserData(data);

            setEditUserData((prevEditUserData) => ({
              ...prevEditUserData,
              authzId: data.authzId || user.sub.replace(/^auth0\|/, ""),
              email: data.email || user.email,
              firstName: data.firstName || "",
              lastName: data.lastName || "",
              birthday: data.birthday || "",
              country: data.country || "",
              city: data.city || "",
            }));
          } else {
            console.error("Error fetching user data:", error);
          }
        }
      } catch (error) {
        console.error("Error fetching user data:", error);
      }
    };

    // Call the fetchUserData function
    fetchUserData();
  }, [getAccessTokenSilently]);

  useEffect(() => {
    const getUserMetadata = async () => {
      try {
        const accessToken = await getAccessTokenSilently();
        console.log(accessToken);

        // Create user on the backend
        if (accessToken) {
          let newUser = {
            authzId: user.sub.replace(/^auth0\|/, ""), // Assuming user.sub is the Auth0 subject (sub) claim
            email: user.email,
            lastName: user.nickname,
            firstName: user.name,
            last_seen: user.updated_at,
          };

          // Call the createUser function from your service
          await createUser(accessToken, newUser);
        }
      } catch (e) {
        console.error(e.message);
      }
    };

    getUserMetadata();
  }, [getAccessTokenSilently, user?.sub]);
  if (!user) {
    return null;
  }
  const handleFormSubmit = async (e) => {
    e.preventDefault();

    try {
      console.log("Submitting form...");
      const accessToken = await getAccessTokenSilently();

      if (accessToken) {
        // Call the updateUser function to update user data
        const result = await updateUser(
          accessToken,
          user.sub.replace(/^auth0\|/, ""),
          editUserData
        );
        const { data, error } = result;
        console.log(data);
        setChangeSaved(true);
        if (data) {
          setUserData(data);
        } else {
          console.error("Error updating user data:", error);
        }
      }
    } catch (error) {
      console.error("Error updating user data!!!:", error);
    }
  };

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          Profile Page
        </h1>
        <div className="content__body">
          <p id="page-description">
            <span>
              <strong>Only authenticated users can access this page.</strong>
            </span>
          </p>
          <div className="profile-grid">
            <div className="profile__header">
              <img
                src={user.picture}
                alt="Profile"
                className="profile__avatar"
              />
              <div className="profile__headline">
                <h2 className="profile__title">{user.name}</h2>
              </div>
            </div>
            {/* Display user information */}
            {userData && (
              <div>
                <h3>User Information</h3>
                {!changeSaved && editMode ? (
                  // Display editable form in edit mode
                  <form onSubmit={handleFormSubmit}>
                    <label>
                      First Name:
                      <input
                        type="text"
                        name="firstName"
                        value={editUserData.firstName}
                        onChange={handleInputChange}
                      />
                    </label>
                    <label>
                      Last Name:
                      <input
                        type="text"
                        name="lastName"
                        value={editUserData.lastName}
                        onChange={handleInputChange}
                      />
                    </label>
                    <label>
                      Birthday:
                      <input
                        type="date"
                        name="birthday"
                        value={editUserData.birthday}
                        onChange={handleInputChange}
                      />
                    </label>
                    <label>
                      Email:
                      <input
                        readOnly={true}
                        type="email"
                        name="email"
                        value={userData.email}
                        onChange={handleInputChange}
                      ></input>
                    </label>
                    <label
                      hidden={true}
                      value={userData.authzId}
                      onChange={handleInputChange}
                    ></label>
                    <label>
                      Country:
                      <select
                        value={editUserData.country}
                        name="country"
                        onChange={handleInputChange}
                      >
                        <option value="">Select</option>
                        {countries.map((country) => (
                          <option key={country} value={country}>
                            {country}
                          </option>
                        ))}
                        <option value="Other">Other</option>
                      </select>
                    </label>
                    <label>
                      City:
                      <select
                        value={editUserData.city}
                        name="city"
                        onChange={handleInputChange}
                      >
                        <option value="">Select</option>
                        {editUserData.country &&
                          citiesByCountry[editUserData.country]?.map((city) => (
                            <option key={city} value={city}>
                              {city}
                            </option>
                          ))}
                        <option value="Other">Other</option>
                      </select>
                      {editUserData.city === "Other" && (
                        <input
                          type="text"
                          name="city"
                          value={editUserData.city}
                          onChange={handleInputChange}
                        />
                      )}
                    </label>
                    <button type="submit">Save Changes</button>
                  </form>
                ) : (
                  // Display non-editable user information
                  <>
                    <p>First Name: {userData.firstName}</p>
                    <p>Last Name: {userData.lastName}</p>
                    <p>Email: {userData.email}</p>
                    <p>Country: {userData.country}</p>
                    <p>City: {userData.city}</p>
                    {/* Add other user properties as needed */}
                    {userData.birthday && <p>Birthday: {userData.birthday}</p>}
                    <button onClick={handleEditToggle}>Edit</button>
                  </>
                )}
              </div>
            )}
          </div>
        </div>
      </div>
    </PageLayout>
  );
};
