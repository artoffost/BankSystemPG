CREATE TABLE users (
  Id SERIAL PRIMARY KEY,
  Username VARCHAR(45) NOT NULL UNIQUE,
  Password TEXT NOT NULL,
  FirstName VARCHAR(45) NOT NULL,
  LastName VARCHAR(45) NOT NULL
);


CREATE TABLE balance (
  idbalance SERIAL PRIMARY KEY,
  username VARCHAR(45) DEFAULT NULL,
  amount DECIMAL(10,2) DEFAULT '0.00',
  CONSTRAINT fk_balance_user FOREIGN KEY (username) REFERENCES users (Username) ON DELETE SET CASCADE
);

