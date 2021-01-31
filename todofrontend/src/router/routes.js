
const routes = [
  {
    name: 'Index',
    path: '/',
    component: () => import('pages/Index.vue')
  },
  {
    name: 'Signin',
    path: '/signin',
    component: () => import('pages/Signin.vue')
  },
  {
    name: 'Home',
    path: '/home',
    component: () => import('pages/Home.vue')
  },
  //UPLOAD COMPONENT
  // {
  //   name: 'Uploadcomp',
  //   path: '/ucomp',
  //   component: () => import('pages/Uploadcomp.vue')
  // },

  // Always leave this as last one,
  // but you can also remove it
  {
    path: '*',
    component: () => import('pages/Error404.vue')
  }
]

export default routes
