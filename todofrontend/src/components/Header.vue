<template>
  <div>
    <div class="row bg-primary header-div justify-between">
		<div clas="col-3">
			<h4 class="text-white"><b>Todo App</b></h4>
		</div>
		<div clas="col-6" v-if="this.$route.path === '/home'">
			<div>
				<template v-if="this.$store.state.session_user_id != 0">
						<q-btn-dropdown borderless push color="primary" :label="userdata.userUserName">
							<q-list>
								<q-item clickable v-close-popup @click="openProfDialog">
									<q-item-section>
										<q-item-label>Profile</q-item-label>
									</q-item-section>
								</q-item>

								<q-item clickable v-close-popup @click="openCPDialog">
									<q-item-section>
										<q-item-label>Change Password</q-item-label>
									</q-item-section>
								</q-item>

							</q-list>
						</q-btn-dropdown>
				</template>
			<q-btn borderless push color="negative" @click="logoutinit" title="Logout"><i class="fa fa-power-off logout"></i></q-btn>
			</div>
		</div>

		<!-- User Profile Dialog -->
		<q-dialog v-model="openProfileDialog" persistent>
			<q-card style="min-width: 30%;height: auto;padding: 1%;">
				<q-card-section>
					<div class="text-h6 card-heading-profile">Profile</div>
					<span class="q-mt-sm text-negative">{{error_msg}}</span>
				</q-card-section>

				<q-card-section class="q-pt-none">
					<div class="q-card-custom1">
						<div class="responsive-img">
							<div v-if="userdata.userImages && (userdata.userImages.length > 0)">
                                <q-img class="avatar-img" v-bind:src="userdata.userImages" @click="openFabCam"/>
								<q-fab id="fab-cam" style="z-index:0;" class="camera" icon=" " direction="left">
									<q-fab-action @click="callFileInput" icon="fa fa-camera" />
									<q-fab-action icon="fa fa-video-camera" />
								</q-fab>
							</div>
							<div v-else-if="userdata.userImages && userdata.userImages.length == 0">
                                <img class="avatar-img" src=".././assets/images/avatar_default.png"/>
                            </div>
							<div v-else-if="userdata.userImages && userdata.userImages == null">
                                <img class="avatar-img" src=".././assets/images/avatar_default.png"/>
                            </div>
                            <div v-else>
								<img class="avatar-img" src=".././assets/images/avatar_default.png"/>
                            </div>
							
							

							<!-- <i class="fa fa-camera camera" @click="callFileInput"></i> -->
								<input id="fileInput" @change="selectedFile" type="file" accept="image/*" style="display:none;" />
								<img id="userimag" style="display:none;"/>
							<img src=".././assets/images/green_dot.jpg" class="status" />
						</div>
						<div>
							<div class="text-h6 cursor-pointer">
								{{userdata.userUserName}}
								<q-popup-edit v-model="userdata.userUserName" content-class="bg-primary text-white">
									<q-input hint="Press enter to save" dark color="white" v-model="userdata.userUserName" @keydown.enter="saveChanges(userdata.userUserName, true, false, false)" dense autofocus counter v-bind:rules="UNRequired">
										<template v-slot:append>
											<q-icon name="edit" />
										</template>
									</q-input>
								</q-popup-edit>
							</div>
							<div class="text-subtitle1">{{userdata.userEmail}}</div>
							<div class="text-caption">{{userdata.userCreateDate | showDate}}</div>
						</div>
					</div>
					<div class="q-card-custom2">
						<div class="responsive-content-left">
							<div class="text-subtitle1">Mobile Number</div>
						</div>
						<div class="responsive-content-right">
							<div class="text-subtitle1 cursor-pointer">
								{{userdata.userMobile}}
								<q-popup-edit v-model="userdata.userMobile" content-class="bg-primary text-white">
									<q-input hint="Press enter to save" dark color="white" v-model="userdata.userMobile" @keydown.enter="saveChanges(userdata.userMobile, false, true, false)" dense autofocus counter v-bind:rules="MRequired">
										<template v-slot:append>
											<q-icon name="edit" />
										</template>
									</q-input>
								</q-popup-edit>
							</div>
							
						</div>
					</div>
					<div class="q-card-custom2">
						<div class="responsive-content-left">
							<div class="text-subtitle1">Password</div>
						</div>
						<div class="responsive-content-right">
							<div v-if="changePassView" class="text-subtitle1">{{userdata.userPassword}}<i class="fa fa-eye q-ml-sm cursor-pointer" @click="changeView"></i></div>
							<div v-else class="text-subtitle1">{{userdata.userPassword | changePassword}}<i class="fa fa-eye-slash q-ml-sm cursor-pointer" @click="changeView"></i></div>
						</div>
					</div>
					<div class="q-card-custom2">
						<div class="responsive-content-left">
							<div class="text-subtitle1">Logout</div>
						</div>
						<div class="responsive-content-right">
							<q-btn borderless push color="negative" @click="logoutinit" title="Logout"><i class="fa fa-power-off logout"></i></q-btn>
						</div>
					</div>
				</q-card-section>
				<q-card-section class="text-right">
					<q-btn v-close-popup label="OK" @click="resetProfile" color="primary" flat class="q-ml-sm"/>
				</q-card-section>
			</q-card>
		</q-dialog>

		<!-- Change Password Dialog -->
		<q-dialog v-model="openCPassDialog" persistent>
			<q-card style="min-width: 30%;height: auto;padding: 1%;">
				<q-card-section>
				<div class="text-h6" style="font-size: 2rem;">Change Password</div>
				<span class="q-mt-sm text-negative">{{error_msg}}</span>
				</q-card-section>

				<q-card-section class="q-pt-none">
					<q-form @submit.prevent="onSubmit">
						<q-input v-bind:type="isPwd ? 'password' : ''" v-model="curpass" label="Current Password" v-bind:rules="CPRequired">
							<template v-slot:append>
								<q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
										class="cursor-pointer"
										v-on:click="isPwd = !isPwd">
								</q-icon>
							</template>
						</q-input>

						<q-input v-model="newpass" v-bind:type="isPwd ? 'password' : ''" label="Password" v-bind:rules="PRequired" ref="newpasschange">
							<template v-slot:append>
								<q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
										class="cursor-pointer"
										v-on:click="isPwd = !isPwd">
								</q-icon>
							</template>
						</q-input>

						<q-input v-model="connewpass" v-bind:type="isPwd ? 'password' : ''" label="Confirm Password" v-bind:rules="CNPRequired" ref="newpasschangeConfirm">
							<template v-slot:append>
								<q-icon :name="isPwd ? 'visibility_off' : 'visibility'"
										class="cursor-pointer"
										v-on:click="isPwd = !isPwd"></q-icon>
							</template>
						</q-input>

						<div>
							<q-btn label="Submit" type="submit" color="primary" @click="autoClose" />
							<q-btn v-close-popup label="Cancel" @click="resetform" type="reset" color="primary" flat class="q-ml-sm"/>
						</div>
					</q-form>
				</q-card-section>
			</q-card>
		</q-dialog>
    </div>
  </div>
</template>

<script>
import { Prop, Component } from "vue-property-decorator";

export default {
	name: "Header",
	computed: {
		UNRequired() {
            return [(v) => !!v || 'User name is required.']
        },
        MRequired() {
            return [
                  (v) => !!v || 'Mobile number is required.', 
                  (v) => v.length == 10 || 'Mobile number of 10 digits.'
                ]
        },
        CPRequired() {
            return [(v) => !!v || 'Current password is required.']
        },
        PRequired() {
            return [(v) => !!v || 'New password is required.']
        },
        CNPRequired() {
            return [
                (v) => !!v || "Confirm password is required.",
                (v) => v == this.$refs.newpasschange.value || "Password and Confirm password are not match."
              ]
		}
	},
    data() {
      return {
		isPwd: "password",
        userdata: {
			userId: Number,
			userUserName: String,
			userEmail: String,
			userPassword: String,
			userMobile: Number,
			userStatus: Number,
			userBlockStatus: Number,
			userCreateDate: String,
			userImages: String
		},
		session_user_id: this.$store.state.session_user_id,
		confirm: false,
		openCPassDialog: false,
		curpass: "",
		newpass: "",
		connewpass: "",
		error_msg: "",
		openProfileDialog: false,
		changePassView: false
      }
	},
	filters: {
		showDate: function (value) {
            if(!value) return ''
            var newDate = new Date(value);
            return newDate.toDateString();
        },
        changePassword: function(value) {
			if (!value) return ''
			let addPass = '';
			let i = 0;
			while(i < value.length) {
				addPass += "*";
				i++;
			}
            return addPass;
		}
	},
	created() {
		if(this.$store.state.session_user_id != 0) {
			this.getUserDetails();
		}
	},
    methods: {
		openFabCam() {
			var a = document.getElementById("fab-cam");
      		a.click();
		}
		,
		notifyMe: function() {

			let options = {
				body: "Hi "+this.userdata.userUserName+"! Welcome to Todo App",
				icon: this.userdata.userImages,
				// image: "https://lh3.googleusercontent.com/RivvntB-z-qe59FF0j9bxy6tBXZCnksHIz_EvXvaSKa039MC9IwZLTORIirdJcZZcE8",//".././assets/images/todo-img.png"
			};
			// Let's check if the browser supports notifications
			if (!("Notification" in window)) {
				alert("This browser does not support desktop notification");
			}

			// Let's check whether notification permissions have already been granted
			else if (Notification.permission === "granted") {
				// If it's okay let's create a notification
				var notification = new Notification("Todo Application", options);
				var image = Notification.image;
			}

			// Otherwise, we need to ask the user for permission
			else if (Notification.permission !== "denied") {
				Notification.requestPermission().then(function (permission) {
					// If the user accepts, let's create a notification
					if (permission === "granted") {
						var notification = new Notification("Todo Application", options);
						var image = Notification.image;
					}
				});
			}
			setTimeout(() => { notification.close() }, 5000);

			// At last, if the user has denied notifications, and you 
			// want to be respectful there is no need to bother them any more.
			
		},
		callFileInput() {
			var a = document.getElementById("fileInput");
      		a.click();
		},
		selectedFile: function(event) {
			let reader = new FileReader();
            reader.readAsDataURL(event.target.files[0]);
            reader.onload = function() {
				document.getElementById('userimag').src = reader.result;
			}
			setTimeout(() => { this.saveChanges("", false, false, true)}, 1000);
		},
        getUserDetails: function() {
            axios.get("https://localhost:44399/api/Users/"+this.session_user_id)
            .then(response => {
				this.userdata = response.data;
				this.$emit('username', this.userdata.userUserName);
				this.notifyMe();
            });
		},
		logoutinit: function(){
			this.confirm = true;
			this.$emit("confirm", this.confirm);
		},
		openCPDialog: function() {
			this.openCPassDialog = true;
		},
		openProfDialog: function() {
			this.openProfileDialog = true;
		},
		changeView: function() {
			if(this.changePassView) {
				this.changePassView = false;
			} else {
				this.changePassView = true;
			}
		},
		onSubmit: function(event){
			event.preventDefault();
			if(this.userdata.userPassword == this.curpass)
			{
				this.userdata.userPassword = this.newpass;
				const response = axios({
					method: "PUT",
					url: "https://localhost:44399/api/Users/"+this.session_user_id,
					data: this.userdata
				})
				.then((response) => {
					this.openCPassDialog = false;
					this.error_msg = "";
					this.curpass= "";
					this.newpass= "";
					this.connewpass= "";
					let pickData = {
						title: 'Success',
						message: 'Password changed successfully.',
						position: 'bottom'
					};
					this.autoClose(pickData);
					this.getUserDetails();
				})
				.catch((err) => {
					console.log(err.message);
					this.error_msg = err.message;
				});
			} else {
				this.error_msg = "Current password is invalid.";
			}
		},
		resetform: function() {
			this.curpass= "";
			this.newpass= "";
			this.connewpass= "";
			this.error_msg= "";
		},
		resetProfile: function() {
			this.changePassView = false;
		},
		autoClose(getData) {
			let seconds = 1;
			let dialog = this.$q.dialog({
				title: getData.title,
				message: getData.message,
				position: getData.position
			})
			const timer = setInterval(() => {
				seconds--;
				if (seconds > 0) {
				}
				else {
					clearInterval(timer)
					dialog.hide();	
				}
			}, 1000)
		},
		saveChanges: function(uservalue, uname, umobile, uimage) {
			if(uname) {
				this.userdata.userUserName = uservalue;
			}
			if(umobile) {
				this.userdata.userMobile = Number(uservalue);
			}
			if(uimage) {
				this.userdata.userImages = document.getElementById('userimag').src;
			}
			const response = axios({
				method: "PUT",
				url: "https://localhost:44399/api/Users/"+this.session_user_id,
				data: this.userdata
			})
			.then((response) => {
				this.getUserDetails();
			})
			.catch((err) => {
				console.log(err.message);
				this.error_msg = err.message;
			});
		}
    }
}
</script>


<style>
	.avatar-img {
		cursor: pointer;
		width: 105px;
		height: 105px;
		background-repeat: no-repeat !important;
		background-size: contain !important;
		border-radius: 50%;
		box-shadow: 0px 0px 6px 1px rgb(189 189 189);
	}

	.header-div {
		padding: 1%;
	}
	.text-white {
		margin:0%;
	}

	.card-heading-profile {
		font-size: 2rem;
	}
	.card-heading-edit {
		font-size: 2rem;
		text-align: right;
	}

	.q-card-custom1 {
		padding: 1%; 
		text-align: center !important;
		box-shadow: 0px 0px 6px 1px rgb(189 189 189);
	}
	.responsive-img {
		position: relative;
		max-width: 100%;
		height: 130px;
	}
	.status {
		position: relative;
		max-width: 5%;
		border-radius: 50%;
		right: -11%;
		bottom: 8px;
	}
	.camera {
		position: relative;
		max-width: 5%;
		border-radius: 50%;
		right: 11%;
		font-size: 1.5vw;
		color: #0e0000;
		bottom: 1px;
		cursor: pointer;
	}
	.q-fab--opened .q-fab__actions {
		position: absolute;
		right: 5.4vw;
	}
	.q-btn--fab .q-btn__wrapper {
		display: none;
	}
	.q-card-custom2 {
		padding: 2%;
		margin-top: 2%; 
		box-shadow: 0px 0px 6px 1px rgb(189 189 189);
	}
	.q-card-custom2 .responsive-content-left {
		float:left;
		text-align: left;
	}
	.q-card-custom2 .responsive-content-right {
		text-align: right;
	}
  	.q-dialog__inner.flex.no-pointer-events.q-dialog__inner--minimized.q-dialog__inner--standard.fixed-full.flex-center .q-dialog-plugin {
		display: none;
	}
  	.q-dialog__inner.flex.no-pointer-events.q-dialog__inner--minimized.q-dialog__inner--bottom.fixed-bottom.justify-center .q-dialog-plugin {
		width: max-content;
		height: 120px;
	}
	.q-dialog__inner.flex.no-pointer-events.q-dialog__inner--minimized.q-dialog__inner--bottom.fixed-bottom.justify-center .q-dialog-plugin .q-btn {
		display: none;
	}
</style>
