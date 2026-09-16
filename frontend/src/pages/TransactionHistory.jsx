import { useEffect, useState } from "react";
import { useParams, useNavigate } from "react-router-dom";
import api from "../services/api";

function TransactionHistory() {
  const { accountId } = useParams();
  const navigate = useNavigate();

  const [transactions, setTransactions] = useState([]);
  const [loading, setLoading] = useState(true);
  const [error, setError] = useState("");

  useEffect(() => {
    const loadTransactions = async () => {
      try {
        const response = await api.get(
          `/Transaction/account/${accountId}`
        );

        setTransactions(response.data);
      } catch {
        setError("Unable to load transaction history.");
      } finally {
        setLoading(false);
      }
    };

    loadTransactions();
  }, [accountId]);

  if (loading) {
    return <p>Loading transactions...</p>;
  }

  if (error) {
    return (
      <div>
        <p>{error}</p>

        <button onClick={() => navigate(`/accounts/${accountId}`)}>
          Back to Account
        </button>
      </div>
    );
  }

  return (
    <div>
      <button onClick={() => navigate(`/accounts/${accountId}`)}>
        Back to Account
      </button>

      <h1>Transaction History</h1>

      {transactions.length === 0 && (
        <p>No transactions found.</p>
      )}

      {transactions.length > 0 && (
        <div>
          {transactions.map((transaction) => (
            <div key={transaction.id}>
              <p>
                <strong>Type:</strong> {transaction.type}
              </p>

              <p>
                <strong>Amount:</strong> ₹{transaction.amount}
              </p>

              <p>
                <strong>Status:</strong> {transaction.status}
              </p>

              <p>
                <strong>From:</strong> {transaction.fromAccountId}
              </p>

              <p>
                <strong>To:</strong> {transaction.toAccountId}
              </p>

              <p>
                <strong>Date:</strong>{" "}
                {new Date(transaction.createdAt).toLocaleString()}
              </p>

              <hr />
            </div>
          ))}
        </div>
      )}
    </div>
  );
}

export default TransactionHistory;