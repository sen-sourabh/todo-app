<template>
  <div class="q-pa-lg flex flex-center">
    <q-select class="no_of_item" v-model="limit" @change="getCurrentPageLimit" :options="options" label="Items/Page"/>
    <q-pagination
      v-model="current_page"
      @change="getCurrentPageLimit"
      :max="Math.ceil(no_of_todos/limit)"
      :direction-links="true"
      :boundary-links="true"
      icon-first="skip_previous"
      icon-last="skip_next"
      icon-prev="fast_rewind"
      icon-next="fast_forward"
    >
    </q-pagination>
  </div>
</template>

<script>
export default {
    name: "Pagination",
    props: {
      no_of_todos: Number,
    },
    data () {
        return {
            
            limit: 8,
            options: [8, 16, 30, 50, 75, 100],
            current_page: 1
        }
    },
    methods: {
      // getLimit: function() {
      //     alert("Limit/Page: "+this.limit);
      //     this.$emit('limit', this.limit);
      // },
      getCurrentPageLimit: function() {
          alert("Limit/Page: "+this.limit);
          alert("Page No: "+this.current_page);
          this.$emit('limit', this.limit);
          this.$emit('current_page', this.current_page);
      }
    }
}
</script>

<style>
    .no_of_item {
        width: 8%;
    }
</style>