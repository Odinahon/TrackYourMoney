using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
    public class Transaction: BaseEntity
    {
        [Key]
        public int TransactionId {get; set;}
        public int Amount {get; set;}
        
        // [ForeingKey("WhoMadeTransaction")]
        public int UserId {get; set;}
        // [ForeignKey("UserId")]
        public User WhoMadeTransaction {get;set;}
    }

}