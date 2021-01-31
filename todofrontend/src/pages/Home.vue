<template>
    <div class="container welcome-home">
        <Header @username="getUsername" @confirm="openLogoutDialog" />
        <!-- <web-cam :height="500" :width="500" started :autoplay="true" screenshotFormat='image/jpeg'/> -->
        <div class="home-content">
            <div v-if="error_msg == ''">
                <div class="taskbtn-inline">
                    <q-btn @click="alert=true" class="text-white bg-primary md btn" label="Add Task" title="Add Task"/>
                    <q-btn class="text-white bg-warning md btn" v-on:click="deleteAllSelectedTodos" label="Delete" title="Delete Selected Tasks" v-bind:style="deleteEnable ? '' : 'display:none;'"/>
                    <q-btn class="text-white bg-primary md btn" v-on:click="getSearchView" label="Search" title="Search"/>
                    <q-btn class="text-white bg-primary md btn" v-on:click="getAdvSearchView" label="Advanced Search" title="Advanced Search"/>
                    <template v-if="!CSVfileData.length">
                        <q-btn class="text-white bg-primary md btn" v-on:click="getDataForCSV" label="Click to Generate CSV file" title="Click to Generate CSV file"/>
                    </template>
                    <template v-else>
                        <vue-csv-downloader class="csv-downloader" :download-name="username+'_TodoApp_'+new Date().toISOString()+'_file.csv'" :data="CSVfileData" :fields="fields"><q-btn class="text-white bg-primary md btn" @click="resetCSVArray" title="Downoload CSV File" label="Downoload CSV File"/></vue-csv-downloader>
                    </template>
                    <q-btn-toggle class="btn-toggle" v-model="listview" toggle-color="primary" :options="listlayout"/>
                    <q-btn class="text-white bg-primary md btn" @click="refreshTodoApp" title="Refresh Page"><i class="fa fa-refresh"></i></q-btn>
                    <q-btn icon="event" round color="primary">
                        <q-popup-proxy transition-show="scale" transition-hide="scale">
                            <q-date
                                minimal
                                v-model="date"
                                :events="events"
                                event-color="red"
                            >
                                <div class="row items-center justify-end q-gutter-sm">
                                    <q-btn label="Close" color="primary" flat v-close-popup />
                                </div>
                            </q-date>
                        </q-popup-proxy>
                    </q-btn>
                    
                    
                    <q-dialog v-model="confirm" persistent>
                        <q-card>
                            <q-card-section class="row items-center">
                            <q-avatar icon="fa fa-power-off" color="negative" text-color="white" />
                            <span class="q-ml-sm">Are you sure. You want to logout from Todo App?</span>
                            </q-card-section>

                            <q-card-actions align="right">
                            <q-btn flat label="Cancel" color="negative" v-close-popup />
                            <q-btn flat label="Logout" color="negative" @click="logout" />
                            </q-card-actions>
                        </q-card>
                    </q-dialog>
                    
                    <div class="sortDiv">
                        <q-select class="sortSelect" v-model="sortlist" :options="sortbylist" label="Sort"/>
                    </div>
                </div>
            
                <div>
                    <q-dialog v-model="alert" persistent>
                        <Addtask @response_add="taskAdded"/>
                    </q-dialog>
                </div>
                <div v-if="isSearch">
                    <Search @searchData="getSearchData" />
                </div>
                <div v-if="isAdvSearch">
                    <AdvSearch @AdvSearchData="getAdvSearchData"/>
                </div>
                <transition appear enter-active-class="animated fadeIn" leave-active-class="animated fadeOut">
                    <div v-if="listview == 'grid'">
                        <GridLayout v-show="showSimulatedReturnData" :todoitems="todoitems" @todoDeleteId="getTodoDeleteId" @todoCompletedId="getTodoCompletedId" @todoNotCompletedId="getNotCompletedId" @responseEditGrid="getEditResp" @updatePriority="getUpdatePriority" @selectedTodos="getSelectedTodos"/>
                    </div>
                    <div v-else>
                        <ListLayout v-show="showSimulatedReturnData" :todoitems="todoitems" @todoDeleteId="getTodoDeleteId" @todoCompletedId="getTodoCompletedId" @todoNotCompletedId="getNotCompletedId" @responseEditList="getEditResp" @updatePriority="getUpdatePriority" @selectedTodos="getSelectedTodos"/>
                    </div>
                </transition>
                <q-inner-loading :showing="visible">
                    <q-spinner-gears size="50px" color="primary" />
                </q-inner-loading>
                <div class="footer-pagination">
                    <div class="q-pa-lg flex flex-center">
                        <q-select class="no_of_item" v-model="limit" @click="getCurrentPage" :options="todoPerPage" label="Items/Page"/>
                        <q-pagination
                            v-model="current_page"
                            @input="getCurrentPage"
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
                </div>
            </div>
            <div v-else>
                <center>
                    <h1 class="Error_msg">{{error_msg}}</h1>
                </center>
            </div>
        </div>
    </div>
</template>

<script>
import Header from "./../components/Header.vue";
import Search from "./../components/Search.vue";
import AdvSearch from "./../components/AdvSearch.vue";
import Addtask from "./../components/Addtask.vue";
import GridLayout from "./../components/GridLayout.vue";
import ListLayout from "./../components/ListLayout.vue";
import axios from "axios";
import VueCsvDownloader from 'vue-csv-downloader';
// import { WebCam } from "vue-web-cam";

export default {
    name: "Home",
    components: {
        Header,
        Search, 
        AdvSearch,
        Addtask,
        GridLayout,
        ListLayout,
        VueCsvDownloader,
        // WebCam
    },
    data(){
        return {
            date: new Date().getFullYear() + 
                        "/" + ((new Date().getMonth() <= 8) ? "0"+(new Date().getMonth() + 1) : (new Date().getMonth() + 1)) + 
                        "/" + ((new Date().getDate() <= 9) ? "0"+new Date().getDate() : new Date().getDate()),
            events: [],
            CSVfileData: [],
            fields: ['todoTitle', 'todoDesc', 'todoDate', 'todoPriorityText', 'isComplete', 'todoCreateDate'],
            deleteEnable: false,
            visible: false,
            showSimulatedReturnData: false,
            session_user_id: this.$store.state.session_user_id,
            todoitems: [],
            selectedtodosfordelete: [],
            alert: false,
            confirm: false,
            sortlist: {label: 'Default', value: 'default'},
            isSearch: false,
            isAdvSearch: false,
            isGridView: true,
            isListView:false,
            listview: 'grid',
            sourabh: "my name is sourabh",
            listlayout: [
                            {icon:'fa fa-th', title:'Grid', click:'getGridView', label: 'Grid', value: 'grid'},
                            {icon:'fa fa-list', title:'List', click:'getListView', label: 'List', value: 'list'}
                        ],
            sortbylist: [
                            {label: 'Default', value: 'default'},
                            {label: 'Date', value: 'date'},
                            {label: 'Title', value: 'title'},
                            {label: 'Priority', value: 'priority'}
                        ],
            options:[
                    {label: 'Critical', value: '5', class: 'bg-negative'},
                    {label: 'High', value: '4', class: 'bg-secondary'},
                    {label: 'Medium', value: '3', class: 'bg-warning'},
                    {label: 'Inform', value: '2', class: 'bg-info'},
                    {label: 'Low', value: '1', class: 'bg-positive'}
                ],
            limit: 8,
            todoPerPage: [8, 16, 30, 50, 75, 100],
            current_page: 1,
            no_of_todos: 0,
            error_msg: "",
            username: "User"
        }
    },
    beforeCreate() {
        if(this.$store.state.session_user_id == 0) 
        {
            window.location.href = '/';
        }
    },
    created() {
        this.getTodoItemAllData();
    },
    methods: {
        getUsername: function(value) {
            this.username = value;
        },
        refreshTodoApp: function() {
            this.getTodoItemAllData();
        },
        getCurrentPage: function() {
            this.getTodoItemData();
        },
        openLogoutDialog: function(val){
            this.confirm = val;
        },
        logout: function() {
            console.log(this.$store.state.session_user_id);
            this.$store.replaceState({
                session_user_id: 0
            });
            console.log(this.$store.state.session_user_id);
            window.location.href = '#/signin';
        },
        resetCSVArray: function() {
            this.CSVfileData = [];
        },
        getDataForCSV: function() {
            axios.get("https://localhost:44399/api/TodoItems/GetUserTodoItemList/"+this.session_user_id)
            .then(response => {
                response.data.forEach(todo => {
                    this.options.forEach(option => {
                        if(option.value == todo.todoPriority) {
                            todo.todoPriorityText = option.label;
                        }
                    })
                    if(todo.isComplete == 1) {
                        todo.isComplete = "Complete";
                    } else {
                        todo.isComplete = "Incomplete";
                    }
                    this.CSVfileData.push(todo);
                });
            });
        },
        showTextLoading () {
            this.visible = true
            this.showSimulatedReturnData = false
            setTimeout(() => {
                this.visible = false
                this.showSimulatedReturnData = true
            }, 2000)
        },
        getTodoItemAllData: function() {
            this.no_of_todos = 0;
            let status = 0;
            axios.get("https://localhost:44399/api/TodoItems/"+this.session_user_id+"/"+status)
            .then(response => {
                response.data.forEach(todo => {
                    let tdate = new Date(todo.todoDate);
                    todo.todoEvent = tdate.getFullYear() + 
                        "/" + ((tdate.getMonth() <= 8) ? "0"+(tdate.getMonth() + 1) : (tdate.getMonth() + 1)) + 
                        "/" + ((tdate.getDate() <= 9) ? "0"+tdate.getDate() : tdate.getDate());
                    this.no_of_todos++;
                    this.events.push(todo.todoEvent);
                });
            });
            this.showTextLoading();
        },
        getTodoItemData: function() {
            this.visible = true;
            this.showSimulatedReturnData = false;
            this.todoitems= [];
            axios.get("https://localhost:44399/api/TodoItems/"+this.session_user_id+"/"+this.current_page+"/"+this.limit)
            .then(response => {
                setTimeout(() => {
                    this.visible = false
                    this.showSimulatedReturnData = true
                    response.data.forEach(todo => {
                        this.options.forEach(option => {
                            if(option.value == todo.todoPriority) {
                                todo.todoPriorityText = option.label;
                            }
                        })
                        this.todoitems.push(todo);
                    });
                })
            })
            .catch((err) => {
                if(err.message === "Network Error") {
                    this.error_msg = "NETWORK ERROR";
                    this.$emit('error_msg', error_msg);
                }
            });
        },
        getSearchView: function() {
            if(this.isAdvSearch) {
                this.isAdvSearch = false;
            }
            this.isSearch =! this.isSearch;
        },
        getAdvSearchView: function() {
            if(this.isSearch) {
                this.isSearch = false;
            }
            this.isAdvSearch =! this.isAdvSearch;
        },
        getUpdatePriority: function(resultData) {
            this.showTextLoading();
            resultData.todoPriority = Number(resultData.todoPriorityText.value);
            resultData.todoColor = resultData.todoPriorityText.class;
            const response = axios({
                method: "PUT",
                url: "https://localhost:44399/api/TodoItems/"+ resultData.todoId,
                data: resultData
            })
            .then((response) => {this.getTodoItemData();})
            .catch((err) => {
                this.error_message = err.message;
            });
            this.refreshTodoApp();
        },
        getTodoDeleteId: function (resultData) {
            this.showTextLoading();
            const response = axios({
                method: "DELETE",
                url: "https://localhost:44399/api/TodoItems/"+ resultData.todoId,
                data: resultData
            })
            .then((response) => {this.getTodoItemData();})
            .catch((err) => {
                this.error_message = err.message;
            });
            this.refreshTodoApp();
        },
        getTodoCompletedId: function(resultData) {
            this.showTextLoading();
            resultData.isComplete = 1;
            const response = axios({
                method: "PUT",
                url: "https://localhost:44399/api/TodoItems/"+ resultData.todoId,
                data: resultData
            })
            .then((response) => {
                this.getTodoItemData();
            })
            .catch((err) => {
                this.error_message = err.message;
            });
        },
        getNotCompletedId: function(resultData) {
            this.showTextLoading();
            resultData.isComplete = 0;
            const response = axios({
                method: "PUT",
                url: "https://localhost:44399/api/TodoItems/"+ resultData.todoId,
                data: resultData
            })
            .then((response) => {
                this.getTodoItemData();
            })
            .catch((err) => {
                this.error_message = err.message;
            });
        },
        sortedArrayByDate: function() {
            this.showTextLoading();
            function compare(a, b) {
                if (a.todoDate < b.todoDate)
                    return -1;
                if (a.todoDate > b.todoDate)
                    return 1;
                return 0;
                }
            return this.todoitems.sort(compare);
        },
        sortedArrayByTitle: function() {
            this.showTextLoading();
            this.todoitems= [];
            axios.get("https://localhost:44399/api/TodoItems/GetSortTodoTitle/"+this.session_user_id+"/"+this.current_page+"/"+this.limit)
            .then(response => {
                response.data.forEach(todo => {
                    this.options.forEach(option => {
                        if(option.value == todo.todoPriority) {
                            todo.todoPriorityText = option.label;
                        }
                    })
                    this.todoitems.push(todo);
                });
            });
        },
        sortedArrayByPriority: function() {
            this.showTextLoading();
            function compare(a, b) {
                if (a.todoPriority > b.todoPriority)
                    return -1;
                if (a.todoPriority < b.todoPriority)
                    return 1;
                return 0;
                }
            return this.todoitems.sort(compare);
        },
        getSearchData: function(searchData) {
            this.showTextLoading();
            this.todoitems= [];
            if(Number.isNaN(searchData.todopriority)) {
                searchData.todopriority = 0;
            }
            if(searchData.todotitle == ""){
                searchData.todotitle = " ";
            }
            if(searchData.tododate == ""){
                searchData.tododate = " ";
            }
            const response = axios({
                method: "GET",
                url: "https://localhost:44399/api/TodoItems/GetTodoItemViaSearch/"+this.session_user_id+"/"+searchData.todotitle+"/"+searchData.todopriority+"/"+searchData.tododate+"/"+this.current_page+"/"+this.limit
            })
            .then(response => {
                response.data.forEach(todo => {
                    this.options.forEach(option => {
                        if(option.value == todo.todoPriority) {
                            todo.todoPriorityText = option.label;
                        }
                    })
                    this.todoitems.push(todo);
                });
            })
            .catch((err) => {
                this.error_message = err.message;
            });
        },
        getAdvSearchData: function(advSearchData) {
            this.showTextLoading();
            this.todoitems= [];
            const response = axios({
                method: "GET",
                url: "https://localhost:44399/api/TodoItems/GetTodoItemViaAdvSearch/"+this.session_user_id+"/"+advSearchData.startDate+"/"+advSearchData.endDate+"/"+this.current_page+"/"+this.limit
            })
            .then(response => {
                response.data.forEach(todo => {
                    this.options.forEach(option => {
                        if(option.value == todo.todoPriority) {
                            todo.todoPriorityText = option.label;
                        }
                    })
                    this.todoitems.push(todo);
                });
            })
            .catch((err) => {
                this.error_message = err.message;
            });
        },
        taskAdded: function() {
            this.alert = false;
            this.refreshTodoApp();
        },
        getEditResp: function() {
            this.refreshTodoApp();
        },
        getSelectedTodos: function(resultIds) {
            this.selectedtodosfordelete = resultIds;
            if(this.selectedtodosfordelete.length > 0) {
                this.deleteEnable = true;
            } else {
                this.deleteEnable = false;
            }
        },
        deleteAllSelectedTodos: function() {
            let i = 0;
            let arr = [];
            this.selectedtodosfordelete.forEach(element => {
                arr[i] = Number(element);
                i++;
            });
            this.showTextLoading();
            const response = axios({
                method: "POST",
                url: "https://localhost:44399/api/TodoItems/DeleteMultiTodoItem",
                data: arr
            })
            .then((response) => {this.getTodoItemData();})
            .catch((err) => {
                this.error_message = err.message;
            });
            this.deleteEnable = false;
            this.refreshTodoApp();
        }
    },
    watch: {
        sortlist: function(sortKey) {
            if(sortKey.value == "date") {
                this.sortedArrayByDate();
            } else if(sortKey.value == "title") {
                this.sortedArrayByTitle();
            } else if(sortKey.value == "priority") {
                this.sortedArrayByPriority();
            } else if(sortKey.value == "default") {
                this.getTodoItemData();
            }
        }
    }
 }
</script>

<style>
    .home-content {
        margin-left: 1%;
        margin-right: 1%;
        margin-bottom: 5%;
    }
    .taskbtn-inline .btn {
        margin: 1%;
    }
    .sortDiv {
        float: right;
        width: 8%;
    }
    .sortSelect {
        width: 90%;
    }
    .content-card-section {
        height: 150px;
    }
    .fa-display-none {
        display:none;
    }
    .error_message {
        color: red;
    }
    .fa-refresh {
        font-size: 1.5vw;
    }
    .dialog-img {
        width: 50%;
        height: 50%;
    }
    .no_of_item {
        width: 8%;
    }
    .q-img__image.absolute-full {
        background-size: contain !important;
        background-repeat: no-repeat !important;
    }
    .csv-downloader {
        text-decoration: none;
    }
    .Error_msg {
        color: #1976d2;
        font-weight: bolder;
        font-size: 5vw;
        font-family: roboto;
    }
    .logout {
        font-size: 1.5vw !important;
    }

/* Card Grid Layout */
    .grid-img {
        width: 130px;
        height: 125px;
    }
    .gridFooter {
        padding: 0px !important;
    }
    .q-field__after, .q-field__append {
        padding-left: 0px !important;
    }

    .task-card {
        margin: 1%;
    }
    .task-card-color-critical {
        background-color: red;
        color: white;
    }

    .task-card .text-h5 {
        cursor: pointer;
    }
    .fa-check-circle-o {
        float: right;
        font-size: 1.5vw;
        position: absolute;
        left: 156%;
        top: 7%;
        color: #105320;  
        z-index: 0;
    }
    .disabled-card-link {
        opacity: .5;
        cursor: default !important;
        pointer-events: none;
    }
    .card-footer .custom-control {
        float: right;
    }
    .custom-control-label {
        cursor: pointer;
        color: #007bff;
    }
    .fa-cloud-download {
        color:white;
        margin-right: 2%;
    }
    .btn-delete {
        font-size: .8vw;
        margin-left: 6%;
    }
    .time-show-input {
        width: 25%;
        padding-left: 3%;
    }
    .imageLetter-grid h1{
        margin-top: 18%;
    }
    .selectCheckbox-grid {
        margin-left: 3%;
        cursor: pointer;
    }

/* Card List Layout */
    .list-img {
        width: 130px;
        height: 125px;
    }
    .task-card-list {
        margin: 1%;
    }
    .task-card-list .text-h5 {
        cursor: pointer;
    }
    .task-card-list .flex-right {
        float: right;
        width: 15%;
    }
    .task-card-list .time-show-input {
        width: 10%;
    }
    .time-show-input-list {
        width: 7%;
        padding-left: 1%;
    }
    .imageLetter-list h1{
        margin-top: 18%;
        margin-left: 30%;
    }
    .task-card-list .fa-list-icon {
        float: left;
        font-size: 1.5vw;
        left: 0%;
        color: #105320;
    }
    .selectCheckbox-list { 
        margin-left: 1.5%;
        cursor: pointer;
    }

/* Search Page Style */
    .search-col {
        padding: 1%;
    }

/* View Detail Model CSS */
    .view-detail-model {
        float: right;
        font-size: 1.5vw;
        position: relative;
        left: 0%;
        top: 22%;
        color: #28a745;
    }

/* Footer Pagination fixed */
    .footer-pagination {
        position: fixed;
        left: 0;
        bottom: 0;
        width: 100%;
        height: 10%;
        padding-top: 0%;
        padding-bottom: 0%;
        background-color: whitesmoke;
        text-align: center;
    }
    .q-pa-lg {
        padding: 0px 0px !important;
    }
</style>