<template>
    <div class="container welcome-addtask">
            <q-card>
                <q-card-section>
                    <div class="text-h6">Edit Task in Todo list</div>
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
                                <img id="todoimaga" style="display:none;"/>
                            </div>
                            <div class="col input-col">
                                <q-select filled v-model="taskpriorityText" :options="options" label="Priority" v-bind:rules="TPRequired"/>
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
    name: "Edittask",
    props:{
        todoedititem:() => [
                {
                    todoId: String,
                    todoTitle: String,
                    todoDesc: String,
                    todoPriority: Number,
                    todoPriorityText: String,
                    todoDate: String,
                    todoCreateDate: String,
                    todoStatus: String,
                    todoImage: String,
                    todoUserId: String,
                    isComplete: String,
                    todoColor: String
                }
        ]
    },
    data() {
        return {
            error_msg: "",
            taskId: this.todoedititem.todoId ? this.todoedititem.todoId : 0,
            title: this.todoedititem.todoTitle.trim() ? this.todoedititem.todoTitle.trim() : "",
            desc: this.todoedititem.todoDesc.trim() ? this.todoedititem.todoDesc.trim() : "",
            taskdate: this.todoedititem.todoDate ? new Date(this.todoedititem.todoDate).getFullYear() + 
                    "-" + ((new Date(this.todoedititem.todoDate).getMonth() <= 8) ? "0"+(new Date(this.todoedititem.todoDate).getMonth() + 1) : (new Date(this.todoedititem.todoDate).getMonth() + 1)) + 
                    "-" + ((new Date(this.todoedititem.todoDate).getDate() <= 9) ? "0"+new Date(this.todoedititem.todoDate).getDate() : new Date(this.todoedititem.todoDate).getDate()) : "",
            tasktime: this.todoedititem.todoDate ? ((new Date(this.todoedititem.todoDate).getHours() <= 9) ? "0"+new Date(this.todoedititem.todoDate).getHours() : new Date(this.todoedititem.todoDate).getHours()) + 
                    ":" + ((new Date(this.todoedititem.todoDate).getMinutes() <= 9) ? "0"+new Date(this.todoedititem.todoDate).getMinutes() : new Date(this.todoedititem.todoDate).getMinutes()) + 
                    ":" + ((new Date(this.todoedititem.todoDate).getSeconds() <= 9) ? "0"+new Date(this.todoedititem.todoDate).getSeconds() : new Date(this.todoedititem.todoDate).getSeconds()) : "",
            taskimage: this.todoedititem.todoImage ? this.todoedititem.todoImage : "",
            taskpriority: this.todoedititem.todoPriority ? this.todoedititem.todoPriority : 0,
            taskpriorityText: this.todoedititem.todoPriorityText ? this.todoedititem.todoPriorityText : "",
            taskUserId: this.todoedititem.todoUserId ? this.todoedititem.todoUserId : this.todoedititem.todoUserId,
            taskComplete: this.todoedititem.isComplete ? this.todoedititem.isComplete : 0,
            taskColor: this.todoedititem.todoColor ? this.todoedititem.todoColor : "bg-positive",
            options:[
                    {label: 'Critical', value: '5', class: 'bg-negative'},
                    {label: 'High', value: '4', class: 'bg-secondary'},
                    {label: 'Medium', value: '3', class: 'bg-warning'},
                    {label: 'Inform', value: '2', class: 'bg-info'},
                    {label: 'Low', value: '1', class: 'bg-positive'}
                ],
            todoedititem: {
                todoId: String,
                todoTitle: String,
                todoDesc: String,
                todoPriority: Number,
                todoPriorityText: String,
                todoDate: String,
                todoCreateDate: String,
                todoStatus: String,
                todoImage: String,
                todoUserId: String,
                isComplete: String,
                todoColor: String
            }
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
                document.getElementById('todoimaga').src = reader.result;
            }
        },
        onSubmit: function(event){
            var todoimaga = document.getElementById('todoimaga').src;
            if(todoimaga != ""){
                this.taskimage = todoimaga;
            }
            var todoPriorityValue = "";
            var todoPriorityClass = "";
            this.options.forEach(option => {
                if(option.label == this.taskpriorityText) {
                    todoPriorityValue = option.value;
                    todoPriorityClass = option.class;
                }
            })
            if(todoPriorityValue == ""){
                todoPriorityValue = this.taskpriorityText.value;
            }
            if(todoPriorityClass == ""){
                todoPriorityClass = this.taskpriorityText.class;
            }

            var taskDateTime = new Date(this.taskdate+" "+this.tasktime);
            var todayDateTime = new Date();
            if(todayDateTime.toISOString() < taskDateTime.toISOString()) {
                let object = {
                    todoId: this.taskId,
                    todoUserId: this.taskUserId,
                    todoTitle: this.title,
                    todoDesc: this.desc,
                    todoDate: this.taskdate+"T"+this.tasktime,
                    todoPriority: Number(todoPriorityValue),
                    todoStatus: 0,
                    isComplete: this.taskComplete,
                    todoColor: todoPriorityClass,
                    todoImage: this.taskimage
                };
                console.log(object);
                const response = axios({
                    method: "PUT",
                    url: "https://localhost:44399/api/TodoItems/"+ this.todoedititem.todoId,
                    data: object
                })
                .then((response) => {
                    this.error_msg = "";
                    this.$emit("response_edit", response);
                })
                .catch((err) => {
                    if(err.message === "Request failed with status code 400") {
                        this.error_msg = "Task already Created on this date & time.";
                    } else if(err.message === "Network Error") {
                        this.error_msg = "Network Error";
                    }
                    console.log("update in error error: ", err.message);
                });
            } else {
                this.error_msg = "Task date & time should be greater than the current date & time.";
            }
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