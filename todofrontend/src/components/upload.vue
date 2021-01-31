<template>
    <div class="container-fluid">
        <div>
            <b>Upload Here</b>
        </div>
        <div class="row">
            <div class="col-6 img-upload">
                <q-uploader
                    accept=".jpg, .png"
                    multiple
                    @added="uploadFile"
                    ref="uploader"
                    auto-expand
                    url=""
                    stack-label="upload image"
                    />
            </div>
        </div>
    </div>
</template>

<script>
export default {
    name: "upload",
    mounted() {
        console.log(document.getElementsByClassName("col")[0]);
        document.getElementsByClassName("col")[0].style.display="none";

        document.getElementsByClassName("q-uploader__header-content")[0].childNodes[0].style.display = "none";

        const listItem = document.getElementsByClassName("q-btn__content")[0].childNodes[0];
        console.log("0: "+listItem);
        const newItem = document.createElement('p');
        console.log("1: "+newItem);
        newItem.innerHTML = 'Drag & Drop Or <a>Browse</a> File to Upload';
        console.log("2: "+newItem);
        listItem.parentNode.replaceChild(newItem, listItem);
    },
    methods: {
        uploadFile(file) {
            console.log(file);
        }
    }
}
</script>

<style>
.container-fluid{
    margin: 5%;
}
.img-upload {
    float: left;
    border: 1px solid black;
    width: 100%;
    min-height: 620px;
    height: 100%;
}
.q-uploader {
    width: 100%;
    min-height: 620px;
    height: 100%;
}
.q-field--auto-height .q-field__control, .q-field--auto-height .q-field__native {
    min-height: 319px;
    background-color: rgb(238, 238, 238);
}
.q-file_uploader i{
    position: relative;
    align-content: center;
}
p {
    margin: 0 0 0px;
}
.q-uploader__file--img {
    height: 100%;
    background-size: contain;
}
/* .a.q-btn.q-btn-item.non-selectable.no-outline.q-btn--flat.q-btn--rectangle.q-btn--actionable.q-focusable.q-hoverable.q-btn--wrap.q-btn--dense {
    position: relative;
    color: black;
    transform: translateY(650%);
    font-weight: 400;
} */
</style>