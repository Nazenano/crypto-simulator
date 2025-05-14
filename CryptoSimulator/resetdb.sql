-- ClearDatabase.sql
USE db;

-- Delete all data in order to respect foreign key constraints
DELETE FROM PriceHistories;
DELETE FROM Transactions;
DELETE FROM Portfolios;
DELETE FROM Wallets;
DELETE FROM CryptoCurrencies;
DELETE FROM Users;

-- Reset identity columns to start from 1
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Wallets', RESEED, 0);
DBCC CHECKIDENT ('CryptoCurrencies', RESEED, 0);
DBCC CHECKIDENT ('Portfolios', RESEED, 0);
DBCC CHECKIDENT ('Transactions', RESEED, 0);
DBCC CHECKIDENT ('PriceHistories', RESEED, 0);
