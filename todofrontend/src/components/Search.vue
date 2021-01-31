<template>
    <div class="home-content">
        <form @submit.prevent="onSubmit">
        <div class="row">
            
                <div class="col-3 search-col">
                    <q-input filled v-model="titleSearch" label="Title"/>
                </div>
                <div class="col-3 search-col">
                    <q-select filled v-model="prioritySearch" :options="options" label="Priority"/>
                </div>
                <div class="col-3 search-col">
                    <q-input filled v-model="dateSearch" type="date" label="Date"/>
                </div>
                <div class="col-3 search-col">
                    <q-btn color="text-white bg-primary md" type="submit" label="Search" />
                </div>
            
        </div>
        </form>
    </div>
</template>

<script>

export default {
    name: "Search",
    data() {
        return {
            titleSearch: "",
            prioritySearch: 0,
            options:[
                    {label: 'Critical', value: '5', class: 'bg-negative'},
                    {label: 'High', value: '4', class: 'bg-secondary'},
                    {label: 'Medium', value: '3', class: 'bg-warning'},
                    {label: 'Inform', value: '2', class: 'bg-info'},
                    {label: 'Low', value: '1', class: 'bg-positive'},
                    {label: '', value: '0', class: ''}
                ],
            dateSearch: ""
        }
    },
    methods: {
        onSubmit: function(event) {
            let object = {
                todotitle: this.titleSearch,
                todopriority: Number(this.prioritySearch.value),
                tododate: this.dateSearch
            }
            console.log(object);
            // return false;
            this.$emit("searchData", object);
        }
    }
}
</script>

<style>

</style>