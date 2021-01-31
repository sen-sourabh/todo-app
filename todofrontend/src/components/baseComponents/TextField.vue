<template>
  <div>
    <q-input v-model="innermodel" :label="labels" :type="typev"/>
  </div>
</template>

<script>

export default {
    data() {
      return{
          innermodel: this.model
        }
    },
    props:{
      labels:{
        default: "hello"
      },
      typev: {
        default: "text"
      },
      model:{
        default: ""
      }

    },
    watch:{
      innermodel: function(value) {
        this.$emit("model", value)
      },
      model: function(value){
        this.innermodel=this.model;
      }
    }
}
</script>


<style>
  
</style>
