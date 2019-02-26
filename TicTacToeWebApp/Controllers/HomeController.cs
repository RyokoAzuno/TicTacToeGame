using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TicTacToeWebApp.Models;

namespace TicTacToeWebApp.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Index(string firstPlayerName, string secondPlayerName)
        {
            // Set default values for players
            if (string.IsNullOrEmpty(firstPlayerName))
                firstPlayerName = "Player1";
            else if (string.IsNullOrEmpty(secondPlayerName))
                secondPlayerName = "Player2";

            string board = "_________";
            PlayerViewModel p1 = new PlayerViewModel { Name = firstPlayerName, Symbol = "X", NumOfWins = 0 };
            PlayerViewModel p2 = new PlayerViewModel { Name = secondPlayerName, Symbol = "O", NumOfWins = 0 };
            GameViewModel game = new GameViewModel(p1, p2, board, 0);
            Session["Game"] = game;

            return RedirectToAction("Game", new { firstPlayerName = game.Player1.Name, secondPlayerName = game.Player2.Name });
        }

        public ActionResult Game(string firstPlayerName, string secondPlayerName, bool? isRedirected)
        {
            GameViewModel game = Session["Game"] as GameViewModel;
            // Check if redirected from Reset action
            if (isRedirected.HasValue)
            {
                game.Board = "_________";
                game.Attempts = 0;
                game.Player1.NumOfWins = game.Player2.NumOfWins = 0;
            }
            // Check if redirected from Winner action
            if (TempData["FromWinner"] != null && (bool)TempData["FromWinner"] == true)
            {
                game.Board = "_________";
                game.Attempts = 0;
            }
            // Check for winner
            if (game.IsWinner(game.Player1.Symbol[0]))
            {
                ViewBag.Winner = $"Winner is - {game.Player1.Name}!!!";
                ++game.Player1.NumOfWins;
                Session["Game"] = game;

                return RedirectToAction("Winner", new { firstPlayerName, secondPlayerName, game.Player1.Name });
            }
            else if (game.IsWinner(game.Player2.Symbol[0]))
            {
                ViewBag.Winner = $"Winner is - {game.Player2.Name}!!!";
                ++game.Player2.NumOfWins;
                Session["Game"] = game;

                return RedirectToAction("Winner", new { firstPlayerName, secondPlayerName, game.Player2.Name });
            }
            // Check for Draw
            if (game.Attempts == 9)
            {
                ViewBag.Winner = $"{game.Player1.Name} vs {game.Player2.Name} = DRAW!!!";
                string name = null;
                Session["Game"] = game;

                return RedirectToAction("Winner", new { firstPlayerName, secondPlayerName, name });
            }
            // Check who's turn (even for X and odd for O)
            if (game.Attempts % 2 == 0)
            {
                ViewBag.Winner = $"It's your turn {game.Player1.Name}!!!";
            }
            else
            {
                ViewBag.Winner = $"It's your turn {game.Player2.Name}!!!";
            }
            Session["Game"] = game;

            return View(game);
        }
        // X or O on every step
        public ActionResult Changer(int? id, char? ch, string firstPlayerName, string secondPlayerName)
        {
            GameViewModel game = Session["Game"] as GameViewModel;
            game.Update(id);
            Session["Game"] = game;

            return RedirectToAction("Game", new { firstPlayerName, secondPlayerName });
        }
        // Reset game board and points
        public ActionResult Reset(string firstPlayerName, string secondPlayerName)
        {
            bool isRedirected = true;

            return RedirectToAction("Game", new { firstPlayerName, secondPlayerName, isRedirected });
        }
        // Winner action
        public ActionResult Winner(string firstPlayerName, string secondPlayerName, string name)
        {
            TempData["FromWinner"] = true;

            return View();
        }
    }
}