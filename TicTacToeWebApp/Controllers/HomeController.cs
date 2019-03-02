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
            if (string.IsNullOrEmpty(secondPlayerName))
                secondPlayerName = "Player2";

            string board = "_________";
            PlayerViewModel p1 = new PlayerViewModel { Name = firstPlayerName, Symbol = "X", NumOfWins = 0 };
            PlayerViewModel p2 = new PlayerViewModel { Name = secondPlayerName, Symbol = "O", NumOfWins = 0 };
            GameViewModel game = new GameViewModel(p1, p2, board, 0);
            Session["Game"] = game;
            ViewBag.Message = $"It's your turn {game.Player1.Name}!!!";

            return PartialView("GamePartial", game);
        }

        // X or O on every step
        public ActionResult Game(int? id, string firstPlayerName, string secondPlayerName)
        {
            GameViewModel game = Session["Game"] as GameViewModel;
            game.Update(id);
            // Check for winner
            if (game.IsWinner(game.Player1.Symbol[0]))
            {
                ViewBag.Message = $"Winner is - {game.Player1.Name}!!!";
                ++game.Player1.NumOfWins;
                Session["Game"] = game;

                return PartialView("ContinuePartial", game.Player1.Name);
            }
            else if (game.IsWinner(game.Player2.Symbol[0]))
            {
                ViewBag.Message = $"Winner is - {game.Player2.Name}!!!";
                ++game.Player2.NumOfWins;
                Session["Game"] = game;

                return PartialView("ContinuePartial", game.Player2.Name);
            }
            // Check for Draw
            if (game.Attempts == 9)
            {
                ViewBag.Message = $"{game.Player1.Name} vs {game.Player2.Name} = DRAW!!!";
                Session["Game"] = game;

                return PartialView("ContinuePartial", "Draw");
            }
            // Check for turn
            if (game.Attempts % 2 == 0)
            {
                ViewBag.Message = $"It's your turn {game.Player1.Name}!!!";
            }
            else
            {
                ViewBag.Message = $"It's your turn {game.Player2.Name}!!!";
            }
            Session["Game"] = game;

            return PartialView("GamePartial", game);
        }
        // Reset game board and points
        public ActionResult Reset()
        {
            GameViewModel game = Session["Game"] as GameViewModel;
            game.Reset();
            Session["Game"] = game;
            ViewBag.Message = $"It's your turn {game.Player1.Name}!!!";

            return PartialView("GamePartial", game);
        }
        // Continue action(after draw or win)
        public ActionResult Continue(string name)
        {
            GameViewModel game = Session["Game"] as GameViewModel;
            game.Board = "_________";
            game.Attempts = 0;
            Session["Game"] = game;
            ViewBag.Message = $"It's your turn {game.Player1.Name}!!!";

            return PartialView("GamePartial", game);
        }
    }
}