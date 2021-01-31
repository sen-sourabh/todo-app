<template>
    <div class="container welcome-addtask">
            <q-card>
                <q-card-section>
                    <div class="text-h6">Add Task in Todo list</div>
                    <p>Manage your tasks</p>
                    <div class="error_message">{{error_msg}}</div>
                </q-card-section>
                
                <q-card-section class="q-pt-none">
                    <form @submit.prevent="onSubmit">
                        <div class="row form-row">
                            <div class="col input-col">
                                <q-input filled v-model="title" label="Title" v-bind:rules="TRequired"/>
                            </div>
                        </div>
                        <div class="row form-row">
                            <div class="col input-col">
                                <textarea v-model="desc" rows="6" placeholder="Description..."></textarea>
                            </div>
                        </div>
                        <div class="row form-row">
                            <div class="col input-col">
                                <q-input class="date-input" v-model="taskdate" type="date" label="Task Date" v-bind:rules="TDRequired"/>
                            </div>
                            <div class="col input-col">
                                <q-input filled v-model="tasktime" mask="fulltime" :rules="['fulltime', TTRequired]">
                                    <template v-slot:append>
                                    <q-icon name="access_time" class="cursor-pointer">
                                        <q-popup-proxy transition-show="scale" transition-hide="scale">
                                        <q-time
                                            v-model="tasktime"
                                            with-seconds
                                            format24h
                                        >
                                            <div class="row items-center justify-end">
                                            <q-btn v-close-popup label="Close" color="primary" flat />
                                            </div>
                                        </q-time>
                                        </q-popup-proxy>
                                    </q-icon>
                                    </template>
                                </q-input>
                            </div>
                        </div>
                        <div class="row form-row">
                            <div class="col input-col">
                                <q-input filled v-model="taskimage" @change="selectFile" type="file" accept="image/*"/>
                                <img id="todoimag" style="display:none;"/>
                            </div>
                            <div class="col input-col">
                                <q-select filled v-model="taskpriority" :options="options" label="Priority" v-bind:rules="TPRequired"/>
                            </div>
                        </div>
                        <div class="row form-row">
                            <div class="col input-col">
                                <center>
                                    <q-btn color="text-white bg-primary md" type="submit" label="Save" />
                                </center>
                            </div>
                            <div class="col input-col">
                                <center>
                                    <q-btn v-close-popup color="white" text-color="black" type="reset" label="Cancel" v-on:click="resetPriority" />
                                </center>
                            </div>
                        </div>
                    </form>
                </q-card-section>
            </q-card>
    </div>
</template>

<script>

export default {
    name: "Addtask",
    data() {
        return {
            error_msg: "",
            title: "",
            desc: "",
            taskdate: "",
            tasktime: "",
            taskimage: "",
            taskpriority: "",
            options:[
                    {label: 'Critical', value: '5', class: 'bg-negative'},
                    {label: 'High', value: '4', class: 'bg-secondary'},
                    {label: 'Medium', value: '3', class: 'bg-warning'},
                    {label: 'Inform', value: '2', class: 'bg-info'},
                    {label: 'Low', value: '1', class: 'bg-positive'}
                ]
        }
    },
    computed:{
        TRequired() {
            return [(v) => !!v || 'Title is required.']
        },
        TDRequired() {
            return [(v) => !!v || 'Date is required.'] 
        },
        TTRequired() {
            return [(v) => !!v || 'Time is required.'] 
        },
        TPRequired() {
            return [(v) => !!v || 'Priority is required.'] 
        }
    },
    methods: {
        resetPriority: function() {
            this.taskpriority = 'Low';
        },
        selectFile: function(event) {
            let reader = new FileReader();
            reader.readAsDataURL(event.target.files[0]);
            reader.onload = function() {
                document.getElementById('todoimag').src = reader.result;
            }
        },
        onSubmit: function(event){
            let session_user_id = this.$store.state.session_user_id;
            this.taskimage = document.getElementById('todoimag').src;
            var taskDateTime = new Date(this.taskdate+" "+this.tasktime);
            var todayDateTime = new Date();
            if(todayDateTime.toISOString() < taskDateTime.toISOString()) {
                let object = {
                    todoUserId: session_user_id,
                    todoTitle: this.title,
                    todoDesc: this.desc,
                    todoPriority: Number(this.taskpriority.value),
                    todoColor: this.taskpriority.class,
                    todoStatus: 0,
                    todoDate: this.taskdate+"T"+this.tasktime,
                    isComplete: 0,
                    todoImage: this.taskimage
                }
                const response = axios({
                    method: "POST",
                    url: "https://localhost:44399/api/TodoItems",
                    data: object
                })
                .then((response) => {
                    this.error_msg = "";
                    this.$emit("response_add", response);
                })
                .catch((err) => {
                    if(err.message === "Request failed with status code 400") {
                        this.error_msg = "Task already Created on this date & time.";
                    } else if(err.message === "Network Error") {
                        this.error_msg = "Network Error";
                    }
                });
                event.target.reset();
            } else {
                this.error_msg = "Task date & time should be greater than the current date & time.";
            }
        },
        onReset() {
            this.title= "";
            this.desc= "";
            this.taskdate= "";
            this.tasktime= "";
            this.taskimage= "";
            this.taskpriority= 'Low';
            this.error_msg= "";
        }
    }
}
</script>

<style scoped>
    .form-row {
        margin-left: 2%;
        margin-right: 2%;
    }
    .input-col {
      padding: 1%;
    }
    .input-col textarea {
        width: 100%;
        background-color: #f2f2f2;
        color: black;
        border: none;
        outline: none;
        padding: 4%;
    }
    .date-input {
        padding-bottom: 0%;
        background-color: #f2f2f2;
        color: black;
    }
</style>