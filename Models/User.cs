using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BankAccounts.Models
{
   
    public class User: BaseEntity
    {
        [Key]
        public int UserId {get; set;}
        
        public string FirstName {get; set;}
    
        public string LastName {get; set;}
        
        public string Email{get; set;}
       
        public string Password{get; set;}
        public int Balance {get; set;}

        // [InverseProperty("WhoMadeTransaction")]
        public List <Transaction> TransactionsIMade {get; set;}
      
        public User ()
        {
            TransactionsIMade = new List<Transaction>();
        }
    }
}
// [DataType(DataType.Date)]
//         [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]