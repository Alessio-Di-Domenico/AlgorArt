using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using AlgorArt.Models;
using AlgorArt.Data;
using Microsoft.EntityFrameworkCore;
using Algorand.Client;
using Algorand.V2.Model;
using Algorand;
using Algorand.V2;
using Account = Algorand.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AlgorArt.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _context;
        private static UserManager<ApplicationUser> _userManager;
        private readonly ILogger<HomeController> _logger;

        public HomeController(ApplicationDbContext context, ILogger<HomeController> logger, UserManager<ApplicationUser> userManager)
        {
            _logger = logger;
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index()
        { 
            return View(await _context.RequestFunds.Include(x => x.User).Include(x => x.Funders).ToListAsync());
        }

        private List<RequestFunds> RequestFundsList()
        {
            List<RequestFunds> list = new List<RequestFunds>();
            list = _context.RequestFunds.Include(x => x.User).ToList();
            return list;
        }
        private List<Funders> FundersList(int? id)
        {
            List<Funders> list = new List<Funders>();
            list = _context.Funders.Include(x => x.User).Where(x => x.RequestFundsId == id).ToList();
            return list;
        }
        
        /*private List<Funders> FundersListAll(){
            List<Funders> list = new List<Funders>();
            list = _context.Funders.Include(x => x.User).ToList();
            
            return list;
        }*/

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var mymodel = new ViewModel();
            mymodel.RequestFundsList = RequestFundsList();
            mymodel.FundersList = FundersList(id);
            mymodel.RequestFunds = await _context.RequestFunds.Include(x => x.Funders).Include(x => x.User).FirstOrDefaultAsync(m => m.Id == id);
            if (mymodel.RequestFunds == null)
            {
                return NotFound();
            }

            return View(mymodel);
        }

        public IActionResult Create(int RequestFundsId, string Receiver)
        {
            Funders funders = new Funders();
            funders.RequestFundsId = RequestFundsId;
            funders.Receiver = Receiver;
            ViewData["RequestFundsId"] = new SelectList(_context.RequestFunds, "Id", "Id", funders.RequestFundsId);
            return View(funders);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Fund([Bind("Id,Amount,RequestFundsId,Receiver,Key")] Funders funders)
        {
            if (ModelState.IsValid)
            {
                var username = await _userManager.FindByNameAsync(User.Identity.Name);
                funders.User = username;
                funders.Key = username.Key;
                FundMethod(username.Key, funders.Receiver, funders.Amount, username.AccountAddress);
                _context.Add(funders);
                ViewBag.Success = "Successfully transfered funds";
                await _context.SaveChangesAsync();
                return RedirectToAction("Details", new {id = funders.RequestFundsId} );
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        public static void FundMethod(string key, string receiver, int amount, string senderAddr)
        {
            string ALGOD_API_ADDR = "https://testnet-algorand.api.purestake.io/ps2"; //find in algod.net
            string ALGOD_API_TOKEN = "B3SU4KcVKi94Jap2VXkK83xx38bsv95K5UZm2lab"; //find in algod.token          
            string SRC_ACCOUNT = key;
            string DEST_ADDR = receiver;
            Account src = new Account(SRC_ACCOUNT);
            AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            try
            {
                var trans = algodApiInstance.TransactionParams();
            }
            catch (ApiException e)
            {
                Console.WriteLine("Exception when calling algod#getSupply:" + e.Message);
            }

            TransactionParametersResponse transParams;
            try
            {
                transParams = algodApiInstance.TransactionParams();
            }
            catch (ApiException e)
            {
                throw new Exception("Could not get params", e);
            }
            var amountsent = Utils.AlgosToMicroalgos(amount);
            var tx = Utils.GetPaymentTransaction(src.Address, new Address(DEST_ADDR), amountsent, "pay message", transParams);
            var signedTx = src.SignTransaction(tx);

            Console.WriteLine("Signed transaction with txid: " + signedTx.transactionID);

            // send the transaction to the network
            try
            {
                var id = Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.TxId);
                Console.WriteLine(Utils.WaitTransactionToComplete(algodApiInstance, id.TxId));
            }
            catch (ApiException e)
            {
                // This is generally expected, but should give us an informative error message.
                Console.WriteLine("Exception when calling algod#rawTransaction: " + e.Message);
            }
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        

    }
}
