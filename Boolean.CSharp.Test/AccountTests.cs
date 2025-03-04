﻿using Boolean.CSharp.Main;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Boolean.CSharp.Test
{
    [TestFixture]
    public class AccountTests
    {
        [Test]
        // Currently testing with null owner because user is not implemented yet
        public void TestCurrentAccountCreation()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new CurrentAccount(user);

            Assert.That(account.GetBalance(), Is.EqualTo(0));
            Assert.That(account.Owner, Is.EqualTo(user));
            Assert.That(account.Transactions.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestSavingsAccountCreation()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new SavingsAccount(user);

            Assert.That(account.GetBalance(), Is.EqualTo(0));
            Assert.That(account.Owner, Is.EqualTo(user));
            Assert.That(account.Transactions.Count, Is.EqualTo(0));
        }

        [Test]
        public void TestAccountDeposit()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new SavingsAccount(user);

            account.Deposit(100);

            Assert.That(account.GetBalance(), Is.EqualTo(100));
            Assert.That(account.Transactions.Count, Is.EqualTo(1));
            Assert.That(account.Owner, Is.EqualTo(user));
        }

        [Test]
        public void TestAccountWithdrawalFail()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new SavingsAccount(user);

            Assert.That(account.Withdraw(100, 0), Is.False);
            Assert.That(account.GetBalance(), Is.EqualTo(0));
            Assert.That(account.Transactions.Count, Is.EqualTo(0));
            Assert.That(account.Owner, Is.EqualTo(user));
        }

        [Test]
        public void TestAccountWithdrawal()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new SavingsAccount(user);

            account.Deposit(200);

            Assert.That(account.Withdraw(100, 0), Is.True);
            Assert.That(account.GetBalance(), Is.EqualTo(100));
            Assert.That(account.Transactions.Count, Is.EqualTo((2)));
            Assert.That(account.Owner, Is.EqualTo(user));
        }

        [Test]
        public void TestAccountDepositTransaction()
        {
            User user = new User(0, "Kristian", BankLocation.Stavanger);
            Account account = new SavingsAccount(user);

            account.Deposit(100);

            Assert.That(account.GetBalance(), Is.EqualTo(100));
            Assert.That(account.Transactions.Count, Is.EqualTo(1));
            Assert.That(account.Transactions[0].Amount, Is.EqualTo(100));
        }

        [Test]
        public void TestOverdraft()
        {
            Bank bank = new Bank(BankLocation.Stavanger);
            User user = bank.CreateUser(0, "Kristian");

            bank.GenerateOverdraftRequest(user, 100);
            bank.ApproveAllOverdraftRequests();

            user.Withdraw(100, user.Account);

            Assert.That(user.OverdraftAmount, Is.EqualTo(100));
            Assert.That(user.Account.GetBalance(), Is.EqualTo(-100));
        }
    }
}
