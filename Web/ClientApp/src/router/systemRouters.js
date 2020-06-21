import Layout from '@/layout'
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
      component: () => import('@/views/system/user'),
      name: 'user',
      meta: { title: '用户管理' }
    },
    {
      path: 'role',
      component: () => import('@/views/system/role'),
      name: 'role',
      meta: { title: '角色管理' }
    },
    {
      path: 'userRole',
      component: () => import('@/views/system/userRole'),
      name: 'userRole',
      meta: { title: '用户授权' }
    },
    {
      path: 'resource',
      component: () => import('@/views/system/resource'),
      name: 'resource',
      meta: { title: '权限资源管理' }
    },
    {
      path: 'roleResource',
      component: () => import('@/views/system/roleResource'),
      name: 'roleResource',
      meta: { title: '角色授权' }
    }

  ]
}

export default systemRouters
