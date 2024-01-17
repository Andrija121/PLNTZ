import { useAuth0 } from "@auth0/auth0-react";
import React, { useEffect, useState } from "react";
import { PageLayout } from "../components/page-layout";
import { getUserWithAuthZId } from "../services/user.service";
import {
  GetFriendshipsForUser,
  GetPendingFriendshipsForUser,
  RespondToFriendshipRequest,
} from "../services/friendship.service";

const FriendsList = () => {
  const { user, getAccessTokenSilently } = useAuth0();
  const [friends, setFriends] = useState([]);

  useEffect(() => {
    const fetchFriends = async () => {
      try {
        const accessToken = await getAccessTokenSilently();
        const authzId = user.sub.replace(/^auth0\|/, "");

        // Get friends for the current user
        const friendsResult = await GetFriendshipsForUser(accessToken, authzId);
        const friendDetails = await Promise.all(
          friendsResult.data.map(async (friendship) => {
            // Decode the second user's ID and convert it to their name
            const friendUserId =
              friendship.user_1_AuthzId === authzId
                ? friendship.user_2_AuthzId
                : friendship.user_1_AuthzId;
            const { data } = await getUserWithAuthZId(
              accessToken,
              friendUserId
            );

            return data
              ? {
                  id: data.id,
                  name: `${data.firstName} ${data.lastName}`,
                  Email: `${data.email}`,
                }
              : null;
          })
        );

        // Filter out null values (friends without valid details)
        const validFriends = friendDetails.filter((friend) => friend !== null);

        setFriends(validFriends);
      } catch (error) {
        console.error("Error fetching friends:", error);
      }
    };

    fetchFriends();
  }, [user, getAccessTokenSilently]);

  return (
    <div className="friends-list">
      <h2 style={{ color: "#635dff", fontSize: "20px", marginTop: "55px" }}>
        Your Friends
      </h2>
      <ul>
        {friends.map((friend) => (
          <li key={friend.id}>
            {friend.name} -{" "}
            <a
              href={`mailto:${friend.Email}`}
              style={{ textDecoration: "underline" }}
              type="email"
            >
              {friend.Email}
            </a>
          </li>
        ))}
      </ul>
    </div>
  );
};

const PendingRequestsList = ({ requests, onRespond, currentUserAuthzId }) => {
  const [senderNames, setSenderNames] = useState({});
  const { getAccessTokenSilently } = useAuth0();

  useEffect(() => {
    const fetchSenderNames = async () => {
      try {
        const accessToken = await getAccessTokenSilently();

        const names = await Promise.all(
          requests.map(async (request) => {
            const { data } = await getUserWithAuthZId(
              accessToken,
              request.user_1_AuthzId
            );
            return data
              ? `${data.firstName} ${data.lastName} \n from ${data.city},${data.country}`
              : "Unknown User";
          })
        );

        const senderNamesMap = requests.reduce((acc, request, index) => {
          acc[request.id] = names[index];
          return acc;
        }, {});

        setSenderNames(senderNamesMap);
      } catch (error) {
        console.error("Error fetching sender names:", error);
      }
    };

    fetchSenderNames();
  }, [getAccessTokenSilently, requests]);
  return (
    <div className="pending-requests-list">
      <h2 style={{ color: "#635dff", fontSize: "20px" }}>
        Pending Friendship Requests
      </h2>
      <ul>
        {requests.map((request) => (
          <li key={request.id}>
            {currentUserAuthzId === request.user_2_AuthzId && (
              <>
                {senderNames[request.id] || "Unknown User"} wants to be friends
                <button
                  className="button__logout"
                  style={{ margin: "5px", width: "10px" }}
                  onClick={() => onRespond(request.friendshipId, true)}
                >
                  Accept
                </button>
                <button
                  className="button__sign-up"
                  style={{ margin: "5px", width: "10px" }}
                  onClick={() => onRespond(request.friendshipId, false)}
                >
                  Decline
                </button>
              </>
            )}
          </li>
        ))}
      </ul>
    </div>
  );
};

export const FriendsPage = () => {
  const { user, getAccessTokenSilently } = useAuth0();
  const [userData, setUserData] = useState([]);
  const [friends, setFriends] = useState([]);
  const [pendingRequests, setPendingRequests] = useState([]);

  useEffect(() => {
    const fetchData = async () => {
      try {
        const accessToken = await getAccessTokenSilently();
        const authzId = user.sub.replace(/^auth0\|/, "");

        // Get user information
        const userResult = await getUserWithAuthZId(accessToken, authzId);
        setUserData(userResult.data);

        // Get friends
        const friendsResult = await GetFriendshipsForUser(accessToken, authzId);
        setFriends(friendsResult.data);

        // Get pending requests
        const pendingResult = await GetPendingFriendshipsForUser(
          accessToken,
          authzId
        );
        setPendingRequests(pendingResult.data);
      } catch (error) {
        console.error("Error fetching data:", error);
      }
    };

    fetchData();
  }, []);

  const handleRespondToRequest = async (friendshipId, accept) => {
    try {
      const accessToken = await getAccessTokenSilently();
      console.log("friendshipId:", friendshipId);
      console.log("accept:", accept);

      // Call RespondToFriendshipRequest with the boolean value directly
      await RespondToFriendshipRequest(accessToken, friendshipId, { accept });

      // Refresh friends and pending requests
      // (you can fetch the updated data or manipulate the state directly)
      window.location.reload();
    } catch (error) {
      console.error("Error responding to request:", error);
    }
  };

  return (
    <PageLayout>
      <div className="content-layout">
        <h1 id="page-title" className="content__title">
          Friendships
        </h1>
        <div className="content__body">
          <b>
            Please click on the email on of the friends in order to establish
            contact
          </b>
          <div className="profile__headline"></div>
          <div className="profile-grid">
            <div className="profile__header" style={{ marginTop: "-30px" }}>
              <img
                src={user.picture}
                alt="Profile"
                className="profile__avatar"
              />
              <h2
                className="profile__title"
                style={{ marginTop: "20px", fontSize: "22px" }}
              >
                Welcome back{" "}
                <b style={{ fontSize: "25px", color: "#635dff" }}>
                  {userData.firstName} {userData.lastName}
                </b>
              </h2>
            </div>
          </div>
          <div>
            {/* Display Friends List */}
            <FriendsList friends={friends} userData={userData} />

            {/* Display Pending Requests List */}
            <PendingRequestsList
              requests={pendingRequests}
              onRespond={handleRespondToRequest}
              currentUserAuthzId={user.sub.replace(/^auth0\|/, "")}
            />
          </div>
        </div>
      </div>
    </PageLayout>
  );
};
