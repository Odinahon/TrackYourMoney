using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BankAccounts.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BankAccounts.Controllers
{
    public class HomeController : Controller
    {
        private LoginContext _context;
 
        public HomeController(LoginContext context)
        {
            _context = context;
        }
 
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            ViewBag.errors = new List<string>();
            return View();
        }
        [HttpPost]
        [Route("adduser")]
        public IActionResult AddUserToDB(RegisterViewModel model)
        {
            if(ModelState.IsValid)
            {
                List<User> CheckEmail = _context.UserTable.Where(theuser => theuser.Email == model.Email).ToList();
                if (CheckEmail.Count > 0)
                    {
                        ViewBag.ErrorRegister = "Email already in use...";
                        return View("Index");
                    }

                // PasswordHasher<RegisterViewModel> Hasher = new PasswordHasher<RegisterViewModel>();
                // model.Password = Hasher.HashPassword(model, model.Password);
                User newuser = new User()
                {
                    FirstName = model.FirstName,
                    LastName = model.LastName,
                    Email = model.Email,
                    Password = model.Password,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };  
                _context.UserTable.Add(newuser);
                _context.SaveChanges();
                
                User JustCreated = _context.UserTable.SingleOrDefault(theUser => theUser.Email == model.Email);
                HttpContext.Session.SetInt32("UserId", (int)JustCreated.UserId);
                HttpContext.Session.SetString("UserName", (string)JustCreated.FirstName + " " + (string)JustCreated.LastName);
                return RedirectToAction("success");// eto kogda mi hotim prosto sdelat refresh
                // return View("success"); //eto kogda mi hotim postroit stranicu zanovo, uvidet formu, dlya registration in dlya login
            }
                
            else
            {
                //   ViewBag.errors=ModelState.Values;
                  return View("Index");
            }
              }

        [HttpGet]
        [Route("login")]
        public IActionResult Login()
        {
            ViewBag.NiceTry = TempData["NiceTry"];
            return View("login");
        }   
        [HttpPost]
        [Route("loginuser")]
        public IActionResult Login(string useremail=null, string userpassword=null)
        {
            if(userpassword != null && useremail != null)
            {
                // Checking if a User this provided Email exists in DB
                List<User> CheckUser = _context.UserTable.Where(theuser => theuser.Email == useremail).ToList();
                if (CheckUser.Count > 0)
                {
                    // Checking if the password matches
                    // var Hasher  = new PasswordHasher<User>();
                    if( CheckUser[0].Password==userpassword)
                    {
                        // If the checks are validated than save his ID and Name in session and redirect to the Dashboard page
                        HttpContext.Session.SetInt32("UserId", (int)CheckUser[0].UserId);
                        HttpContext.Session.SetString("UserName", (string)CheckUser[0].FirstName + " " + (string)CheckUser[0].LastName);
                        return RedirectToAction("success");
                    }
                }
            }
            // If check are not validated display an error
            ViewBag.ErrorLogin = "Invalid Login Data...";
            return View("login");
        }

        [HttpGet]
        [Route("success")]
        public IActionResult Success()
        {
            // int? id=HttpContext.Session.GetInt32("UserId");
            // Console.WriteLine("%%%%%%%%%%%%%%%%%"+id);
            // User found=_context.UserTable.SingleOrDefault(u=>u.UserId==);
            int? id = HttpContext.Session.GetInt32("UserId");
            // User userId = _context.UserTable.Where(u =>u.UserId==id).ToList();
            User useruser= _context.UserTable.SingleOrDefault( i => i.UserId==id);
            ViewBag.UserName = (string)HttpContext.Session.GetString("UserName");
            User Users= _context.UserTable.Include(user => user.TransactionsIMade).SingleOrDefault(i =>i.UserId==id);

            return View("success",useruser);
        } 
        [HttpGet]
        [Route("logout")]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("login");
        }
        [HttpPost]
        [Route("account")]
        public IActionResult DepositWithdraw(int accountaction)
        {   
            int? id = HttpContext.Session.GetInt32("UserId");
            User newuser = _context.UserTable.SingleOrDefault(i =>i.UserId==id);
            int sum = accountaction*100;
            // Transaction newtr = new Transaction(){
            //     Amount= sum,
            //     CreatedAt= DateTime.Now
            // };
            // _context.Transaction.Add(newtr);
            // _context.SaveChanges();
            if(sum<0)
            {
              if(newuser.Balance<(-1)*sum)
                {
                ViewBag.moneyerror="There is not enough money to withdraw";
                return View("success",newuser);
                } 
               Transaction newtr = new Transaction()
               {
                Amount= sum,
                CreatedAt= DateTime.Now,
                UserId= newuser.UserId
               };
                _context.Transaction.Add(newtr);
                newuser.Balance = newuser.Balance + accountaction*100;
                // List <User> Users= _context.UserTable.Include(user => user.TransactionsIMade).ToList();// this is how we fill up our list in User class
                _context.SaveChanges(); 
                // return RedirectToAction("success",newuser); 
            }
            else{
                Transaction newtrans = new Transaction()
               {
                Amount= sum,
                CreatedAt= DateTime.Now,
                UserId= newuser.UserId
               };
            _context.Transaction.Add(newtrans);
            newuser.Balance=newuser.Balance + sum;
            }
            // User Users= _context.UserTable.Include(user => user.TransactionsIMade).SingleOrDefault(i =>i.UserId==id);
            _context.SaveChanges();
            return RedirectToAction("success");
        }


        // [HttpPost]
        // [Route("depositwithdraw")]
        // public IActionResult Deposit(int accountaction)
        // {
        //     int sum = accountaction*100;
        //     Transaction newtr = new Transaction(){
        //         Amount= sum,
        //         CreatedAt= DateTime.Now
        //     };
        //     _context.Transaction.Add(newtr);
        //     _context.SaveChanges();


        // }


        }
}

  
