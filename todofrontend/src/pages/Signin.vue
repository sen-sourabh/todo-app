<template>
    <div class="container welcome-signup">
      <Header />
      <center>
        <h2><b>Welcome to ToDo App</b></h2>
        <h5>Sign In, Manage your tasks</h5>
        <div class="error-message">{{error_message}}</div>
        <q-form @submit.prevent="onSubmit">
            <div class="row form-row">
                <div class="col input-col">
                  <q-input v-model="email" type="email" label="Email Id" v-bind:rules="[val => !!val || 'Email is required.', isValidEmail]" hint="Please enter valid and registered email id"/>
                </div>
                <div class="col input-col">
                  <q-input v-model="password" v-bind:type="isPwd ? 'password' : ''" label="Password" v-bind:rules="PRequired" ref="fldPasswordChange">
                    <template v-slot:append>
                        <q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
                                class="cursor-pointer"
                                v-on:click="isPwd = !isPwd">
                        </q-icon>
                    </template>
                  </q-input>
                </div>
            </div>
            <div class="row form-row">
                <div class="col">
                  <q-btn color="primary" type="submit"  label="SignIn" />
                </div>
                <div class="col">
                  <q-btn color="white" text-color="black" type="reset" label="Cancel" @click="onReset"/>
                </div>
            </div>
            <div class="row form-row">
                <div class="col input-col">   
                    <router-link to="/" class="alreadyaccountlink">Haven't an account?  </router-link>
                </div>
            </div>
        </q-form>
      </center>
    </div>
</template>

<script>
import Header from "./../components/Header.vue";

export default {
  name: 'Signin' ,
    components: {
        Header
    },
    data(){
        return {
            isPwd: "password",
            email: "",
            password: "",
            error_message: ""
        }
    },
    computed: {
        PRequired() {
            return [(v) => !!v || 'Password is required.']
        }
    },
    methods: {
        isValidEmail (v) {
          const emailPattern = /^(?=[a-zA-Z0-9@._%+-]{6,254}$)[a-zA-Z0-9._%+-]{1,64}@(?:[a-zA-Z0-9-]{1,63}\.){1,8}[a-zA-Z]{2,63}$/;
          return emailPattern.test(v) || 'Invalid email';
        },
        onSubmit: function(event){
              let object = {
                  useremail: this.email,
                  userpassword: this.password
              };
              const response = axios({
                method: "GET",
                url: "https://localhost:44399/api/Users/GetUserLogin/"+ this.email + "/"+ this.password
              })
              .then((response) => {
                if (response.status === 200) {
                    this.$store.commit("storeUserid", response.data.userId);
                    window.location.href = '#/home';
                }
              })
              .catch((err) => {
                if(err.message === "Request failed with status code 404") {
                  this.error_message = "Invalid Email or Password.";
                }
              });
        },
        onReset() {
                this.email= "";
                this.password= "";
                this.error_message= "";
        }
    }
}
</script>

<style>
    .form-row {
        margin-left: 10%;
        margin-right: 10%;
    }
    .input-col {
      padding: 1%;
    }
    .alreadyaccountlink {
        text-decoration: none;
    }
    .alreadyaccountlink:hover {
        text-decoration: none;
    }
    .error-message {
        color: red;
        font-weight: bolder;
        font-size: 1.5vw;
    }
</style>