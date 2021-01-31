<template>
    <div class="container welcome-signup">
      <Header />
      <center>
        <h2><b>Welcome to ToDo App</b></h2>
        <h5>Sign Up, Just Do It</h5>
        <div class="error-message">{{error_message}}</div>
        <div class="success-message">{{success_message}}</div>
        <q-form @submit.prevent="onSubmit">
            <div class="row form-row">
                <div class="col input-col">
                    <q-input v-model="firstname" label="First Name" v-bind:rules="FNRequired"/>
                </div>
                <div class="col input-col">
                  <q-input v-model="lastname" label="Last Name" v-bind:rules="LNRequired"/>
                </div>
            </div>
            <div class="row form-row">
                <div class="col input-col">
                  <q-input v-model="email" type="email" label="Email Id" v-bind:rules="[val => !!val || 'Email is required.', isValidEmail]" hint="Please enter valid email id"/>
                </div>
                <div class="col input-col">
                  <q-input v-model="mobilenumber" label="(+91) - xxxxxxxxxx" v-bind:rules="MRequired"/>
                </div>
            </div>
            <div class="row form-row">
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
                <div class="col input-col">
                  <q-input v-model="confirmpassword" v-bind:type="isPwd ? 'password' : ''" label="Confirm Password" v-bind:rules="CPRequired" ref="fldPasswordChangeConfirm">
                    <template v-slot:append>
                        <q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
                                class="cursor-pointer"
                                v-on:click="isPwd = !isPwd"></q-icon>
                    </template>
                  </q-input>
                </div>
            </div>
            <div class="row form-row">
                <div class="col">
                    <q-btn color="primary" type="submit" label="SignUp" />
                </div>
                <div class="col">
                  <q-btn color="white" text-color="black" type="reset" label="Cancel" @click="onReset" />
                </div>
            </div>
            <div class="row form-row">
                <div class="col input-col">   
                    <router-link to="/signin" class="alreadyaccountlink">Already have an account?  </router-link>
                </div>
            </div>
        </q-form>
      </center>
    </div>
</template>

<script>
import Header from "./../components/Header.vue";
import axios from "axios";

export default {
  name: 'Index' ,
    components: {
        Header
    },
    data(){
        return {
            isPwd: "password",
            firstname: "",
            lastname: "",
            email: "",
            password: "",
            confirmpassword: "",
            mobilenumber: "",
            error_message: "",
            success_message: ""
        }
    },
    computed: {
        FNRequired() {
            return [(v) => !!v || 'First name is required.']
        },
        LNRequired() {
            return [(v) => !!v || 'Last name is required.']
        },
        MRequired() {
            return [
                  (v) => !!v || 'Mobile number is required.', 
                  (v) => v.length == 10 || 'Mobile number of 10 digits.'
                ]
        },
        PRequired() {
            return [(v) => !!v || 'Password is required.']
        },
        CPRequired() {
            return [
                (v) => !!v || "Confirm password is required.",
                (v) => v == this.$refs.fldPasswordChange.value || "Password and Confirm password are not match."
              ]
        }
    },
    methods: {
        isValidEmail (v) {
          const emailPattern = /^(?=[a-zA-Z0-9@._%+-]{6,254}$)[a-zA-Z0-9._%+-]{1,64}@(?:[a-zA-Z0-9-]{1,63}\.){1,8}[a-zA-Z]{2,63}$/;
          return emailPattern.test(v) || 'Invalid email';
        },
        onSubmit: function(event){
          event.preventDefault();
          let object = {
            userUserName: this.firstname + " " + this.lastname,
            userEmail: this.email,
            userPassword: this.password,
            userMobile: Number(this.mobilenumber),
            userBlockStatus: 0,
            userStatus: 0
          };
          const response = axios({
            method: "POST",
            url: "https://localhost:44399/api/Users",
            data: object
          })
          .then((response) => {
            this.success_message = "User registered successfully.";
          })
          .catch((err) => {
            if(err.message === "Request failed with status code 404") {
              this.error_message = "Email is already used. Please try with different email id.";
            }
          });
          this.firstname= "";
          this.lastname= "";
          this.email= "";
          this.password= "";
          this.confirmpassword= "";
          this.mobilenumber= "";
          event.target.reset();
        },
        onReset() {
          this.firstname= "";
          this.lastname= "";
          this.email= "";
          this.password= "";
          this.confirmpassword= "";
          this.mobilenumber= "";
          this.error_message= "";
          this.success_message= "";
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
    .success-message {
        color: green;
        font-weight: bolder;
        font-size: 1.5vw;
    }
</style>