<template>
  <div id="game-screen">
    <div class="current-game-stats">
      <div class="player-stats">
        <h1>{{game.name}}</h1>
        <div class="stats">
          <p>Your balance: ${{(game.balance).toFixed(2)}}</p>
          <p>Your total worth: ${{(thisPlayer.totalWorth).toFixed(2)}}</p>
          <p>Start date: {{game.startDateAsString}}</p>
          <p>End date: {{game.endDateAsString}}</p>
        </div>
      </div>
      <div class="ranking">
        <game-leaderboard class="leaderboard" :players="players"/>
      </div>
    </div>
    <invite-friends class="invite" v-if="!isCompleted" />
    <div class="main-content">
    <current-investments class="investments" v-if="isActive"/>
    <h1 class="final-screen-text" v-if="!isActive && !isCompleted">This game has not yet begun</h1>
    <h1 class="final-screen-text" v-if="isCompleted">This game has been completed</h1>
    <stock-market class="available-stocks" v-if="isActive"/>
    </div>
  </div>
</template>

<script>
import gamesService from "@/services/GamesService";
// import stocksService from '@/services/StocksService';
import InviteFriends from "@/components/InviteFriends";
import StockMarket from "@/components/StockMarket";
import CurrentInvestments from "@/components/CurrentInvestments";
import GameLeaderboard from "@/components/GameLeaderboard";
export default {
  data() {
    return {
      players: [],
      thisPlayer: {}
    };
  },
  components: {
    InviteFriends,
    StockMarket,
    CurrentInvestments,
    GameLeaderboard,
  },
  computed: {
    game() {
      return this.$store.state.currentGame;
    },
    isActive() {
      return this.now >= this.startDate && this.now <= this.endDate;
    },
    isCompleted() {
      return this.now >= this.endDate;
    },
    now() {
      return Date.now();
    },
    startDate() {
      return Date.parse(this.game.startDate);
    },
    endDate() {
      return Date.parse(this.game.endDate);
    },
    // totalWorth() {
    //   let thisPlayer = this.players.find(player => {
    //     return player.username === this.$store.state.user.username
    //   });
    //   return thisPlayer.totalWorth
    // }
  },
  created() {
    gamesService.getGameById(this.$route.params.gameId).then((response) => {
      if (response.status === 200) {
        this.$store.commit("SET_CURRENT_GAME", response.data);
      }
    });
    gamesService.getPlayersInGame(this.$route.params.gameId).then((response) => {
      if (response.status === 200) {
        this.players = response.data;
        this.thisPlayer = this.players.find(player => {
        return player.username === this.$store.state.user.username
      });
      }
    });
  },
};
</script>

<style>
#game-screen {
  display: grid;
  grid-template-columns: 235px 1fr 240px;
  grid-template-areas:
    "stats title invite"
    "stats gameplay invite";
}

.main-content {
  display: grid;
  grid-template-areas:
    "investments"
    "stocks";
  grid-template-rows: 1fr 1fr;
  height: 95vh;
}

.investments {
  grid-area: investments;
  margin-left: 50px;
  margin-right: 0px;
}

.available-stocks {
  grid-area: stocks;
}

.current-game-stats {
  display: flex;
  flex-direction: column;
  justify-content: space-around;
  grid-area: stats;
  background: rgba(173, 214, 255, 0.9);
  width: 275px;
  height: 95.4vh;
  padding-left: 5px;
  padding-right: 5px;
}

.final-screen-text{
  color: white;
  /* background: radial-gradient(#fcd5b6, 	#f06e04); */
  background: #f06e04;
  align-self: center;
  justify-self: center;
  width: 600px;
  padding-top: 20px;
  padding-bottom: 20px;
  border-radius: 7px;
  opacity: 0.92;
  animation-name: anim;
  animation-duration: 4s;
  animation-delay: 2s;
  animation-iteration-count: infinite;
  animation-direction: alternate;
  animation-timing-function: linear;
}

@keyframes anim {
  /* from {background: radial-gradient(#fcd5b6, 	#f06e04);}
  to {background: radial-gradient(#5cadff, #e7f3ff);} */
  0%   {background-color: #f06e04;}
  25%  {background-color: #f68e4f;}
  50%  {background-color: #5cadff;}
  100% {background-color: #add6ff;}
}

.stats {
  background: rgba(0, 26, 51, 0.7);
  border-radius: 8px;
  padding-top: 4px;
  padding-bottom: 4px;
  margin-left: 4px;
  margin-right: 4px;
}

.gameplay {
  grid-area: gameplay;
}

.invite {
  grid-area: invite;
  background: rgba(173, 214, 255, 0.9);
  width: 250px;
  height: 95.4vh;
  width: 235px;
  padding-right: 5px;
}

.leaderboard {
  grid-area: leaderboard;
}

.player-stats > h1 {
  color: #003366;
}
</style>