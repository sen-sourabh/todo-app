<template>
    <div class="container">
        <div>
            <b>Upload Here</b>
        </div>
        <div class="row">
            <div class="col-6">
                <q-card @click="cardImgUploader" class="my-card task-card overflow-hidden">
                    <q-uploader 
                        id = "show-here"
                        class="my-uploader show-image"
                        url=""
                        accept=".jpg, .pdf, image/*"
                        @added="file_selected"
                        />
                        <!-- <q-btn @click="uploadFile" label="Upload" /> -->
                </q-card>
            </div>
        </div>
    </div>
</template>

<script>
function onFileChange($event) {
    Event.$emit("fetchdata",$event);
};

export default {
    name: "uploader",
    data() {
        return{
            // url: ""
            selected_file:'',
            check_if_document_upload:false
        }
    },
    mounted() {
        // console.log(document.getElementsByClassName("my-uploader")[0].childNodes[0]);
        document.getElementsByClassName("my-uploader")[0].childNodes[0].innerHTML = "<span class='dnd'>Drag & Drop or "
            +"<label id='browse-label' for='file-upload' class='browse-label'>Browse</label><input id='file-upload' type='file' onChange='onFileChange' style='display:none;'/> File to Upload</span>";
        
        // document.getElementsByClassName("my-uploader")[0].innerHTML = "<span class='dnd'>Drag & Drop or "
            // +"<label id='browse-label' for='file-upload' class='browse-label'>Browse</label><input id='file-upload' type='file' style='display:none;'/> File to Upload</span>";
        
        document.getElementById('browse-label').classList.add("browse-label");
        
        document.getElementsByClassName("my-uploader")[0].classList.add("justify-center");
        Event.$on("fetchdata", group => {
            this.fetchData($event);
        });

        
    },
    methods: {
        file_selected(file) {
            if(file != undefined) {
                document.getElementsByClassName("my-uploader")[0].classList.remove('.show-image');
            }
            // console.log(document.getElementsByClassName("my-uploader"));
            console.log("rough FILE: "+JSON.stringify(file));
            this.selected_file = file[0];
            console.log("FILE: "+JSON.stringify(this.selected_file));
            this.check_if_document_upload=true;
            console.log("Check doc: "+this.check_if_document_upload);
        },
        cardImgUploader() {
            document.querySelector('#file-upload').click();
            // alert('this is clicked');
            let input = document.getElementById('file-upload');
            this.url = URL.createObjectURL(input.files[0]);
            // console.log('tarun2: '+ JSON.stringify(input.files[0]));
            console.log(this.url);
            // this.file_selected(input.files[0]);
        },
        fetchData() {
            alert($event);
        }
    },
}
</script>

<style>

/* .browse-label {
    color: rgb(4, 35, 73);
    cursor: pointer;
    text-decoration: underline;
} */

/* .dnd {
    transform: translateY(40px);
} */

/* span.dnd {
    color: black;
    position: relative;
    top: 1670%;
}
.container{
    margin: 5%;
}
.my-card {
    float: left;
    border: 1px solid lightgray;
    width: 100%;
    min-height: 320px;
    height: 100%;
    cursor: pointer;
}
.my-uploader {
    transform: scale(1.1);
    border: 1px solid red;
    width: 100%;
    min-height: 320px;
    height: 100%;
    align-content: center;
    vertical-align: middle;
    background-color: whitesmoke;
    
}
.show-image {
    text-align: center;
    background-image: url(".././assets/images/cloud-upload-1.png");
    background-repeat: no-repeat;
    background-attachment: inherit;
    background-size: 15%;
    background-position: center;
}
.my-uploader .q-uploader__file--img {
    color: #fff;
    height: 100% !important;
    min-width: 100% !important;
    background-position: 50% 50%;
    background-size: cover;
    background-repeat: no-repeat;
}
.q-uploader__list .q-uploader__file--img {
    height: 100%;
    background-size: contain;
}
.scroll {
    overflow: auto;
    padding: 5%;
}
.q-uploader__subtitle {
    display: none;
} */

/* .position-relative {
    position: relative;
}
.position-absolute {
    
    position: absolute;
    Top:0;
} */
</style>