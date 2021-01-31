<template>
    <div class="home-content">
        <form @submit.prevent="onSubmit">
            <div class="row">
                <div class="col-3 search-col">
                    <q-input filled v-model="dateStartSearch" type="date"/>
                </div>
                <div class="col-3 search-col">
                    <q-input filled v-model="dateEndSearch" type="date"/>
                </div>
                <div class="col-3 search-col">
                    <q-btn color="text-white bg-primary md" type="submit" label="Adv Search" />
                </div>
            </div>
        </form>
    </div>
</template>

<script>

export default {
    name: "AdvSearch",
    data() {
        return {
            dateStartSearch: "",
            dateEndSearch: ""
        }
    },
    methods: {
        onSubmit: function(event) {
            let object = {
                startDate: this.dateStartSearch,
                endDate: this.dateEndSearch
            }
            console.log(object);
            this.$emit("AdvSearchData", object);
        }
    }
}
</script>

<style>

</style>