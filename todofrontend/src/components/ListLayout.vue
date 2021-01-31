<template>
    <div>
        <div class="row">
                <div class="col-md-12" v-for="result in todoitems" :key="result.todoId">
                    <q-card class="my-card task-card-list" flat bordered>
                        <q-card-section horizontal class="content-card-section text-white" :class="result.todoColor">
                            <q-card-section class="col-5 flex flex-right">
                                <div v-if="result.todoImage.length == 0">
                                    <avatar :fullname="result.todoTitle | getTitleFirstLetter" :size="96"></avatar>
                                </div>
                                <div v-else-if="result.todoImage == null">
                                    <avatar :fullname="result.todoTitle | getTitleFirstLetter" :size="96"></avatar>
                                </div>
                                <div v-else class="imageLetter-list">
                                    <q-img class="rounded-borders list-img" v-bind:src="result.todoImage" />
                                </div>
                            </q-card-section>
                            <q-card-section class="q-pt-xs">
                                <div class="text-overline">{{result.todoDate | showDate}} <i class="fa-list-icon fa fa-check-circle-o" v-bind:class="[result.isComplete ? '' : ' fa-display-none']"></i></div>
                                <div class="text-h5 q-mt-sm q-mb-xs" @click="openmodel(result)"> {{result.todoTitle | titleTrim | showTitle}} </div>
                        
                                <div class="text-caption">
                                    {{result.todoDesc | showDesc}}
                                </div>
                            </q-card-section>
                        </q-card-section>

                        <q-separator />

                        <q-card-actions>
                            <div style="font-weight:500;margin-left:1%;margin-right: 1%;">{{result.todoDate | showTime}}</div>
                            <q-select class="select-priority" @input="getPriorityValue(result)" borderless v-model="result.todoPriorityText" :options="options"/>
                            <q-btn @click="getSelectedId(result)" class="bg-negative text-white" label="Delete"/>
                            <q-btn @click="getCompletedId(result)" class="bg-light text-black" v-bind:class="[result.isComplete ? ' fa-display-none' : '']" label="Done"/>
                            <q-btn @click="getNotCompletedId(result)" class="bg-positive text-white" v-bind:class="[result.isComplete ? '' : ' fa-display-none']" label="UnDone"/>
                            <input class="selectCheckbox-list" type="checkbox" @change="getCheckBoxData(result)" />
                        </q-card-actions>
                    </q-card>
                </div>

                <q-dialog v-model="infoModel" persistent>
                    <q-card>
                        <q-card-section>
                            <div class="text-overline">{{todomodal.todoDate | showModalDate}}<i class="fa fa-check-circle-o view-detail-model" v-bind:class="[todomodal.isComplete ? '' : ' fa-display-none']"></i></div>
                            <div class="text-h6">{{todomodal.todoTitle}}</div>
                            <div>
                                <div v-if="todomodal.todoImage.length == 0">
                                    <avatar :fullname="todomodal.todoTitle | getTitleFirstLetter" :size="96"></avatar>
                                </div>
                                <div v-else-if="todomodal.todoImage == null">
                                    <avatar :fullname="todomodal.todoTitle | getTitleFirstLetter" :size="96"></avatar>
                                </div>
                                <div v-else >
                                    <img :src="todomodal.todoImage" class="dialog-img" />
                                </div>
                            </div>
                        </q-card-section>

                        <q-card-section class="q-pt-none">
                            <p class="text-justify">
                                {{todomodal.todoDesc}} 
                            </p>
                        </q-card-section>

                        <q-card-actions align="right">
                            <q-btn flat color="dark" label="Edit" @click="openEditModel(todomodal)" title="Edit Task" />
                            <q-btn flat label="Cancel" color="red" v-close-popup />
                        </q-card-actions>
                    </q-card>
                </q-dialog>

                <div>
                    <q-dialog v-model="editModel" persistent>
                        <Edittask :todoedititem="todoeditmodal" @response_edit="getEditResponse"/>
                    </q-dialog>
                </div>
            </div>
            
        </div>
</template>


<script>
import Vue from "vue";
import Edittask from "./../components/Edittask.vue";
import { Prop, Component } from "vue-property-decorator";
import Avatar from 'vue-avatar-component';

export default {
    name: "ListLayout",
    components: {
        Edittask,
        Avatar
    },
    props: {
        todoitems: () => [
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
    data(){
        return{
            selectTodoId: [],
            editModel: false,
            infoModel: false,
            editModel: false,
            alert: false,
            deleteCheck: false,
            completeMark: false,
            tasktime: '07:30 PM',
            options:[
                    {label: 'Critical', value: '5', class: 'bg-negative'},
                    {label: 'High', value: '4', class: 'bg-secondary'},
                    {label: 'Medium', value: '3', class: 'bg-warning'},
                    {label: 'Inform', value: '2', class: 'bg-info'},
                    {label: 'Low', value: '1', class: 'bg-positive'}
                ],
            todomodal: {
                todoId: String,
                todoTitle: String,
                todoDesc: String,
                todoPriority: String,
                todoDate: String,
                todoCreateDate: String,
                todoStatus: String,
                todoImage: String,
                todoUserId: String,
                isComplete: String,
                todoColor: String
            },
            todoeditmodal: {
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
    filters: {
        getTitleFirstLetter: function(value) {
            if (!value) return ''
            return value.charAt(0).toUpperCase();
        },
        titleTrim: function(value) {
            if (!value) return ''
            return value.trim();
        },
        showTitle: function (value) {
            if (!value) return ''
            value = value.substring(0, 10)+'...';
            return value.charAt(0).toUpperCase() + value.slice(1);
        },
        showDesc: function (value) {
            if (!value) return ''
            value = value.trim().substring(0, 300)+'...';
            return value.charAt(0).toUpperCase() + value.slice(1);
        },
        showDate: function (value) {
            if(!value) return ''
            var newDate = new Date(value);
            return newDate.toDateString();
        },
        showModalDate: function (value) {
            if(!value) return ''
            var newmodaldate = new Date(value);
            return newmodaldate.toDateString() + " " + newmodaldate.toLocaleTimeString();
        },
        showTime: function (value) {
            if(!value) return ''
            var newdate = new Date(value);
            var hours = newdate.getHours();
            var minutes = newdate.getMinutes();
            var ampm = hours >= 12 ? 'PM' : 'AM';
            hours = hours % 12;
            hours = hours ? hours : 12; // the hour '0' should be '12'
            minutes = minutes < 10 ? '0'+minutes : minutes;
            var strTime = hours + ':' + minutes + ' ' + ampm;
            return strTime;
        }
    },
    methods: {
        getCheckBoxData(result) {
            // const id = result.todoId;
            // const index = this.selectTodoId.length;
            // if(!this.selectTodoId.includes(id)) {
            //     this.selectTodoId[index] = id;
            // }
            // else {
            //     const indice = this.selectTodoId.indexOf(id)
            //     this.selectTodoId[indice] = null;
            // }
            // this.$emit("selectedTodos", this.selectTodoId);
            // alert(JSON.stringify(this.selectTodoId));

            const id = result.todoId;
            // console.log(typeof id);
            if(!this.selectTodoId.includes(id)) {
                this.selectTodoId.push(id);
            } else {
                const indice = this.selectTodoId.indexOf(id)
                if (indice > -1) {
                    this.selectTodoId.splice(indice, 1);
                }
            }
            
            this.$emit("selectedTodos", this.selectTodoId);
            // alert(JSON.stringify(this.selectTodoId));
        },
        openmodel: function(modeldata) {
            this.infoModel = true;
            this.todomodal =(modeldata);
        },
        openEditModel: function(editmodal) {
            this.editModel = true;
            this.todoeditmodal = editmodal;
        },
        getSelectedId: function(resultData) {
            this.$emit("todoDeleteId", resultData);
        },
        getCompletedId: function(resultData) {
            this.$emit("todoCompletedId", resultData);
        },
        getNotCompletedId: function(resultData) {
            this.$emit("todoNotCompletedId", resultData);
        },
        getEditResponse: function() {
            this.editModel = false;
            this.infoModel = false;
            this.$emit("responseEditList");
        },
        getPriorityValue(resultData) {
            this.$emit("updatePriority", resultData);
        }
    }
} 
</script>