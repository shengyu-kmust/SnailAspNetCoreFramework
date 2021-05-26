import Vue from 'vue'
import Router from 'vue-router'
// import { iframeRuntime } from '@/utils'

Vue.use(Router)

/* Layout */
// 用于嵌套到iframe里并去除菜单页，但前端微服务时用
const Layout = () => {
  // if (iframeRuntime()) {
  //   return import('@/layout/noLayout')
  // }
  return import('@/layout')
}

/** 定义界面 */
const redirect = () => import('@/views/redirect/index')
const login = () => import('@/views/login/index')
const notFound = () => import('@/views/404')
const test = () => import('@/views/test')
const dashboard = () => import('@/views/dashboard/index')

/** 引入所有的动态路由 */
import systemRouters from './systemRouters'
import basicRouters from './basicRouters'

/**
 * Note: sub-menu only appear when route children.length >= 1
 * Detail see: https://panjiachen.github.io/vue-element-admin-site/guide/essentials/router-and-nav.html
 *
 * hidden: true                   if set true, item will not show in the sidebar(default is false)
 * alwaysShow: true               if set true, will always show the root menu
 *                                if not set alwaysShow, when item has more than one children route,
 *                                it will becomes nested mode, otherwise not show the root menu
 * redirect: noRedirect           if set noRedirect will no redirect in the breadcrumb
 * name:'router-name'             the name is used by <keep-alive> (must set!!!)
 * meta : {
    roles: ['admin','editor']    control the page roles (you can set multiple roles)
    title: 'title'               the name show in sidebar and breadcrumb (recommend set)
    icon: 'svg-name'             the icon show in the sidebar
    breadcrumb: false            if set false, the item will hidden in breadcrumb(default is true)
    activeMenu: '/example/list'  if set path, the sidebar will highlight the path you set
    resourceCode:''               用于做权限控制的前后台资源唯一code约定
  }
 */

/**
 * constantRoutes
 * a base page that does not have permission requirements
 * all roles can be accessed
 */
export const constantRoutes = [
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      name: 'dashboard',
      component: dashboard,
      meta: { title: '控制台', icon: 'dashboard' }
    }]
  },
  {
    path: '/redirect', // 不要删除 ，这是用来配合刷新当前路由页面
    component: Layout,
    hidden: true,
    children: [
      {
        path: '/redirect/:path(.*)',
        component: redirect
      }
    ]
  },
  {
    path: '/login',
    component: login,
    hidden: true// hidden为true时，就不显示在菜单里
  },

  {
    path: '/404',
    component: notFound,
    hidden: true
  },
  {
    path: '/test',
    component: test,
    hidden: true
  },
  {
    path: '/crudSample',
    component: () => import('@/views/crudSample'),
    hidden: true
  },
  {
    path: '/mocktest',
    component: () => import('@/views/test/mocktest'),
    hidden: true
  }

  // 404 page must be placed at the end !!!
  // { path: '*', redirect: '/404', hidden: true } //如果此句打开后，页面刷新会直接到404页面，暂不知道原因， todo
]

export const asyncRoutes = [
  basicRouters,
  systemRouters
]

export const allRoute = constantRoutes.concat(asyncRoutes)
const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: allRoute
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465,todo 不用这种，可以删除？
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
