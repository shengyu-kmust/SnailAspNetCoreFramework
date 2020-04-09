import Vue from 'vue'
import Router from 'vue-router'

Vue.use(Router)

/* Layout */
import Layout from '@/layout'

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
    path: '/login',
    component: () => import('@/views/login/index'),
    hidden: true// hidden为true时，就不显示在菜单里
  },

  {
    path: '/404',
    component: () => import('@/views/404'),
    hidden: true
  },
  {
    path: '/',
    component: Layout,
    redirect: '/dashboard',
    children: [{
      path: 'dashboard',
      name: 'Dashboard',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '控制台', icon: 'dashboard' }
    }]
  },
  {
    path: '/example',
    component: Layout,
    redirect: '/tableSample',
    meta: {
      title: '示例'
    },
    children: [{
      path: 'tableSample',
      name: 'tableSample',
      component: () => import('@/views/tableSample/index'),
      meta: { title: 'table示例', icon: 'dashboard' }
    }, {
      path: 'formSample',
      name: 'formSample',
      component: () => import('@/views/formSample/index'),
      meta: { title: 'form示例', icon: 'dashboard' }
    }, {
      path: 'crudSample',
      name: 'crudSample',
      component: () => import('@/views/crudSample/index'),
      meta: { title: 'crud示例', icon: 'dashboard' }
    }]
  },
  {
    path: 'external-link',
    component: Layout,
    children: [
      {
        path: 'https://panjiachen.github.io/vue-element-admin-site/#/',
        meta: { title: '外部链接', icon: 'link' }
      }
    ]
  },

  // 404 page must be placed at the end !!!
  { path: '*', redirect: '/404', hidden: true }
]

export const asyncRoutes = [
  {
    path: '/permission',
    component: Layout,
    redirect: '/dashboard',
    meta: { title: '含权限的页面', icon: 'dashboard' },
    children: [{
      path: 'permission1',
      name: 'permission1',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '含权限的页面1', icon: 'dashboard' }
    }, {
      path: 'permission2',
      name: 'permission2',
      component: () => import('@/views/dashboard/index'),
      meta: { title: '含权限的页面2', icon: 'dashboard' }
    }]
  }
]
const createRouter = () => new Router({
  // mode: 'history', // require service support
  scrollBehavior: () => ({ y: 0 }),
  routes: constantRoutes
})

const router = createRouter()

// Detail see: https://github.com/vuejs/vue-router/issues/1234#issuecomment-357941465
export function resetRouter() {
  const newRouter = createRouter()
  router.matcher = newRouter.matcher // reset router
}

export default router
