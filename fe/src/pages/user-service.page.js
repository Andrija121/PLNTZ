import React, { useEffect, useState } from "react";
import {
  createUser,
  updateUser,
  deleteUser,
  getAllUsers,
  getUserWithAuthZId,
} from "../services/user.service";
import { useAuth0 } from "@auth0/auth0-react";
import { PageLayout } from "../components/page-layout";

export const UserServicePage = () => {
  const [users, setUsers] = useState([]);
  const [fetchedUser, setFetchedUser] = useState(null);
  const { getAccessTokenSilently } = useAuth0();
  const [newUser, setNewUser] = useState({
    authzId: "",
    email: "",
    firstName: "",
    lastName: "",
    birthday: "",
    lastSeen: "",
    isActive: false,
  });

  useEffect(() => {
    // Fetch all users when the component mounts
    fetchAllUsers();
  }, []);

  const fetchAllUsers = async () => {
    try {
      const accessToken = await getAccessTokenSilently();
      const { data, error } = await getAllUsers(accessToken);
      if (error) {
        console.error("Error fetching users:", error);
      } else {
        setUsers(data || []);
      }
    } catch (error) {
      console.error("Error fetching users:", error);
    }
  };

  const handleCreateUser = async () => {
    try {
      const accessToken = await getAccessTokenSilently();
      const currentDate = new Date();
      const formattedDate = currentDate.toISOString();

      const createdUserData = {
        ...newUser,
        lastSeen: formattedDate,
        isActive: true,
      };

      const { data, error } = await createUser(accessToken, createdUserData);
      if (error) {
        console.error("Error creating user:", error);
        alert(`Error creating user: ${error}`);
      } else {
        // Update the users state with the new user
        setUsers((prevUsers) => [...prevUsers, data]);

        setNewUser({
          authzId: "",
          email: "",
          firstName: "",
          lastName: "",
          birthday: "",
          lastSeen: "",
          isActive: false,
        });
      }
    } catch (error) {
      console.error("Error creating user:", error);
    }
  };

  const handleUpdateUser = async (authzId, updatedUserData) => {
    console.log("handleUpdateUser called with:", { authzId, updatedUserData });
    try {
      const accessToken = await getAccessTokenSilently();
      const currentUser = users.find((user) => user.authzId === authzId);
      const updatedUserData = {
        firstName: newUser.firstName || currentUser.firstName,
        lastName: newUser.lastName || currentUser.lastName,
        birthday: newUser.birthday || currentUser.birthday,
      };
      const { error } = await updateUser(
        accessToken,
        authzId,
        JSON.stringify(updatedUserData)
      );

      if (error) {
        console.error("Error updating user:", error);
      } else {
        // Update the users state with the updated user
        setUsers((prevUsers) =>
          prevUsers.map((user) =>
            user.authzId === authzId ? { ...user, ...updatedUserData } : user
          )
        );
      }
    } catch (error) {
      console.error("Error updating user:", error);
    }
  };

  const handleDeleteUser = async (authzId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      const { error } = await deleteUser(accessToken, authzId);
      if (error) {
        console.error("Error deleting user:", error);
      } else {
        // Update the users state by removing the deleted user
        setUsers((prevUsers) =>
          prevUsers.filter((user) => user.authzId !== authzId)
        );
      }
    } catch (error) {
      console.error("Error deleting user:", error);
    }
  };

  const handleFetchUser = async (authzId) => {
    try {
      const accessToken = await getAccessTokenSilently();
      const { data, error } = await getUserWithAuthZId(accessToken, authzId);
      if (error) {
        console.error("Error fetching user:", error);
      } else {
        // Log or use the fetched user data as needed
        console.log("Fetched user:", data);

        // Update the state with the fetched user data
        setUsers((prevUsers) =>
          prevUsers.map((user) =>
            user.authzId === authzId ? { ...user, ...data } : user
          )
        );
        setFetchedUser(data);
      }
    } catch (error) {
      console.error("Error fetching user:", error);
    }
  };

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          User Service Page
        </h1>
        <div className="content__body">
          <div>
            {/* Display users */}
            <ul>
              {users.map((user) => (
                <li key={user.authzId}>
                  {user.firstName} - {user.lastName} - {user.birthday} -{" "}
                  {user.authzId}
                  {/* Add other fields as needed */}
                  <button
                    style={{ backgroundColor: "blue", margin: "10px" }}
                    className="button"
                    onClick={() => handleUpdateUser(user.authzId)}
                  >
                    Update
                  </button>
                  <button
                    style={{ backgroundColor: "blue", margin: "10px" }}
                    className="button"
                    onClick={() => handleDeleteUser(user.authzId)}
                  >
                    Delete
                  </button>
                  <button
                    style={{
                      backgroundColor: "blue",
                      marginLeft: "10px",
                    }}
                    className="button"
                    onClick={() => handleFetchUser(user.authzId)}
                  >
                    Fetch User
                  </button>
                </li>
              ))}
            </ul>

            {/* Fetch user form */}
            {/* Display fetched user */}
            {fetchedUser && (
              <div
                style={{
                  display: "flex",
                  alignContent: "center",
                  alignItems: "center",
                  margin: "30px",
                }}
              >
                <h1
                  className="content__title"
                  style={{
                    color: "white",
                    marginTop: "-2px",
                    marginRight: "15px",
                    fontSize: "25px",
                  }}
                >
                  Fetched User Details
                </h1>
                <p style={{ marginLeft: "5px", fontSize: "20px" }}>
                  ID: {fetchedUser.authzId};
                </p>
                <p style={{ marginLeft: "5px", fontSize: "20px" }}>
                  Name: {fetchedUser.firstName} {fetchedUser.lastName};
                </p>
                <p style={{ marginLeft: "5px", fontSize: "20px" }}>
                  Birthday: {fetchedUser.birthday};
                </p>
                <p
                  style={{
                    marginLeft: "5px",
                    fontSize: "20px",
                  }}
                >
                  Authz ID: {fetchedUser.authzId};
                </p>
                {/* Add other fields as needed */}
              </div>
            )}

            {/* Create user form */}
            <div
              style={{
                width: "200px",
                margin: "30px  ",
                alignItems: "center",
                alignContent: "center",
              }}
            >
              <h2
                className="content__title"
                style={{
                  marginRight: "10px",
                  wordSpacing: "10px",
                  wordWrap: "inherit",
                }}
              >
                Create New User
              </h2>
              <label style={{ marginInline: "5px" }}>
                Name:
                <input
                  type="text"
                  value={newUser.firstName}
                  onChange={(e) =>
                    setNewUser({ ...newUser, firstName: e.target.value })
                  }
                />
              </label>
              <label style={{ marginInline: "5px" }}>
                Last Name:
                <input
                  type="text"
                  value={newUser.lastName}
                  onChange={(e) =>
                    setNewUser({ ...newUser, lastName: e.target.value })
                  }
                />
              </label>
              <label style={{ marginInline: "5px" }}>
                Auth0Id:
                <input
                  type="text"
                  value={newUser.authzId}
                  onChange={(e) =>
                    setNewUser({ ...newUser, authzId: e.target.value })
                  }
                />
              </label>
              <label style={{ marginInline: "5px" }}>
                Email:
                <input
                  type="email"
                  value={newUser.email}
                  onChange={(e) =>
                    setNewUser({ ...newUser, email: e.target.value })
                  }
                />
              </label>
              <label style={{ marginInline: "5px" }}>
                Birthday:
                <input
                  type="date"
                  value={newUser.birthday}
                  onChange={(e) =>
                    setNewUser({ ...newUser, birthday: e.target.value })
                  }
                />
              </label>
              <button
                style={{ margin: "10px", width: "110px" }}
                className="button__login"
                onClick={handleCreateUser}
              >
                Create User
              </button>
            </div>
          </div>
        </div>
      </div>
    </PageLayout>
  );
};
