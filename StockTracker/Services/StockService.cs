using Microsoft.AspNet.Identity;
using StockTracker.Domain;
using StockTracker.Infrastructure;
using StockTracker.Services.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StockTracker.Services {
    public class StockService {

        private StockRepository _stockRepo;
        private TransactionRepository _transactionRepo;
        private ApplicationUserManager _userRepo;

        public StockService(StockRepository stockRepo, TransactionRepository transRepo, ApplicationUserManager userRepo) {
            _stockRepo = stockRepo;
            _transactionRepo = transRepo;
            _userRepo = userRepo;
        }
        public IList<StockDTO> Search(string searchTerms) {
            return (from s in _stockRepo.FindStocksLike(searchTerms)
                    select new StockDTO() {
                        Ticker = s.Ticker,
                        Name = s.Name,
                        Price = s.Price,
                        OpenPrice = s.OpenPrice
                    }).ToList();
        }
        public ExpandedStockDTO GetStockWithTransactions(string ticker, string username) {
            return (from s in _stockRepo.FindStock(ticker)
                    select new ExpandedStockDTO() {
                        Ticker = s.Ticker,
                        Name = s.Name,
                        Price = s.Price,
                        OpenPrice = s.OpenPrice,
                        LowPrice = s.LowPrice,
                        HighPrice = s.HighPrice,
                        Description = s.Description,
                        Transactions = (from t in s.Transactions
                                        where t.User.UserName == username
                                        orderby t.DateCreated descending
                                        select new TransactionDTO() {
                                            Price = t.Price,
                                            Quantity = t.Quantity,
                                            DateCreated = t.DateCreated
                                        }).ToList()
                    }).FirstOrDefault();
        }
        public TransactionDTO Buy(string ticker, int quantity, string username) {

            var stock = _stockRepo.FindStock(ticker).FirstOrDefault();
            var user = _userRepo.FindByName(username);
            var transaction = new Transaction() {
                Stock = stock,
                User = user,
                Price = stock.Price,
                Quantity = quantity,
                Type = "buy"

            };
            _transactionRepo.Add(transaction);
            _transactionRepo.SaveChanges();

            return new TransactionDTO() {
                Ticker = transaction.Stock.Ticker,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                DateCreated = transaction.DateCreated
            };
        }

        public TransactionDTO Sell(string ticker, int quantity, string username) {

            var stock = _stockRepo.FindStock(ticker).FirstOrDefault();
            var user = _userRepo.FindByName(username);
            var transaction = new Transaction() {
                Stock = stock,
                User = user,
                Price = stock.Price,
                Quantity = -quantity,
                Type = "sell"

            };
            _transactionRepo.Add(transaction);
            _transactionRepo.SaveChanges();

            return new TransactionDTO() {
                Ticker = transaction.Stock.Ticker,
                Price = transaction.Price,
                Quantity = transaction.Quantity,
                DateCreated = transaction.DateCreated
            };
        }

        public bool CheckExists(string ticker) {
            return _stockRepo.CheckExists(ticker);
        }

    }
}
