﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Capstone.DAO;
using Capstone.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Capstone.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        private readonly GameSqlDAO gameSqlDAO;
        private string Username
        {
            get
            {
                return User?.Identity?.Name;
            }
        }
        public GamesController(GameSqlDAO gameSqlDAO)
        {
            this.gameSqlDAO = gameSqlDAO;
        }

        [HttpGet]
        public ActionResult<List<Game>> GetActiveGames()
        {
            try
            {
                return Ok(gameSqlDAO.GetActiveGames(Username));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("completed")]
        public ActionResult<List<Game>> GetCompletedGames()
        {
            try
            {
                return Ok(gameSqlDAO.GetCompletedGames(Username));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("{gameId}")]
        public ActionResult<Game> GetGameById(int gameId)
        {
            try
            {
                return Ok(gameSqlDAO.GetGameById(Username, gameId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("{gameId}/investments")]
        public ActionResult<List<BuyModel>> GetCurrentInvestments(int gameId)
        {
            try
            {
                return Ok(gameSqlDAO.GetCurrentInvestments(Username, gameId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPost]
        public ActionResult<Game> CreateGame(Game game)
        {
            try
            {
                //if (game.StartDateAsTicks > game.EndDateAsTicks || game.StartDateAsTicks < DateTime.Now.Ticks)
                //{
                //    return Forbid();
                //}
                game = gameSqlDAO.CreateGame(game, Username);
                if (game == null)
                {
                    return Forbid();
                }
                string location = $"api/games/{game.GameId}";
                return Created(location, game);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPost]
        [Route("invite")]
        public ActionResult<bool> InviteUsersToGame(List<UserGame> userGames)
        {
            try
            {
                if (gameSqlDAO.InviteUsersToGame(userGames, Username))
                {
                    string location = $"api/games/invite/success";
                    return Created(location, true);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("{gameId}/invite")]
        public ActionResult<List<UserInfo>> GetUsersToInviteToGame(int gameId)
        {
            try
            {
                return Ok(gameSqlDAO.GetUsersToInviteToGame(gameId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("{gameId}/players")]
        public ActionResult<List<UserInfo>> GetActivePlayersInGame(int gameId)
        {
            try
            {
                return Ok(gameSqlDAO.GetActivePlayersInGame(gameId));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpGet]
        [Route("pending")]
        public ActionResult<List<Game>> GetPendingGames()
        {
            try
            {
                return Ok(gameSqlDAO.GetPendingGames(Username));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("accept")]
        public ActionResult<bool> AcceptInvitation(UserGame userGame)
        {
            try
            {
                if (gameSqlDAO.AcceptInvitation(userGame, Username))
                {
                    return Ok(true);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
        [HttpPut]
        [Route("decline")]
        public ActionResult<bool> DeclineInvitation(UserGame userGame)
        {
            try
            {
                if (gameSqlDAO.DeclineInvitation(userGame, Username))
                {
                    return Ok(true);
                }
                else
                {
                    return Forbid();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }

    }
}