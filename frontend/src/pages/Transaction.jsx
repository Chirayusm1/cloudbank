import { useEffect, useState } from "react";
import api from "../services/api";

function Transaction() {
  const [accounts, setAccounts] = useState([]);

  const [fromAccountId, setFromAccountId] = useState("");
  const [toAccountId, setToAccountId] = useState("");
  const [amount, setAmount] = useState("");
  const [type, setType] = useState("Transfer");

  const [message, setMessage] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false);

  useEffect(() => {
    const loadAccounts = async () => {
      try {
        const response = await api.get("/Account");

        setAccounts(response.data);
      } catch {
        setError("Unable to load accounts.");
      }
    };

    loadAccounts();
  }, []);

  const handleTransfer = async (event) => {
    event.preventDefault();

    setMessage("");
    setError("");

    if (fromAccountId === toAccountId) {
      setError("From and To accounts must be different.");
      return;
    }

    if (Number(amount) <= 0) {
      setError("Amount must be greater than zero.");
      return;
    }

    setLoading(true);

    try {
      await api.post("/Transaction", {
        fromAccountId,
        toAccountId,
        amount: Number(amount),
        type,
      });

      setMessage("Transaction completed successfully.");

      setAmount("");
      setFromAccountId("");
      setToAccountId("");
    } catch (error) {
      if (error.response?.data?.message) {
        setError(error.response.data.message);
      } else {
        setError("Transaction failed.");
      }
    } finally {
      setLoading(false);
    }
  };

  return (
    <div>
      <h1>Transfer Money</h1>

      <form onSubmit={handleTransfer}>
        <div>
          <label>From Account</label>

          <select
            value={fromAccountId}
            onChange={(event) =>
              setFromAccountId(event.target.value)
            }
            required
          >
            <option value="">Select account</option>

            {accounts.map((account) => (
              <option key={account.id} value={account.id}>
                {account.accountNumber} - ₹{account.balance}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label>To Account</label>

          <select
            value={toAccountId}
            onChange={(event) =>
              setToAccountId(event.target.value)
            }
            required
          >
            <option value="">Select account</option>

            {accounts.map((account) => (
              <option key={account.id} value={account.id}>
                {account.accountNumber}
              </option>
            ))}
          </select>
        </div>

        <div>
          <label>Amount</label>

          <input
            type="number"
            min="0.01"
            step="0.01"
            value={amount}
            onChange={(event) => setAmount(event.target.value)}
            required
          />
        </div>

        <div>
          <label>Transaction Type</label>

          <select
            value={type}
            onChange={(event) => setType(event.target.value)}
          >
            <option value="Transfer">Transfer</option>
          </select>
        </div>

        {error && <p>{error}</p>}

        {message && <p>{message}</p>}

        <button type="submit" disabled={loading}>
          {loading ? "Processing..." : "Transfer"}
        </button>
      </form>
    </div>
  );
}

export default Transaction;
