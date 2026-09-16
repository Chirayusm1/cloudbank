import { useEffect, useState } from "react";
import api from "../services/api";
import { useAuth } from "../context/AuthContext";
import { useNavigate } from "react-router-dom";

function Dashboard() {
  const { logout } = useAuth();

  const [accounts, setAccounts] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");
  const navigate = useNavigate();

  useEffect(() => {
    const loadAccounts = async () => {
      try {
        const response = await api.get("/Account");

        setAccounts(response.data);
      } catch (error) {
        setError("Unable to load accounts.");
      } finally {
        setLoading(false);
      }
    };

    loadAccounts();
  }, []);

  return (
    <div>
      <h1>CloudBank Dashboard</h1>
        <button onClick={() => navigate("/transactions")}>
        Transfer Money
        </button>
      <button onClick={logout}>
        Logout
      </button>

      <h2>My Accounts</h2>

      {loading && <p>Loading accounts...</p>}

      {error && <p>{error}</p>}

      {!loading && !error && accounts.length === 0 && (
        <p>No accounts found.</p>
      )}

      {!loading && !error && accounts.length > 0 && (
        <div>
          {accounts.map((account) => (
            <div key={account.id}>
              <h3>Account</h3>

              <p>
                Account Number: {account.accountNumber}
              </p>

              <p>
                Balance: ₹{account.balance}
              </p>

              <p>
                Status: {account.status}
              </p>
              <button
                onClick={() => navigate(`/accounts/${account.id}`)}
                >
                View Account
                </button>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default Dashboard;
