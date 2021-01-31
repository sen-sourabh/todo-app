const vm = new Vue({
    el: '#q-app',
    data: {
      results: []
    },
    mounted () {
        this.getPosts('home');
    },
    methods: {
        getPosts(section) {
            let url = "https://localhost:44399/todoitems";
            axios.get(url).then((response) => {
            this.results = response.data.results;
            }).catch( error => { console.log(error); });
        }
    },
    computed: {
        processedPosts() {
          let posts = this.results;
    
          // Add image_url attribute
          posts.map(post => {
            let imgObj = post.multimedia.find(media => media.format === "superJumbo");
            post.todoImage = imgObj ? imgObj.url : post.todoTitle.substring(0,1).toUpperCase();
          });
    
          // Put Array into Chunks
          let i, j, chunkedArray = [], chunk = 4;
          for (i=0, j=0; i < posts.length; i += chunk, j++) {
            chunkedArray[j] = posts.slice(i,i+chunk);
          }
          return chunkedArray;
        }
    }
});