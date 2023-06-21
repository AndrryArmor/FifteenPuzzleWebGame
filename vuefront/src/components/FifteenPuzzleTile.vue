<template>
  <div
    class="tile d-flex rounded-4 shadow justify-content-center align-items-center"
    :class="{'bg-transparent shadow-none': number === 16 }"
  >
    <span v-if="number != 16" class="fs-3">{{ number }}</span>
  </div>
</template>

<script>
export default {
  name: "FifteenPuzzleTile",
  props: {
    number: Number,
  },
};
</script>

<style scoped>
.tile {
  width: 5rem;
  height: 5rem;
  background-color: burlywood;
  transition: transform 0.3s ease;
}
</style>
