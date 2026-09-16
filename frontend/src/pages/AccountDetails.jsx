import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";

function AccountDetails() {
  const { id } = useParams();
  const navigate = useNavigate();

  const [account, setAccount] = useState(null);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadAccount = async () => {
      try {
        const response = await api.get(`/Account/${id}`);

        setAccount(response.data);
      } catch (error) {
        if (error.response?.status === 404) {
          setError("Account not found.");
        } else {
          setError("Unable to load account.");
        }
      } finally {
        setLoading(false);
      }
    };

    loadAccount();
  }, [id]);

  if (loading) {
    return <p>Loading account...</p>;
  }

  if (error) {
    return (
      <div>
        <p>{error}</p>

        <button onClick={() => navigate("/dashboard")}>
          Back to Dashboard
        </button>
      </div>
    );
  }

  return (
    <div>
      <button onClick={() => navigate("/dashboard")}>
        Back to Dashboard
      </button>
      <button
        onClick={() =>
            navigate(`/accounts/${account.id}/transactions`)
        }
        >
        Transaction History
      </button>

      <h1>Account Details</h1>

      <p>
        <strong>Account Number:</strong> {account.accountNumber}
      </p>

      <p>
        <strong>Balance:</strong> ₹{account.balance}
      </p>

      <p>
        <strong>Status:</strong> {account.status}
      </p>

      <p>
        <strong>Created:</strong>{" "}
        {new Date(account.createdAt).toLocaleString()}
      </p>
    </div>
  );
}

export default AccountDetails;