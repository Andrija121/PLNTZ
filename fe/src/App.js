import "./App.css";
import LoginButton from "./LoginButton";
import LogoutButton from "./LogoutButton";
import Profile from "./Profile";

function App() {
  return (
    //class name could be removed
    <div className="App">
      <header></header>
      <LoginButton />
      <LogoutButton />
      <Profile />
    </div>
  );
}

export default App;
