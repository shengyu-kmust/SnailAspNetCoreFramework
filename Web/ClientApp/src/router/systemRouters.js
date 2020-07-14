import { iframeRuntime } from '@/utils'
const Layout = () => {
  if (iframeRuntime()) {
    return import('@/layout/noLayout')
  }
  return import('@/layout')
}

// 定义界面
const user = () => import('@/views/system/user')
const role = () => import('@/views/system/role')
const userRole = () => import('@/views/system/userRole')
const resource = () => import('@/views/system/resource')
const roleResource = () => import('@/views/system/roleResource')
const config = () => import('@/views/system/config')

const systemRouters = {
  path: '/systems',
  component: Layout,
  redirect: 'noRedirect',
  name: 'systems',
  meta: {
    title: '系统管理',
    iconClass: 'el-icon-setting'
  },
  children: [
    {
      path: 'user',
      component: user,
      name: 'user',
      meta: { title: '用户管理' }
    },
    {
      path: 'role',
      component: role,
      name: 'role',
      meta: { title: '角色管理' }
    },
    {
      path: 'userRole',
      component: userRole,
      name: 'userRole',
      meta: { title: '用户授权' }
    },
    {
      path: 'config',
      component: resource,
      name: 'resource',
      meta: { title: '权限资源管理' }
    },
    {
      path: 'roleResource',
      component: roleResource,
      name: 'roleResource',
      meta: { title: '角色授权' }
    },
    {
      path: 'resource',
      component: config,
      name: 'config',
      meta: { title: '配置' }
    }
  ]
}

export default systemRouters
