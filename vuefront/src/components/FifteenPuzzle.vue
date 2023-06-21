<template>
  <div class="row mx-0 justify-content-center">
    <div class="col text-center">
      <h3>Current time:<br />{{ timeString }}</h3>
      <br />
      <h3>Moves made:<br />{{ moves }}</h3>
    </div>
    <div class="puzzle col-auto rounded-4 shadow-lg p-1">
      <div class="tile-container">
        <transition-group name="tile-transition">
          <FifteenPuzzleTile
            v-for="tile in tiles"
            :key="tile.id"
            :number="tile.id"
            @click="moveTile(tile)"
          />
        </transition-group>
      </div>
    </div>
    <div class="col text-center">
      <h3>
        Best time:
        <br />
        {{
          bestTime != null && bestTimeUser != null
            ? `${bestTimeUser} - ${timeToString(bestTime)}`
            : "none"
        }}
      </h3>
      <br />
      <h3>
        Best moves result:
        <br />
        {{
          bestMovesCount != null || bestMovesCountUser != null
            ? `${bestMovesCountUser} - ${bestMovesCount}`
            : "none"
        }}
      </h3>
    </div>
  </div>
  <div v-show="isSolved" class="row mx-0 justify-content-center">
    <div class="col text-center">
      <div>
        <br />
        <h1>SOLVED!</h1>
        <button
          type="button"
          class="btn btn-lg btn-success"
          :disabled="isResultSaved"
          @click="saveResult(seconds, moves)"
        >
          <span v-if="!isResultSaved">Save my result</span>
          <span v-else>Your result is saved, {{ nickname }}</span>
        </button>
      </div>
    </div>
  </div>
</template>

<script>
import FifteenPuzzleTile from "./FifteenPuzzleTile.vue";

export default {
  name: "FifteenPuzzle",
  components: {
    FifteenPuzzleTile: FifteenPuzzleTile,
  },

  data() {
    const tiles = [];
    for (let i = 1; i <= 16; i++) {
      tiles.push({ id: i });
    }

    return {
      tiles: tiles,
      nickname: "",
      moves: 0,
      seconds: 0,
      stopwatch: null,
      bestTime: null,
      bestTimeUser: null,
      bestMovesCount: null,
      bestMovesCountUser: null,
      isResultSaved: false,
    };
  },

  computed: {
    isSolved() {
      return this.tiles.every((tile, index) => tile.id == index + 1);
    },
    timeString() {
      return this.timeToString(this.seconds);
    },
  },

  watch: {
    isSolved(isSolved) {
      if (isSolved) {
        clearInterval(this.stopwatch);
      }
    },
  },
  
  mounted() {
    this.randomizePuzzle(50, 200);
    // this.randomizePuzzle(2, 2);
    this.getBestResults();

    this.stopwatch = setInterval(() => {
      this.seconds++;
    }, 1000);
  },

  methods: {
    timeToString(timeInSeconds) {
      let hours = Math.floor(timeInSeconds / 3600);
      let minutes = Math.floor(timeInSeconds / 60) % 60;
      let seconds = timeInSeconds % 60;
      let hoursString = `${hours < 10 ? "0" : ""}${hours}`;
      let minutesString = `${minutes < 10 ? "0" : ""}${minutes}`;
      let secondsString = `${seconds < 10 ? "0" : ""}${seconds}`;
      return `${hoursString}:${minutesString}:${secondsString}`;
    },
    /** Gets random number from range [min, max). */
    getRandomFromRange(min, max) {
      return Math.floor(Math.random() * (max - min) + min);
    },
    createCoords(row, column) {
      return { row, column };
    },
    coordsFromIndex(index) {
      return this.createCoords(Math.floor(index / 4), index % 4);
    },
    indexFromCoords(coords) {
      return coords.row * 4 + coords.column;
    },
    addCoords(coord1, coord2) {
      let newCoord = { ...coord1 };
      newCoord.row += coord2.row;
      newCoord.column += coord2.column;
      return newCoord;
    },
    swap(array, first, last) {
      [array[first], array[last]] = [array[last], array[first]];
    },
    /**Randomizes the puzzle making random numbers of moves
     * from range [min, max) */
    randomizePuzzle(min, max) {
      for (let i = 0; i < this.getRandomFromRange(min, max); i++) {
        let emptyTileIndex = this.tiles.findIndex((tile) => tile.id === 16);
        let emptyTileCoords = this.coordsFromIndex(emptyTileIndex);

        let upTileCoords = this.addCoords(
          emptyTileCoords,
          this.createCoords(-1, 0)
        );
        let rightTileCoords = this.addCoords(
          emptyTileCoords,
          this.createCoords(0, 1)
        );
        let downTileCoords = this.addCoords(
          emptyTileCoords,
          this.createCoords(1, 0)
        );
        let leftTileCoords = this.addCoords(
          emptyTileCoords,
          this.createCoords(0, -1)
        );

        let coords = [
          upTileCoords,
          rightTileCoords,
          downTileCoords,
          leftTileCoords,
        ];
        let swapTiles = [];
        coords.forEach((coord) => {
          if (
            0 <= coord.row &&
            coord.row < 4 &&
            0 <= coord.column &&
            coord.column < 4
          ) {
            swapTiles.push(this.indexFromCoords(coord));
          }
        });

        this.swap(
          this.tiles,
          emptyTileIndex,
          swapTiles[this.getRandomFromRange(0, swapTiles.length)]
        );
      }
    },
    moveTile(clickedTile) {
      if (this.isSolved) return;

      const emptyIndex = this.tiles.findIndex((tile) => tile.id === 16);
      const clickedIndex = this.tiles.indexOf(clickedTile);

      const rowDifference = Math.abs(
        Math.floor(emptyIndex / 4) - Math.floor(clickedIndex / 4)
      );
      const columnDifference = Math.abs((emptyIndex % 4) - (clickedIndex % 4));

      if (rowDifference + columnDifference == 1) {
        this.swap(this.tiles, emptyIndex, clickedIndex);
        this.moves++;
      }
    },
    async getBestResults() {
      try {
        const response = await fetch(
          "https://localhost:7243/api/fifteen-puzzle/best"
        );
        if (!response.ok) {
          throw new Error("Error fetching data.");
        }

        const bestResults = await response.json();
        this.bestTime = bestResults.bestTime;
        this.bestTimeUser = bestResults.bestTimeUser;
        this.bestMovesCount = bestResults.bestMovesCount;
        this.bestMovesCountUser = bestResults.bestMoveCountUser;
      } catch (error) {
        console.error(error);
      }
    },
    async saveResult(seconds, moves) {
      if (this.isResultSaved) return;

      this.nickname = prompt("Enter your nickname:");
      if (this.nickname != null && this.nickname != "") {
        try {
          const response = await fetch(
            "https://localhost:7243/api/fifteen-puzzle/new-record",
            {
              method: "POST",
              headers: {
                "Content-Type": "application/json",
              },
              body: JSON.stringify({
                nickname: this.nickname,
                time: seconds,
                movesCount: moves,
              }),
            }
          );
          if (!response.ok) {
            throw new Error("Error while sending data.");
          }

          this.isResultSaved = true;
          this.getBestResults();
        } catch (error) {
          console.error(error);
        }
      }
    },
  },
};
</script>

<style scoped>
.puzzle {
  background-color: bisque;
}

.tile-container {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  grid-template-rows: repeat(4, 1fr);
  gap: 5px;
}
</style>
